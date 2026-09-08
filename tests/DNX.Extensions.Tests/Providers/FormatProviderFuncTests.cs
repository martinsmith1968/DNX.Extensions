using System;
using DNX.Extensions.Providers;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Providers;

public class FormatProviderFuncTests
{
    [Fact]
    public void Create_returns_format_provider()
    {
        var provider = FormatProviderFunc.Create(_ => null);

        provider.ShouldBeOfType<FormatProviderFunc>();
    }

    [Fact]
    public void GetFormat_passes_requested_type_to_function_and_returns_result()
    {
        var requestedType = typeof(DateTime);
        var expected = new object();
        var provider = FormatProviderFunc.Create(type =>
        {
            type.ShouldBe(requestedType);
            return expected;
        });

        provider.GetFormat(requestedType).ShouldBe(expected);
    }

    [Fact]
    public void GetFormat_returns_null_when_function_returns_null()
    {
        var provider = FormatProviderFunc.Create(_ => null);

        provider.GetFormat(typeof(string)).ShouldBeNull();
    }

    [Fact]
    public void GetFormat_propagates_exception_from_function()
    {
        var expected = new InvalidOperationException("format unavailable");
        var provider = FormatProviderFunc.Create(_ => throw expected);

        var actual = Should.Throw<InvalidOperationException>(
            () => provider.GetFormat(typeof(string)));

        actual.ShouldBeSameAs(expected);
    }
}
