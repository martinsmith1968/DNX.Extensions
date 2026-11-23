# DNX.Extensions.Arrays

Extension methods for working with `Array` instances in general, and `byte[]` in particular.

## ArrayExtensions

Extend any `Array` (and, for the generic methods, specifically `T[]`).

| Method Name | Parameters | Description |
| --- | --- | --- |
| IsNullOrEmpty | | `true` if the array is null or has zero elements; otherwise `false` |
| PadLeft\<T\> | length | Returns a new `T[]` of size `length`, padding with `default(T)` on the left if `length` is greater than the input length, or keeping only the right-most `length` elements if `length` is smaller. Throws `ArgumentNullException` (`input`) if the array is null |
| ShiftLeft\<T\> | by (default 1), fillValue (default `default(T)`) | Returns a new array the same size as the input, with every element moved `by` positions to the left; the vacated slots on the right are filled with `fillValue`. Returns an empty array (not null) if the input is null |
| ShiftRight\<T\> | by (default 1), fillValue (default `default(T)`) | Returns a new array the same size as the input, with every element moved `by` positions to the right; the vacated slots on the left are filled with `fillValue`. Returns an empty array (not null) if the input is null |
| Reduce\<T\> | targetSize, `Func<T, T, T>` method | If the array already has `targetSize` or fewer elements, returns it unchanged. Otherwise, wraps the array around a buffer of `targetSize` slots: the first `targetSize` elements seed the buffer, and every later element at index `i` is combined into slot `i % targetSize` using `method(bufferValue, nextValue)`. Throws `ArgumentNullException` (`inputArray`) if the array is null |

Examples:

```csharp
// PadLeft
var values = new[] { "1", "2", "3", "4", "5", "6" };
var padded = values.PadLeft(8);     // [null, null, "1", "2", "3", "4", "5", "6"]
var trimmed = values.PadLeft(4);    // ["3", "4", "5", "6"]

// ShiftLeft / ShiftRight
var left = values.ShiftLeft(by: 3);              // ["4", "5", "6", null, null, null]
var right = values.ShiftRight(by: 2, fillValue: "0"); // ["0", "0", "1", "2", "3", "4"]

// Reduce - XOr's each wrapped-around value into its target slot
var bytes = new byte[] { 1, 2, 3, 4, 5, 6 };
byte XOr(byte a, byte b) => (byte)(a ^ b);

var reduced = bytes.Reduce(3, XOr);
// reduced[0] = 1 ^ 4 = 5
// reduced[1] = 2 ^ 5 = 7
// reduced[2] = 3 ^ 6 = 5
// reduced = [5, 7, 5]
```

## ByteArrayExtensions

Extend `byte[]`.

| Method Name | Parameters | Description |
| --- | --- | --- |
| GetAsciiString | | Converts the byte array into an ASCII string via `Encoding.ASCII.GetString`. Returns `null` if the array is null |
| ToHexString | format (default `"X2"`) | Converts the byte array into a hex string, formatting each byte with the supplied numeric format string. Returns `null` if the array is null |

Examples:

```csharp
byte[] bytes = [65, 66, 67, 68, 69, 70];

var ascii = bytes.GetAsciiString();   // "ABCDEF"

byte[] numbers = [1, 2, 3, 4, 5, 6];

var hex = numbers.ToHexString();      // "010203040506"
```
