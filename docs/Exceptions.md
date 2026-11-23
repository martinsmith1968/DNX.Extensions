# DNX.Extensions.Exceptions

Custom exception types used throughout the library to carry structured information about what
went wrong (the value that failed to convert, the invalid parameter, the type that wasn't an
enum, etc.), plus a couple of extension methods for walking an exception's `InnerException`
chain.

## ConversionException class

Thrown when a conversion to a specified type fails.

| Property | Type | Description |
| --- | --- | --- |
| Value | string | The value that could not be converted |
| ConvertType | `Type` | The type conversion was being attempted to, or `null` if not supplied |

| Constructor | Parameters | Description |
| --- | --- | --- |
| ConversionException(string, string) | value, message | Equivalent to the three-argument constructor with `type: null` |
| ConversionException(string, string, Type) | value, message, type | Full constructor; sets `Value`, `Message` and `ConvertType` |

Example:

```csharp
var ex = new ConversionException("abc", "Unable to convert value", typeof(int));

// ex.Value == "abc"
// ex.Message == "Unable to convert value"
// ex.ConvertType == typeof(int)
```

## EnumTypeException class

Thrown when a `Type` that was expected to be an enumeration type is not.

| Property | Type | Description |
| --- | --- | --- |
| Type | `Type` | The type the exception was thrown for (may be `null`) |

| Constructor | Parameters | Description |
| --- | --- | --- |
| EnumTypeException(Type) | type | Uses the default message template `"{0} is not an enumeration type"` |
| EnumTypeException(Type, string) | type, messageTemplate | Formats `messageTemplate` with `type.Name` |

A `null` type is accepted without throwing - `Type` is simply `null` and the template is
formatted with a `null` name.

Example:

```csharp
var ex = new EnumTypeException(typeof(int));

// ex.Type == typeof(int)
// ex.Message == "Int32 is not an enumeration type"
```

## EnumValueException\<T\> class

Thrown when a value is not a valid member of enum type `T`.

| Property | Type | Description |
| --- | --- | --- |
| Type | `Type` | `typeof(T)` |
| Value | `T` | The value that was not a valid member of `T` |

| Constructor | Parameters | Description |
| --- | --- | --- |
| EnumValueException(T) | value | Uses the default message template `"{0} is not a valid {1} value"` |
| EnumValueException(T, string) | value, messageTemplate | Formats `messageTemplate` with `{0}` = `value`, `{1}` = `typeof(T).Name` |

Example:

```csharp
public enum MyEnum { One = 1, Two = 2, Three = 3, Four = 4, Five = 5 }

var ex = new EnumValueException<MyEnum>(MyEnum.Four);

// ex.Type == typeof(MyEnum)
// ex.Value == MyEnum.Four
// ex.Message == "Four is not a valid MyEnum value"
```

## ParameterException class

An exception for identifying issues with expected parameters (for example, a required parameter
that was not supplied).

| Property | Type | Description |
| --- | --- | --- |
| ParamName | string | Name of the parameter |
| ParamValue | object | The value that was supplied for the parameter (`null` if not supplied) |

| Constructor | Parameters | Description |
| --- | --- | --- |
| ParameterException(string, string) | paramName, message | Sets `ParamName` and `Message`; `ParamValue` stays `null` |
| ParameterException(string, object, string) | paramName, paramValue, message | As above, also sets `ParamValue` |
| ParameterException(string, string, Exception) | paramName, message, innerException | As the first constructor, with an inner exception |
| ParameterException(string, object, string, Exception) | paramName, paramValue, message, innerException | As the second constructor, with an inner exception |

Example:

```csharp
var ex = new ParameterException("name", 42, "message");

// ex.ParamName == "name"
// ex.ParamValue == 42
// ex.Message == "message"
```

## ParameterInvalidException class

Derives from `ParameterException`. Carries exactly the same `ParamName`/`ParamValue` contract and
constructor overloads, but as a distinct type so calling code can specifically catch (or filter
on) "the parameter was present but its value was invalid" separately from "there was a problem
with a parameter" in general.

| Constructor | Parameters | Description |
| --- | --- | --- |
| ParameterInvalidException(string, string) | paramName, message | |
| ParameterInvalidException(string, object, string) | paramName, paramValue, message | |
| ParameterInvalidException(string, string, Exception) | paramName, message, innerException | |
| ParameterInvalidException(string, object, string, Exception) | paramName, paramValue, message, innerException | |

Example:

```csharp
var ex = new ParameterInvalidException("name", 42, "message");

// ex is also a ParameterException
// ex.ParamName == "name"
// ex.ParamValue == 42
```

## ReadOnlyListException\<T\> class

Thrown when an attempt is made to modify a list that is read-only.

| Property | Type | Description |
| --- | --- | --- |
| List | `IList<T>` | The list that was read-only |

| Constructor | Parameters | Description |
| --- | --- | --- |
| ReadOnlyListException(IList\<T\>) | list | `Message` is built from the template `"The list of type {0} is read-only and cannot be modified"`, where `{0}` is `list.GetType().Name` |

Note that `Type.Name` for a closed generic type includes the arity suffix - for a `List<char>`
this is `"List`1"`, not `"List"`.

Example:

```csharp
var list = "abcdefghij".ToCharArray().ToList();

var ex = new ReadOnlyListException<char>(list);

// ex.List == list
// ex.Message == "The list of type List`1 is read-only and cannot be modified"
```

## ExceptionExtensions

Two extension methods on `Exception` that walk the `InnerException` chain, starting with the
exception they're called on and following `.InnerException` down until it is `null`.

| Method Name | Parameters | Description |
| --- | --- | --- |
| GetMessageList | this `Exception` ex | Returns the `Message` of `ex`, then of each `InnerException` in turn, outermost first |
| GetExceptionList | this `Exception` ex | Returns `ex`, then each `InnerException` in turn, outermost first |

Example:

```csharp
var ex1 = new Exception("ex1");
var ex2 = new Exception("ex2", ex1);
var ex3 = new Exception("ex3", ex2);
var ex4 = new Exception("ex4", ex3);

var exceptions = ex4.GetExceptionList();
// exceptions == [ ex4, ex3, ex2, ex1 ]

var messages = ex4.GetMessageList();
// messages == [ "ex4", "ex3", "ex2", "ex1" ]
```
