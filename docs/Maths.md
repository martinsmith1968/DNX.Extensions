# DNX.Extensions.Maths

Range checks ("is this value between two others?") for every built-in type. `MathsExtensions.cs.template`
is processed by `BuildAll.ps1` (using the type list in `BuildData.ps1`) to produce one
`Maths{Type}Extensions` static class per type - 16 in total: Byte, SByte, Int16, UInt16, Int32,
UInt32, Int64, UInt64, Float, Double, Decimal, DateTime, DateTimeOffset, TimeSpan, Guid,
BigInteger - all in namespace `DNX.Extensions.Maths.BuiltInTypes`.

## Maths{Type}Extensions classes (generated)

Each generated class (e.g. `MathsInt32Extensions`) exposes the same set of methods for its type:

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsBetween | value, min, max | Inclusive check that `value` is between `min` and `max`; `min`/`max` must already be given in the right order |
| IsBetween | value, min, max, boundsType | As above, with explicit control over which bound(s) are inclusive/exclusive |
| IsBetweenEither | value, min, max | Inclusive check between whichever of `min`/`max` is lower and whichever is higher, regardless of the order they're passed in |
| IsBetweenEither | value, min, max, boundsType | As above, with explicit bounds control |
| IsBetween | value, min, max, allowEitherOrder, boundsType | Full control - combines the "either order" and bounds-type behaviour of the methods above |
| GetLowerBound | min, max | Returns `min` as-is (no reordering) |
| GetLowerBound | min, max, allowEitherOrder | Returns the smaller of `min`/`max` when `allowEitherOrder` is `true`, otherwise returns `min` |
| GetUpperBound | min, max | Returns `max` as-is |
| GetUpperBound | min, max, allowEitherOrder | Returns the larger of `min`/`max` when `allowEitherOrder` is `true`, otherwise returns `max` |

Example (`Int32`, representative of all 16 generated classes):

```csharp
using DNX.Extensions.Maths;
using DNX.Extensions.Maths.BuiltInTypes;

5.IsBetween(1, 10);                                 // true  (inclusive by default)
10.IsBetween(1, 10, IsBetweenBoundsType.Exclusive);  // false (upper bound excluded)

5.IsBetween(10, 1);          // false - min/max given in the wrong order
5.IsBetweenEither(10, 1);    // true  - order doesn't matter

MathsInt32Extensions.GetLowerBound(10, 1);         // 10 - returned as-is, no reordering
MathsInt32Extensions.GetLowerBound(10, 1, true);   // 1  - allowEitherOrder sorts it
```

## IsBetweenBoundsType enum

Controls which end(s) of the range are inclusive. Defined once in `DNX.Extensions.Maths`
(`IsBetweenBoundsType.cs`, not generated) and shared by every `Maths{Type}Extensions` class and by
the Validation namespace's `Guard.IsBetween` family.

| Value | Alias | Comparison |
| --- | --- | --- |
| IncludeLowerAndUpper | Inclusive | `value >= lower && value <= upper` |
| ExcludeLowerAndUpper | Exclusive | `value > lower && value < upper` |
| IncludeLowerExcludeUpper | GreaterThanOrEqualToLowerLessThanUpper | `value >= lower && value < upper` |
| ExcludeLowerIncludeUpper | GreaterThanLowerLessThanOrEqualToUpper | `value > lower && value <= upper` |

The same file also defines `IsBetweenTypeExtensions.GetLimitDescriptionFormat()`, an extension on
the enum that returns a human-readable format string for the chosen bounds type (e.g.
`"between {0} and {1}"` for `Inclusive`) - this is what the Validation namespace's `Guard.IsBetween`
uses to build its exception messages.

`Guid` has no native ordering, so its `IsBetween`/`GetLowerBound`/`GetUpperBound` implementations
round-trip each value through `BigInteger` (`value.ToBigInteger() >= lowerBound.ToBigInteger()`,
etc., behind an `#if MathsExtensionsGuid` in the template) to compare them; `BigInteger` itself and
all the other numeric/date/time types compare natively with their own `<`/`>`/`<=`/`>=` operators.
