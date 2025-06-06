//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

using DNX.Extensions.Converters.BuiltInTypes;
using DNX.Extensions.Exceptions;
using DNX.Extensions.Tests.Conversion.BuiltInTypes.TestsDataSource;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Conversion.BuiltInTypes;

public class ConversionFloatExtensionsTests
{
    [Theory]
    [MemberData(nameof(ConversionFloatExtensionsTestsDataSource.IsFloat), MemberType = typeof(ConversionFloatExtensionsTestsDataSource))]
    public void Test_IsFloat(string text, bool expectedResult)
    {
        var result = text.IsFloat();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ConversionFloatExtensionsTestsDataSource.ToFloat), MemberType = typeof(ConversionFloatExtensionsTestsDataSource))]
    public void Test_ToFloat(string text, float expectedResult)
    {
        var result = text.ToFloat();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ConversionFloatExtensionsTestsDataSource.ToFloatThrows), MemberType = typeof(ConversionFloatExtensionsTestsDataSource))]
    public void Test_ToFloat_Throws(string text, bool expectedResult)
    {
        try
        {
            text.ToFloat();

            expectedResult.ShouldBeTrue();
        }
        catch (ConversionException ex)
        {
            ex.Value.ShouldBe(text);

            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [MemberData(nameof(ConversionFloatExtensionsTestsDataSource.ToFloatWithDefault), MemberType = typeof(ConversionFloatExtensionsTestsDataSource))]
    public void Test_ToFloat_with_default(string text, float defaultValue, float expectedResult)
    {
        var result = text.ToFloat(defaultValue);

        result.ShouldBe(expectedResult);
    }
}

