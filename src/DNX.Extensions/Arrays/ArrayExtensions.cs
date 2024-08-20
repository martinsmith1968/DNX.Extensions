using System;

#pragma warning disable 1591

namespace DNX.Extensions.Arrays;

/// <summary>
/// Array Extensions
/// </summary>
public static class ArrayExtensions
{
    public static bool IsNullOrEmpty(this Array input)
    {
        return input == null || input.Length == 0;
    }

    public static T[] PadLeft<T>(this T[] input, int length)
    {
        var paddedArray = new T[length];
        var startIdx = length - input.Length;
        if (length >= input.Length)
        {
            Array.Copy(input, 0, paddedArray, startIdx, input.Length);
        }
        else
        {
            Array.Copy(input, Math.Abs(startIdx), paddedArray, 0, length);
        }

        return paddedArray;
    }

    public static T[] ShiftLeft<T>(this T[] input)
    {
        if (input == null)
        {
            return new T[0];
        }

        var shiftedArray = new T[input.Length];
        Array.Copy(input, 1, shiftedArray, 0, input.Length - 1);

        return shiftedArray;
    }
}
