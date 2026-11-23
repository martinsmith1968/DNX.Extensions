# DNX.Extensions.Reflection

Helpers for working with .NET reflection: reading and matching custom attributes, extracting
member names from `Expression<Func<T>>` trees, filtering instances by attribute, converting
objects to and from property dictionaries, and some everyday `Type` checks. Several of the
reading/writing methods below default their `BindingFlags` from the `DNX.Extensions.Defaults`
class (`PropertyInfoReaderBindingFlags`, `PropertyInfoWriterBindingFlags`,
`FieldInfoReaderBindingFlags` - all `Instance | Public | Get/SetProperty` or `Get/SetField`), so
by default only public instance members are seen; pass an explicit `BindingFlags` to widen that.

## AttributeExtensions

Reads custom attributes from a `Type`, an object instance, or a `MemberInfo` (property/field).
Unlike a plain `Type.GetCustomAttributes(..., inherit: true)` call - which only walks up the
*class* hierarchy - `GetTypeAttributes<T>` also walks every implemented interface when
`inherit` is `true`, so attributes declared on an interface are picked up too.

| Method Name | Parameters | Description |
| --- | --- | --- |
| GetTypeAttributes\<T\> | inherit | Gets every attribute of type `T` on the `Type`: attributes from the class hierarchy (standard .NET `inherit` behaviour) plus, if `inherit` is `true`, attributes declared on any implemented interface. Throws `ArgumentNullException` if `type` is null |
| GetTypeAttributesFromInstance\<T\> | inherit | As above, resolved from `instance.GetType()`. Throws `ArgumentNullException` if `instance` is null |
| GetMemberAttributes\<T\> | inherit | Gets every attribute of type `T` declared directly on a `MemberInfo` (property or field). Throws `ArgumentNullException` if `memberInfo` is null |
| HasTypeAttributes\<T\> | inherit | `true` if `GetTypeAttributes<T>` returns at least one match |
| InstanceHasTypeAttributes\<T\> | inherit | `true` if `GetTypeAttributesFromInstance<T>` returns at least one match |

Example:

```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true)]
internal sealed class ReflectionMarkerAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}

[ReflectionMarker("interface")]
internal interface IReflectionMarked { }

[ReflectionMarker("base")]
internal class ReflectionMarkedBase
{
    public int Number { get; set; }
}

internal sealed class ReflectionMarkedChild : ReflectionMarkedBase, IReflectionMarked
{
    public string Name { get; set; }
}

var typeAttributes = typeof(ReflectionMarkedChild).GetTypeAttributes<ReflectionMarkerAttribute>(true);
// typeAttributes contains the "interface" attribute even though [ReflectionMarker] is declared
// on IReflectionMarked, not on ReflectionMarkedChild itself - plus the inherited "base" one

var instance = new ReflectionMarkedChild();
var instanceAttributes = instance.GetTypeAttributesFromInstance<ReflectionMarkerAttribute>(true);
// same result as above, resolved via instance.GetType()

var member = typeof(ReflectionMarkedChild).GetProperty(nameof(ReflectionMarkedChild.Name));
var memberAttributes = member.GetMemberAttributes<ReflectionMarkerAttribute>(true);
// memberAttributes.Count == 0 - Name has no ReflectionMarkerAttribute applied

var hasAttributes = typeof(ReflectionMarkedChild).HasTypeAttributes<ReflectionMarkerAttribute>(true);
// true

var instanceHasAttributes = instance.InstanceHasTypeAttributes<ReflectionMarkerAttribute>(true);
// true
```

## ExpressionExtensions

Inspects an `Expression<Func<T>>` to determine its shape and, where possible, extract the name
of the member/variable it refers to. These are plain static methods (not extension methods),
so they're called as `ExpressionExtensions.GetMemberName(...)`.

The library's own `Guard` class (`DNX.Extensions.Validation.BuiltInTypes.Guard`) uses
`GetMemberName` internally to turn an argument expression like `() => someParameter` into the
string `"someParameter"` for use as the `ParamName` of the exception it throws - this is how
`Guard.IsNotNull(() => value)` can report a meaningful parameter name without the caller having
to pass it as a separate string.

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsMemberExpression\<T\> | `Expression<Func<T>>` exp | `true` if the expression body is a `MemberExpression` (a field/property access, or a captured local variable) |
| IsLambdaExpression\<T\> | `Expression<Func<T>>` exp | `true` if the expression body is itself a `LambdaExpression` (a nested lambda) |
| IsUnaryExpression\<T\> | `Expression<Func<T>>` exp | `true` if the expression body is a `UnaryExpression` (e.g. a boxing conversion) |
| GetMemberName\<T\> | `Expression<Func<T>>` exp | Returns the member/variable name if the body is a `MemberExpression`; otherwise `null` |
| GetLambdaName\<T\> | `Expression<Func<T>>` exp | Returns the `Name` of the body if it's a `LambdaExpression`; otherwise `null`. Compiler-generated nested lambdas have no name, so this is usually `null` for `() => () => ...` expressions |
| GetUnaryName\<T\> | `Expression<Func<T>>` exp | Returns the member name if the body is a `UnaryExpression` whose operand is a `MemberExpression` (e.g. boxing a member access); otherwise `null` |
| GetExpressionName\<T\> | `Expression<Func<T>>` exp | Tries `GetMemberName`, then `GetLambdaName`, then `GetUnaryName`, in that order, and returns the first non-null result, or `null` if none match |

Example:

```csharp
var localVar = 5;

ExpressionExtensions.IsMemberExpression(() => localVar);          // true
ExpressionExtensions.GetMemberName(() => localVar);                // "localVar"

Expression<Func<Func<int>>> nestedLambda = () => () => 5;
ExpressionExtensions.IsLambdaExpression(nestedLambda);              // true
ExpressionExtensions.GetLambdaName(nestedLambda);                   // null - compiler-generated, has no name

ExpressionExtensions.IsUnaryExpression(() => (object)localVar);     // true
ExpressionExtensions.GetUnaryName(() => (object)localVar);          // "localVar"

ExpressionExtensions.GetExpressionName(() => localVar);             // "localVar" (via GetMemberName)
ExpressionExtensions.GetExpressionName(() => (object)localVar);     // "localVar" (via GetUnaryName)
ExpressionExtensions.GetExpressionName(nestedLambda);                 // null (GetLambdaName found nothing to report)

// How the library's own Guard class uses this:
string name = null;
try
{
    Guard.IsNotNull(() => name);
}
catch (ArgumentNullException ex)
{
    var paramName = ex.ParamName; // "name" - extracted from the expression, not passed explicitly
}
```

## InstanceResolutionExtensions

Filters a list of instances down to those whose runtime type carries an attribute satisfying a
given condition - useful for picking specific implementations out of a set discovered via
`Assembly.CreateInstancesOfConcreteTypesThatImplementType<T>()` (see `docs/Assemblies.md`).

| Method Name | Parameters | Description |
| --- | --- | --- |
| ResolveInstancesOfTypeWithAttributeAndCondition\<T, TA\> | `Func<TA, bool>` func, inherit (default `true`) | Keeps only the instances whose type has at least one `TA` attribute (found via `GetTypeAttributesFromInstance<TA>(inherit)`) matching `func`. Returns an empty list if `instances` is `null` |

Example:

```csharp
internal interface IType { }

internal interface ITypeB : IType { }
[MyTag("CC")]
internal class CTypeC : ITypeB { }
internal class CTypeCA : CTypeC { } // no attribute of its own - inherits CTypeC's [MyTag("CC")]

IList<IType> instances = Assembly.GetExecutingAssembly()
    .CreateInstancesOfConcreteTypesThatImplementType<IType>();

var ccInherited = instances.ResolveInstancesOfTypeWithAttributeAndCondition<IType, MyTagAttribute>(
    a => a.Letter == "CC");
// ccInherited contains 2 instances: CTypeC (declares [MyTag("CC")] directly)
// and CTypeCA (picks it up because inherit defaults to true)

var ccNotInherited = instances.ResolveInstancesOfTypeWithAttributeAndCondition<IType, MyTagAttribute>(
    a => a.Letter == "CC", inherit: false);
// ccNotInherited contains only 1 instance: CTypeC - CTypeCA's match came solely from
// its base class, which is excluded when inherit is false
```

## ReflectionExtensions

Everyday reflection chores: read a property/field by name (including private members and
dot-notation child paths), list the properties/fields of one or more types, and convert objects
to and from `IDictionary<string, object>`.

### Reading properties and fields

| Method Name | Parameters | Description |
| --- | --- | --- |
| GetPropertyValueByName\<T\> | propertyName, `BindingFlags` flags, defaultValue (default `null`) | Gets a property's value by name using the supplied `BindingFlags`. Returns `defaultValue` if the property isn't found, or if invoking its getter returns `null` |
| GetPrivatePropertyValue\<T\> | propertyName, defaultValue (default `null`) | Shortcut for `GetPropertyValueByName` using `BindingFlags.Instance \| BindingFlags.NonPublic` |
| GetPropertiesForType | type | Gets the `PropertyInfo` list for a `Type`, using `Defaults.PropertyInfoReaderBindingFlags` (`Instance \| Public \| GetProperty`). Not an extension method |
| GetPropertiesForType | type, `BindingFlags` bindingFlags | As above, with explicit flags. Not an extension method |
| GetPropertiesForTypes | `IEnumerable<Type>` types | Gets the combined `PropertyInfo` list across several types, using the default reader flags. Not an extension method |
| GetPropertiesForTypes | `IEnumerable<Type>` types, `BindingFlags` bindingFlags | As above, with explicit flags. Not an extension method |
| GetFieldsForType | type | Gets the `FieldInfo` list for a `Type`, using `Defaults.FieldInfoReaderBindingFlags` (`Instance \| Public \| GetField`). Not an extension method |
| GetFieldsForType | type, `BindingFlags` bindingFlags | As above, with explicit flags. Not an extension method |
| GetPropertyInfoByName | propertyName | Gets a `PropertyInfo` by name (case-insensitive), supporting dot notation to drill into child properties, e.g. `"Address.City"`. Returns `null` if any segment doesn't resolve, or if `instance` is `null` |
| IsPropertyNameValid | propertyName | `true` if `GetPropertyInfoByName` resolves to a non-null `PropertyInfo` |
| GetNestedPropertyValueByName | propertyName | Gets a property's value by name, supporting the same dot notation as `GetPropertyInfoByName` |
| GetPropertiesDictionary | instance | Combines all public instance properties and fields of `instance` into one dictionary, using the default reader flags; where a property and field share a name, the property wins. Returns `null` if `instance` is `null`. Not an extension method |
| GetPropertiesDictionary | instance, `BindingFlags` bindingFlags | As above, with explicit flags applied to both the property and field lookup. Not an extension method |

Example:

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    private string Secret { get; set; } = "hidden";
}

var person = new Person { Name = "Alice", Age = 30 };

person.GetPropertyValueByName(nameof(Person.Name), BindingFlags.Instance | BindingFlags.Public);
// "Alice"

person.GetPrivatePropertyValue("Secret");
// "hidden"

ReflectionExtensions.GetPropertiesForType(typeof(Person)).Select(p => p.Name);
// ["Name", "Age"] - public instance properties only; "Secret" is private so it's excluded

person.GetPropertyInfoByName("name").Name;
// "Name" - lookup by name is case-insensitive

person.IsPropertyNameValid("Age");      // true
person.IsPropertyNameValid("Missing");  // false

person.GetNestedPropertyValueByName("Name");
// "Alice"
```

### Converting to and from dictionaries

| Method Name | Parameters | Description |
| --- | --- | --- |
| AsDictionaryTyped\<T\> | `BindingFlags` bindingFlags (default `Defaults.PropertyInfoReaderBindingFlags`) | Typed wrapper over `AsDictionary`. Returns `null` if `instance` is `null` |
| AsDictionary | `BindingFlags` bindingFlags (default `Defaults.PropertyInfoReaderBindingFlags`) | Converts an object to an `IDictionary<string, object>`. If `instance` is itself an `IDictionary`, its entries are copied (keys converted via `Convert.ToString`); if it's an `ExpandoObject`, its entries are copied directly; otherwise it falls back to `GetPropertiesDictionary`. Returns `null` if `instance` is `null` |
| AsInstance\<T\> | `BindingFlags` bindingFlags (default `Defaults.PropertyInfoWriterBindingFlags`) | Creates a new `T` (via `new T()`, so `T` needs a public parameterless constructor) and populates it from a dictionary via `PopulateFrom`. Returns `default` if `dict` is `null` |
| PopulateFrom\<T\> | `IDictionary<string, object>` dict, `BindingFlags` bindingFlags (default `Defaults.PropertyInfoWriterBindingFlags`) | Sets each writable property on `instance` whose name matches a key in `dict`; keys with no matching property, and properties with no matching key, are left alone. Returns `instance` unchanged if `dict` is `null` |

Example:

```csharp
var person = new Person { Name = "Alice", Age = 30 };

var dict = person.AsDictionary();
// dict["Name"] == "Alice"
// dict["Age"]  == 30

var restored = dict.AsInstance<Person>();
// restored.Name == "Alice"
// restored.Age  == 30

person.PopulateFrom(new Dictionary<string, object> { ["Name"] = "Bob" });
// person.Name == "Bob" - person.Age stays 30, because "Age" wasn't a key in the dictionary

var expando = new ExpandoObject() as IDictionary<string, object>;
expando["Name"] = "Carol";
((object)expando).AsDictionary()["Name"];
// "Carol" - ExpandoObject entries are copied directly, rather than reflected over
```

## TypeExtensions

Everyday `Type` checks: nullability, assignability, and creating default values/instances.
`CreateDefaultInstance<T>()` has no `Type` to extend, so it's called as a plain static method
(`TypeExtensions.CreateDefaultInstance<T>()`); the rest are extension methods on `Type`.

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsNullable | | `true` if the type is a reference type, or a `Nullable<T>` value type; `false` for any other value type. Throws `ArgumentNullException` if `type` is null |
| IsA\<T\> | | `true` if `type` is assignable to `T` (i.e. `T` is `type` itself, or a base class/interface of it) |
| IsA | `Type` baseClassOrInterface | `true` if `type` is assignable to `baseClassOrInterface` (delegates to `baseClassOrInterface.IsAssignableFrom(type)`) |
| GetDefaultValue | | Returns `Activator.CreateInstance(type)` for value types (e.g. `0` for `int`), or `null` for reference types - equivalent to C#'s `default` keyword applied to `type` |
| CreateDefaultInstance\<T\> | | Returns `Activator.CreateInstance<T>()`. Not an extension method |
| CreateDefaultInstance | | Returns `Activator.CreateInstance(type)` |

Example:

```csharp
typeof(int).IsNullable();     // false
typeof(int?).IsNullable();    // true
typeof(string).IsNullable();  // true - any reference type counts as nullable

typeof(string).IsA<object>();                      // true
typeof(ArgumentException).IsA<Exception>();        // true
typeof(ArgumentException).IsA(typeof(Exception));  // true - non-generic overload

typeof(int).GetDefaultValue();     // 0
typeof(string).GetDefaultValue();  // null

public class Order
{
    public int Quantity { get; set; }

    public Order()
    {
        Quantity = 123;
    }
}

var order1 = TypeExtensions.CreateDefaultInstance<Order>();
// order1.Quantity == 123 - the parameterless constructor runs, it's not a blank "default" value

var order2 = typeof(Order).CreateDefaultInstance() as Order;
// order2.Quantity == 123
```
