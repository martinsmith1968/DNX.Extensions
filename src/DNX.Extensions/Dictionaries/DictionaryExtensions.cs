using System;
using System.Collections.Generic;
using System.Linq;

namespace DNX.Extensions.Dictionaries;

/// <summary>
/// Dictionary Extensions
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Gets the specified key value
    /// </summary>
    /// <typeparam name="TK">The type of the k.</typeparam>
    /// <typeparam name="TV">The type of the v.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static TV Get<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV defaultValue = default)
    {
        return dictionary != null && key != null && dictionary.TryGetValue(key, out var value)
            ? value
            : defaultValue;
    }

    /// <summary>
    /// Converts a string to Dictionary&lt;string, string&gt;
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="elementSeparator">The element separator.</param>
    /// <param name="valueSeparator">The value separator.</param>
    /// <returns></returns>
    public static IDictionary<string, string> ToStringDictionary(this string text, string elementSeparator = "|", string valueSeparator = "=")
    {
        var dictionary = (text ?? string.Empty)
            .Split(elementSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .ToDictionary(
                x => x.Split(valueSeparator.ToCharArray()).FirstOrDefault(),
                x => string.Join(valueSeparator, x.Split(valueSeparator.ToCharArray()).Skip(1))
            );

        return dictionary;
    }

    /// <summary>
    /// Converts a string to Dictionary&lt;string, object&gt;
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="elementSeparator">The element separator.</param>
    /// <param name="valueSeparator">The value separator.</param>
    /// <returns></returns>
    public static IDictionary<string, object> ToStringObjectDictionary(this string text, string elementSeparator = "|", string valueSeparator = "=")
    {
        var dictionary = text
            .ToStringDictionary(elementSeparator, valueSeparator)
            .ToDictionary(
                x => x.Key,
                x => (object)x.Value
            );

        return dictionary;
    }
}
