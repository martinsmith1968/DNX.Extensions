using System.Security.Cryptography;
using DNX.Extensions.Conversion;
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
        outputHelper.WriteLine($"Text: {text} = {result}");

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
        result.ShouldNotBe(Guid.Empty);
        result.ToString().ShouldNotBe(text);
        result.ShouldBe(result2);
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
}
