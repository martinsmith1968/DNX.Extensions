using System.Security.Cryptography;
using DNX.Extensions.Conversion;
using DNX.Extensions.Strings;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace DNX.Extensions.Tests.Conversion;

public class GuidExtensionsTests(ITestOutputHelper outputHelper)
{
    [Theory]
    [InlineData("ABC")]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("497111F7-1511-49A8-8DB2-31B5E40953CB")]
    public void ToDeterministicGuid_can_create_a_guid_from_any_string(string text)
    {
        // Act
        var result = text.ToDeterministicGuid();
        outputHelper.WriteLine($"Text: {text} = {result}");

        // Assert
        result.ShouldNotBe(Guid.Empty);
        result.ToString().ShouldNotBe(text);
    }

    [Theory]
    [MemberData(nameof(ToDeterministicGuid_custom_algorithm_Data))]
    public void ToDeterministicGuid_can_create_a_guid_from_any_string_with_a_custom_algorithm(string text, HashAlgorithm algorithm)
    {
        // Act
        var result = text.ToDeterministicGuid(algorithm);
        outputHelper.WriteLine($"Text: {text} [{algorithm.GetType().FullName}] = {result}");

        // Assert
        result.ShouldNotBe(Guid.Empty);
        result.ToString().ShouldNotBe(text);
    }

    [Theory]
    [MemberData(nameof(ToDeterministicGuid_custom_algorithm_specific_reduce_method_Data))]
    public void ToDeterministicGuid_can_create_a_guid_from_any_string_with_a_custom_algorithm_and_reduce_method(string text, HashAlgorithm algorithm, Func<byte, byte, byte> reduceMethod)
    {
        // Act
        var result = text.ToDeterministicGuid(algorithm, reduceMethod);
        outputHelper.WriteLine($"Text: {text} [{algorithm.GetType().FullName}] [{reduceMethod.Method.Name}] = {result}");

        // Assert
        result.ShouldNotBe(Guid.Empty);
        result.ToString().ShouldNotBe(text);
    }

    [Theory]
    [InlineData("ABC", "d2bd2f90-dfb1-4f0c-70b4-a5d23525e932")]
    [InlineData("abc", "98500190-d23c-b04f-d696-3f7d28e17f72")]
    [InlineData("", "d98c1dd4-008f-04b2-e980-0998ecf8427e")]
    [InlineData(null, "d98c1dd4-008f-04b2-e980-0998ecf8427e")]
    [InlineData("497111F7-1511-49A8-8DB2-31B5E40953CB", "811f4c34-f777-5718-e90f-5b03aee2b92f")]
    [InlineData("Strings can be any length, and can be much longer than a standard Guid length", "151991b0-80dc-5902-d525-3a2f3c901477")]
    public void ToDeterministicGuid_will_always_generate_a_predictable_result(string text, string expected)
    {
        // Act
        var result = text.ToDeterministicGuid();
        var result2 = text.ToDeterministicGuid();
        outputHelper.WriteLine($"Text: {text} = {result}");

        // Assert
        result.ToString().ShouldNotBe(text);
        result.ShouldBe(result2);
        result.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, "00000001-0000-0000-0000-000000000000")]
    [InlineData(123, "0000007b-0000-0000-0000-000000000000")]
    [InlineData(12345, "00003039-0000-0000-0000-000000000000")]
    [InlineData(short.MinValue, "00008000-0000-0000-0000-000000000000")]
    [InlineData(short.MaxValue, "00007fff-0000-0000-0000-000000000000")]
    public void ToGuid_from_short_generates_a_predictable_result(short value, string expected)
    {
        // Act
        var result = value.ToGuid();
        outputHelper.WriteLine($"Text: {value} = {result}");

        // Assert
        result.ToString().ShouldNotBe(value.ToString());
        result.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, "00000001-0000-0000-0000-000000000000")]
    [InlineData(123, "0000007b-0000-0000-0000-000000000000")]
    [InlineData(12345, "00003039-0000-0000-0000-000000000000")]
    [InlineData(32767, "00007fff-0000-0000-0000-000000000000")]
    [InlineData(int.MinValue, "80000000-0000-0000-0000-000000000000")]
    [InlineData(int.MaxValue, "7fffffff-0000-0000-0000-000000000000")]
    public void ToGuid_from_int_generates_a_predictable_result(int value, string expected)
    {
        // Act
        var result = value.ToGuid();
        outputHelper.WriteLine($"Text: {value} = {result}");

        // Assert
        result.ToString().ShouldNotBe(value.ToString());
        result.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, "00000001-0000-0000-0000-000000000000")]
    [InlineData(123, "0000007b-0000-0000-0000-000000000000")]
    [InlineData(12345, "00003039-0000-0000-0000-000000000000")]
    [InlineData(32767, "00007fff-0000-0000-0000-000000000000")]
    [InlineData(uint.MinValue, "00000000-0000-0000-0000-000000000000")]
    [InlineData(uint.MaxValue, "ffffffff-0000-0000-0000-000000000000")]
    public void ToGuid_from_uint_generates_a_predictable_result(uint value, string expected)
    {
        // Act
        var result = value.ToGuid();
        outputHelper.WriteLine($"Text: {value} = {result}");

        // Assert
        result.ToString().ShouldNotBe(value.ToString());
        result.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, "00000001-0000-0000-0000-000000000000")]
    [InlineData(123, "0000007b-0000-0000-0000-000000000000")]
    [InlineData(12345, "00003039-0000-0000-0000-000000000000")]
    [InlineData(32767, "00007fff-0000-0000-0000-000000000000")]
    [InlineData(int.MinValue, "80000000-0000-0000-0000-000000000000")]
    [InlineData(int.MaxValue, "7fffffff-0000-0000-0000-000000000000")]
    [InlineData(long.MinValue, "00000000-0000-8000-0000-000000000000")]
    [InlineData(long.MaxValue, "ffffffff-ffff-7fff-0000-000000000000")]
    public void ToGuid_from_long_generates_a_predictable_result(long value, string expected)
    {
        // Act
        var result = value.ToGuid();
        outputHelper.WriteLine($"Text: {value} = {result}");

        // Assert
        result.ToString().ShouldNotBe(value.ToString());
        result.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, "00000001-0000-0000-0000-000000000000")]
    [InlineData(123, "0000007b-0000-0000-0000-000000000000")]
    [InlineData(12345, "00003039-0000-0000-0000-000000000000")]
    [InlineData(32767, "00007fff-0000-0000-0000-000000000000")]
    [InlineData(uint.MinValue, "00000000-0000-0000-0000-000000000000")]
    [InlineData(uint.MaxValue, "ffffffff-0000-0000-0000-000000000000")]
    [InlineData(ulong.MinValue, "00000000-0000-0000-0000-000000000000")]
    [InlineData(ulong.MaxValue, "ffffffff-ffff-ffff-0000-000000000000")]
    public void ToGuid_from_ulong_generates_a_predictable_result(ulong value, string expected)
    {
        // Act
        var result = value.ToGuid();
        outputHelper.WriteLine($"Text: {value} = {result}");

        // Assert
        result.ToString().ShouldNotBe(value.ToString());
        result.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, "00000001-0000-0000-0000-000000000000")]
    [InlineData(12345.67, "00003039-0000-0000-0000-000000000000")]
    [InlineData(123456789.12, "075bcd18-0000-0000-0000-000000000000")]
    [InlineData(int.MinValue, "80000000-0000-0000-0000-000000000000")]
    [InlineData(int.MaxValue, "80000000-0000-0000-0000-000000000000")]
    [InlineData(float.MinValue, "00000000-0000-0000-0000-000000010000")]
    [InlineData(float.MaxValue, "00000000-0000-0000-0000-000000ffffff")]
    public void ToGuid_from_float_generates_a_predictable_result(float value, string expected)
    {
        // Act
        var result = value.ToGuid();
        outputHelper.WriteLine($"Text: {value} = {result}");

        // Assert
        result.ToString().ShouldNotBe(value.ToString());
        result.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, "00000001-0000-0000-0000-000000000000")]
    [InlineData(12345.67, "00003039-0000-0000-0000-000000000000")]
    [InlineData(123456789.12, "075bcd15-0000-0000-0000-000000000000")]
    [InlineData(int.MinValue, "80000000-0000-0000-0000-000000000000")]
    [InlineData(int.MaxValue, "7fffffff-0000-0000-0000-000000000000")]
    public void ToGuid_from_decimal_generates_a_predictable_result(decimal value, string expected)
    {
        // Act
        var result = value.ToGuid();
        outputHelper.WriteLine($"Text: {value} = {result}");

        // Assert
        result.ToString().ShouldNotBe(value.ToString());
        result.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(1, "00000001-0000-0000-0000-000000000000")]
    [InlineData(12345.67, "00003039-0000-0000-0000-000000000000")]
    [InlineData(123456789.12, "075bcd15-0000-0000-0000-000000000000")]
    [InlineData(int.MinValue, "80000000-0000-0000-0000-000000000000")]
    [InlineData(int.MaxValue, "7fffffff-0000-0000-0000-000000000000")]
    [InlineData(float.MinValue, "00000000-0000-0000-0000-000000010000")]
    [InlineData(float.MaxValue, "00000000-0000-0000-0000-000000ffffff")]
    public void ToGuid_from_double_generates_a_predictable_result(double value, string expected)
    {
        // Act
        var result = value.ToGuid();
        outputHelper.WriteLine($"Text: {value} = {result}");

        // Assert
        result.ToString().ShouldNotBe(value.ToString());
        result.ToString().ShouldBe(expected);
    }

    // Useful : https://www.compscilib.com/calculate/binaryxor?variation=default
    public static TheoryData<string, HashAlgorithm> ToDeterministicGuid_custom_algorithm_Data()
    {
        var algorithms = new HashAlgorithm[]
        {
            MD5.Create(),
            SHA1.Create(),
            SHA256.Create(),
        };

        var data = new TheoryData<string, HashAlgorithm>();

        foreach (var algorithm in algorithms)
        {
            data.Add("ABC", algorithm);
            data.Add("abc", algorithm);
            data.Add("", algorithm);
            data.Add(null, algorithm);
            data.Add("497111F7-1511-49A8-8DB2-31B5E40953CB", algorithm);
        }

        return data;
    }

    public static TheoryData<string, HashAlgorithm, Func<byte, byte, byte>> ToDeterministicGuid_custom_algorithm_specific_reduce_method_Data()
    {
        var algorithms = new HashAlgorithm[]
        {
            MD5.Create(),
            SHA1.Create(),
            SHA256.Create(),
        };

        var reduceMethods = new[]
        {
            GuidExtensions.KeepOriginalValue,
            GuidExtensions.ExclusiveOr,
        };

        var data = new TheoryData<string, HashAlgorithm, Func<byte, byte, byte>>();

        foreach (var reduceMethod in reduceMethods)
        {
            foreach (var algorithm in algorithms)
            {
                data.Add("ABC", algorithm, reduceMethod);
                data.Add("abc", algorithm, reduceMethod);
                data.Add("", algorithm, reduceMethod);
                data.Add(null, algorithm, reduceMethod);
                data.Add("497111F7-1511-49A8-8DB2-31B5E40953CB", algorithm, reduceMethod);
            }
        }

        return data;
    }
}
