# DNX.Extensions.Comparers

Some .NET APIs require an `IComparer<T>`/`IEqualityComparer<T>`/`IFormatProvider` instance to be
passed in to provide a result. Constructing an entire class for this is often cumbersome and
relocates simple logic away from where it's used. The classes in this namespace let you build a
valid instance of each of these inline from a plain delegate.

## ComparerFunc\<T\> class

Builds a valid `IComparer<T>` from a `Func<T, T, int>` - the same signature as `IComparer<T>.Compare`.

| Static Method | Parameters | Description |
| --- | --- | --- |
| Create | `Func<T, T, int>` | Creates an `IComparer<T>` that delegates to the supplied function |

Example:

```csharp
var list = new[] { 1, 2, 3, 4, 5 };

Func<int, int, int> evensFirst = (x, y) =>
{
    if (x == y) return 0;

    if (x % 2 == y % 2)
    {
        return x < y ? -1 : 1;
    }

    return (x % 2 == 0) ? -1 : 1;
};

var sortedList = list.OrderBy(x => x, ComparerFunc<int>.Create(evensFirst)).ToList();

// sortedList = [2, 4, 1, 3, 5]
```

## EqualityComparerFunc\<T\> class

Builds a valid `IEqualityComparer<T>` from a `Func<T, T, bool>` equality function.

| Static Method | Parameters | Description |
| --- | --- | --- |
| Create | `Func<T, T, bool>` equals | Creates an `IEqualityComparer<T>` whose `GetHashCode` falls back to `EqualityComparer<T>.Default` |
| Create | `Func<T, T, bool>` equals, `Func<T, int>` hash | As above, but with an explicit hash function |

`GetHashCode` must agree with `Equals`, or hash-based collections and LINQ methods (`Dictionary<TKey,TValue>`,
`HashSet<T>`, `Union`, `Distinct`, etc.) will silently misbehave - pass an explicit `hash` function
whenever equality doesn't match the default hash for `T` (as in the example below, where two values
that differ only by sign must hash identically to be found in the same bucket).

Example:

```csharp
Func<int, int, bool> absoluteEquals = (x, y) => Math.Abs(x) == Math.Abs(y);
Func<int, int> absoluteHash = x => Math.Abs(x).GetHashCode();

var comparer = EqualityComparerFunc<int>.Create(absoluteEquals, absoluteHash);

var list = new[] { 1, -2, 3, -4, 5 };

var r1 = list.Contains(1, comparer); // true
var r2 = list.Contains(2, comparer); // true (matches -2)
var r3 = list.Contains(6, comparer); // false
```

## StringComparisonEqualityComparer class

An `IEqualityComparer<string>` built from a `StringComparison` value, for APIs that want a
comparer instance rather than a comparison mode.

```csharp
var comparer = new StringComparisonEqualityComparer(StringComparison.OrdinalIgnoreCase);

var set = new HashSet<string>(new[] { "a", "A", "b" }, comparer);
// set.Count == 2
```

## FormatProviderFunc class

(Namespace: `DNX.Extensions.Providers`) Builds a valid `IFormatProvider` from a `Func<Type, object>`,
matching `IFormatProvider.GetFormat`.

| Static Method | Parameters | Description |
| --- | --- | --- |
| Create | `Func<Type, object>` | Creates an `IFormatProvider` that delegates to the supplied function |
