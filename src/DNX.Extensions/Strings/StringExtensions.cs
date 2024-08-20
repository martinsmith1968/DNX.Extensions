using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DNX.Extensions.Enumerations;

// ReSharper disable InconsistentNaming

// ReSharper disable InvertIf
// ReSharper disable LoopCanBeConvertedToQuery

namespace DNX.Extensions.Strings;

/// <summary>
/// String Extensions
/// </summary>
public static class StringExtensions
{
    private const string Wordify_RegexText = @"\B[A-Z]";
    private static readonly Regex Wordify_Regex = new(Wordify_RegexText);

    /// <summary>
    /// Determines whether the specified text is null or empty
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns><c>true</c> if [is null or empty] [the specified text]; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty(this string text)
    {
        return string.IsNullOrEmpty(text);
    }

    /// <summary>
    /// Determines whether the specified text is null or white space
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns><c>true</c> if [is null or white space] [the specified text]; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrWhiteSpace(this string text)
    {
        return string.IsNullOrWhiteSpace(text);
    }

    /// <summary>
    /// Return null is text is null or empty
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string NullIfEmpty(this string text)
    {
        return string.IsNullOrEmpty(text) ? null : text;
    }

    /// <summary>
    /// Return null if text is null or whitespace
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string NullIfEmptyOrWhitespace(this string text)
    {
        return string.IsNullOrWhiteSpace(text) ? null : text;
    }

    /// <summary>
    /// Ensure a string starts with a prefix string
    /// </summary>
    /// <param name="text"></param>
    /// <param name="prefix"></param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string EnsureStartsWith(this string text, string prefix)
    {
        if (!string.IsNullOrEmpty(prefix))
        {
            if (string.IsNullOrEmpty(text) || !text.StartsWith(prefix))
            {
                text = string.Concat(prefix, text);
            }
        }

        return text;
    }

    /// <summary>
    /// Ensures a string ends with a suffix string.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="suffix">The suffix.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string EnsureEndsWith(this string text, string suffix)
    {
        if (!string.IsNullOrEmpty(suffix))
        {
            if (string.IsNullOrEmpty(text) || !text.EndsWith(suffix))
            {
                text = string.Concat(text, suffix);
            }
        }

        return text;
    }

    /// <summary>
    /// Ensures a string starts and ends with a prefix / suffix string.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="prefixSuffix">The prefix / suffix.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string EnsureStartsAndEndsWith(this string text, string prefixSuffix)
    {
        return text.EnsureStartsWith(prefixSuffix)
            .EnsureEndsWith(prefixSuffix);
    }

    /// <summary>
    /// Ensures a string starts a prefix string and ends with a suffix string.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="prefix">The prefix.</param>
    /// <param name="suffix">The suffix.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string EnsureStartsAndEndsWith(this string text, string prefix, string suffix)
    {
        return text.EnsureStartsWith(prefix)
            .EnsureEndsWith(suffix);
    }

    /// <summary>
    /// Ensures a string does not start with a prefix string
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="prefix">The prefix.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string RemoveStartsWith(this string text, string prefix)
    {
        if (!string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(text))
        {
            while (text.StartsWith(prefix))
            {
                text = text.Substring(prefix.Length);
            }
        }

        return text;
    }

    /// <summary>
    /// Ensures a string does not end with a suffix string
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="suffix">The suffix.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string RemoveEndsWith(this string text, string suffix)
    {
        if (!string.IsNullOrEmpty(suffix) && !string.IsNullOrEmpty(text))
        {
            while (text.EndsWith(suffix))
            {
                text = text.Substring(0, text.Length - suffix.Length);
            }
        }

        return text;
    }

    /// <summary>
    /// Ensures a string does not start or end with a prefix / suffix string
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="prefixSuffix">The prefix and suffix.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string RemoveStartsAndEndsWith(this string text, string prefixSuffix)
    {
        return text.RemoveStartsAndEndsWith(prefixSuffix, prefixSuffix);
    }

    /// <summary>
    /// Ensures a string does not start with a prefix string or end with a suffix string
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="prefix">The prefix.</param>
    /// <param name="suffix">The suffix.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string RemoveStartsAndEndsWith(this string text, string prefix, string suffix)
    {
        return text.RemoveStartsWith(prefix)
            .RemoveEndsWith(suffix);
    }

    /// <summary>
    /// Gets the text between the specified start and end text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="startText">The start text.</param>
    /// <param name="endText">The end text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>System.String.</returns>
    public static string Between(this string text, string startText, string endText, StringComparison comparison = StringComparison.Ordinal)
    {
        return text
                .After(startText, comparison)
                .Before(endText, comparison)
            ;
    }

    /// <summary>
    /// Gets the text between the first specified start and the last end text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="startText">The start text.</param>
    /// <param name="endText">The end text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>System.String.</returns>
    public static string BetweenFirstAndLast(this string text, string startText, string endText, StringComparison comparison = StringComparison.Ordinal)
    {
        return text
                .After(startText, comparison)
                .BeforeLast(endText, comparison)
            ;
    }

    /// <summary>
    /// Gets the text before the specified end text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="endText">The end text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>System.String.</returns>
    public static string Before(this string text, string endText, StringComparison comparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var endIndex = string.IsNullOrEmpty(endText)
            ? -1
            : text.IndexOf(endText, comparison);

        var result = endIndex >= 0
            ? text.Substring(0, endIndex)
            : null;

        return result;
    }

    /// <summary>
    /// Gets the text before the last instance of the specified end text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="endText">The end text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>System.String.</returns>
    public static string BeforeLast(this string text, string endText, StringComparison comparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var endIndex = string.IsNullOrEmpty(endText)
            ? -1
            : text.LastIndexOf(endText, comparison);

        var result = endIndex >= 0
            ? text.Substring(0, endIndex)
            : null;

        return result;
    }

    /// <summary>
    /// Gets the text after the specified start text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="startText">The start text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>System.String.</returns>
    public static string After(this string text, string startText, StringComparison comparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var startTextLength = startText?.Length ?? 0;

        var startIndex = string.IsNullOrEmpty(startText)
            ? -1
            : text.IndexOf(startText, comparison);

        var result = startIndex >= 0
            ? text.Substring(startIndex + startTextLength)
            : null;

        return result;
    }

    /// <summary>
    /// Gets the text after the last instance of the specified start text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="startText">The start text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>System.String.</returns>
    public static string AfterLast(this string text, string startText, StringComparison comparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(text))
        {
            return null;
        }

        var startTextLength = startText?.Length ?? 0;

        var startIndex = string.IsNullOrEmpty(startText)
            ? -1
            : text.LastIndexOf(startText, comparison);

        var result = startIndex >= 0
            ? text.Substring(startIndex + startTextLength)
            : null;

        return result;
    }

    /// <summary>
    /// Gets the text after the specified start text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="startText">The start text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>System.String.</returns>
    public static string From(this string text, string startText, StringComparison comparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(startText))
        {
            return null;
        }

        var startIndex = text.IndexOf(startText, comparison);

        var result = startIndex >= 0
            ? text.Substring(startIndex)
            : null;

        return result;
    }

    /// <summary>
    /// Gets the text after the specified start text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="startText">The start text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>System.String.</returns>
    public static string FromLast(this string text, string startText, StringComparison comparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(startText))
        {
            return null;
        }

        var startIndex = text.LastIndexOf(startText, comparison);

        var result = startIndex >= 0
            ? text.Substring(startIndex)
            : null;

        return result;
    }

    /// <summary>
    /// Gets the text before the specified end text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="endText">The end text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>System.String.</returns>
    public static string AsFarAs(this string text, string endText, StringComparison comparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(endText))
        {
            return null;
        }

        var endTextLength = endText?.Length ?? 0;

        var endIndex = text.IndexOf(endText, comparison);

        var result = endIndex >= 0
            ? text.Substring(0, endIndex + endTextLength)
            : null;

        return result;
    }

    /// <summary>
    /// Gets the text before the last instance of the specified end text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="endText">The end text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>System.String.</returns>
    public static string AsFarAsLast(this string text, string endText, StringComparison comparison = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(endText))
        {
            return null;
        }

        var endTextLength = endText?.Length ?? 0;

        var endIndex = text.LastIndexOf(endText, comparison);

        var result = endIndex >= 0
            ? text.Substring(0, endIndex + endTextLength)
            : null;

        return result;
    }

    /// <summary>
    /// Determines whether the text contains the specified search text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="searchText">The search text.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns><c>true</c> if the specified search text contains text; otherwise, <c>false</c>.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static bool ContainsText(this string text, string searchText, StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
    {
        if (text == null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(searchText))
        {
            return true;
        }

        return text.IndexOf(searchText, comparison) >= 0;
    }

    /// <summary>
    /// Determines whether text contains only the specified characters.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="characters">The characters.</param>
    /// <returns><c>true</c> if the specified characters contains only; otherwise, <c>false</c>.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static bool ContainsOnly(this string text, string characters)
    {
        return ContainsOnly(text, (characters ?? string.Empty).ToCharArray());
    }

    /// <summary>
    /// Determines whether text contains only the specified characters.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="characters">The characters.</param>
    /// <returns>System.Boolean.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static bool ContainsOnly(this string text, IList<char> characters)
    {
        if (text == null || !characters.HasAny())
        {
            return false;
        }

        foreach (var ch in text)
        {
            if (!characters.Contains(ch))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Removes any of the specified characters from a string
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="charsToRemove">The chars to remove.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string RemoveAny(this string text, string charsToRemove)
    {
        return RemoveAny(text, charsToRemove.ToCharArray());
    }

    /// <summary>
    /// Removes any of the specified characters from a string
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="charsToRemove">The chars to remove.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string RemoveAny(this string text, char[] charsToRemove)
    {
        if (charsToRemove.HasAny() && !string.IsNullOrEmpty(text))
        {
            foreach (var c in charsToRemove)
            {
                text = text.Replace(c.ToString(), string.Empty);
            }
        }

        return text;
    }

    /// <summary>
    /// Removes any characters from a string so that only instances of the specified characters remain
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="charsToKeep">The chars to keep.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string RemoveAnyExcept(this string text, string charsToKeep)
    {
        return RemoveAnyExcept(text, (charsToKeep ?? string.Empty).ToCharArray());
    }

    /// <summary>
    /// Removes any characters from a string so that only instances of the specified characters remain
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="charsToKeep">The chars to keep.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string RemoveAnyExcept(this string text, IList<char> charsToKeep)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        var sb = new StringBuilder(text.Length);

        if (charsToKeep.HasAny())
        {
            foreach (var c in text.ToCharArray())
            {
                if (charsToKeep.Contains(c))
                {
                    sb.Append(c);
                }
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Reverses the specified text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string Reverse(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        var charArray = text.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    /// <summary>
    /// Splits the text by the specified delimiters.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="delimiters">The delimiters.</param>
    /// <param name="options">The options.</param>
    /// <param name="delimiterType">Type of the delimiter.</param>
    /// <returns>IEnumerable&lt;System.String&gt;.</returns>
    /// <exception cref="ArgumentOutOfRangeException">delimiterType - Value must be one of {string.Join(",", Enum.GetNames(typeof(SplitDelimiterType)))}</exception>
    /// <remarks>Also available as an extension method</remarks>
    public static IEnumerable<string> SplitText(this string text, string delimiters, StringSplitOptions options = StringSplitOptions.None, SplitDelimiterType delimiterType = SplitDelimiterType.Any)
    {
        return delimiterType switch
        {
            SplitDelimiterType.Any => text.Split(delimiters.ToCharArray(), options),
            SplitDelimiterType.All =>
                // NOTE: A native text.Split(string, ...) is available in NET Core 2.1+
                text.SplitByText(delimiters, options),
            _ => throw new ArgumentOutOfRangeException(nameof(delimiterType), delimiterType, $"Value must be one of {string.Join(",", Enum.GetNames(typeof(SplitDelimiterType)))}")
        };
    }

    /// <summary>
    /// Splits the text by the specified text string.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="delimiterText">The delimiter text.</param>
    /// <param name="options">The options.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>IEnumerable&lt;System.String&gt;.</returns>
    public static IEnumerable<string> SplitByText(this string text, string delimiterText, StringSplitOptions options = StringSplitOptions.None, StringComparison comparison = StringComparison.CurrentCulture)
    {
        var elements = new List<string>();

        var tempText = text;

        while (!string.IsNullOrEmpty(delimiterText) && tempText.Contains(delimiterText))
        {
            var index = tempText.IndexOf(delimiterText, comparison);

            var element = index == 0 ? string.Empty : tempText.Substring(0, index);

            if (options == StringSplitOptions.None || !string.IsNullOrEmpty(element))
            {
                elements.Add(element);
            }

            tempText = tempText.Substring(element.Length + delimiterText.Length);
        }

        if (!string.IsNullOrEmpty(tempText))
        {
            elements.Add(tempText);
        }

        return elements;
    }

    /// <summary>
    /// Coalesces this value with the first value that is not null or empty.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static string CoalesceNullWith(this string text, params string[] items)
    {
        return text ?? items.CoalesceNull();
    }

    /// <summary>
    /// Coalesces this value with the first value that is not null or empty.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static string CoalesceNullWith(this string text, IList<string> items)
    {
        return text ?? items.CoalesceNull();
    }

    /// <summary>
    /// Coalesces the list of strings to find the first not null
    /// </summary>
    /// <param name="strings">The strings.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string CoalesceNull(this IList<string> strings)
    {
        var value = strings.FirstOrDefault(s => s != null);

        return value;
    }

    /// <summary>
    /// Coalesces this value with the first value that is not null or empty.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static string CoalesceNullOrEmptyWith(this string text, params string[] items)
    {
        return string.IsNullOrEmpty(text)
            ? items.CoalesceNullOrEmpty()
            : text;
    }

    /// <summary>
    /// Coalesces this value with the first value that is not null or empty.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static string CoalesceNullOrEmptyWith(this string text, IList<string> items)
    {
        return string.IsNullOrEmpty(text)
            ? items.CoalesceNullOrEmpty()
            : text;
    }

    /// <summary>
    /// Coalesces the list of strings to find the first not null or empty.
    /// </summary>
    /// <param name="strings">The strings.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string CoalesceNullOrEmpty(this IList<string> strings)
    {
        var value = strings?.FirstOrDefault(s => !string.IsNullOrEmpty(s));

        return value;
    }

    /// <summary>
    /// Coalesces this value with the first value that is not null or empty.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static string CoalesceNullOrWhitespaceWith(this string text, params string[] items)
    {
        return string.IsNullOrWhiteSpace(text)
            ? items.CoalesceNullOrWhitespace()
            : text;
    }

    /// <summary>
    /// Coalesces this value with the first value that is not null or empty.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="items">The items.</param>
    /// <returns></returns>
    public static string CoalesceNullOrWhitespaceWith(this string text, IList<string> items)
    {
        return string.IsNullOrWhiteSpace(text)
            ? items.CoalesceNullOrWhitespace()
            : text;
    }

    /// <summary>
    /// Coalesces the list of strings to find the first not null or whitespace.
    /// </summary>
    /// <param name="strings">The strings.</param>
    /// <returns>System.String.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static string CoalesceNullOrWhitespace(this IList<string> strings)
    {
        var value = strings?.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));

        return value;
    }

    /// <summary>
    /// Builds the number validation regex for the specified cultureinfo
    /// </summary>
    /// <param name="cultureInfo">The culture information.</param>
    /// <returns>System.String.</returns>
    internal static string BuildNumberValidationRegexForCulture(CultureInfo cultureInfo)
    {
        var pattern =string.Format(
            @"^[\{2}\{3}]?(0|[1-9][0-9]*|[1-9][0-9]{{0,{4}}}(\{0}[0-9]{{{5},{5}}})*)([\{1}]+[0-9]+)?$",
            cultureInfo.NumberFormat.NumberGroupSeparator,
            cultureInfo.NumberFormat.NumberDecimalSeparator,
            cultureInfo.NumberFormat.NegativeSign,
            cultureInfo.NumberFormat.PositiveSign,
            cultureInfo.NumberFormat.NumberGroupSizes.First() - 1,
            cultureInfo.NumberFormat.NumberGroupSizes.First()
        );

        return pattern;
    }

    /// <summary>
    /// Determines whether the specified text is numeric conforming to the current Culture NumberFormat.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns><c>true</c> if the specified culture information is numeric; otherwise, <c>false</c>.</returns>
    public static bool IsValidNumber(this string text)
    {
        var pattern = BuildNumberValidationRegexForCulture(CultureInfo.DefaultThreadCurrentCulture);

        return Regex.IsMatch(text, pattern);
    }

    /// <summary>
    /// Determines whether the specified text is numeric conforming to the specified Culture NumberFormat.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="cultureInfo">The culture information.</param>
    /// <returns><c>true</c> if the specified culture information is numeric; otherwise, <c>false</c>.</returns>
    public static bool IsValidNumber(this string text, CultureInfo cultureInfo)
    {
        var pattern = BuildNumberValidationRegexForCulture(cultureInfo);

        return Regex.IsMatch(text, pattern);
    }

    /// <summary>
    /// Add spaces to separate the capitalized words in the string,
    /// i.e. insert a space before each uppercase letter that is
    /// either preceded by a lowercase letter or followed by a
    /// lowercase letter (but not for the first char in string).
    /// This keeps groups of uppercase letters - e.g. preservedWords - together.
    /// </summary>
    /// <param name="text">A string in PascalCase</param>
    /// <returns></returns>
    public static string Wordify(this string text)
    {
        return text.Wordify(null);
    }

    /// <summary>
    /// Add spaces to separate the capitalized words in the string,
    /// i.e. insert a space before each uppercase letter that is
    /// either preceded by a lowercase letter or followed by a
    /// lowercase letter (but not for the first char in string).
    /// This keeps groups of uppercase letters - e.g. acronyms - together.
    /// </summary>
    /// <param name="text">A string in PascalCase</param>
    /// <param name="preservedWords">The preservedWords - beyond acronyms</param>
    /// <returns>System.String.</returns>
    public static string Wordify(this string text, IList<string> preservedWords)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var result = Wordify_Regex.Replace(text, " $0");

        if (preservedWords.HasAny())
        {
            var acronymDict = preservedWords
                    .Distinct()
                    .ToDictionary(x => x.Wordify(), x => x)
                ;

            result = acronymDict.Aggregate(result,
                (current, dict) => current.Replace(dict.Key, dict.Value)
            );
        }

        return result;
    }

    /// <summary>
    /// Truncates a string to the given length if it is longer than the given length, otherwise returns the original string.
    /// </summary>
    public static string Truncate(this string input, int length)
    {
        return input != null && input.Length > length
            ? input.Substring(0, length)
            : input;
    }

    /// <summary>
    /// Converts an ASCII encoded string to Hexadecimal
    /// </summary>
    /// <param name="input">ASCII Encoded Input</param>
    /// <returns>Input as Hexadecimal</returns>
    public static string ToHexString(this string input, string format = "X2")
    {
        return string.Join("", input.Select(c => ((int)c).ToString(format)));
    }

    /// <summary>
    /// Converts string into a byte[] using the default encoding (UTF-8)
    /// </summary>
    /// <param name="input">The string to turn into the byte[]</param>
    public static byte[] GetBytes(this string input)
    {
        return input.GetBytes(Encoding.UTF8);
    }

    /// <summary>
    /// Converts string into a byte[] using the specified encoding
    /// </summary>
    /// <param name="input">The string to turn into the byte[]</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns></returns>
    public static byte[] GetBytes(this string input, Encoding encoding)
    {
        encoding ??= Encoding.UTF8;

        return encoding.GetBytes(input);
    }
}
