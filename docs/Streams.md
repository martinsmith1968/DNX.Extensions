# DNX.Extensions.Streams

Extension methods for reading an entire `Stream` as text, lines or bytes, and for writing text or
bytes to a `Stream` in one call.

## StreamExtensions class

| Method Name | Parameters | Description |
| --- | --- | --- |
| ReadAllText | | Reads the entire stream as a single string (via a `StreamReader`). Returns `null` if `stream` is null |
| ReadAllLines | estimatedCapacity (optional) | Reads the entire stream as an `IList<string>` of lines. `estimatedCapacity`, if supplied, is used as the initial list capacity. Returns `null` if `stream` is null |
| ReadAllBytes | bufferSize (default `4096`) | Reads the entire stream into a `byte[]`, copying it in chunks of `bufferSize`. Returns `null` if `stream` is null |
| WriteAllText | text, encoding (default `Encoding.UTF8`) | Writes `text` to the stream using `encoding` to get its bytes. A no-op if `stream` or `text` is null |
| WriteAllBytes | bytes | Writes `bytes` to the stream and flushes it. A no-op if `stream` or `bytes` is null |

`ReadAllText` and `ReadAllLines` each read via an internal `StreamReader`, which disposes the
underlying stream once it goes out of scope - so the stream is closed when either call returns.
`ReadAllBytes`, `WriteAllText` and `WriteAllBytes` do not dispose the stream, so it remains open
and usable afterwards (e.g. for further writes, or for repositioning and reading back).

Examples:

```csharp
// ReadAllText
var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello, World!"));
var text = stream.ReadAllText(); // "Hello, World!" - and the stream is now disposed

MemoryStream nullStream = null;
var nullText = nullStream.ReadAllText(); // null

// ReadAllLines
var linesStream = new MemoryStream(Encoding.UTF8.GetBytes($"line-one{Environment.NewLine}line-two"));
var lines = linesStream.ReadAllLines();
// lines == [ "line-one", "line-two" ]

// ReadAllBytes
var bytesStream = new MemoryStream(Encoding.UTF8.GetBytes("payload"));
var bytes = bytesStream.ReadAllBytes();
// bytes == Encoding.UTF8.GetBytes("payload"); bytesStream remains open (not disposed)

// WriteAllText (default UTF8 encoding)
using var writeStream = new MemoryStream();
writeStream.WriteAllText("Hello, World!");
// writeStream.ToArray() == Encoding.UTF8.GetBytes("Hello, World!")

// WriteAllText with an explicit encoding
using var unicodeStream = new MemoryStream();
unicodeStream.WriteAllText("Hello, World!", Encoding.Unicode);
// unicodeStream.ToArray() == Encoding.Unicode.GetBytes("Hello, World!")

// WriteAllBytes followed by ReadAllBytes round-trips through the same stream
using var roundTripStream = new MemoryStream();
var payload = Encoding.UTF8.GetBytes("payload");
roundTripStream.WriteAllBytes(payload);
roundTripStream.Position = 0;
var roundTripResult = roundTripStream.ReadAllBytes();
// roundTripResult == payload (the stream was left open by WriteAllBytes, so it can be rewound and re-read)

// null-safety: no exceptions, and the stream/text/bytes are left untouched
MemoryStream missingStream = null;
missingStream.WriteAllText("text");   // no-op
missingStream.WriteAllBytes([1, 2, 3]); // no-op

using var untouchedStream = new MemoryStream();
untouchedStream.WriteAllText(null);   // no-op
untouchedStream.WriteAllBytes(null);  // no-op
// untouchedStream.ToArray() is still empty
```
