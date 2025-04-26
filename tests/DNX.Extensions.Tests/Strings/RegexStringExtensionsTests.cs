using System.Text.RegularExpressions;
using DNX.Extensions.Strings;
using Shouldly;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace DNX.Extensions.Tests.Strings;

public class RegexStringExtensionsTests
{
    [Theory]
    [InlineData("ABC:123", "[A-Z]+\\:[0-9]+", true)]
    [InlineData("ABC", "[0-9]+", false)]
    public void IsMatch_can_match_text_and_patterns_successfully(string text, string pattern, bool expectedResult)
    {
        // Act
        var result = text.IsMatch(pattern);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("Qwerty:123", "([^:]+):(.*)", "1", "2", "Qwerty=123")]
    [InlineData("UseFlag=Yes", "(?<name>[^=]+)=(?<value>.*)", "name", "value", "UseFlag=Yes")]
    [InlineData("Notparseable", "([^=]+):(.*)", "1", "2", "=")]
    public void ParseToKeyValuePair_can_parse_the_input_successfully(string input, string regex, string keyGroupName, string valueGroupName, string expectedResult)
    {
        // Act
        var kvp = input.ParseToKeyValuePair(regex, keyGroupName, valueGroupName);
        var result = $"{kvp.Key}={kvp.Value}";

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("Qwerty:123;Blah:456;Pelham:123", "([^:]+):(.*)", "1", "2", "Blah=456;Pelham=123;Qwerty=123")]
    [InlineData("UseFlag=Yes;Compile=True;ErrorLevel=4", "(?<name>[^=]+)=(?<value>.*)", "name", "value", "Compile=True;ErrorLevel=4;UseFlag=Yes")]
    [InlineData("Notparseable", "([^=]+):(.*)", "1", "2", "")]
    public void ParseToDictionary_can_parse_the_input_successfully(string input, string regex, string keyGroupName, string valueGroupName, string expectedResult)
    {
        var list = input.Split(";")
            .ToList();

        // Act
        var dictionary = list.ParseToDictionary(regex, keyGroupName, valueGroupName);
        var result = string.Join(";",
            dictionary
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => $"{kvp.Key}={kvp.Value}")
        );

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Fact]
    public void ParseToDictionaryList_can_parse_named_groups_with_a_single_match()
    {
        const string fieldNameRegex = @"(?<FieldName>[A-Za-z0-9]+)[\[]*(?<IndexerName>[A-Za-z0-9]*)[\]]*";

        const string input = "CustomField[Blah]";

        // Act
        var result = input.ParseToDictionaryList(fieldNameRegex);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);

        var dictionary = result.First();
        dictionary["FieldName"].ShouldBe("CustomField");
        dictionary["IndexerName"].ShouldBe("Blah");
    }

    [Fact]
    public void ParseToDictionaryList_can_parse_unnamed_groups_with_a_single_match()
    {
        const string fieldNameRegex = @"([A-Za-z0-9]+)[\[]*([A-Za-z0-9]*)[\]]*";

        const string input = "CustomField[Blah]";

        // Act
        var result = input.ParseToDictionaryList(fieldNameRegex);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);

        var dictionary = result.First();
        dictionary["1"].ShouldBe("CustomField");
        dictionary["2"].ShouldBe("Blah");
    }

    [Fact]
    public void ParseFirstMatchToDictionary_can_parse_named_groups_with_a_single_match()
    {
        const string fieldNameRegex = @"(?<FieldName>[A-Za-z0-9]+)[\[]*(?<IndexerName>[A-Za-z0-9]*)[\]]*";

        const string input = "CustomField[Blah]";

        // Act
        var result = input.ParseFirstMatchToDictionary(fieldNameRegex);

        // Assert
        result.ShouldNotBeNull();
        result["FieldName"].ShouldBe("CustomField");
        result["IndexerName"].ShouldBe("Blah");
    }

    [Fact]
    public void GetGroupName_can_extract_group_names()
    {
        const string fieldNameRegex = @"(?<FieldName>[A-Za-z0-9]+)[\[]*(?<IndexerName>[A-Za-z0-9]*)[\]]*";

        // Act
        var regex = new Regex(fieldNameRegex);

        // Assert
        regex.GetGroupName(0).ShouldBe("0");
        regex.GetGroupName(1).ShouldBe("FieldName");
        regex.GetGroupName(2).ShouldBe("IndexerName");
        regex.GetGroupName(3).ShouldBe("3");
    }
}
