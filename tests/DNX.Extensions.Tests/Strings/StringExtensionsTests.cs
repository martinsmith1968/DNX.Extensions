using System.Globalization;
using System.Text;
using DNX.Extensions.Strings;
using Shouldly;
using Xunit;

#pragma warning disable CA1861 // Arrays in InlineData
#pragma warning disable xUnit1025   // Duplicate inline test cases

namespace DNX.Extensions.Tests.Strings;

public class StringExtensionsTests
{
    public class IsNull
    {
        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData(" ", false)]
        [InlineData("a", false)]
        [InlineData(" A ", false)]
        [InlineData("ABC", false)]
        public void Test_IsNullOrEmpty(string text, bool expectedResult)
        {
            // Assert
            text.IsNullOrEmpty().ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData(" ", true)]
        [InlineData("a", false)]
        [InlineData(" A ", false)]
        [InlineData("ABC", false)]
        public void Test_IsNullOrWhiteSpace(string text, bool expectedResult)
        {
            text.IsNullOrWhiteSpace().ShouldBe(expectedResult);
        }
    }

    public class NullIf
    {
        [Theory]
        [InlineData("bob", false)]
        [InlineData("", true)]
        [InlineData(null, true)]
        [InlineData(" ", false)]
        public void NullIfEmpty_evaluates_text_correctly(string text, bool isNull)
        {
            var result = text.NullIfEmpty();

            if (isNull)
                result.ShouldBeNull();
            else
                result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("bob", false)]
        [InlineData("", true)]
        [InlineData(null, true)]
        [InlineData(" ", true)]
        public void NullIfEmptyOrWhitespace_evaluates_text_correctly(string text, bool isNull)
        {
            var result = text.NullIfEmptyOrWhitespace();

            if (isNull)
                result.ShouldBeNull();
            else
                result.ShouldNotBeNull();
        }
    }

    public class Ensure
    {
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

            // Assert
            result.ShouldBe(expectedResult);
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

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("hello", "#", "#hello#")]
        [InlineData("hello", "", "hello")]
        [InlineData("hello", null, "hello")]
        [InlineData("hello", "_", "_hello_")]
        [InlineData("", "_", "_")]
        public void Test_EnsureStartsAndEndsWith(string text, string prefixSuffix, string expectedResult)
        {
            var result = text.EnsureStartsAndEndsWith(prefixSuffix);

            // Assert
            result.ShouldBe(expectedResult);
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

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class Remove
    {
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

            // Assert
            result.ShouldBe(expectedResult);
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

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("hello", null, "hello")]
        [InlineData("hello", "", "hello")]
        [InlineData("", "h", "")]
        [InlineData(null, "h", null)]
        [InlineData("hello", "h", "ello")]
        [InlineData("hello", "o", "hell")]
        [InlineData("bob", "b", "o")]
        public void Test_RemoveStartsAndEndsWith(string text, string prefixSuffix, string expectedResult)
        {
            var result = text.RemoveStartsAndEndsWith(prefixSuffix);

            // Assert
            result.ShouldBe(expectedResult);
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

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class Between
    {
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

            // Assert
            result.ShouldBe(expectedResult);
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
        [InlineData("[Reference[0]]", "[", "]", "Reference[0]")]
        public void Test_BetweenFirstAndLast(string text, string startText, string endText, string expectedResult)
        {
            var result = text.BetweenFirstAndLast(startText, endText);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("[Section Name]", "[", "]", StringComparison.CurrentCulture, "Section Name")]
        [InlineData("[Section Name]", "(", ")", StringComparison.CurrentCulture, null)]
        [InlineData("A123B", "a", "b", StringComparison.OrdinalIgnoreCase, "123")]
        [InlineData("A123B", "a", "b", StringComparison.Ordinal, null)]
        public void Test_Between_ComparisonType(string text, string startText, string endText, StringComparison comparison, string expectedResult)
        {
            var result = text.Between(startText, endText, comparison);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class Before
    {
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

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("This is some text", "some", StringComparison.CurrentCulture, "This is ")]
        [InlineData("This is some text", "bob", StringComparison.CurrentCulture, null)]
        [InlineData("This is some text", "SOME", StringComparison.Ordinal, null)]
        [InlineData("This is some text", "SOME", StringComparison.OrdinalIgnoreCase, "This is ")]
        public void Test_Before_ComparisonType(string text, string endText, StringComparison comparison, string expectedResult)
        {
            var result = text.Before(endText, comparison);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("This is some text with some more text", "some", "This is some text with ")]
        [InlineData("This is some text", "bob", null)]
        [InlineData("This is some [[Red]]text[[/Red]]", "[[", "This is some [[Red]]text")]
        [InlineData("This is some text", " ", "This is some")]
        [InlineData("This is some text", "", null)]
        [InlineData("This is some text", null, null)]
        [InlineData(null, "o", null)]
        public void Test_BeforeLast(string text, string endText, string expectedResult)
        {
            var result = text.BeforeLast(endText);

            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("This is some text with some more text", "some", StringComparison.CurrentCulture, "This is some text with ")]
        [InlineData("This is some text", "bob", StringComparison.CurrentCulture, null)]
        [InlineData("This is some text with some more text", "SOME", StringComparison.Ordinal, null)]
        [InlineData("This is some text with some more text", "SOME", StringComparison.OrdinalIgnoreCase, "This is some text with ")]
        public void Test_BeforeLast_ComparisonType(string text, string endText, StringComparison comparison, string expectedResult)
        {
            var result = text.BeforeLast(endText, comparison);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class After
    {
        [Theory]
        [InlineData("This is some text", "some", " text")]
        [InlineData("This is some text with some more text", "some", " text with some more text")]
        [InlineData("This is some text", "bob", null)]
        [InlineData("This is some [[Red]]text[[/Red]]", "[[", "Red]]text[[/Red]]")]
        [InlineData("This is some text", " ", "is some text")]
        [InlineData("This is some text", "", null)]
        [InlineData("This is some text", null, null)]
        [InlineData(null, "o", null)]
        public void Test_After(string text, string startText, string expectedResult)
        {
            var result = text.After(startText);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("This is some text", "some", StringComparison.CurrentCulture, " text")]
        [InlineData("This is some text with some more text", "some", StringComparison.CurrentCulture, " text with some more text")]
        [InlineData("This is some text", "bob", StringComparison.CurrentCulture, null)]
        [InlineData("This is some text", "SOME", StringComparison.Ordinal, null)]
        [InlineData("This is some text", "SOME", StringComparison.OrdinalIgnoreCase, " text")]
        public void Test_After_ComparisonType(string text, string startText, StringComparison comparison, string expectedResult)
        {
            var result = text.After(startText, comparison);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("This is some text", "some", " text")]
        [InlineData("This is some text with some more text", "some", " more text")]
        [InlineData("This is some text", "bob", null)]
        [InlineData("This is some [[Red]]text[[/Red]]", "[[", "/Red]]")]
        [InlineData("This is some text", " ", "text")]
        [InlineData("This is some text", "", null)]
        [InlineData("This is some text", null, null)]
        [InlineData(null, "o", null)]
        public void Test_AfterLast(string text, string startText, string expectedResult)
        {
            var result = text.AfterLast(startText);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("This is some text", "some", StringComparison.CurrentCulture, " text")]
        [InlineData("This is some text with some more text", "some", StringComparison.CurrentCulture, " more text")]
        [InlineData("This is some text", "bob", StringComparison.CurrentCulture, null)]
        [InlineData("This is some text", "SOME", StringComparison.Ordinal, null)]
        [InlineData("This is some text", "SOME", StringComparison.OrdinalIgnoreCase, " text")]
        public void Test_AfterLast_ComparisonType(string text, string startText, StringComparison comparison, string expectedResult)
        {
            var result = text.AfterLast(startText, comparison);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class From
    {
        [Theory]
        [InlineData("bob", null, null)]
        [InlineData(null, "bob", null)]
        [InlineData(null, null, null)]
        [InlineData("bob", "b", "bob")]
        [InlineData("bob", "o", "ob")]
        [InlineData("bobob", "b", "bobob")]
        [InlineData("bobob", "o", "obob")]
        [InlineData("bob", "x", null)]
        public void Test_From(string text, string searchText, string expectedResult)
        {
            var result = text.From(searchText);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("bob", null, StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData(null, "bob", StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData(null, null, StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData("bob", "b", StringComparison.CurrentCulture, "bob")]
        [InlineData("bob", "B", StringComparison.CurrentCultureIgnoreCase, "bob")]
        [InlineData("bob", "O", StringComparison.CurrentCulture, null)]
        [InlineData("bob", "O", StringComparison.CurrentCultureIgnoreCase, "ob")]
        [InlineData("bob", "x", StringComparison.CurrentCultureIgnoreCase, null)]
        public void Test_From_ComparisonType(string text, string searchText, StringComparison comparison, string expectedResult)
        {
            var result = text.From(searchText, comparison);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("bob", null, null)]
        [InlineData(null, "bob", null)]
        [InlineData(null, null, null)]
        [InlineData("bob", "b", "b")]
        [InlineData("bob", "o", "ob")]
        [InlineData("bobob", "b", "b")]
        [InlineData("bobob", "o", "ob")]
        [InlineData("bob", "x", null)]
        public void Test_FromLast(string text, string searchText, string expectedResult)
        {
            var result = text.FromLast(searchText);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("bob", null, StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData(null, "bob", StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData(null, null, StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData("bob", "b", StringComparison.CurrentCulture, "b")]
        [InlineData("bob", "B", StringComparison.CurrentCultureIgnoreCase, "b")]
        [InlineData("bob", "O", StringComparison.CurrentCulture, null)]
        [InlineData("bob", "O", StringComparison.CurrentCultureIgnoreCase, "ob")]
        [InlineData("bob", "x", StringComparison.CurrentCultureIgnoreCase, null)]
        public void Test_FromLast_ComparisonType(string text, string searchText, StringComparison comparison, string expectedResult)
        {
            var result = text.FromLast(searchText, comparison);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class AsFarAs
    {
        [Theory]
        [InlineData("bob", null, null)]
        [InlineData(null, "bob", null)]
        [InlineData(null, null, null)]
        [InlineData("bob", "b", "b")]
        [InlineData("bob", "o", "bo")]
        [InlineData("bobob", "b", "b")]
        [InlineData("bobob", "o", "bo")]
        [InlineData("bob", "x", null)]
        public void Test_AsFarAs(string text, string searchText, string expectedResult)
        {
            var result = text.AsFarAs(searchText);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("bob", null, StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData(null, "bob", StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData(null, null, StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData("bob", "b", StringComparison.CurrentCulture, "b")]
        [InlineData("bob", "B", StringComparison.CurrentCultureIgnoreCase, "b")]
        [InlineData("bob", "O", StringComparison.CurrentCulture, null)]
        [InlineData("bob", "O", StringComparison.CurrentCultureIgnoreCase, "bo")]
        [InlineData("bob", "x", StringComparison.CurrentCultureIgnoreCase, null)]
        public void Test_AsFarAs_ComparisonType(string text, string searchText, StringComparison comparison, string expectedResult)
        {
            var result = text.AsFarAs(searchText, comparison);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("bob", null, null)]
        [InlineData(null, "bob", null)]
        [InlineData(null, null, null)]
        [InlineData("bob", "b", "bob")]
        [InlineData("bob", "o", "bo")]
        [InlineData("bobob", "b", "bobob")]
        [InlineData("bobob", "o", "bobo")]
        [InlineData("bob", "x", null)]
        public void Test_AsFarAsLast(string text, string searchText, string expectedResult)
        {
            var result = text.AsFarAsLast(searchText);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("bob", null, StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData(null, "bob", StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData(null, null, StringComparison.CurrentCultureIgnoreCase, null)]
        [InlineData("bob", "b", StringComparison.CurrentCulture, "bob")]
        [InlineData("bob", "B", StringComparison.CurrentCultureIgnoreCase, "bob")]
        [InlineData("bob", "O", StringComparison.CurrentCulture, null)]
        [InlineData("bob", "O", StringComparison.CurrentCultureIgnoreCase, "bo")]
        [InlineData("bob", "x", StringComparison.CurrentCultureIgnoreCase, null)]
        public void Test_AsFarAsLast_ComparisonType(string text, string searchText, StringComparison comparison, string expectedResult)
        {
            var result = text.AsFarAsLast(searchText, comparison);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class Contains
    {
        [Theory]
        [InlineData("bob", "b", StringComparison.CurrentCultureIgnoreCase, true)]
        [InlineData("BOB", "o", StringComparison.CurrentCultureIgnoreCase, true)]
        [InlineData("bob", "b", StringComparison.CurrentCulture, true)]
        [InlineData("BOB", "o", StringComparison.CurrentCulture, false)]
        [InlineData("BOB", "", StringComparison.CurrentCulture, true)]
        [InlineData("BOB", null, StringComparison.CurrentCulture, true)]
        [InlineData("", "b", StringComparison.CurrentCulture, false)]
        [InlineData(null, "o", StringComparison.CurrentCulture, false)]
        public void Test_ContainsText(string text, string searchText, StringComparison comparison, bool expectedResult)
        {
            var result = text.ContainsText(searchText, comparison);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("hello", "helo", true)]
        [InlineData("1234.56", "1234567890.", true)]
        [InlineData("1,234.56", "1234567890.", false)]
        [InlineData("1,234.56", "", false)]
        [InlineData("1,234.56", null, false)]
        public void Test_ContainsOnly(string text, string characters, bool expectedResult)
        {
            var result = text.ContainsOnly(characters);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("1234.56", new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' }, true)]
        [InlineData("1,234.56", new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' }, false)]
        [InlineData("1,234.56", new char[] { }, false)]
        [InlineData("1,234.56", null, false)]
        [InlineData(null, new[] { 'a', 'e', 'i', 'o', 'u' }, false)]
        public void Test_ContainsOnlyCharacterArray(string text, IList<char> characters, bool expectedResult)
        {
            var result = text.ContainsOnly(characters);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class RemoveText
    {
        [Theory]
        [InlineData("obviously this piece of text contains at least one of every vowel", "aeiou", "bvsly ths pc f txt cntns t lst n f vry vwl")]
        [InlineData("123,456,789.00", ",.", "12345678900")]
        public void Test_RemoveAny(string text, string charsToRemove, string expectedResult)
        {
            var result = text.RemoveAny(charsToRemove);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("", new[]
        {
            'a', 'e', 'i', 'o', 'u'
        }, "")]
        [InlineData(null, new[]
        {
            'a', 'e', 'i', 'o', 'u'
        }, null)]
        [InlineData("aeiou", new char[]
        {
        }, "aeiou")]
        [InlineData("aeiou", null, "aeiou")]
        [InlineData("obviously this piece of text contains at least one of every vowel", new[]
        {
            'a', 'e', 'i', 'o', 'u'
        }, "bvsly ths pc f txt cntns t lst n f vry vwl")]
        [InlineData("123,456,789.00", new[]
        {
            ',', '.'
        }, "12345678900")]
        public void Test_RemoveAny_char_array(string text, char[] charsToRemove, string expectedResult)
        {
            var result = text.RemoveAny(charsToRemove);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("", "1234567890.", "")]
        [InlineData(null, "1234567890.", null)]
        [InlineData("hello", "", "")]
        [InlineData("hello", null, "")]
        [InlineData("The amount to pay is: 123,456.00", "1234567890.", "123456.00")]
        public void Test_RemoveAnyExcept(string text, string charsToKeep, string expectedResult)
        {
            var result = text.RemoveAnyExcept(charsToKeep);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("", new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'
        }, "")]
        [InlineData(null, new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'
        }, null)]
        [InlineData("The amount to pay is: 123,456.00", new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'
        }, "123456.00")]
        public void Test_RemoveAnyExcept_char_array(string text, char[] charsToKeep, string expectedResult)
        {
            var result = text.RemoveAnyExcept(charsToKeep);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class IsValid
    {
        [Theory]
        // GB
        [InlineData("0", "en-gb", true)]
        [InlineData("1", "en-gb", true)]
        [InlineData("-1", "en-gb", true)]
        [InlineData("+1", "en-gb", true)]
        [InlineData("123.72", "en-gb", true)]
        [InlineData("-123.72", "en-gb", true)]
        [InlineData("+123.72", "en-gb", true)]
        [InlineData("3,123.451", "en-gb", true)]
        [InlineData("-3,123.451", "en-gb", true)]
        [InlineData("+3,123.451", "en-gb", true)]
        [InlineData("3412123.76543", "en-gb", true)]
        [InlineData("-3412123.76543", "en-gb", true)]
        [InlineData("+3412123.76543", "en-gb", true)]
        [InlineData("7,034.989", "en-gb", true)]
        [InlineData("-7,034.989", "en-gb", true)]
        [InlineData("+7,034.989", "en-gb", true)]
        // DE
        [InlineData("3,123.451", "de-DE", false)]
        [InlineData("3.123,451", "de-DE", true)]
        public void Test_IsValidNumber_default_culture(string text, string cultureInfoName, bool expectedResult)
        {
            var previousCulture = CultureInfo.DefaultThreadCurrentCulture;

            try
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(cultureInfoName);

                // Act
                var result = text.IsValidNumber();

                // Assert
                result.ShouldBe(expectedResult);
            }
            finally
            {
                CultureInfo.DefaultThreadCurrentCulture = previousCulture;
            }
        }

        [Theory]
        // GB
        [InlineData("0", "en-gb", true)]
        [InlineData("1", "en-gb", true)]
        [InlineData("-1", "en-gb", true)]
        [InlineData("+1", "en-gb", true)]
        [InlineData("123.72", "en-gb", true)]
        [InlineData("-123.72", "en-gb", true)]
        [InlineData("+123.72", "en-gb", true)]
        [InlineData("3,123.451", "en-gb", true)]
        [InlineData("-3,123.451", "en-gb", true)]
        [InlineData("+3,123.451", "en-gb", true)]
        [InlineData("3412123.76543", "en-gb", true)]
        [InlineData("-3412123.76543", "en-gb", true)]
        [InlineData("+3412123.76543", "en-gb", true)]
        [InlineData("7,034.989", "en-gb", true)]
        [InlineData("-7,034.989", "en-gb", true)]
        [InlineData("+7,034.989", "en-gb", true)]
        // DE
        [InlineData("3,123.451", "de-DE", false)]
        [InlineData("3.123,451", "de-DE", true)]
        public void Test_IsValidNumber(string text, string cultureInfoName, bool expectedResult)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(cultureInfoName);

            // Act
            var result = text.IsValidNumber(cultureInfo);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class Reverse
    {
        [Theory]
        [InlineData("hello world", "dlrow olleh")]
        [InlineData("12345", "54321")]
        [InlineData("abcba", "abcba")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void Test_Reverse(string text, string expectedResult)
        {
            var result = text.Reverse();

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class Split
    {
        [Theory]
        [InlineData("a-b-c-d-e", "-", StringSplitOptions.None, "a,b,c,d,e")]
        [InlineData("a-b[c]d=e", "-[]=", StringSplitOptions.None, "a,b,c,d,e")]
        [InlineData("a-b--d-e", "-", StringSplitOptions.RemoveEmptyEntries, "a,b,d,e")]
        [InlineData("a-b[]d=e", "-[]=", StringSplitOptions.RemoveEmptyEntries, "a,b,d,e")]
        [InlineData("a-b[]d=e", "-[]=", StringSplitOptions.None, "a,b,,d,e")]
        public void Test_SplitText(string text, string delimiters, StringSplitOptions options, string expectedResult)
        {
            var result = text.SplitText(delimiters, options);

            // Assert
            string.Join(",", result).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("a-b-c-d-e", "-", StringSplitOptions.None, SplitDelimiterType.Any, "a,b,c,d,e")]
        [InlineData("a-b[c]d=e", "-[]=", StringSplitOptions.None, SplitDelimiterType.Any, "a,b,c,d,e")]
        [InlineData("a-b--d-e", "-", StringSplitOptions.RemoveEmptyEntries, SplitDelimiterType.Any, "a,b,d,e")]
        [InlineData("a-b[]d=e", "-[]=", StringSplitOptions.RemoveEmptyEntries, SplitDelimiterType.Any, "a,b,d,e")]
        [InlineData("a-b[]d=e", "-[]=", StringSplitOptions.None, SplitDelimiterType.Any, "a,b,,d,e")]
        [InlineData("a-b-c-d-e", "-", StringSplitOptions.None, SplitDelimiterType.All, "a,b,c,d,e")]
        [InlineData("a-b--d-e", "--", StringSplitOptions.RemoveEmptyEntries, SplitDelimiterType.All, "a-b,d-e")]
        [InlineData("a-b----d-e", "--", StringSplitOptions.None, SplitDelimiterType.All, "a-b,,d-e")]
        [InlineData("a-b[]d=e", "[]", StringSplitOptions.RemoveEmptyEntries, SplitDelimiterType.All, "a-b,d=e")]
        public void Test_SplitText_DelimiterType(string text, string delimiters, StringSplitOptions options, SplitDelimiterType delimiterType, string expectedResult)
        {
            var result = text.SplitText(delimiters, options, delimiterType);

            // Assert
            string.Join(",", result).ShouldBe(expectedResult);
        }

        [Fact]
        public void Test_SplitText_InvalidDelimiterType()
        {
            // Arrange
            var text = "a,b,c";
            var delimiters = ",";

            // Act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => text.SplitText(delimiters, StringSplitOptions.None, (SplitDelimiterType)int.MaxValue)
            );

            // Assert
            ex.ShouldNotBeNull();
            ex.ParamName.ShouldBe("delimiterType");
            Enum.GetValues<SplitDelimiterType>()
                .ToList()
                .ForEach(x => ex.Message.ShouldContain(x.ToString()));
        }

        [Theory]
        [InlineData("a-b-c-d-e", "-", StringSplitOptions.None, "a,b,c,d,e")]
        [InlineData("a-b[c]d-e", "-", StringSplitOptions.None, "a,b[c]d,e")]
        [InlineData("a-b--d-e", "-", StringSplitOptions.RemoveEmptyEntries, "a,b,d,e")]
        [InlineData("a-b[]d=e", "[]", StringSplitOptions.RemoveEmptyEntries, "a-b,d=e")]
        [InlineData("a-b//d=e", "/", StringSplitOptions.None, "a-b,,d=e")]
        [InlineData("a-b//d=e", "/", StringSplitOptions.RemoveEmptyEntries, "a-b,d=e")]
        public void Test_SplitByText(string text, string delimiters, StringSplitOptions options, string expectedResult)
        {
            var result = text.SplitByText(delimiters, options);

            // Assert
            string.Join(",", result).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("Line 1/Line 2//Line 4", StringSplitOptions.None, StringComparison.CurrentCulture, "Line 1/Line 2//Line 4")]
        [InlineData("Line 1/Line 2//Line 4", StringSplitOptions.RemoveEmptyEntries, StringComparison.CurrentCulture, "Line 1/Line 2/Line 4")]
        [InlineData("Line 1///Line 2//Line 4/", StringSplitOptions.None, StringComparison.CurrentCulture, "Line 1///Line 2//Line 4")]
        [InlineData("Line 1/Line 2//Line 4", StringSplitOptions.RemoveEmptyEntries, StringComparison.CurrentCulture, "Line 1/Line 2/Line 4")]
        public void Test_SplitByText_ComplexDelimiter(string text, StringSplitOptions options, StringComparison comparison, string expectedResult)
        {
            var delimiters = Environment.NewLine;

            var adjustedText = string.Join(Environment.NewLine, text.Split('/'));

            // Act
            var lines = adjustedText.SplitByText(delimiters, options, comparison);

            // Assert
            string.Join("/", lines).ShouldBe(expectedResult);
        }
    }

    public class CoalesceNull
    {
        [Theory]
        [InlineData("text", "a", "b", "c", "text")]
        [InlineData("text", "a", null, null, "text")]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", null, null, "a")]
        [InlineData(null, null, "b", "c", "b")]
        [InlineData(null, null, null, "c", "c")]
        [InlineData(null, null, null, null, null)]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", "", "", "a")]
        [InlineData(null, "", "b", "c", "")]
        [InlineData(null, "", "", "c", "")]
        [InlineData(null, " ", "", "c", " ")]
        [InlineData(null, "", "", "", "")]
        public void Test_CoalesceNullWith_params(string text, string a, string b, string c, string expectedResult)
        {
            var result = text.CoalesceNullWith(a, b, c);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("text", "a", "b", "c", "text")]
        [InlineData("text", "a", null, null, "text")]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", null, null, "a")]
        [InlineData(null, null, "b", "c", "b")]
        [InlineData(null, null, null, "c", "c")]
        [InlineData(null, null, null, null, null)]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", "", "", "a")]
        [InlineData(null, "", "b", "c", "")]
        [InlineData(null, "", "", "c", "")]
        [InlineData(null, " ", "", "c", " ")]
        [InlineData(null, "", "", "", "")]
        public void Test_CoalesceNullWith_list(string text, string a, string b, string c, string expectedResult)
        {
            var list = new List<string>()
            {
                a,
                b,
                c
            };

            var result = text.CoalesceNullWith(list);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("a", "b", "c", "a")]
        [InlineData("a", null, null, "a")]
        [InlineData(null, "b", "c", "b")]
        [InlineData(null, null, "c", "c")]
        [InlineData(null, null, null, null)]
        [InlineData("a", "b", "c", "a")]
        [InlineData("a", "", "", "a")]
        [InlineData("", "b", "c", "")]
        [InlineData("", "", "c", "")]
        [InlineData(" ", "", "c", " ")]
        [InlineData("", "", "", "")]
        public void Test_CoalesceNull_list(string a, string b, string c, string expectedResult)
        {
            var list = new List<string>()
            {
                a,
                b,
                c
            };

            var result = list.CoalesceNull();

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class CoalesceNullOrEmpty
    {
        [Theory]
        [InlineData("text", "a", "b", "c", "text")]
        [InlineData("text", "a", null, null, "text")]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", null, null, "a")]
        [InlineData(null, null, "b", "c", "b")]
        [InlineData(null, null, null, "c", "c")]
        [InlineData(null, null, null, null, null)]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", "", "", "a")]
        [InlineData(null, "", "b", "c", "b")]
        [InlineData(null, "", "", "c", "c")]
        [InlineData(null, "", " ", "c", " ")]
        [InlineData(null, "", "", "", null)]
        public void Test_CoalesceNullOrEmptyWith_params(string text, string a, string b, string c, string expectedResult)
        {
            var result = text.CoalesceNullOrEmptyWith(a, b, c);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("text", "a", "b", "c", "text")]
        [InlineData("text", "a", null, null, "text")]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", null, null, "a")]
        [InlineData(null, null, "b", "c", "b")]
        [InlineData(null, null, null, "c", "c")]
        [InlineData(null, null, null, null, null)]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", "", "", "a")]
        [InlineData(null, "", "b", "c", "b")]
        [InlineData(null, "", "", "c", "c")]
        [InlineData(null, "", " ", "c", " ")]
        [InlineData(null, "", "", "", null)]
        public void Test_CoalesceNullOrEmptyWith_list(string text, string a, string b, string c, string expectedResult)
        {
            var list = new List<string>()
            {
                a,
                b,
                c
            };

            var result = text.CoalesceNullOrEmptyWith(list);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("a", "b", "c", "a")]
        [InlineData("a", null, null, "a")]
        [InlineData(null, "b", "c", "b")]
        [InlineData(null, null, "c", "c")]
        [InlineData(null, null, null, null)]
        [InlineData("a", "b", "c", "a")]
        [InlineData("a", "", "", "a")]
        [InlineData("", "b", "c", "b")]
        [InlineData("", "", "c", "c")]
        [InlineData("", " ", "c", " ")]
        [InlineData("", "", "", null)]
        public void Test_CoalesceNullOrEmpty_list(string a, string b, string c, string expectedResult)
        {
            var list = new List<string>()
            {
                a,
                b,
                c
            };

            var result = list.CoalesceNullOrEmpty();

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class CoalesceNullOrWhitespace
    {
        [Theory]
        [InlineData("text", "a", "b", "c", "text")]
        [InlineData("text", "a", null, null, "text")]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", null, null, "a")]
        [InlineData(null, null, "b", "c", "b")]
        [InlineData(null, null, null, "c", "c")]
        [InlineData(null, null, null, null, null)]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", "", "", "a")]
        [InlineData(null, "", "b", "c", "b")]
        [InlineData(null, "", "", "c", "c")]
        [InlineData(null, "", " ", "c", "c")]
        [InlineData(null, "", "", "", null)]
        public void Test_CoalesceNullOrWhitespaceWith_params(string text, string a, string b, string c, string expectedResult)
        {
            var result = text.CoalesceNullOrWhitespaceWith(a, b, c);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("text", "a", "b", "c", "text")]
        [InlineData("text", "a", null, null, "text")]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", null, null, "a")]
        [InlineData(null, null, "b", "c", "b")]
        [InlineData(null, null, null, "c", "c")]
        [InlineData(null, null, null, null, null)]
        [InlineData(null, "a", "b", "c", "a")]
        [InlineData(null, "a", "", "", "a")]
        [InlineData(null, "", "b", "c", "b")]
        [InlineData(null, "", "", "c", "c")]
        [InlineData(null, "", " ", "c", "c")]
        [InlineData(null, "", "", "", null)]
        public void Test_CoalesceNullOrWhitespaceWith_list(string text, string a, string b, string c, string expectedResult)
        {
            var list = new List<string>()
            {
                a,
                b,
                c
            };

            var result = text.CoalesceNullOrWhitespaceWith(list);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("a", "b", "c", "a")]
        [InlineData("a", null, null, "a")]
        [InlineData(null, "b", "c", "b")]
        [InlineData(null, null, "c", "c")]
        [InlineData(null, null, null, null)]
        [InlineData("a", "b", "c", "a")]
        [InlineData("a", "", "", "a")]
        [InlineData("", "b", "c", "b")]
        [InlineData("", "", "c", "c")]
        [InlineData("", " ", "c", "c")]
        [InlineData("", "", "", null)]
        public void Test_CoalesceNullOrWhitespace_list(string a, string b, string c, string expectedResult)
        {
            var list = new List<string>()
            {
                a,
                b,
                c
            };

            var result = list.CoalesceNullOrWhitespace();

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class Wordify
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("TwoWords", "Two Words")]
        [InlineData("ThreeWordsTogether", "Three Words Together")]
        [InlineData("pascalCase", "pascal Case")]
        [InlineData("Already Spaced", "Already Spaced")]
        [InlineData("AnEntireSentenceSquashedTogetherIntoOneSingleWord", "An Entire Sentence Squashed Together Into One Single Word")]
        [InlineData("SouthendUnitedFCAreRubbish", "Southend United F C Are Rubbish")]
        [InlineData("BobCarolgeesIsALegend", "Bob Carolgees Is A Legend")]
        [InlineData("IsITVDramaBetterThanBBCDrama", "Is I T V Drama Better Than B B C Drama")]
        [InlineData("IsTheHoLAnOutdatedInstitution", "Is The Ho L An Outdated Institution")]
        [InlineData("VirusesLikeH1N1AndH5N1WereWarningsForThePanDemicToCome", "Viruses Like H1 N1 And H5 N1 Were Warnings For The Pan Demic To Come")]
        public void Wordify_Tests(string text, string expectedResult)
        {
            // Act
            var result = text.Wordify();

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", null, "")]
        [InlineData(null, "", null)]
        [InlineData("", "", "")]
        [InlineData("TwoWords", null, "Two Words")]
        [InlineData("ThreeWordsTogether", null, "Three Words Together")]
        [InlineData("pascalCase", null, "pascal Case")]
        [InlineData("Already Spaced", null, "Already Spaced")]
        [InlineData("AnEntireSentenceSquashedTogetherIntoOneSingleWord", null, "An Entire Sentence Squashed Together Into One Single Word")]
        [InlineData("IsITVDramaBetterThanBBCDrama", null, "Is I T V Drama Better Than B B C Drama")]
        [InlineData("IsITVDramaBetterThanBBCDrama", "ITV|BBC", "Is ITV Drama Better Than BBC Drama")]
        [InlineData("IsTheHoLAnOutdatedInstitution", null, "Is The Ho L An Outdated Institution")]
        [InlineData("IsTheHoLAnOutdatedInstitution", "HoL", "Is The HoL An Outdated Institution")]
        [InlineData("VirusesLikeH1N1AndH5N1WereWarningsForThePanDemicToCome", null, "Viruses Like H1 N1 And H5 N1 Were Warnings For The Pan Demic To Come")]
        [InlineData("VirusesLikeH1N1AndH5N1WereWarningsForThePanDemicToCome", "H1N1|H5N1|PanDemic", "Viruses Like H1N1 And H5N1 Were Warnings For The PanDemic To Come")]
        [InlineData("SouthendUnitedFCAreRubbish", null, "Southend United F C Are Rubbish")]
        [InlineData("SouthendUnitedFCAreRubbish", "FC", "Southend United FC Are Rubbish")]
        [InlineData("BobCarolgeesIsALegend", null, "Bob Carolgees Is A Legend")]
        [InlineData("BobCarolgeesIsALegend", "FC", "Bob Carolgees Is A Legend")]
        public void Wordify_with_reserved_words_Tests(string text, string reservedWordsText, string expectedResult)
        {
            var reservedWordsList = reservedWordsText?
                .Split("|")
                .ToArray();

            // Act
            var result = text.Wordify(reservedWordsList);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class Left
    {
        [Theory]
        [InlineData("Some text", 5, "Some ")]
        [InlineData("Some text", 9, "Some text")]
        [InlineData("Some text", 20, "Some text")]
        [InlineData("Some text", 0, "")]
        [InlineData("", 0, "")]
        [InlineData("", 5, "")]
        [InlineData(null, 2, null)]
        public void Test_Left(string text, int length, string expectedResult)
        {
            var result = text.Left(length);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class Right
    {
        [Theory]
        [InlineData("Some text", 5, " text")]
        [InlineData("Some text", 9, "Some text")]
        [InlineData("Some text", 20, "Some text")]
        [InlineData("Some text", 0, "")]
        [InlineData("", 0, "")]
        [InlineData("", 5, "")]
        [InlineData(null, 2, null)]
        public void Test_Right(string text, int length, string expectedResult)
        {
            var result = text.Right(length);

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class ToHexString
    {
        [Theory]
        [InlineData("ABCDEF", "414243444546")]
        public void Test_ToHexString(string number, string expectedHexString)
        {
            var result = number.ToHexString();

            result.ShouldBe(expectedHexString);
        }
    }

    public class GetBytes
    {
        [Theory]
        [InlineData("ABCDEF", "65,66,67,68,69,70")]
        [InlineData("abcdef", "97,98,99,100,101,102")]
        public void Test_GetBytes(string text, string expectedBytesText)
        {
            var bytes = text.GetBytes();

            var bytesText = string.Join(",", bytes.Select(x => x.ToString()));

            bytesText.ShouldBe(expectedBytesText);
        }

        [Theory]
        [MemberData(nameof(GetBytes_with_Encoding_Data))]
        public void Test_GetBytes_with_encoding(string text, Encoding encoding, byte[] expectedResult)
        {
            // Act
            var result = text.GetBytes(encoding);

            result.ShouldBeEquivalentTo(expectedResult);
        }

        public static TheoryData<string, Encoding, byte[]> GetBytes_with_Encoding_Data()
        {
            var str1 = new string(new[] { (char)100, (char)200, (char)300 });

            return new TheoryData<string, Encoding, byte[]>
            {
                { "ABCDEF", Encoding.ASCII, [65, 66, 67, 68, 69, 70] },
                { "ABCDEF", Encoding.UTF8, [65, 66, 67, 68, 69, 70] },
                { "abcdef", Encoding.ASCII, [97, 98, 99, 100, 101, 102] },
                { "abcdef", Encoding.UTF8, [97, 98, 99, 100, 101, 102] },
                { str1, Encoding.ASCII, Encoding.ASCII.GetBytes(str1) },
                { str1, Encoding.UTF8, Encoding.UTF8.GetBytes(str1) },
            };
        }
    }
}
