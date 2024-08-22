using DNX.Extensions.Arrays;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

#pragma warning disable IDE0290

namespace DNX.Extensions.Tests.Arrays
{
    public class ArrayExtensionsTests(ITestOutputHelper outputHelper)
    {
        [Theory]
        [InlineData("2,3,4", false)]
        [InlineData("", true)]
        [InlineData(null, true)]
        public void Test_IsNullOrEmpty(string byteArray, bool isEmpty)
        {
            var bytes = byteArray?.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToByte(x))
                .ToArray();

            bytes.IsNullOrEmpty().Should().Be(isEmpty);
        }

        [Fact]
        public void Test_PadLeft_when_greater_than_existing_array_length()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();

            var result = values.PadLeft(8);

            values.Length.Should().Be(6);
            result.Length.Should().Be(8);

            for (var i = 1; i <= result.Length; ++i)
            {
                outputHelper.WriteLine($"Index: {i}");
                result[^i].Should().Be(i > values.Length ? default : values[^i]);
            }
        }

        [Fact]
        public void Test_PadLeft_when_less_than_existing_array_length()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();

            var result = values.PadLeft(4);

            values.Length.Should().Be(6);
            result.Length.Should().Be(4);

            for (var i = 1; i <= result.Length; ++i)
            {
                outputHelper.WriteLine($"Index: {i}");
                result[^i].Should().Be(values[^i]);
            }
        }

        [Fact]
        public void Test_ShiftLeft_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();

            var result = values.ShiftLeft();

            result.Length.Should().Be(values.Length);

            for (var i = 0; i < result.Length; ++i)
            {
                outputHelper.WriteLine($"Index: {i}");
                result[i].Should().Be(i >= values.Length - 1 ? default : values[i + 1]);
            }
        }

        [Fact]
        public void Test_ShiftLeft_null_array()
        {
            string[] values = null;

            var result = values.ShiftLeft();

            result.Should().NotBeNull();
            result.Length.Should().Be(0);
        }
    }
}
