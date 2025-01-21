using System;
using DNX.Extensions.Exceptions;

//==============================================================================
// This source file was generated by a script - do not edit manually
//==============================================================================

namespace DNX.Extensions.Converters.BuiltInTypes;

/// <summary>
/// Class ConvertDecimalExtensions.
/// </summary>
public static class ConvertDecimalExtensions
{
    /// <summary>
    /// Converts the string to a decimal
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>decimal</returns>
    /// <exception cref="DNX.Extensions.Exceptions.ConversionException">Unable to convert value to Type</exception>
    public static decimal ToDecimal(this string text)
    {
        decimal result;

        if (!decimal.TryParse(text, out result))
        {
            throw new ConversionException(text, "Unable to convert value to Type", typeof(decimal));
        }

        return result;
    }

    /// <summary>
    /// Converts the string to a decimal, or returns the default value if the conversion fails
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>decimal</returns>
    public static decimal ToDecimal(this string text, decimal defaultValue)
    {
        try
        {
            var result = text.ToDecimal();

            return result;
        }
        catch (ConversionException)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Determines if the string can be converted to a decimal or not
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns><c>true</c> if the specified text is a decimal; otherwise, <c>false</c>.</returns>
    public static bool IsDecimal(this string text)
    {
        try
        {
            text.ToDecimal();

            return true;
        }
        catch (ConversionException)
        {
            return false;
        }
    }
}

