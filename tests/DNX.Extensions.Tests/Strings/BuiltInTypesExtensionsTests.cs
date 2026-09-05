using DNX.Extensions.Strings;
using Shouldly;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace DNX.Extensions.Tests.Strings;

public class BuiltInTypesExtensionsTests
{
    [Theory]
    [InlineData(true, "True")]
    [InlineData(false, "False")]
    public void Test_ToText(bool value, string expectedResult)
    {
        var result = value.ToText();

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(true, "Way", "No Way", "Way")]
    [InlineData(false, "Way", "No Way", "No Way")]
    public void Test_ToText_custom_text(bool value, string trueText, string falseText, string expectedResult)
    {
        var result = value.ToText(trueText, falseText);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(true, "Yes")]
    [InlineData(false, "No")]
    public void Test_ToYesNo(bool value, string expectedResult)
    {
        var result = value.ToYesNo();

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(255, "FF")]
    [InlineData(1, "01")]
    [InlineData(65535, "FFFF")]
    public void Test_ToHexString_Number(int number, string expectedHexString)
    {
        var result = number.ToHexString();

        result.ShouldBe(expectedHexString);
    }

    [Theory]
    [InlineData(255, "X4", "00FF")]
    [InlineData(1, "X4", "0001")]
    [InlineData(65535, "X4", "FFFF")]
    public void Test_ToHexString_Number_with_format(int number, string format, string expectedHexString)
    {
        var result = number.ToHexString(format);

        result.ShouldBe(expectedHexString);
    }
}
