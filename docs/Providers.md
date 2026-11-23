# DNX.Extensions.Providers

Some .NET APIs require an `IFormatProvider` instance to be passed in. Constructing an entire
class for this is often cumbersome and relocates simple logic away from where it's used. This
follows the same pattern as `ComparerFunc<T>`/`EqualityComparerFunc<T>` in the
`DNX.Extensions.Comparers` namespace - build a valid instance of the interface inline from a plain
delegate.

## FormatProviderFunc class

Builds a valid `IFormatProvider` from a `Func<Type, object>` - the same signature as
`IFormatProvider.GetFormat(Type)`. The constructor is private; instances are only created via the
`Create` factory method.

| Static Method | Parameters | Description |
| --- | --- | --- |
| Create | `Func<Type, object>` func | Creates an `IFormatProvider` whose `GetFormat(Type formatType)` delegates to `func(formatType)` |

`GetFormat` returns whatever `func` returns, including `null`, and any exception `func` throws
propagates unchanged out of `GetFormat`.

Example:

```csharp
var expected = new object();

var provider = FormatProviderFunc.Create(formatType =>
{
    // formatType is whatever type the caller of GetFormat asked for, e.g. typeof(DateTime)
    return expected;
});

provider.GetFormat(typeof(DateTime));  // expected

var nullProvider = FormatProviderFunc.Create(_ => null);

nullProvider.GetFormat(typeof(string));  // null
```
