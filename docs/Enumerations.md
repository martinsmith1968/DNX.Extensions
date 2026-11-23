# DNX.Extensions.Enumerations

Extension methods for working with `enum` types and values - parsing strings to enum values,
reading attributes (including `[Description]`) off enum members, validating values, finding
min/max values, manipulating `[Flags]` bitmasks, and converting a whole enum type to a lookup
dictionary.

## EnumerationExtensions class

| Method Name | Parameters | Description |
| --- | --- | --- |
| ParseEnum\<T\> | value | Parses `value` to an enum value of `T`, case-sensitively. Throws `ArgumentException` if there is no matching member |
| ParseEnum\<T\> | value, ignoreCase | As above, with an explicit case-sensitivity flag |
| ParseEnumOrDefault\<T\> | value, defaultValue | Parses `value` to `T` case-sensitively, returning `defaultValue` instead of throwing if parsing fails |
| ParseEnumOrDefault\<T\> | value, ignoreCase, defaultValue | As above, with an explicit case-sensitivity flag |
| GetAttributes\<T\> | | Gets every attribute of type `T` declared on the enum value's member (non-inherited) |
| GetAttributes\<T\> | inherit | As above, with an explicit `inherit` flag passed through to reflection |
| GetAttribute\<T\> | | Gets the first attribute of type `T` declared on the enum value's member, or `default` if none is found |
| GetAttribute\<T\> | inherit | As above, with an explicit `inherit` flag |
| GetDescription | | Gets the `Description` from the member's `[Description]` attribute, or `null` if the member has none |
| GetDescription | inherit | As above, with an explicit `inherit` flag |
| GetDescriptionOrName | | Gets `GetDescription()`, falling back to `value.ToString()` (the member name) if the description is `null` |
| GetDescriptionOrName | inherit | As above - note the `inherit` parameter is accepted but not actually passed on to `GetDescription`, so it has no effect |
| IsValidEnum\<T\> | value | Determines whether an enum value (`T`) is a defined member of `T` |
| IsValidEnum\<T\> | value (string) | Determines whether a string is the (case-sensitive) name of a member of `T` |
| IsValidEnum\<T\> | value (string), ignoreCase | As above, with an explicit case-sensitivity flag |
| IsValidEnum | value (string), type, ignoreCase | Determines whether `value` is a member name of the given enum `type`. Throws `ArgumentNullException` if `type` is null, or `EnumTypeException` if `type` is not an enum type |
| GetMaxValue\<T\> (static) | | Returns the member of `T` with the highest underlying value |
| GetMinValue\<T\> (static) | | Returns the member of `T` with the lowest underlying value |
| IsValueOneOf\<T\> | allowed (params T[]) | Determines whether `value` is one of the supplied `allowed` values |
| IsValueOneOf\<T\> | allowed (IList\<T\>) | As above, taking the allowed values as a list |
| ManipulateFlag\<T\> | flag, set | Sets (`set: true`) or clears (`set: false`) the given `flag` bit within `value` and returns the result |
| SetFlag\<T\> | flag | Shorthand for `ManipulateFlag(flag, true)` |
| UnsetFlag\<T\> | flag | Shorthand for `ManipulateFlag(flag, false)` |
| GetSetValues\<T\> | | Returns an array of every defined member of `T` for which `enumValue.HasFlag(member)` is true |
| ToDictionaryByName\<T\> (static) | | Converts every value of `T` into an `IDictionary<string, T>` keyed by member name |
| ToDictionaryByValue\<T\> (static) | | Converts every value of `T` into an `IDictionary<T, string>` keyed by value, with the member name as the value |

Examples:

```csharp
public enum MyTestEnum1
{
    [Description("First")] One = 1,
    [Description("Second")] Two = 2,
    [Description("Third")] Three = 3,
    [Description("Fourth")] Four = 4,
    [Description("Fifth")] Five = 5,
}

[Flags]
public enum MyTestEnum2
{
    Flag1 = 1, Flag2 = 2, Flag3 = 4, Flag4 = 8, Flag5 = 16,
}

// ParseEnum / ParseEnumOrDefault
var parsed = "One".ParseEnum<MyTestEnum1>();              // MyTestEnum1.One
var parsedCi = "tHrEe".ParseEnum<MyTestEnum1>(true);      // MyTestEnum1.Three (ignoreCase: true)
// "Six".ParseEnum<MyTestEnum1>() would throw ArgumentException, message contains "'Six'"

var orDefault = "Seven".ParseEnumOrDefault(MyTestEnum1.One);        // MyTestEnum1.One (parse fails, default returned)
var orDefaultCi = "THREE".ParseEnumOrDefault(true, MyTestEnum1.One); // MyTestEnum1.Three

// GetDescription / GetDescriptionOrName
var description = MyTestEnum1.Two.GetDescription();        // "Second"
var noDescription = MyTestEnum2.Flag2.GetDescription();     // null (no [Description] attribute present)
var nameFallback = MyTestEnum2.Flag2.GetDescriptionOrName(); // "Flag2" (falls back to ToString())

// IsValidEnum
var validValue = MyTestEnum1.One.IsValidEnum();     // true
var invalidValue = ((MyTestEnum1)99).IsValidEnum();  // false (not a defined member)
var validName = "One".IsValidEnum<MyTestEnum1>();          // true (case-sensitive)
var invalidCaseName = "one".IsValidEnum<MyTestEnum1>();    // false
var validCiName = "one".IsValidEnum<MyTestEnum1>(true);    // true (ignoreCase: true)

// GetMaxValue / GetMinValue
var max = EnumerationExtensions.GetMaxValue<MyTestEnum1>(); // MyTestEnum1.Five
var min = EnumerationExtensions.GetMinValue<MyTestEnum1>(); // MyTestEnum1.One

// IsValueOneOf
var allowed = new[] { MyTestEnum1.One, MyTestEnum1.Three, MyTestEnum1.Five };
var isOneOf = MyTestEnum1.Three.IsValueOneOf(allowed); // true
var isNotOneOf = MyTestEnum1.Four.IsValueOneOf(allowed); // false

// ManipulateFlag / SetFlag / UnsetFlag
var flags = MyTestEnum2.Flag2 | MyTestEnum2.Flag4;
flags = flags.SetFlag(MyTestEnum2.Flag3);
// flags == MyTestEnum2.Flag2 | MyTestEnum2.Flag3 | MyTestEnum2.Flag4

flags = flags.UnsetFlag(MyTestEnum2.Flag2);
// flags == MyTestEnum2.Flag3 | MyTestEnum2.Flag4

// GetSetValues
var combo = MyTestEnum2.Flag2 | MyTestEnum2.Flag4 | MyTestEnum2.Flag5;
var setValues = combo.GetSetValues();
// setValues == [ MyTestEnum2.Flag2, MyTestEnum2.Flag4, MyTestEnum2.Flag5 ]

// ToDictionaryByName / ToDictionaryByValue
var byName = EnumerationExtensions.ToDictionaryByName<MyTestEnum1>();
// byName["Two"] == MyTestEnum1.Two

var byValue = EnumerationExtensions.ToDictionaryByValue<MyTestEnum1>();
// byValue[MyTestEnum1.Two] == "Two"
```
