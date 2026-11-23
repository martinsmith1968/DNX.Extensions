# DNX.Extensions.Conversion

Converts strings, numeric types and Guids between each other. The generated classes in this
namespace live under the source folder `Conversion\BuiltInTypes`, but declare themselves in
namespace `DNX.Extensions.Converters.BuiltInTypes` (note `Converters`, not `Conversion`) - this
folder/namespace mismatch is intentional and has been left as-is. The two hand-written classes
(`BigIntegerExtensions` and `GuidExtensions`) use the namespace the folder name suggests,
`DNX.Extensions.Conversion`.

## Convert{Type}Extensions classes (generated)

`ConvertExtensions.cs.template` is processed by `BuildAll.ps1` (using the type list in
`BuildData.ps1`) to produce one `Convert{Type}Extensions` static class per built-in type - 17 in
total: Bool, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Float, Double, Decimal,
DateTime, DateTimeOffset, TimeSpan, Guid, BigInteger. Each generated class (e.g.
`ConvertInt32Extensions`) exposes the same three string-extension methods for its type:

| Method Name | Parameters | Description |
| --- | --- | --- |
| To{Type} | text | Parses `text` to `{Type}`; throws `ConversionException` if it cannot be parsed |
| To{Type} | text, defaultValue | As above, but returns `defaultValue` instead of throwing if parsing fails |
| Is{Type} | text | Returns `true` if `text` can be parsed to `{Type}`, otherwise `false` |

For every type except `DateTime`/`DateTimeOffset`, parsing is a straight call to `{Type}.TryParse`.
`DateTime`/`DateTimeOffset` are special-cased (behind an `#if` in the template) to use
`DateTime.TryParse(text, CultureInfo.CurrentCulture, flags, out result)` with
`DateTimeStyles.AssumeUniversal`, adding `AdjustToUniversal` when the text ends in `"Z"`.
`Bool` is also notable: because it defers to `bool.TryParse`, only `"True"`/`"False"` (any casing)
are recognised - `"Yes"`, `"No"`, `"1"` and `"0"` are all rejected.

Example (`Int32`, representative of all 17 generated classes):

```csharp
using DNX.Extensions.Converters.BuiltInTypes;

var ok = "100".IsInt32();              // true
var bad = "£100".IsInt32();            // false

var value = "100".ToInt32();           // 100
var fallback = "abcdef".ToInt32(25);   // 25 (parse fails, default is returned instead)

"abcdef".ToInt32();                    // throws ConversionException
```

## BigIntegerExtensions class (hand-written)

Converts between `Guid` and `BigInteger`, treating the Guid's 16-byte representation as the
byte array backing the `BigInteger`.

| Method Name | Parameters | Description |
| --- | --- | --- |
| ToBigInteger | this Guid value | Converts a `Guid` to a `BigInteger` via `new BigInteger(value.ToByteArray())` |
| ToGuid | this BigInteger value | Converts a `BigInteger` back to a `Guid` |

`BigInteger.ToByteArray()` returns the minimal two's-complement representation, which can be
shorter than the 16 bytes a `Guid` needs. `ToGuid` fills the gap with a sign-extension byte
(`0xFF` for negative values, `0x00` otherwise) rather than always zero-filling, so that negative
`BigInteger` values round-trip through `Guid` without corruption.

```csharp
using System.Numerics;
using DNX.Extensions.Conversion;

var guid = Guid.Parse("00000001-0000-0000-0000-000000000000");
var bigInt = guid.ToBigInteger();          // 1

var roundTripped = bigInt.ToGuid();        // 00000001-0000-0000-0000-000000000000

new BigInteger(long.MaxValue).ToGuid();    // ffffffff-ffff-7fff-0000-000000000000
```

## GuidExtensions class (hand-written)

Deterministic string-to-Guid hashing, plus conversion of numeric values to a `Guid`.

| Method Name | Parameters | Description |
| --- | --- | --- |
| KeepOriginalValue | byte value1, byte value2 | Reduce-method that keeps `value1` and discards `value2` - the default used to collapse an oversized hash down to 16 bytes |
| ExclusiveOr | byte value1, byte value2 | Reduce-method that XORs the two bytes together |
| ToDeterministicGuid | this string input | Hashes `input` with MD5 and reduces the hash to 16 bytes using `KeepOriginalValue`, producing a repeatable `Guid` for the same input |
| ToDeterministicGuid | this string input, HashAlgorithm provider | As above, with a caller-supplied hash algorithm |
| ToDeterministicGuid | this string input, HashAlgorithm provider, Func\<byte,byte,byte\> reduceMethod | As above, with a caller-supplied reduce method for collapsing hashes longer than 16 bytes (e.g. SHA256's 32 bytes) |
| ToGuid | this short/int/uint/long/ulong/float/decimal/double value | Converts the numeric value to a `Guid` by routing it through `new BigInteger(value).ToGuid()` |

`ToDeterministicGuid` never parses a Guid-looking string directly as a Guid - it is always hashed,
so the same string always produces the same (different-looking) `Guid`:

```csharp
using DNX.Extensions.Conversion;

var guid1 = "ABC".ToDeterministicGuid();
var guid2 = "ABC".ToDeterministicGuid();
// guid1 == guid2, and guid1.ToString() == "d2bd2f90-dfb1-4f0c-70b4-a5d23525e932"

short.MaxValue.ToGuid();       // 00007fff-0000-0000-0000-000000000000
(123456789.12).ToGuid();       // 075bcd15-0000-0000-0000-000000000000 (double)
```
