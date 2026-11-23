# DNX.Extensions.Objects

General-purpose extension methods on `object`.

## ObjectExtensions

| Method Name | Parameters | Description |
| --- | --- | --- |
| ToStringOrDefault | defaultValue (default `""`) | Returns `obj.ToString()`, or `defaultValue` if `obj` is null |
| To\<T\> | defaultValue (default `default(T)`) | Casts `obj` directly to `T` (`(T)obj`), returning `defaultValue` if `obj` is null or the cast fails. This is a direct cast, not a type conversion - it will not turn a numeric string into an `int`, for example |
| ToDictionary | | Returns a `Dictionary<string, object>` of the object's public instance properties (name to value), including inherited ones. Skips indexers and write-only properties. Returns `null` if `obj` is null |
| ToDictionary | `BindingFlags` bindingFlags | As above, but reads properties matching the supplied `BindingFlags` instead of the public-instance default |
| ChangeType\<T\> | | Converts `obj` to `T` via `Convert.ChangeType`. Throws if the conversion isn't supported |
| ChangeType | `Type` type | Converts `obj` to `type` via `Convert.ChangeType`. Throws if the conversion isn't supported |
| ChangeType\<T\> | defaultValue | As `ChangeType<T>()`, but returns `defaultValue` instead of throwing if the conversion fails |
| GetUniqueInstanceId | | Returns `"{FullTypeName}:{hashCode}"` for the object, using `obj.GetHashCode()` |
| GetUniqueInstanceId | instanceIdOverride | Returns `"{FullTypeName}:{instanceIdOverride}"`, falling back to `obj.GetHashCode()` if `instanceIdOverride` is null or empty |
| CoalesceNull (static) | `params object[]` objects | Returns the first non-null value in `objects`, or `null` if they're all null. Also available as an extension method below |
| CoalesceNull (extension) | | Returns the first non-null value in the `IList<object>` it's called on, or `null` if they're all null |

The default `ToDictionary()` (no `BindingFlags`) reads properties matching `BindingFlags.Instance
| BindingFlags.Public | BindingFlags.GetProperty` - so private, protected and static properties
are excluded unless you pass your own `bindingFlags`.

Examples:

```csharp
// ToStringOrDefault
object value = 12.34;
var text = value.ToStringOrDefault();      // "12.34"

object missing = null;
var fallback = missing.ToStringOrDefault("bob"); // "bob"

// To<T> - direct cast, not conversion
var good = ((object)42).To<int>();   // 42
var bad = "abc".To<int>(99);         // 99 (the cast from string to int fails, so the default is returned)

// ToDictionary
public class Person
{
    public int Id { get; set; } = 1;
    public string Name { get; set; } = "bob";
}

var dict = new Person().ToDictionary();
// dict["Id"] = 1
// dict["Name"] = "bob"

// ChangeType
object raw = "42";
var number = raw.ChangeType<int>();          // 42
var withDefault = "not-an-int".ChangeType(99); // 99

// GetUniqueInstanceId
var person = new Person();
var id = person.GetUniqueInstanceId();       // "MyNamespace.Person:<hash code>"

// CoalesceNull
var result = ObjectExtensions.CoalesceNull(null, "b", "c"); // "b"
```
