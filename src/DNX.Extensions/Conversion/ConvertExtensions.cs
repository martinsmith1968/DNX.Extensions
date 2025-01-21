using System;

namespace DNX.Extensions.Conversion;

/// <summary>
/// Conversion Extensions.
/// </summary>
public static class ConvertExtensions
{
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

    /// <summary>
    /// Changes the value to the specified type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <returns>T.</returns>
    public static T ChangeType<T>(this object value)
    {
        return (T)ChangeType(value, typeof(T));
    }

    /// <summary>
    /// Changes the value to the specified type
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="type">The type.</param>
    /// <returns>object</returns>
    public static object ChangeType(this object value, Type type)
    {
        return Convert.ChangeType(value, type);
    }

    /// <summary>
    /// Changes the value to the specified type, with a default value if conversion fails
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>T.</returns>
    public static T ChangeType<T>(this object value, T defaultValue)
    {
        try
        {
            return ChangeType<T>(value);
        }
        catch
        {
            return defaultValue;
        }
    }
}
