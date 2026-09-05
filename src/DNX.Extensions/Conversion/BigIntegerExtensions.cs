using System;
using System.Linq;
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
        var bytes = new byte[16];
        var byteArray = value.ToByteArray().Take(16).ToArray();
        byteArray.CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}
