# DNX.Extensions.Validation

Guard-clause helpers for validating method arguments and throwing the appropriate `Argument*`
exception. The defining idea of this namespace is that every check takes an
`Expression<Func<T>>` (e.g. `() => myParameter`) rather than a plain value: `Guard` compiles and
invokes the expression to get the value, then inspects the expression tree itself
(`ExpressionExtensions.GetMemberName`) to recover the variable/parameter's *name*, and uses that
name as the thrown exception's `ParamName`. This means callers never have to pass `nameof(x)`
(or a string that can drift out of sync with the real parameter) just to get a useful exception.

Most methods come in two overloads: `(exp)`, which compiles and invokes `exp` to get the value,
and `(exp, val)`, which takes an already-computed value instead - for use, per the source
comments, "if you are not happy that the expression `exp` will be invoked more than once by your
method" (compiling and invoking a lambda on every call is not free).

## Guard class - core methods (`Guard.cs`, hand-written)

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsTrue | exp | Throws `ArgumentOutOfRangeException` unless `exp` evaluates to `true` |
| IsTrue | exp, val | As above, using a precomputed `bool` instead of re-invoking `exp` |
| IsFalse | exp [, val] | Mirror of `IsTrue` - throws unless the value is `false` |
| IsNotNull\<T\> | exp [, val] | Throws `ArgumentNullException` if the reference-type value is `null` |
| IsNotNullOrEmpty | exp [, val] (string) | Throws `ArgumentException` if the string is `null` or empty |
| IsNotNullOrEmpty\<T\> | exp [, val] (IEnumerable\<T\>) | Throws `ArgumentException` if the sequence is `null` or has no elements |
| IsNotNullOrWhitespace | exp [, val] (string) | Throws `ArgumentException` if the string is `null`, empty, or all whitespace |

```csharp
using DNX.Extensions.Validation.BuiltInTypes;

void Process(object value)
{
    Guard.IsNotNull(() => value);
    // ...
}

Process(null);
// throws ArgumentNullException
//   ParamName: "value"
//   Message contains "value must not be null"
```

```csharp
var score = 7;

Guard.IsGreaterThan(() => score, 10);
// throws ArgumentOutOfRangeException
//   ParamName:   "score"
//   ActualValue: 7
//   Message contains "score must be greater than 10"
```

## Guard class - enum methods (`GuardEnums.cs`, hand-written)

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsValidEnum\<T\> | exp [, val] | Throws `ArgumentException` if the value is not a defined member of enum `T` |
| IsEnumOneOf\<T\> | exp, params T[] allowed | Throws `ArgumentException` if the value is not one of `allowed` |
| IsEnumOneOf\<T\> | exp, IList\<T\> allowed | As above, with `allowed` supplied as a list |
| IsEnumOneOf\<T\> | exp, val, allowed | As above, using a precomputed value |

```csharp
private enum Status { Pending, Active, Disabled }

var status = Status.Active;

Guard.IsValidEnum(() => status);                  // passes
Guard.IsValidEnum(() => (Status)99);              // throws ArgumentException - 99 is not a member

Guard.IsEnumOneOf(() => status, Status.Pending, Status.Active);                     // passes
Guard.IsEnumOneOf(() => Status.Disabled, new[] { Status.Pending, Status.Active });  // throws ArgumentException
```

## Guard{Type} methods (generated, `GuardBuiltInTypes.cs.template`)

`GuardBuiltInTypes.cs.template` is processed by `BuildAll.ps1` (using the type list in
`BuildData.ps1`) to produce one file per type - 16 in total: Byte, SByte, Int16, UInt16, Int32,
UInt32, Int64, UInt64, Float, Double, Decimal, DateTime, DateTimeOffset, TimeSpan, Guid,
BigInteger. Unlike the generated classes in Conversion and Maths, each generated file does not
declare its own class - it declares `public static partial class Guard`, so all 16 generated
files (`GuardByteExtensions.generated.cs`, `GuardInt32Extensions.generated.cs`, etc.) contribute
methods directly to the single `DNX.Extensions.Validation.BuiltInTypes.Guard` class documented
above.

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsGreaterThan | exp, min [, val] | Throws `ArgumentOutOfRangeException` unless the value is greater than `min` |
| IsGreaterThanOrEqualTo | exp, min [, val] | ... unless the value is greater than or equal to `min` |
| IsLessThan | exp, max [, val] | ... unless the value is less than `max` |
| IsLessThanOrEqualTo | exp, max [, val] | ... unless the value is less than or equal to `max` |
| IsBetween | exp, min, max | Inclusive bounds check (same semantics as `Maths{Type}Extensions.IsBetween`) |
| IsBetween | exp, min, max, boundsType | As above, with explicit `IsBetweenBoundsType` control |
| IsBetween | exp, bound1, bound2, allowEitherOrder, boundsType | Full control - bounds may be given in either order |
| IsBetween | exp, val, bound1, bound2, allowEitherOrder, boundsType | As above, using a precomputed value |

```csharp
using DNX.Extensions.Maths;
using DNX.Extensions.Validation.BuiltInTypes;

var score = 7;

Guard.IsGreaterThan(() => score, 5);                                       // passes (7 > 5)
Guard.IsBetween(() => score, 5, 10);                                       // passes (inclusive)
Guard.IsBetween(() => score, 10, 5, true, IsBetweenBoundsType.Inclusive);  // passes - reversed bounds allowed because allowEitherOrder is true

Guard.IsGreaterThan(() => score, 10);
// throws ArgumentOutOfRangeException - 7 is not greater than 10
```

As in Maths, `Guid` has no native ordering, so these comparisons round-trip through `BigInteger`
(`val.ToBigInteger() > min.ToBigInteger()`, etc., behind an `#if GuardBuiltInExtensionsGuid` in
the template); every other type compares natively with its own operators.
