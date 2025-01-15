using DNX.Extensions.Arrays;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Arrays;

public class ByteArrayExtensionsTests
{
    [Theory]
    [InlineData("65,66,67,68,69,70", "ABCDEF")]
    [InlineData("97,98,99,100,101,102", "abcdef")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void Test_GetAsciiString(string byteText, string expectedResult)
    {
        var bytes = byteText == null
            ? []
            : byteText.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToByte(x)).ToArray();

        var result = bytes.GetAsciiString();

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("1,2,3,4,5,6", "010203040506")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void Test_ToHexString(string byteText, string expectedResult)
    {
        var bytes = byteText == null
            ? []
            : byteText.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToByte(x)).ToArray();

        var result = bytes.ToHexString();

        result.ShouldBe(expectedResult);
    }
}
