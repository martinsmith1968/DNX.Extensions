# DNX.Extensions.Linq

Extension methods and small helper types for working with `IEnumerable<T>`/`IList<T>`,
`NameValueCollection`, and `Tuple`s, plus a lightweight pairwise item-comparison utility.

## EnumerableExtensions

Extend any `IEnumerable<T>` (and, for a couple of methods, `IList<T>`) with null-safe checks,
membership tests, random/positional access, and simple append semantics.

| Method Name | Parameters | Description |
| --- | --- | --- |
| HasAny | | `true` if the enumerable is not null and has at least one element |
| HasAny | `Func<T, bool>` predicate | `true` if the enumerable is not null and has at least one element matching the predicate |
| IsOneOf | `params T[]` candidates | `true` if the value equals one of the candidates (default equality) |
| IsOneOf | `IEqualityComparer<T>` comparer, `params T[]` candidates | As above, using the supplied comparer |
| IsOneOf | `IEnumerable<T>` candidates | `true` if the value equals one of the candidates (default equality) |
| IsOneOf | `IEnumerable<T>` candidates, `IEqualityComparer<T>` comparer | As above, using the supplied comparer |
| IsIn | `params T[]` list | `true` if the value is found in the list (default equality) |
| IsIn | `IList<T>` list, `IEqualityComparer<T>` comparer = null, `bool` treatNullListAs = false | `true` if the value is found in the list; returns `treatNullListAs` when `list` is null |
| IsNotIn | `params T[]` list | `true` if the value is **not** found in the list (default equality) |
| IsNotIn | `IList<T>` list, `IEqualityComparer<T>` comparer = null, `bool` treatNullListAs = true | `true` if the value is **not** found in the list; returns `treatNullListAs` when `list` is null |
| GetRandomItem | `Random` randomizer = null | Returns a random item from the enumerable (or `default` if null/empty); uses a shared internal `Random` unless one is supplied |
| GetRandomItem | `Func<int>` indexProvider | Returns the item at the index produced by `indexProvider` (or `default` if null/empty) |
| GetAt | `int` index | Gets the item at `index` (negative counts back from the end), or `default` if out of range |
| ToConcreteList | | Converts the enumerable to an `IList<T>`, or an empty list if the source is null |
| AppendItem | `T` instance | Appends `instance` to the enumerable; duplicates are always allowed |
| AppendItem | `T` instance, `IEqualityComparer<T>` comparer | Appends `instance` only if no existing element is equal under `comparer` (uses `Enumerable.Union`, so pre-existing duplicates within the source are also collapsed) |

Examples:

```csharp
// HasAny
new[] { 1, 2, 3 }.HasAny();                 // true
Array.Empty<int>().HasAny();                // false
((IEnumerable<int>)null).HasAny();          // false

// IsOneOf
var candidates = new[] { "One", "Two", "Three", "Four", "Five" };
"Three".IsOneOf(candidates);                // true
"Six".IsOneOf(candidates);                  // false
"five".IsOneOf(StringComparer.OrdinalIgnoreCase, "One", "Two", "Three", "Four", "Five"); // true

// IsIn / IsNotIn
"3".IsIn("1", "2", "3", "4", "5");          // true
"0".IsIn("1", "2", "3", "4", "5");          // false
"A".IsIn(new[] { "a", "b", "c" }, StringComparer.OrdinalIgnoreCase); // true
"0".IsNotIn("1", "2", "3", "4", "5");       // true

// GetRandomItem (deterministic via indexProvider)
var items = new[] { "a", "b", "c" };
items.GetRandomItem(() => 1);                // "b"

// GetAt
items.GetAt(0);                              // "a"
items.GetAt(-1);                             // "c" (last item)
items.GetAt(10);                             // null (out of range)

// ToConcreteList
IEnumerable<int> source = null;
var list = source.ToConcreteList();          // list is an empty IList<int>, Count == 0

// AppendItem
new[] { "a", "b", "c" }.AppendItem("d");     // "a","b","c","d"
new[] { "a", "b", "c" }.AppendItem("a");     // "a","b","c","a" (no comparer -> duplicate kept)

var caseInsensitive = EqualityComparerFunc<string>.Create(
    (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase),
    s => s?.ToUpperInvariant().GetHashCode() ?? 0);

new[] { "a", "b", "c", "d" }.AppendItem("D", caseInsensitive); // "a","b","c","d" ("D" matches existing "d", not appended)
```

## ListExtensions

Extend `IList<T>` with bounds-checked, negative-index-aware access, and in-place reordering.

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsIndexValid | `int` index | `true` if the list is non-empty and `index` is within `0..Count-1` |
| GetAbsoluteIndex | `int` index | Converts a negative `index` to its positive position from the end of the list (`(Count + index) % Count`); a non-negative index, or an index for a null/empty list, is returned unchanged |
| GetItemAt | `int` index, `T` defaultValue = default | Gets the item at `index` (negative counts back from the end), or `defaultValue` if out of range |
| Move | `int` oldIndex, `int` newIndex | Moves the item at `oldIndex` to `newIndex` (both support negative indexing). Throws `ArgumentOutOfRangeException` (`oldIndex`/`newIndex`) for an invalid index, or `ReadOnlyListException<T>` if the list is read-only |
| Swap | `int` oldIndex, `int` newIndex | Swaps the items at `oldIndex` and `newIndex` (both support negative indexing). Same exceptions as `Move` |
| CreateList | `params T[]` values | Static method. Builds an `IList<T>` from the supplied values |

Examples:

```csharp
var letters = "a,b,c,d,e,f,g,h,i,j".Split(',').ToList();

letters.IsIndexValid(5);                     // true
letters.IsIndexValid(10);                    // false
letters.IsIndexValid(-1);                    // false

letters.GetAbsoluteIndex(-1);                // 9
letters.GetAbsoluteIndex(-4);                // 6

letters.GetItemAt(-1);                       // "j"
letters.GetItemAt(10, "?");                  // "?" (out of range -> defaultValue)

var moveList = "a,b,c,d,e,f,g,h,i,j".Split(',').ToList();
moveList.Move(1, 5);
string.Join(",", moveList);                  // "a,c,d,e,f,b,g,h,i,j" ("b" relocated to index 5)

var swapList = "a,b,c,d,e,f,g,h,i,j".Split(',').ToList();
swapList.Swap(1, 5);
string.Join(",", swapList);                  // "a,f,c,d,e,b,g,h,i,j" ("b" and "f" exchanged)

var created = ListExtensions.CreateList(1, 2, 3);
created.Count;                                // 3
```

## NameValueExtensions

Converts a `NameValueCollection` (e.g. `HttpRequest.QueryString`) into an
`IDictionary<string, string>`, fanning out multi-valued keys and merging them via
`DictionaryExtensions.MergeAll` from the `DNX.Extensions.Dictionaries` namespace.

| Method Name | Parameters | Description |
| --- | --- | --- |
| ToDictionary | | Converts using `MergeTechnique.Unique`; throws `ArgumentException` if any key occurs more than once |
| ToDictionary | `MergeTechnique` mergeTechnique | Converts using the specified merge technique. Throws `ArgumentNullException` (`collection`) if `collection` is null. A key added with a null value is dropped (`NameValueCollection.GetValues` returns null, not a single-null-element array), and a null-named entry is dropped too (`Dictionary<string, string>` doesn't allow a null key) |

Examples:

```csharp
var collection = new NameValueCollection
{
    { "One", "1" },
    { "Two", "2" },
    { "Three", "3" }
};

var dict = collection.ToDictionary();
dict["One"];                                  // "1"
dict.Count;                                    // 3

var withDuplicates = new NameValueCollection
{
    { "One", "1" },
    { "Two", "2" },
    { "One", "3" },
    { "Two", "4" },
    { "Three", "5" }
};

withDuplicates.ToDictionary(MergeTechnique.TakeFirst)["One"]; // "1"
withDuplicates.ToDictionary(MergeTechnique.TakeLast)["One"];  // "3"
withDuplicates.ToDictionary(MergeTechnique.Unique);           // throws ArgumentException ("One" and "Two" clash)
```

## TupleExtensions

Adds `Add(item1, item2, ...)` overloads directly onto `IList<Tuple<...>>` (arities 2 through 6),
so a plain `List<Tuple<T1, T2>>` can be populated without constructing each `Tuple` by hand.

| Method Name | Parameters | Description |
| --- | --- | --- |
| Add\<T1, T2\> | T1 item1, T2 item2 | Adds `Tuple.Create(item1, item2)` to the list |
| Add\<T1, T2, T3\> | T1 item1, T2 item2, T3 item3 | Adds `Tuple.Create(item1, item2, item3)` to the list |
| Add\<T1, T2, T3, T4\> | T1 item1, T2 item2, T3 item3, T4 item4 | Adds `Tuple.Create(item1, item2, item3, item4)` to the list |
| Add\<T1, T2, T3, T4, T5\> | T1 item1, T2 item2, T3 item3, T4 item4, T5 item5 | Adds `Tuple.Create(item1, item2, item3, item4, item5)` to the list |
| Add\<T1, T2, T3, T4, T5, T6\> | T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6 | Adds `Tuple.Create(item1, item2, item3, item4, item5, item6)` to the list |

Examples:

```csharp
var pairs = new List<Tuple<int, int>>();
pairs.Add(1, 2);
pairs.Count;                                  // 1
pairs[0];                                     // Tuple(1, 2)

var triples = new List<Tuple<int, int, int>>();
triples.Add(1, 2, 3);
triples.Count;                                // 1
```

## TupleList\<T1, T2\> .. TupleList\<T1, T2, T3, T4, T5, T6\>

A family of classes (arities 2 through 6) that each derive from `List<Tuple<...>>` and add their
own `Add(item1, item2, ...)` method. Because the method is a real instance method (rather than an
extension), it enables the collection-initializer syntax (`{ item1, item2 }`) for building a list
of tuples inline - the scenario the class exists for (see the class remarks and the referenced
Stack Overflow question on tuple collection initializers).

| Method Name | Parameters | Description |
| --- | --- | --- |
| Add | T1 item, T2 item2 | (`TupleList<T1, T2>`) Adds a new `Tuple<T1, T2>` built from the arguments |
| Add | T1 item, T2 item2, T3 item3 | (`TupleList<T1, T2, T3>`) Adds a new `Tuple<T1, T2, T3>` built from the arguments |
| Add | T1 item, T2 item2, T3 item3, T4 item4 | (`TupleList<T1, T2, T3, T4>`) Adds a new `Tuple<T1, T2, T3, T4>` built from the arguments |
| Add | T1 item, T2 item2, T3 item3, T4 item4, T5 item5 | (`TupleList<T1, T2, T3, T4, T5>`) Adds a new `Tuple<T1, T2, T3, T4, T5>` built from the arguments |
| Add | T1 item, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6 | (`TupleList<T1, T2, T3, T4, T5, T6>`) Adds a new `Tuple<T1, T2, T3, T4, T5, T6>` built from the arguments |

Example:

```csharp
var list = new TupleList<int, int>
{
    { 1, 2 }
};

list.Add(1, 2);

list.Count;                                   // 2
```

## ItemComparison\<T1, T2\> and ItemComparisonList

A small pairwise-matching utility: given a source list and a target list plus a function that
decides whether a source item and a target item "match", it produces one `ItemComparison<T1, T2>`
per source item (paired with its matching target, or null if none), plus one extra entry for every
target item that had no matching source - so every input item from both lists is represented
exactly once.

`ItemComparison<T1, T2>` properties:

| Property | Type | Description |
| --- | --- | --- |
| Source | T1 | The source item, or null if this entry represents a target-only match |
| Target | T2 | The target item, or null if this entry represents a source-only match |
| Matched | bool | `true` if both `Source` and `Target` are non-null |
| SourceOnly | bool | `true` if `Source` is non-null and `Target` is null |
| TargetOnly | bool | `true` if `Source` is null and `Target` is non-null |

| Static Method | Parameters | Description |
| --- | --- | --- |
| Create | T1 source, T2 target | Creates an `ItemComparison<T1, T2>` (the constructor is private; this is the only way to build one) |

`ItemComparisonList` static methods:

| Static Method | Parameters | Description |
| --- | --- | --- |
| Create\<T1, T2\> | `IList<T1>` sourceList, `IList<T2>` targetList, `Func<T1, T2, bool>` matchFunc | Builds the full `IList<ItemComparison<T1, T2>>` described above, using `matchFunc` to decide matches |
| Create\<T\> | `IList<T>` sourceList, `IList<T>` targetList, `Func<T, T, bool>` matchFunc | Same as above, for the case where both lists share the same item type |

Example:

```csharp
var sourceList = new[]
{
    new MyTestClass1 { Id = 1, Description = "One" },
    new MyTestClass1 { Id = 2, Description = "Two" },
    new MyTestClass1 { Id = 3, Description = "Three" },
    new MyTestClass1 { Id = 4, Description = "Four" },
};

var targetList = new[]
{
    new MyTestClass2 { Id = 2, Name = "Dave" },
    new MyTestClass2 { Id = 4, Name = "Tom" },
};

var comparisons = ItemComparisonList.Create(sourceList, targetList, (a, b) => a.Id == b.Id);

comparisons.Count(c => c.Matched);            // 2 (Id 2 and Id 4)
comparisons.Count(c => c.SourceOnly);         // 2 (Id 1 and Id 3 have no match in targetList)
comparisons.Count(c => c.TargetOnly);         // 0
```

## MergeTechnique enum

Controls how key clashes are resolved when merging dictionaries. Defined in this namespace but
used by `NameValueExtensions.ToDictionary` above, and primarily consumed by
`DictionaryExtensions.MergeWith` / `MergeAll` in the `DNX.Extensions.Dictionaries` namespace
(documented separately).

| Value | Description |
| --- | --- |
| Unique | All keys must be unique across the inputs being merged; a clash throws |
| TakeFirst | When keys clash, the first-found value for that key wins |
| TakeLast | When keys clash, the last-found value for that key wins |
