using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DNX.Extensions.Strings;

/// <summary>
/// Extensions for textual argument parsing
/// </summary>
public static class ArgumentParserExtensions
{
    /// <summary>
    /// Parses a raw command line string into an array of parts
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>IList&lt;System.String&gt;.</returns>
    /// <remarks>
    /// See: https://stackoverflow.com/questions/14655023/split-a-string-that-has-white-spaces-unless-they-are-enclosed-within-quotes
    /// </remarks>
    public static IList<string> ParseToIndividualArguments(this string text)
    {
        var parts = Regex.Matches(text, @"[\""].+?[\""]|[^ ]+")
            .Cast<Match>()
            .Select(m => m.Value.Trim("\"".ToCharArray()))
            .ToList();

        return parts;
    }
}
