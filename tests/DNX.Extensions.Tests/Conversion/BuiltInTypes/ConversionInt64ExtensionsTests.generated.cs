//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

using DNX.Extensions.Converters.BuiltInTypes;
using DNX.Extensions.Exceptions;
using DNX.Extensions.Tests.Conversion.BuiltInTypes.TestsDataSource;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Conversion.BuiltInTypes;

public class ConversionInt64ExtensionsTests
{
    [Theory]
    [MemberData(nameof(ConversionInt64ExtensionsTestsDataSource.IsInt64), MemberType = typeof(ConversionInt64ExtensionsTestsDataSource))]
    public void Test_IsInt64(string text, bool expectedResult)
    {
        var result = text.IsInt64();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ConversionInt64ExtensionsTestsDataSource.ToInt64), MemberType = typeof(ConversionInt64ExtensionsTestsDataSource))]
    public void Test_ToInt64(string text, long expectedResult)
    {
        var result = text.ToInt64();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ConversionInt64ExtensionsTestsDataSource.ToInt64Throws), MemberType = typeof(ConversionInt64ExtensionsTestsDataSource))]
    public void Test_ToInt64_Throws(string text, bool expectedResult)
    {
        try
        {
            text.ToInt64();

            expectedResult.ShouldBeTrue();
        }
        catch (ConversionException ex)
        {
            ex.Value.ShouldBe(text);

            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [MemberData(nameof(ConversionInt64ExtensionsTestsDataSource.ToInt64WithDefault), MemberType = typeof(ConversionInt64ExtensionsTestsDataSource))]
    public void Test_ToInt64_with_default(string text, long defaultValue, long expectedResult)
    {
        var result = text.ToInt64(defaultValue);

        result.ShouldBe(expectedResult);
    }
}

