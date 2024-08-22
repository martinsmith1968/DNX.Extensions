using DNX.Extensions.Conversion;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace DNX.Extensions.Tests.Conversion
{
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
            result.Should().NotBe(Guid.Empty);
            result.ToString().Should().NotBe(text);
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
            result.Should().NotBe(Guid.Empty);
            result.ToString().Should().NotBe(text);
            result.Should().Be(result2);
            result.ToString().Should().Be(expected);
        }
    }
}
