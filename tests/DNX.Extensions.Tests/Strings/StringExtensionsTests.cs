﻿using System.Globalization;
using DNX.Extensions.Strings;
using NUnit.Framework;
using Shouldly;

namespace DNX.Extensions.Tests.Strings
{
    public class StringExtensionsTests
    {
        [TestCase(null, ExpectedResult = true)]
        [TestCase("", ExpectedResult = true)]
        [TestCase(" ", ExpectedResult = false)]
        [TestCase("a", ExpectedResult = false)]
        [TestCase(" A ", ExpectedResult = false)]
        [TestCase("ABC", ExpectedResult = false)]
        public bool Test_IsNullOrEmpty(string text)
        {
            return text.IsNullOrEmpty();
        }

        [TestCase(null, ExpectedResult = true)]
        [TestCase("", ExpectedResult = true)]
        [TestCase(" ", ExpectedResult = true)]
        [TestCase("a", ExpectedResult = false)]
        [TestCase(" A ", ExpectedResult = false)]
        [TestCase("ABC", ExpectedResult = false)]
        public bool Test_IsNullOrWhiteSpace(string text)
        {
            return text.IsNullOrWhiteSpace();
        }

        [TestCase(null, "hello", ExpectedResult = "hello")]
        [TestCase("", "", ExpectedResult = "")]
        [TestCase("hello", null, ExpectedResult = "hello")]
        [TestCase("hello", "h", ExpectedResult = "hello")]
        [TestCase("hello", "he", ExpectedResult = "hello")]
        [TestCase("hello", "hel", ExpectedResult = "hello")]
        [TestCase("hello", "hell", ExpectedResult = "hello")]
        [TestCase("hello", "hello", ExpectedResult = "hello")]
        [TestCase("hello", " ", ExpectedResult = " hello")]
        [TestCase(" hello", " ", ExpectedResult = " hello")]
        [TestCase(" hello", "  ", ExpectedResult = "   hello")]
        public string Test_EnsureStartsWith(string text, string prefix)
        {
            var result = text.EnsureStartsWith(prefix);

            return result;
        }

        [TestCase(null, "hello", ExpectedResult = "hello")]
        [TestCase("", "", ExpectedResult = "")]
        [TestCase("hello", null, ExpectedResult = "hello")]
        [TestCase("hello", "o", ExpectedResult = "hello")]
        [TestCase("hello", "lo", ExpectedResult = "hello")]
        [TestCase("hello", "llo", ExpectedResult = "hello")]
        [TestCase("hello", "ello", ExpectedResult = "hello")]
        [TestCase("hello", "hello", ExpectedResult = "hello")]
        [TestCase("hello", " ", ExpectedResult = "hello ")]
        [TestCase("hello ", " ", ExpectedResult = "hello ")]
        [TestCase("hello ", "  ", ExpectedResult = "hello   ")]
        public string Test_EnsureEndsWith(string text, string suffix)
        {
            var result = text.EnsureEndsWith(suffix);

            return result;
        }

        [TestCase("hello", "#", ExpectedResult = "#hello#")]
        [TestCase("hello", "", ExpectedResult = "hello")]
        [TestCase("hello", null, ExpectedResult = "hello")]
        [TestCase("hello", "_", ExpectedResult = "_hello_")]
        [TestCase("", "_", ExpectedResult = "_")]
        public string Test_EnsureStartsAndEndsWith(string text, string prefixsuffix)
        {
            var result = text.EnsureStartsAndEndsWith(prefixsuffix);

            return result;
        }

        [TestCase("hello", "[", "]", ExpectedResult = "[hello]")]
        [TestCase("hello", "", ":", ExpectedResult = "hello:")]
        [TestCase("hello", null, ":", ExpectedResult = "hello:")]
        [TestCase("hello", ":", "", ExpectedResult = ":hello")]
        [TestCase("hello", ":", null, ExpectedResult = ":hello")]
        [TestCase("", "[", "]", ExpectedResult = "[]")]
        public string Test_EnsureStartsAndEndsWith_prefix_and_suffix(string text, string prefix, string suffix)
        {
            var result = text.EnsureStartsAndEndsWith(prefix, suffix);

            return result;
        }

        [TestCase(null, "hello", ExpectedResult = null)]
        [TestCase("", "hello", ExpectedResult = "")]
        [TestCase("hello", "", ExpectedResult = "hello")]
        [TestCase("hello", null, ExpectedResult = "hello")]
        [TestCase("hello", "h", ExpectedResult = "ello")]
        [TestCase("00099", "0", ExpectedResult = "99")]
        public string Test_RemoveStartsWith(string text, string prefix)
        {
            var result = text.RemoveStartsWith(prefix);

            return result;
        }

        [TestCase(null, "hello", ExpectedResult = null)]
        [TestCase("", "hello", ExpectedResult = "")]
        [TestCase("hello", "", ExpectedResult = "hello")]
        [TestCase("hello", null, ExpectedResult = "hello")]
        [TestCase("hello", "o", ExpectedResult = "hell")]
        [TestCase("00099", "9", ExpectedResult = "000")]
        [TestCase("123232323", "23", ExpectedResult = "1")]
        public string Test_RemoveEndsWith(string text, string suffix)
        {
            var result = text.RemoveEndsWith(suffix);

            return result;
        }

        [TestCase("hello", null, ExpectedResult = "hello")]
        [TestCase("hello", "", ExpectedResult = "hello")]
        [TestCase("", "h", ExpectedResult = "")]
        [TestCase(null, "h", ExpectedResult = null)]
        [TestCase("hello", "h", ExpectedResult = "ello")]
        [TestCase("hello", "o", ExpectedResult = "hell")]
        [TestCase("bob", "b", ExpectedResult = "o")]
        public string Test_RemoveStartsAndEndsWith(string text, string prefixsuffix)
        {
            var result = text.RemoveStartsAndEndsWith(prefixsuffix);

            return result;
        }

        [TestCase("hello", null, null, ExpectedResult = "hello")]
        [TestCase("hello", "", "", ExpectedResult = "hello")]
        [TestCase("", "h", "e", ExpectedResult = "")]
        [TestCase(null, "h", "e", ExpectedResult = null)]
        [TestCase("hello", "h", "o", ExpectedResult = "ell")]
        [TestCase("hello", "o", "h", ExpectedResult = "hello")]
        [TestCase("bob", "b", "b", ExpectedResult = "o")]
        public string Test_RemoveStartsAndEndsWith_prefix_and_suffix(string text, string prefix, string suffix)
        {
            var result = text.RemoveStartsAndEndsWith(prefix, suffix);

            return result;
        }

        [TestCase("[Section Name]", "[", "]", ExpectedResult = "Section Name")]
        [TestCase("[[Red]]This is some text[[/Red]]", "[[Red]]", "[[/Red]]", ExpectedResult = "This is some text")]
        [TestCase("[Section Name]", "", "]", ExpectedResult = null)]
        [TestCase("[Section Name]", "[", "", ExpectedResult = null)]
        [TestCase("[Section Name]", null, "]", ExpectedResult = null)]
        [TestCase("[Section Name]", "[", null, ExpectedResult = null)]
        [TestCase("[Section Name]", null, null, ExpectedResult = null)]
        [TestCase("A123B", "A", "B", ExpectedResult = "123")]
        [TestCase("[Section Name]", "(", ")", ExpectedResult = null)]
        [TestCase("", "[", "]", ExpectedResult = null)]
        [TestCase(null, "[", "]", ExpectedResult = null)]
        public string Test_Between(string text, string startText, string endText)
        {
            var result = text.Between(startText, endText);

            return result;
        }

        [TestCase("[Section Name]", "[", "]", StringComparison.CurrentCulture, ExpectedResult = "Section Name")]
        [TestCase("[Section Name]", "(", ")", StringComparison.CurrentCulture, ExpectedResult = null)]
        [TestCase("A123B", "a", "b", StringComparison.OrdinalIgnoreCase, ExpectedResult = "123")]
        [TestCase("A123B", "a", "b", StringComparison.Ordinal, ExpectedResult = null)]
        public string Test_Between_ComparisonType(string text, string startText, string endText, StringComparison comparison)
        {
            var result = text.Between(startText, endText, comparison);

            return result;
        }

        [TestCase("This is some text", "some", ExpectedResult = "This is ")]
        [TestCase("This is some text", "bob", ExpectedResult = null)]
        [TestCase("This is some [[Red]]text[[/Red]]", "[[", ExpectedResult = "This is some ")]
        [TestCase("This is some text", " ", ExpectedResult = "This")]
        [TestCase("This is some text", "", ExpectedResult = null)]
        [TestCase("This is some text", null, ExpectedResult = null)]
        [TestCase(null, "o", ExpectedResult = null)]
        public string Test_Before(string text, string endText)
        {
            var result = text.Before(endText);

            return result;
        }

        [TestCase("This is some text", "some", StringComparison.CurrentCulture, ExpectedResult = "This is ")]
        [TestCase("This is some text", "bob", StringComparison.CurrentCulture, ExpectedResult = null)]
        [TestCase("This is some text", "SOME", StringComparison.Ordinal, ExpectedResult = null)]
        [TestCase("This is some text", "SOME", StringComparison.OrdinalIgnoreCase, ExpectedResult = "This is ")]
        public string Test_Before_ComparisonType(string text, string endText, StringComparison comparison)
        {
            var result = text.Before(endText, comparison);

            return result;
        }

        [TestCase("This is some text", "some", ExpectedResult = " text")]
        [TestCase("This is some text", "bob", ExpectedResult = null)]
        [TestCase("This is some [[Red]]text[[/Red]]", "[[", ExpectedResult = "Red]]text[[/Red]]")]
        [TestCase("This is some text", " ", ExpectedResult = "is some text")]
        [TestCase("This is some text", "", ExpectedResult = null)]
        [TestCase("This is some text", null, ExpectedResult = null)]
        [TestCase(null, "o", ExpectedResult = null)]
        public string Test_After(string text, string startText)
        {
            var result = text.After(startText);

            return result;
        }

        [TestCase("This is some text", "some", StringComparison.CurrentCulture, ExpectedResult = " text")]
        [TestCase("This is some text", "bob", StringComparison.CurrentCulture, ExpectedResult = null)]
        [TestCase("This is some text", "SOME", StringComparison.Ordinal, ExpectedResult = null)]
        [TestCase("This is some text", "SOME", StringComparison.OrdinalIgnoreCase, ExpectedResult = " text")]
        public string Test_After_ComparisonType(string text, string startText, StringComparison comparison)
        {
            var result = text.After(startText, comparison);

            return result;
        }

        [TestCase("bob", "b", StringComparison.CurrentCultureIgnoreCase, ExpectedResult = true)]
        [TestCase("BOB", "o", StringComparison.CurrentCultureIgnoreCase, ExpectedResult = true)]
        [TestCase("bob", "b", StringComparison.CurrentCulture, ExpectedResult = true)]
        [TestCase("BOB", "o", StringComparison.CurrentCulture, ExpectedResult = false)]
        [TestCase("BOB", "", StringComparison.CurrentCulture, ExpectedResult = true)]
        [TestCase("BOB", null, StringComparison.CurrentCulture, ExpectedResult = true)]
        [TestCase("", "b", StringComparison.CurrentCulture, ExpectedResult = false)]
        [TestCase(null, "o", StringComparison.CurrentCulture, ExpectedResult = false)]
        public bool Test_ContainsText(string text, string searchText, StringComparison comparison)
        {
            var result = text.ContainsText(searchText, comparison);

            return result;
        }

        [TestCase("hello", "helo", ExpectedResult = true)]
        [TestCase("1234.56", "1234567890.", ExpectedResult = true)]
        [TestCase("1,234.56", "1234567890.", ExpectedResult = false)]
        [TestCase("1,234.56", "", ExpectedResult = false)]
        [TestCase("1,234.56", null, ExpectedResult = false)]
        public bool Test_ContainsOnly(string text, string characters)
        {
            var result = text.ContainsOnly(characters);

            return result;
        }

        [TestCase("1234.56", new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' }, ExpectedResult = true)]
        [TestCase("1,234.56", new [] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' }, ExpectedResult = false)]
        [TestCase("1,234.56", new char[] {}, ExpectedResult = false)]
        [TestCase("1,234.56", null, ExpectedResult = false)]
        [TestCase(null, new[] { 'a', 'e', 'i', 'o', 'u' }, ExpectedResult = false)]
        public bool Test_ContainsOnlyCharacterArray(string text, IList<char> characters)
        {
            var result = text.ContainsOnly(characters);

            return result;
        }

        [TestCase("obviously this piece of text contains at least one of every vowel", "aeiou", ExpectedResult = "bvsly ths pc f txt cntns t lst n f vry vwl")]
        [TestCase("123,456,789.00", ",.", ExpectedResult = "12345678900")]
        public string Test_RemoveAny(string text, string charsToRemove)
        {
            var result = text.RemoveAny(charsToRemove);

            return result;
        }

        [TestCase("", new [] { 'a', 'e', 'i', 'o', 'u' }, ExpectedResult = "")]
        [TestCase(null, new[] { 'a', 'e', 'i', 'o', 'u' }, ExpectedResult = null)]
        [TestCase("aeiou", new char[] { }, ExpectedResult = "aeiou")]
        [TestCase("aeiou", null, ExpectedResult = "aeiou")]
        [TestCase("obviously this piece of text contains at least one of every vowel", new[] { 'a', 'e', 'i', 'o', 'u' }, ExpectedResult = "bvsly ths pc f txt cntns t lst n f vry vwl")]
        [TestCase("123,456,789.00", new [] { ',', '.' }, ExpectedResult = "12345678900")]
        public string Test_RemoveAny_char_array(string text, char[] charsToRemove)
        {
            var result = text.RemoveAny(charsToRemove);

            return result;
        }

        [TestCase("", "1234567890.", ExpectedResult = "")]
        [TestCase(null, "1234567890.", ExpectedResult = null)]
        [TestCase("hello", "", ExpectedResult = "")]
        [TestCase("hello", null, ExpectedResult = "")]
        [TestCase("The amount to pay is: 123,456.00", "1234567890.", ExpectedResult = "123456.00")]
        public string Test_RemoveAnyExcept(string text, string charsToKeep)
        {
            var result = text.RemoveAnyExcept(charsToKeep);

            return result;
        }

        // GB
        [TestCase("0", "en-gb", ExpectedResult = true)]
        [TestCase("1", "en-gb", ExpectedResult = true)]
        [TestCase("-1", "en-gb", ExpectedResult = true)]
        [TestCase("+1", "en-gb", ExpectedResult = true)]
        [TestCase("123.72", "en-gb", ExpectedResult = true)]
        [TestCase("-123.72", "en-gb", ExpectedResult = true)]
        [TestCase("+123.72", "en-gb", ExpectedResult = true)]
        [TestCase("3,123.451", "en-gb", ExpectedResult = true)]
        [TestCase("-3,123.451", "en-gb", ExpectedResult = true)]
        [TestCase("+3,123.451", "en-gb", ExpectedResult = true)]
        [TestCase("3412123.76543", "en-gb", ExpectedResult = true)]
        [TestCase("-3412123.76543", "en-gb", ExpectedResult = true)]
        [TestCase("+3412123.76543", "en-gb", ExpectedResult = true)]
        [TestCase("7,034.989", "en-gb", ExpectedResult = true)]
        [TestCase("-7,034.989", "en-gb", ExpectedResult = true)]
        [TestCase("+7,034.989", "en-gb", ExpectedResult = true)]
        // DE
        [TestCase("3,123.451", "de-DE", ExpectedResult = false)]
        [TestCase("3.123,451", "de-DE", ExpectedResult = true)]
        public bool Test_IsValidNumber_default_culture(string text, string cultureInfoName)
        {
            var previousCulture = CultureInfo.DefaultThreadCurrentCulture;

            try
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(cultureInfoName);

                var result = text.IsValidNumber();

                return result;
            }
            finally
            {
                CultureInfo.DefaultThreadCurrentCulture = previousCulture;
            }
        }

        // GB
        [TestCase("0", "en-gb", ExpectedResult = true)]
        [TestCase("1", "en-gb", ExpectedResult = true)]
        [TestCase("-1", "en-gb", ExpectedResult = true)]
        [TestCase("+1", "en-gb", ExpectedResult = true)]
        [TestCase("123.72", "en-gb", ExpectedResult = true)]
        [TestCase("-123.72", "en-gb", ExpectedResult = true)]
        [TestCase("+123.72", "en-gb", ExpectedResult = true)]
        [TestCase("3,123.451", "en-gb", ExpectedResult = true)]
        [TestCase("-3,123.451", "en-gb", ExpectedResult = true)]
        [TestCase("+3,123.451", "en-gb", ExpectedResult = true)]
        [TestCase("3412123.76543", "en-gb", ExpectedResult = true)]
        [TestCase("-3412123.76543", "en-gb", ExpectedResult = true)]
        [TestCase("+3412123.76543", "en-gb", ExpectedResult = true)]
        [TestCase("7,034.989", "en-gb", ExpectedResult = true)]
        [TestCase("-7,034.989", "en-gb", ExpectedResult = true)]
        [TestCase("+7,034.989", "en-gb", ExpectedResult = true)]
        // DE
        [TestCase("3,123.451", "de-DE", ExpectedResult = false)]
        [TestCase("3.123,451", "de-DE", ExpectedResult = true)]
        public bool Test_IsValidNumber(string text, string cultureInfoName)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(cultureInfoName);

            var result = text.IsValidNumber(cultureInfo);

            return result;
        }

        [TestCase("hello world", ExpectedResult = "dlrow olleh")]
        [TestCase("12345", ExpectedResult = "54321")]
        [TestCase("abcba", ExpectedResult = "abcba")]
        [TestCase("", ExpectedResult = "")]
        [TestCase(null, ExpectedResult = null)]
        public string Test_Reverse(string text)
        {
            var result = text.Reverse();

            return result;
        }

        [TestCase("a-b-c-d-e", "-", StringSplitOptions.None, ExpectedResult = "a,b,c,d,e")]
        [TestCase("a-b[c]d=e", "-[]=", StringSplitOptions.None, ExpectedResult = "a,b,c,d,e")]
        [TestCase("a-b--d-e", "-", StringSplitOptions.RemoveEmptyEntries, ExpectedResult = "a,b,d,e")]
        [TestCase("a-b[]d=e", "-[]=", StringSplitOptions.RemoveEmptyEntries, ExpectedResult = "a,b,d,e")]
        [TestCase("a-b[]d=e", "-[]=", StringSplitOptions.None, ExpectedResult = "a,b,,d,e")]
        public string Test_SplitText(string text, string delimiters, StringSplitOptions options)
        {
            var result = text.SplitText(delimiters, options);

            return string.Join(",", result);
        }

        [TestCase("a-b-c-d-e", "-", StringSplitOptions.None, SplitDelimiterType.Any, ExpectedResult = "a,b,c,d,e")]
        [TestCase("a-b[c]d=e", "-[]=", StringSplitOptions.None, SplitDelimiterType.Any, ExpectedResult = "a,b,c,d,e")]
        [TestCase("a-b--d-e", "-", StringSplitOptions.RemoveEmptyEntries, SplitDelimiterType.Any, ExpectedResult = "a,b,d,e")]
        [TestCase("a-b[]d=e", "-[]=", StringSplitOptions.RemoveEmptyEntries, SplitDelimiterType.Any, ExpectedResult = "a,b,d,e")]
        [TestCase("a-b[]d=e", "-[]=", StringSplitOptions.None, SplitDelimiterType.Any, ExpectedResult = "a,b,,d,e")]
        [TestCase("a-b-c-d-e", "-", StringSplitOptions.None, SplitDelimiterType.All, ExpectedResult = "a,b,c,d,e")]
        [TestCase("a-b--d-e", "--", StringSplitOptions.RemoveEmptyEntries, SplitDelimiterType.All, ExpectedResult = "a-b,d-e")]
        [TestCase("a-b----d-e", "--", StringSplitOptions.None, SplitDelimiterType.All, ExpectedResult = "a-b,,d-e")]
        [TestCase("a-b[]d=e", "[]", StringSplitOptions.RemoveEmptyEntries, SplitDelimiterType.All, ExpectedResult = "a-b,d=e")]
        public string Test_SplitText_DelimiterType(string text, string delimiters, StringSplitOptions options, SplitDelimiterType delimiterType)
        {
            var result = text.SplitText(delimiters, options, delimiterType);

            return string.Join(",", result);
        }

        [Test]
        public void Test_SplitText_InvalidDelimiterType()
        {
            // Arrange
            var text = "a,b,c";
            var delimiters = ",";

            // Act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => text.SplitText(delimiters, StringSplitOptions.None, (SplitDelimiterType) int.MaxValue)
            );

            // Assert
            ex.ShouldNotBeNull();
            ex.ParamName.ShouldBe("delimiterType");
            ex.Message.ShouldContain(nameof(SplitDelimiterType.Any));
            ex.Message.ShouldContain(nameof(SplitDelimiterType.All));
        }

        [TestCase("a-b-c-d-e", "-", StringSplitOptions.None, ExpectedResult = "a,b,c,d,e")]
        [TestCase("a-b[c]d-e", "-", StringSplitOptions.None, ExpectedResult = "a,b[c]d,e")]
        [TestCase("a-b--d-e", "-", StringSplitOptions.RemoveEmptyEntries, ExpectedResult = "a,b,d,e")]
        [TestCase("a-b[]d=e", "[]", StringSplitOptions.RemoveEmptyEntries, ExpectedResult = "a-b,d=e")]
        [TestCase("a-b//d=e", "/", StringSplitOptions.None, ExpectedResult = "a-b,,d=e")]
        [TestCase("a-b//d=e", "/", StringSplitOptions.RemoveEmptyEntries, ExpectedResult = "a-b,d=e")]
        public string Test_SplitByText(string text, string delimiters, StringSplitOptions options)
        {
            var result = text.SplitByText(delimiters, options);

            return string.Join(",", result);
        }

        [TestCase("Line 1/Line 2//Line 4", StringSplitOptions.None, StringComparison.CurrentCulture, ExpectedResult = "Line 1/Line 2//Line 4")]
        [TestCase("Line 1/Line 2//Line 4", StringSplitOptions.RemoveEmptyEntries, StringComparison.CurrentCulture, ExpectedResult = "Line 1/Line 2/Line 4")]
        [TestCase("Line 1///Line 2//Line 4/", StringSplitOptions.None, StringComparison.CurrentCulture, ExpectedResult = "Line 1///Line 2//Line 4")]
        [TestCase("Line 1/Line 2//Line 4", StringSplitOptions.RemoveEmptyEntries, StringComparison.CurrentCulture, ExpectedResult = "Line 1/Line 2/Line 4")]
        public string Test_SplitByText_ComplexDelimiter(string text, StringSplitOptions options, StringComparison comparison)
        {
            var delimiters = Environment.NewLine;

            var adjustedText = string.Join(Environment.NewLine, text.Split('/'));

            var  lines = adjustedText.SplitByText(delimiters, options, comparison);

            return string.Join("/", lines);
        }

        [TestCase("a", "b", "c", ExpectedResult = "a")]
        [TestCase("a", null, null, ExpectedResult = "a")]
        [TestCase(null, "b", "c", ExpectedResult = "b")]
        [TestCase(null, null, "c", ExpectedResult = "c")]
        [TestCase(null, null, null, ExpectedResult = null)]
        [TestCase("a", "b", "c", ExpectedResult = "a")]
        [TestCase("a", "", "", ExpectedResult = "a")]
        [TestCase("", "b", "c", ExpectedResult = "")]
        [TestCase("", "", "c", ExpectedResult = "")]
        [TestCase("", "", "", ExpectedResult = "")]

        public string Test_CoalesceNull(string a, string b, string c)
        {
            var result = StringExtensions.CoalesceNull(a, b, c);

            return result;
        }

        [TestCase("a", "b", "c", ExpectedResult = "a")]
        [TestCase("a", null, null, ExpectedResult = "a")]
        [TestCase(null, "b", "c", ExpectedResult = "b")]
        [TestCase(null, null, "c", ExpectedResult = "c")]
        [TestCase(null, null, null, ExpectedResult = null)]
        [TestCase("a", "b", "c", ExpectedResult = "a")]
        [TestCase("a", "", "", ExpectedResult = "a")]
        [TestCase("", "b", "c", ExpectedResult = "b")]
        [TestCase("", "", "c", ExpectedResult = "c")]
        [TestCase("", "", "", ExpectedResult = null)]

        public string Test_CoalesceNullOrEmpty(string a, string b, string c)
        {
            var result = StringExtensions.CoalesceNullOrEmpty(a, b, c);

            return result;
        }

        [TestCase("a", "b", "c", ExpectedResult = "a")]
        [TestCase(" ", "b", "c", ExpectedResult = "b")]
        [TestCase("  ", "   ", "c", ExpectedResult = "c")]
        [TestCase(" ", "  ", null, ExpectedResult = null)]

        public string Test_CoalesceNullOrWhitespace(string a, string b, string c)
        {
            var result = StringExtensions.CoalesceNullOrWhitespace(a, b, c);

            return result;
        }

        [TestCase(null, null, null)]
        [TestCase("", null, "")]
        [TestCase("TwoWords", null, "Two Words")]
        [TestCase("ThreeWordsTogether", null, "Three Words Together")]
        [TestCase("pascalCase", null, "pascal Case")]
        [TestCase("Already Spaced", null, "Already Spaced")]
        [TestCase("AnEntireSentenceSquashedTogetherIntoOneSingleWord", null, "An Entire Sentence Squashed Together Into One Single Word")]
        [TestCase("IsITVDramaBetterThanBBCDrama", null, "Is ITV Drama Better Than BBC Drama")]
        [TestCase("IsTheHoLAnOutdatedInstitution", null, "Is The Ho L An Outdated Institution")]
        [TestCase("IsTheHoLAnOutdatedInstitution", "HoL", "Is The HoL An Outdated Institution")]
        [TestCase("VirusesLikeH1N1AndH5N1WereWarningsForThePanDemicToCome", null, "Viruses Like H1N1 And H5N1 Were Warnings For The Pan Demic To Come")]
        [TestCase("VirusesLikeH1N1AndH5N1WereWarningsForThePanDemicToCome", "H1N1|H5N1|PanDemic", "Viruses Like H1N1 And H5N1 Were Warnings For The PanDemic To Come")]
        public void Wordify_Tests(string text, string preservedWordsText, string expected)
        {
            // Arrange
            var preservedWords = preservedWordsText?
                .Split("|")
                .ToArray();

            // Act
            var result = text.Wordify(preservedWords);

            // Assert
            result.ShouldBe(expected);
        }
    }
}
