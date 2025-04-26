using System.Text;

namespace DNX.Extensions.Arrays;

/// <summary>
/// Byte Array Extensions
/// </summary>
public static class ByteArrayExtensions
{
    //private static string Base62CodingSpace = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Converts A byte array into an ASCII string
    /// </summary>
    /// <param name="input">The byte[] to turn into the string</param>
    public static string GetAsciiString(this byte[] input)
    {
        return Encoding.ASCII.GetString(input);
    }

    /// <summary>
    /// Converts A byte array into an hex string
    /// </summary>
    /// <param name="input">The byte[] to turn into the string</param>
    /// <param name="format">The format to use for each byte</param>
    public static string ToHexString(this byte[] input, string format = "X2")
    {
        var hex = new StringBuilder(input.Length * 2);
        var byteFormat = "{0:" + format + "}";
        foreach (var b in input)
            hex.AppendFormat(byteFormat, b);
        return hex.ToString();
    }
}
