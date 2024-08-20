using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNX.Extensions.Strings;
using FluentAssertions;
using Xunit;

namespace DNX.Extensions.Tests.Strings;
public class StringExtensionsTests2
{
    [Theory]
    [InlineData("1,2,3,4,5", StringSplitOptions.None, "1")]
    [InlineData(",,3,4,5", StringSplitOptions.None, "3")]
    [InlineData(",,3,,5", StringSplitOptions.None, "3")]
    [InlineData(",,,,", StringSplitOptions.None, null)]
    [InlineData("1,2,3,4,5", StringSplitOptions.RemoveEmptyEntries, "1")]
    [InlineData(",,3,4,5", StringSplitOptions.RemoveEmptyEntries, "3")]
    [InlineData(",,3,,5", StringSplitOptions.RemoveEmptyEntries, "3")]
    [InlineData(",,,,", StringSplitOptions.RemoveEmptyEntries, null)]
    public void Test_CoalesceNullOrEmpty(string itemText, StringSplitOptions options, string expectedResult)
    {
        var items = itemText?.Split(",", options).ToArray();

        var result = StringExtensions.CoalesceNullOrEmpty(items);

        result.Should().Be(expectedResult);
    }

    [Fact]
    public void Test_CoalesceNullOrEmpty_Null_Items()
    {
        const string[] items = null;

        var result = StringExtensions.CoalesceNullOrEmpty(items);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData("some text", "1,2,3,4,5", StringSplitOptions.None, "some text")]
    [InlineData("some text", ",,3,4,5", StringSplitOptions.None, "some text")]
    [InlineData("some text", ",,3,,5", StringSplitOptions.None, "some text")]
    [InlineData("some text", ",,,,", StringSplitOptions.None, "some text")]
    [InlineData("", "1,2,3,4,5", StringSplitOptions.None, "1")]
    [InlineData(null, ",,3,4,5", StringSplitOptions.None, "3")]
    [InlineData("", ",,3,,5", StringSplitOptions.None, "3")]
    [InlineData(null, ",,,,", StringSplitOptions.None, null)]
    [InlineData("some text", "1,2,3,4,5", StringSplitOptions.RemoveEmptyEntries, "some text")]
    [InlineData("some text", ",,3,4,5", StringSplitOptions.RemoveEmptyEntries, "some text")]
    [InlineData("some text", ",,3,,5", StringSplitOptions.RemoveEmptyEntries, "some text")]
    [InlineData("some text", ",,,,", StringSplitOptions.RemoveEmptyEntries, "some text")]
    [InlineData("", "1,2,3,4,5", StringSplitOptions.RemoveEmptyEntries, "1")]
    [InlineData(null, ",,3,4,5", StringSplitOptions.RemoveEmptyEntries, "3")]
    [InlineData("", ",,3,,5", StringSplitOptions.RemoveEmptyEntries, "3")]
    [InlineData(null, ",,,,", StringSplitOptions.RemoveEmptyEntries, null)]
    public void Test_CoalesceNullOrEmptyWith(string text, string itemText, StringSplitOptions options, string expectedResult)
    {
        var items = itemText?.Split(",", options).ToArray();

        var result = text.CoalesceNullOrEmptyWith(items);

        result.Should().Be(expectedResult);
    }

    [Fact]
    public void Test_CoalesceNullOrEmptyWith_Null_Items()
    {
        const string text = null;
        const string[] items = null;

        var result = text.CoalesceNullOrEmptyWith(items);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData("bob", false)]
    [InlineData("", true)]
    [InlineData(null, true)]
    [InlineData(" ", false)]
    public void NullIfEmpty_evaluates_text_correctly(string text, bool isNull)
    {
        var result = text.NullIfEmpty();

        if (isNull)
            result.Should().BeNull();
        else
            result.Should().NotBeNull();
    }

    [Theory]
    [InlineData("bob", false)]
    [InlineData("", true)]
    [InlineData(null, true)]
    [InlineData(" ", true)]
    public void NullIfWhitespace_evaluates_text_correctly(string text, bool isNull)
    {
        var result = text.NullIfWhitespace();

        if (isNull)
            result.Should().BeNull();
        else
            result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null, "hello", "hello")]
    [InlineData("", "", "")]
    [InlineData("hello", null, "hello")]
    [InlineData("hello", "h", "hello")]
    [InlineData("hello", "he", "hello")]
    [InlineData("hello", "hel", "hello")]
    [InlineData("hello", "hell", "hello")]
    [InlineData("hello", "hello", "hello")]
    [InlineData("hello", " ", " hello")]
    [InlineData(" hello", " ", " hello")]
    [InlineData(" hello", "  ", "   hello")]
    public void Test_EnsureStartsWith(string text, string prefix, string expectedResult)
    {
        var result = text.EnsureStartsWith(prefix);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, "hello", "hello")]
    [InlineData("", "", "")]
    [InlineData("hello", null, "hello")]
    [InlineData("hello", "o", "hello")]
    [InlineData("hello", "lo", "hello")]
    [InlineData("hello", "llo", "hello")]
    [InlineData("hello", "ello", "hello")]
    [InlineData("hello", "hello", "hello")]
    [InlineData("hello", " ", "hello ")]
    [InlineData("hello ", " ", "hello ")]
    [InlineData("hello ", "  ", "hello   ")]
    public void Test_EnsureEndsWith(string text, string suffix, string expectedResult)
    {
        var result = text.EnsureEndsWith(suffix);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("hello", "#", "#hello#")]
    [InlineData("hello", "", "hello")]
    [InlineData("hello", null, "hello")]
    [InlineData("hello", "_", "_hello_")]
    [InlineData("", "_", "_")]
    public void Test_EnsureStartsAndEndsWith(string text, string prefixsuffix, string expectedResult)
    {
        var result = text.EnsureStartsAndEndsWith(prefixsuffix);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("hello", "[", "]", "[hello]")]
    [InlineData("hello", "", ":", "hello:")]
    [InlineData("hello", null, ":", "hello:")]
    [InlineData("hello", ":", "", ":hello")]
    [InlineData("hello", ":", null, ":hello")]
    [InlineData("", "[", "]", "[]")]
    public void Test_EnsureStartsAndEndsWith_prefix_and_suffix(string text, string prefix, string suffix, string expectedResult)
    {
        var result = text.EnsureStartsAndEndsWith(prefix, suffix);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, "hello", null)]
    [InlineData("", "hello", "")]
    [InlineData("hello", "", "hello")]
    [InlineData("hello", null, "hello")]
    [InlineData("hello", "h", "ello")]
    [InlineData("00099", "0", "99")]
    public void Test_RemoveStartsWith(string text, string prefix, string expectedResult)
    {
        var result = text.RemoveStartsWith(prefix);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, "hello", null)]
    [InlineData("", "hello", "")]
    [InlineData("hello", "", "hello")]
    [InlineData("hello", null, "hello")]
    [InlineData("hello", "o", "hell")]
    [InlineData("00099", "9", "000")]
    [InlineData("123232323", "23", "1")]
    public void Test_RemoveEndsWith(string text, string suffix, string expectedResult)
    {
        var result = text.RemoveEndsWith(suffix);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("hello", null, "hello")]
    [InlineData("hello", "", "hello")]
    [InlineData("", "h", "")]
    [InlineData(null, "h", null)]
    [InlineData("hello", "h", "ello")]
    [InlineData("hello", "o", "hell")]
    [InlineData("bob", "b", "o")]
    public void Test_RemoveStartsAndEndsWith(string text, string prefixsuffix, string expectedResult)
    {
        var result = text.RemoveStartsAndEndsWith(prefixsuffix);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("hello", null, null, "hello")]
    [InlineData("hello", "", "", "hello")]
    [InlineData("", "h", "e", "")]
    [InlineData(null, "h", "e", null)]
    [InlineData("hello", "h", "o", "ell")]
    [InlineData("hello", "o", "h", "hello")]
    [InlineData("bob", "b", "b", "o")]
    public void Test_RemoveStartsAndEndsWith_prefix_and_suffix(string text, string prefix, string suffix, string expectedResult)
    {
        var result = text.RemoveStartsAndEndsWith(prefix, suffix);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("[Section Name]", "[", "]", "Section Name")]
    [InlineData("[[Red]]This is some text[[/Red]]", "[[Red]]", "[[/Red]]", "This is some text")]
    [InlineData("[Section Name]", "", "]", null)]
    [InlineData("[Section Name]", "[", "", null)]
    [InlineData("[Section Name]", null, "]", null)]
    [InlineData("[Section Name]", "[", null, null)]
    [InlineData("[Section Name]", null, null, null)]
    [InlineData("A123B", "A", "B", "123")]
    [InlineData("[Section Name]", "(", ")", null)]
    [InlineData("", "[", "]", null)]
    [InlineData(null, "[", "]", null)]
    public void Test_Between(string text, string startText, string endText, string expectedResult)
    {
        var result = text.Between(startText, endText);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("[Section Name]", "[", "]", StringComparison.CurrentCulture, "Section Name")]
    [InlineData("[Section Name]", "(", ")", StringComparison.CurrentCulture, null)]
    [InlineData("A123B", "a", "b", StringComparison.OrdinalIgnoreCase, "123")]
    [InlineData("A123B", "a", "b", StringComparison.Ordinal, null)]
    public void Test_Between_ComparisonType(string text, string startText, string endText, StringComparison comparison, string expectedResult)
    {
        var result = text.Between(startText, endText, comparison);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("This is some text", "some", "This is ")]
    [InlineData("This is some text", "bob", null)]
    [InlineData("This is some [[Red]]text[[/Red]]", "[[", "This is some ")]
    [InlineData("This is some text", " ", "This")]
    [InlineData("This is some text", "", null)]
    [InlineData("This is some text", null, null)]
    [InlineData(null, "o", null)]
    public void Test_Before(string text, string endText, string expectedResult)
    {
        var result = text.Before(endText);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("This is some text", "some", StringComparison.CurrentCulture, "This is ")]
    [InlineData("This is some text", "bob", StringComparison.CurrentCulture, null)]
    [InlineData("This is some text", "SOME", StringComparison.Ordinal, null)]
    [InlineData("This is some text", "SOME", StringComparison.OrdinalIgnoreCase, "This is ")]
    public void Test_Before_ComparisonType(string text, string endText, StringComparison comparison, string expectedResult)
    {
        var result = text.Before(endText, comparison);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("This is some text", "some", "This is ")]
    [InlineData("This is some text", "bob", null)]
    [InlineData("This is some [[Red]]text[[/Red]]", "[[", "This is some [[Red]]text")]
    [InlineData("This is some text", " ", "This is some")]
    [InlineData("This is some text", "", null)]
    [InlineData("This is some text", null, null)]
    [InlineData(null, "o", null)]
    public void Test_BeforeLast(string text, string endText, string expectedResult)
    {
        var result = text.BeforeLast(endText);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("This is some text", "some", " text")]
    [InlineData("This is some text", "bob", null)]
    [InlineData("This is some [[Red]]text[[/Red]]", "[[", "Red]]text[[/Red]]")]
    [InlineData("This is some text", " ", "is some text")]
    [InlineData("This is some text", "", null)]
    [InlineData("This is some text", null, null)]
    [InlineData(null, "o", null)]
    public void Test_After(string text, string startText, string expectedResult)
    {
        var result = text.After(startText);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("This is some text", "some", StringComparison.CurrentCulture, " text")]
    [InlineData("This is some text", "bob", StringComparison.CurrentCulture, null)]
    [InlineData("This is some text", "SOME", StringComparison.Ordinal, null)]
    [InlineData("This is some text", "SOME", StringComparison.OrdinalIgnoreCase, " text")]
    public void Test_After_ComparisonType(string text, string startText, StringComparison comparison, string expectedResult)
    {
        var result = text.After(startText, comparison);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("This is some text", "some", " text")]
    [InlineData("This is some text", "bob", null)]
    [InlineData("This is some [[Red]]text[[/Red]]", "[[", "/Red]]")]
    [InlineData("This is some text", " ", "text")]
    [InlineData("This is some text", "", null)]
    [InlineData("This is some text", null, null)]
    [InlineData(null, "o", null)]
    public void Test_AfterLast(string text, string startText, string expectedResult)
    {
        var result = text.AfterLast(startText);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("This is some text", "some", "This is some")]
    [InlineData("This is some text", "bob", null)]
    [InlineData("This is some [[Red]]text[[/Red]]", "[[", "This is some [[")]
    [InlineData("This is some text", "This", "This")]
    [InlineData("This is some text", "", null)]
    [InlineData("This is some text", null, null)]
    [InlineData(null, "o", null)]
    public void Test_UpTo(string text, string endText, string expectedResult)
    {
        var result = text.AsFarAs(endText);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("This is some text", "some", StringComparison.CurrentCulture, "This is some")]
    [InlineData("This is some text", "bob", StringComparison.CurrentCulture, null)]
    [InlineData("This is some text", "SOME", StringComparison.Ordinal, null)]
    [InlineData("This is some text", "SOME", StringComparison.OrdinalIgnoreCase, "This is some")]
    public void Test_UpTo_ComparisonType(string text, string endText, StringComparison comparison, string expectedResult)
    {
        var result = text.AsFarAs(endText, comparison);

        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("Some text", 5, "Some ")]
    [InlineData("Some text", 20, "Some text")]
    [InlineData("Some text", 0, "")]
    [InlineData("", 0, "")]
    [InlineData("", 5, "")]
    [InlineData(null, 2, null)]
    public void Test_Truncate(string text, int length, string expectedResult)
    {
        var result = text.Truncate(length);

        result.Should().Be(expectedResult);
    }
}
