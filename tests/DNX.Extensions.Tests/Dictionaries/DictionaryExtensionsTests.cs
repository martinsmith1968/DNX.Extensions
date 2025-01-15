using DNX.Extensions.Dictionaries;
using Shouldly;
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
        value.ShouldBe(expectedValue);
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
        value.ShouldBe(expectedValue);
    }

    [Fact]
    public void Can_split_well_formed_string_into_string_dictionary()
    {
        // Arrange
        var text = "Liverpool=1|Southend United=7";

        // Act
        var result = text.ToStringDictionary();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(Dictionary<string, string>));
        result.Keys.Count.ShouldBe(2);
        result.ContainsKey("Liverpool").ShouldBe(true);
        result["Liverpool"].ShouldBe("1");
        result.ContainsKey("Southend United").ShouldBe(true);
        result["Southend United"].ShouldBe("7");
    }

    [Fact]
    public void Null_string_is_handled_into_string_dictionary()
    {
        // Arrange
        const string text = null;

        // Act
        var result = text.ToStringDictionary();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(Dictionary<string, string>));
        result.Keys.Count.ShouldBe(0);
    }

    [Fact]
    public void Can_split_well_formed_string_into_string_object_dictionary()
    {
        // Arrange
        var text = "Liverpool=1|Southend United=7";

        // Act
        var result = text.ToStringObjectDictionary();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(Dictionary<string, object>));
        result.Keys.Count.ShouldBe(2);
        result.ContainsKey("Liverpool").ShouldBe(true);
        result["Liverpool"].ShouldBe("1");
        result.ContainsKey("Southend United").ShouldBe(true);
        result["Southend United"].ShouldBe("7");
    }
}