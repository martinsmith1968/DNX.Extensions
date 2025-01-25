//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

using DNX.Extensions.Converters.BuiltInTypes;
using DNX.Extensions.Exceptions;
using DNX.Extensions.Tests.Conversion.BuiltInTypes.TestsDataSource;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Conversion.BuiltInTypes;

public class ConversionUInt64ExtensionsTests
{
    [Theory]
    [MemberData(nameof(ConversionUInt64ExtensionsTestsDataSource.IsUInt64), MemberType = typeof(ConversionUInt64ExtensionsTestsDataSource))]
    public void Test_IsUInt64(string text, bool expectedResult)
    {
        var result = text.IsUInt64();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ConversionUInt64ExtensionsTestsDataSource.ToUInt64), MemberType = typeof(ConversionUInt64ExtensionsTestsDataSource))]
    public void Test_ToUInt64(string text, ulong expectedResult)
    {
        var result = text.ToUInt64();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ConversionUInt64ExtensionsTestsDataSource.ToUInt64Throws), MemberType = typeof(ConversionUInt64ExtensionsTestsDataSource))]
    public void Test_ToUInt64_Throws(string text, bool expectedResult)
    {
        try
        {
            text.ToUInt64();

            expectedResult.ShouldBeTrue();
        }
        catch (ConversionException ex)
        {
            ex.Value.ShouldBe(text);

            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [MemberData(nameof(ConversionUInt64ExtensionsTestsDataSource.ToUInt64WithDefault), MemberType = typeof(ConversionUInt64ExtensionsTestsDataSource))]
    public void Test_ToUInt64_with_default(string text, ulong defaultValue, ulong expectedResult)
    {
        var result = text.ToUInt64(defaultValue);

        result.ShouldBe(expectedResult);
    }
}

