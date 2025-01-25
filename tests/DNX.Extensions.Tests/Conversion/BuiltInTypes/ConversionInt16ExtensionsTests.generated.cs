//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

using DNX.Extensions.Converters.BuiltInTypes;
using DNX.Extensions.Exceptions;
using DNX.Extensions.Tests.Conversion.BuiltInTypes.TestsDataSource;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Conversion.BuiltInTypes;

public class ConversionInt16ExtensionsTests
{
    [Theory]
    [MemberData(nameof(ConversionInt16ExtensionsTestsDataSource.IsInt16), MemberType = typeof(ConversionInt16ExtensionsTestsDataSource))]
    public void Test_IsInt16(string text, bool expectedResult)
    {
        var result = text.IsInt16();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ConversionInt16ExtensionsTestsDataSource.ToInt16), MemberType = typeof(ConversionInt16ExtensionsTestsDataSource))]
    public void Test_ToInt16(string text, short expectedResult)
    {
        var result = text.ToInt16();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ConversionInt16ExtensionsTestsDataSource.ToInt16Throws), MemberType = typeof(ConversionInt16ExtensionsTestsDataSource))]
    public void Test_ToInt16_Throws(string text, bool expectedResult)
    {
        try
        {
            text.ToInt16();

            expectedResult.ShouldBeTrue();
        }
        catch (ConversionException ex)
        {
            ex.Value.ShouldBe(text);

            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [MemberData(nameof(ConversionInt16ExtensionsTestsDataSource.ToInt16WithDefault), MemberType = typeof(ConversionInt16ExtensionsTestsDataSource))]
    public void Test_ToInt16_with_default(string text, short defaultValue, short expectedResult)
    {
        var result = text.ToInt16(defaultValue);

        result.ShouldBe(expectedResult);
    }
}

