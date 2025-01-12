using Xunit;
using DNX.Extensions.Conversion;
using DNX.Extensions.Exceptions;
using FluentAssertions;

namespace DNX.Extensions.Tests.Conversion;

public class ConvertInt32ExtensionsTests
{
    [Theory]
    [InlineData("160", 160)]
    [InlineData("0", 0)]
    [InlineData("-1", -1)]
    [InlineData("2147483647", 2147483647)]
    public void ToInt32_without_override_can_convert_successfully(string text, int expected)
    {
        var result = text.ToInt32();

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("2147483648")]
    [InlineData("NotAnInt32")]
    public void ToInt32_for_invalid_value_without_override_fails_as_expected(string text)
    {
        Action action = () => text.ToInt32();

        // Act / Assert
        var q = action.Should()
                .Throw<ConversionException>()
                .Where(e => e.Value == text)
                .Where(e => e.ConvertType == typeof(int))
            ;
    }

    [Theory]
    [InlineData("160", 42, 160)]
    [InlineData("0", 57, 0)]
    [InlineData("-1", 5, -1)]
    [InlineData("2147483647", 12345, 2147483647)]
    [InlineData("2147483648", 12345, 12345)]
    [InlineData("NotAnInt32", 0, 0)]
    [InlineData("NotAnInt32", 222, 222)]
    public void ToInt32_with_override_can_convert_successfully(string text, int defaultValue, int expected)
    {
        var result = text.ToInt32(defaultValue);

        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Bob", false)]
    [InlineData("12345", true)]
    [InlineData("12,345", false)]
    public void IsInt32(string text, bool expectedResult)
    {
        // Act
        var result = text.IsInt32();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Bob", false)]
    [InlineData("12345", true)]
    [InlineData("12,345", false)]
    public void IsInt32_with_output_value(string text, bool expectedResult)
    {
        // Act
        var result = text.IsInt32(out var value);

        // Assert
        result.Should().Be(expectedResult);
        if (result)
            value.Should().Be(text.ToInt32());
    }
}
