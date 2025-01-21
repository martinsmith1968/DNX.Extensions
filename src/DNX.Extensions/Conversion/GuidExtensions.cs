using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using DNX.Extensions.Arrays;

// ReSharper disable InconsistentNaming

namespace DNX.Extensions.Conversion;

/// <summary>
/// Extensions for working with Guids
/// </summary>
public static class GuidExtensions
{
    public const int MAX_GUID_BYTE_ARRAY_SIZE = 16;

    private static byte KeepOriginalValue(byte value1, byte value2) => value1;
    private static byte ExclusiveOr(byte value1, byte value2) => (byte)(value1 ^ value2);

    /// <summary>Convert any text item to a guid.</summary>
    /// <param name="input">The text to convert</param>
    /// <returns>A <see cref="Guid" /></returns>
    /// <remarks>
    /// The result is deterministic in that each text item will always generate the same result
    /// NOTE: If the text item is actually a Guid, it will NOT be parsed directly to a Guid
    /// </remarks>
    public static Guid ToDeterministicGuid(this string input)
    {
        return ToDeterministicGuid(input, MD5.Create(), KeepOriginalValue);
    }

    /// <summary>Convert any text item to a guid.</summary>
    /// <param name="input">The text to convert</param>
    /// <param name="provider">The hash provider.</param>
    /// <returns>A <see cref="Guid" /></returns>
    /// <remarks>
    /// The result is deterministic in that each text item will always generate the same result
    /// NOTE: If the text item is actually a Guid, it will NOT be parsed directly to a Guid
    /// </remarks>
    public static Guid ToDeterministicGuid(this string input, HashAlgorithm provider)
    {
        return ToDeterministicGuid(input, provider, KeepOriginalValue);
    }

    /// <summary>Convert any text item to a guid.</summary>
    /// <param name="input">The text to convert</param>
    /// <param name="provider">The hash provider.</param>
    /// <param name="reduceMethod">The method to use to reduce an oversized byte array to something a GUID will accept.</param>
    /// <returns>A <see cref="Guid" /></returns>
    /// <remarks>
    /// The result is deterministic in that each text item will always generate the same result
    /// NOTE: If the text item is actually a Guid, it will NOT be parsed directly to a Guid
    /// </remarks>
    public static Guid ToDeterministicGuid(this string input, HashAlgorithm provider, Func<byte, byte, byte> reduceMethod)
    {
        input ??= string.Empty;

        var inputBytes = Encoding.Default.GetBytes(input);

        var hashBytes = provider.ComputeHash(inputBytes);
        hashBytes = hashBytes.Reduce(MAX_GUID_BYTE_ARRAY_SIZE, reduceMethod);

        //generate a guid from the hash:
        var hashGuid = new Guid(hashBytes);

        return hashGuid;
    }

    /// <summary>
    /// Converts the value to a Guid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A Guid equivalent of the value</returns>
    public static Guid ToGuid(this short value)
        => new BigInteger(value).ToGuid();

    /// <summary>
    /// Converts the value to a Guid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A Guid equivalent of the value</returns>
    public static Guid ToGuid(this int value)
        => new BigInteger(value).ToGuid();

    /// <summary>
    /// Converts the value to a Guid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A Guid equivalent of the value</returns>
    public static Guid ToGuid(this uint value)
        => new BigInteger(value).ToGuid();

    /// <summary>
    /// Converts the value to a Guid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A Guid equivalent of the value</returns>
    public static Guid ToGuid(this long value)
        => new BigInteger(value).ToGuid();

    /// <summary>
    /// Converts the value to a Guid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A Guid equivalent of the value</returns>
    public static Guid ToGuid(this ulong value)
        => new BigInteger(value).ToGuid();

    /// <summary>
    /// Converts the value to a Guid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A Guid equivalent of the value</returns>
    public static Guid ToGuid(this float value)
        => new BigInteger(value).ToGuid();

    /// <summary>
    /// Converts the value to a Guid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A Guid equivalent of the value</returns>
    public static Guid ToGuid(this decimal value)
        => new BigInteger(value).ToGuid();

    /// <summary>
    /// Converts the value to a Guid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A Guid equivalent of the value</returns>
    public static Guid ToGuid(this double value)
        => new BigInteger(value).ToGuid();
}
