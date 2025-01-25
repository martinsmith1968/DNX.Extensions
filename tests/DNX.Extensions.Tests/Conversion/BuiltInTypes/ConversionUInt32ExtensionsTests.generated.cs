//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

using DNX.Extensions.Converters.BuiltInTypes;
using DNX.Extensions.Exceptions;
using DNX.Extensions.Tests.Conversion.BuiltInTypes.TestsDataSource;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Conversion.BuiltInTypes;

public class ConversionUInt32ExtensionsTests
{
    [Theory]
    [MemberData(nameof(ConversionUInt32ExtensionsTestsDataSource.IsUInt32), MemberType = typeof(ConversionUInt32ExtensionsTestsDataSource))]
    public void Test_IsUInt32(string text, bool expectedResult)
    {
        var result = text.IsUInt32();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ConversionUInt32ExtensionsTestsDataSource.ToUInt32), MemberType = typeof(ConversionUInt32ExtensionsTestsDataSource))]
    public void Test_ToUInt32(string text, uint expectedResult)
    {
        var result = text.ToUInt32();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ConversionUInt32ExtensionsTestsDataSource.ToUInt32Throws), MemberType = typeof(ConversionUInt32ExtensionsTestsDataSource))]
    public void Test_ToUInt32_Throws(string text, bool expectedResult)
    {
        try
        {
            text.ToUInt32();

            expectedResult.ShouldBeTrue();
        }
        catch (ConversionException ex)
        {
            ex.Value.ShouldBe(text);

            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [MemberData(nameof(ConversionUInt32ExtensionsTestsDataSource.ToUInt32WithDefault), MemberType = typeof(ConversionUInt32ExtensionsTestsDataSource))]
    public void Test_ToUInt32_with_default(string text, uint defaultValue, uint expectedResult)
    {
        var result = text.ToUInt32(defaultValue);

        result.ShouldBe(expectedResult);
    }
}

