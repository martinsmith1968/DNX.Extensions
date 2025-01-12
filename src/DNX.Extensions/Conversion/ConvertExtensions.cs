using System;

namespace DNX.Extensions.Conversion;

/// <summary>
/// Extensions to simplify type conversion
/// </summary>
public static class ConvertExtensions
{
    /// <summary>
    /// Converts to string, or default.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>System.String.</returns>
    public static string ToStringOrDefault(this object obj, string defaultValue = "")
    {
        return obj?.ToString() ?? defaultValue;
    }

    /// <summary>
    /// Converts to enum.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="text">The text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>T.</returns>
    public static T ToEnum<T>(this string text, T defaultValue = default)
        where T : struct
    {
        return Enum.TryParse(text, true, out T value)
            ? value
            : defaultValue;
    }

    /// <summary>
    /// Converts to guid.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>Guid.</returns>
    public static Guid ToGuid(this string text)
    {
        return text.ToGuid(Guid.Empty);
    }

    /// <summary>
    /// Converts to guid.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>Guid.</returns>
    public static Guid ToGuid(this string text, Guid defaultValue)
    {
        return Guid.TryParse(text, out var result)
            ? result
            : defaultValue;
    }

    /// <summary>
    /// Converts to the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">The object.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>T.</returns>
    public static T To<T>(this object obj, T defaultValue = default)
    {
        try
        {
            return (T)obj ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }
}
