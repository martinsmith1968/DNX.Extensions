using System;
using System.Linq;

namespace DNX.Extensions.Strings;

public static class BuiltInTypesExtensions
{
    /// <summary>
    /// Converts a boolean to text.
    /// </summary>
    /// <param name="value">if set to <c>true</c> [value].</param>
    /// <param name="trueText">The true text.</param>
    /// <param name="falseText">The false text.</param>
    /// <returns></returns>
    public static string ToText(this bool value, string trueText = "True", string falseText = "False")
    {
        return value ? trueText : falseText;
    }

    /// <summary>
    /// Converts a boolean to text, using "Yes" and "No"
    /// </summary>
    /// <param name="value">if set to <c>true</c> [value].</param>
    /// <returns></returns>
    public static string ToYesNo(this bool value) => value.ToText("Yes", "No");

    /// <summary>
    /// Converts an int32 to hexstring.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="format">The format.</param>
    /// <returns></returns>
    public static string ToHexString(this int input, string format = "X2")
    {
        return input.ToString(format);
    }
}
