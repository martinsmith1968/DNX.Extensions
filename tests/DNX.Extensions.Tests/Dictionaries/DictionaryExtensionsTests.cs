using DNX.Extensions.Dictionaries;
using FluentAssertions;
using Xunit;

namespace DNX.Extensions.Tests.Dictionaries;

public class DictionaryExtensionsTests
{
    [Theory]
    [InlineData("Sunday", 0)]
    [InlineData("Monday", 1)]
    [InlineData("Thursday", 4)]
    [InlineData("Saturday", 6)]
    [InlineData("NotADay", 0)]
    public void Can_get_dictionary_value_safely_or_default(string key, int expectedValue)
    {
        // Arrange
        var dict = Enum.GetNames(typeof(DayOfWeek))
            .ToDictionary(
                x => x,
                x => (int)(Enum.Parse<DayOfWeek>(x))
            );

        // Act
        var value = dict.Get(key);

        // Assert
        value.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData("Sunday", 999, 0)]
    [InlineData("Monday", 999, 1)]
    [InlineData("NotADay", 999, 999)]
    [InlineData("NotADay", 0, 0)]
    public void Can_get_dictionary_value_safely_overriding_default(string key, int defaultValue, int expectedValue)
    {
        // Arrange
        var dict = Enum.GetNames(typeof(DayOfWeek))
            .ToDictionary(
                x => x,
                x => (int)(Enum.Parse<DayOfWeek>(x))
            );

        // Act
        var value = dict.Get(key, defaultValue);

        // Assert
        value.Should().Be(expectedValue);
    }

    [Fact]
    public void Can_split_well_formed_string_into_string_dictionary()
    {
        // Arrange
        var text = "Liverpool=1|Southend United=7";

        // Act
        var result = text.ToStringDictionary();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Dictionary<string, string>));
        result.Keys.Count.Should().Be(2);
        result.ContainsKey("Liverpool").Should().Be(true);
        result["Liverpool"].Should().Be("1");
        result.ContainsKey("Southend United").Should().Be(true);
        result["Southend United"].Should().Be("7");
    }

    [Fact]
    public void Null_string_is_handled_into_string_dictionary()
    {
        // Arrange
        const string text = null;

        // Act
        var result = text.ToStringDictionary();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Dictionary<string, string>));
        result.Keys.Count.Should().Be(0);
    }

    [Fact]
    public void Can_split_well_formed_string_into_string_object_dictionary()
    {
        // Arrange
        var text = "Liverpool=1|Southend United=7";

        // Act
        var result = text.ToStringObjectDictionary();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Dictionary<string, object>));
        result.Keys.Count.Should().Be(2);
        result.ContainsKey("Liverpool").Should().Be(true);
        result["Liverpool"].Should().Be("1");
        result.ContainsKey("Southend United").Should().Be(true);
        result["Southend United"].Should().Be("7");
    }
}