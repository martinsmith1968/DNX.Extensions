using System;
using System.Numerics;

namespace DNX.Extensions.Conversion;

public static class BigIntegerExtensions
{
    /// <summary>
    /// Converts a Gid to a BigInteger.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>a BigInteger</returns>
    /// <remarks>
    /// Source : https://stackoverflow.com/questions/54353695/convert-a-guid-string-into-biginteger-and-vice-versa
    /// </remarks>
    public static BigInteger ToBigInteger(this Guid value)
    {
        var bigInt = new BigInteger(value.ToByteArray());
        return bigInt;
    }

    /// <summary>
    /// Converts a BigInteger to a Guid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A Guid</returns>
    /// <remarks>
    /// Source : https://stackoverflow.com/questions/54353695/convert-a-guid-string-into-biginteger-and-vice-versa
    /// </remarks>
    public static Guid ToGuid(this BigInteger value)
    {
        // BigInteger.ToByteArray() returns the minimal two's-complement representation, which
        // can be shorter than 16 bytes - the gap must be filled with the sign-extension byte
        // (0xFF for negative values), not always 0x00, or the round trip corrupts the value.
        var fill = (byte)(value.Sign < 0 ? 0xFF : 0x00);
        var bytes = new byte[16];
        for (var i = 0; i < bytes.Length; ++i)
        {
            bytes[i] = fill;
        }

        var byteArray = value.ToByteArray();
        var length = Math.Min(byteArray.Length, bytes.Length);
        Array.Copy(byteArray, bytes, length);

        return new Guid(bytes);
    }
}
