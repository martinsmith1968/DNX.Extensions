using System;
using System.Security.Cryptography;
using System.Text;

namespace DNX.Extensions.Conversion;

/// <summary>
/// Extensions for working with Guids
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// Convert any text item to a guid.
    /// </summary>
    /// <remarks>
    /// The result is deterministic in that each text item will always generate the same result
    /// NOTE: If the text item is actually a Guid, it will NOT be parsed directly to a Guid
    /// </remarks>
    /// <param name="input"></param>
    /// <returns>
    /// A <see cref="Guid"/>
    /// </returns>
    public static Guid ToDeterministicGuid(this string input)
    {
        input ??= string.Empty;

        //use MD5 hash to get a 16-byte hash of the string:
        using var provider = new MD5CryptoServiceProvider();

        var inputBytes = Encoding.Default.GetBytes(input);
        var hashBytes = provider.ComputeHash(inputBytes);

        //generate a guid from the hash:
        var hashGuid = new Guid(hashBytes);

        return hashGuid;
    }
}
