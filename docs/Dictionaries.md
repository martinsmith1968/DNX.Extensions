# DNX.Extensions.Dictionaries

Extension methods for working with `IDictionary<TKey, TValue>` instances - safe value access,
key renaming, parsing simple delimited strings into dictionaries, merging multiple dictionaries
together, and converting a dictionary into a dynamic `ExpandoObject`.

## DictionaryExtensions class

| Method Name | Parameters | Description |
| --- | --- | --- |
| GetValue | key, defaultValue (optional) | Returns the value for `key`, or `defaultValue` if the key is not found. Throws `ArgumentNullException` if `dictionary` is null, or if `key` is null and `TK` is a reference type |
| SetValue | key, value | Sets (adds or overwrites) the value for `key`. Throws `ArgumentNullException` if `dictionary` is null, or if `key` is null and `TK` is a reference type |
| RenameKey | fromKey, toKey | Renames `fromKey` to `toKey`, keeping its value. A no-op if `dictionary` is null, `fromKey` is not present, or `toKey` is already present. Throws `ArgumentNullException` if `fromKey` or `toKey` is null |
| ToStringDictionary | elementSeparator (default `"|"`), valueSeparator (default `"="`) | Splits a string of `key=value` pairs into an `IDictionary<string, string>` |
| ToStringObjectDictionary | elementSeparator (default `"|"`), valueSeparator (default `"="`) | As `ToStringDictionary`, but returns an `IDictionary<string, object>` |
| MergeWith | other, mergeTechnique (default `MergeTechnique.Unique`) | Merges this dictionary with `other` using the given technique, returning a new dictionary and leaving both source dictionaries untouched |
| MergeAll (static) | mergeTechnique, dictionaries (params) | Merges any number of dictionaries using the given `MergeTechnique` |
| MergeUnique (static) | dictionaries (params) | Merges dictionaries assuming every key across all of them is unique; throws `ArgumentException` on a key clash |
| MergeFirst (static) | dictionaries (params) | Merges dictionaries; on a key clash, keeps the value from the first dictionary it was found in |
| MergeLast (static) | dictionaries (params) | Merges dictionaries; on a key clash, keeps the value from the last dictionary it was found in |
| ToExpando\<T\> | | Converts an `IDictionary<string, T>` into an `ExpandoObject`, recursively converting any nested `IDictionary<string, object>` values (and such dictionaries found inside collections) into `ExpandoObject`s too |

`MergeWith` and `MergeAll` take a `mergeTechnique` of type `MergeTechnique`, which lives in the
`DNX.Extensions.Linq` namespace rather than this one - it has three values: `Unique`, `TakeFirst`
and `TakeLast`, corresponding directly to `MergeUnique`, `MergeFirst` and `MergeLast` below. An
unrecognised `MergeTechnique` value causes `MergeAll` (and therefore `MergeWith`) to throw an
`EnumValueException<MergeTechnique>`.

Examples:

```csharp
// GetValue
var dict = new Dictionary<string, int> { ["A"] = 10, ["B"] = 20 };
var a = dict.GetValue("A");          // 10
var missing = dict.GetValue("Missing", 99); // 99 (key not found, so defaultValue is returned)

// SetValue
dict.SetValue("A", 11);  // updates existing key: dict["A"] == 11
dict.SetValue("C", 30);  // adds new key: dict["C"] == 30

// RenameKey
var renameDict = new Dictionary<string, string> { ["a"] = "1", ["b"] = "2", ["c"] = "3" };
renameDict.RenameKey("b", "q");
// renameDict now contains: a=1, c=3, q=2

// ToStringDictionary / ToStringObjectDictionary
var stringDict = "Liverpool=1|Southend United=7".ToStringDictionary();
// stringDict["Liverpool"] == "1", stringDict["Southend United"] == "7"

var customSeparators = "A:1|B:two".ToStringDictionary("|", ":");
// customSeparators["A"] == "1", customSeparators["B"] == "two"

// MergeFirst / MergeLast / MergeWith
var dict1 = new Dictionary<string, int> { ["A1"] = 1, ["A2"] = 2, ["A3"] = 3, ["A4"] = 4, ["A5"] = 5 };
var dict2 = new Dictionary<string, int> { ["A2"] = 12, ["A4"] = 14, ["A6"] = 16 };
var dict3 = new Dictionary<string, int> { ["A1"] = 21, ["A3"] = 23, ["A5"] = 25 };

var first = DictionaryExtensions.MergeFirst(dict1, dict2, dict3);
// first: A1=1, A2=2, A3=3, A4=4, A5=5, A6=16 (first dictionary found wins on clashes)

var last = DictionaryExtensions.MergeLast(dict1, dict2, dict3);
// last: A1=21, A2=12, A3=23, A4=14, A5=25, A6=16 (last dictionary found wins on clashes)

var otherDict = new Dictionary<string, int> { ["B1"] = 11, ["B2"] = 12, ["B3"] = 13 };
var merged = dict1.MergeWith(otherDict, MergeTechnique.Unique);
// merged contains all 8 entries from dict1 and otherDict; dict1 and otherDict are unchanged

// ToExpando
var expandoSource = new Dictionary<string, object>
{
    ["Key1"] = Guid.NewGuid(),
    ["Key2"] = new Dictionary<string, object> { ["Key21"] = Guid.NewGuid(), ["Key22"] = Guid.NewGuid() },
    ["Key3"] = Guid.NewGuid(),
};
dynamic expando = expandoSource.ToExpando();
var nested = expando.Key2.Key21; // same Guid value as expandoSource["Key2"]["Key21"], now reached via dynamic property access
```
