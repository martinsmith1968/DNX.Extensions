using DNX.Extensions.Arrays;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

#pragma warning disable IDE0290

namespace DNX.Extensions.Tests.Arrays;

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

        // Act
        var result = values.PadLeft(4);

        // Assert
        values.Length.Should().Be(6);
        result.Length.Should().Be(4);

        for (var i = 1; i <= result.Length; ++i)
        {
            outputHelper.WriteLine($"Index: {i}");
            result[^i].Should().Be(values[^i]);
        }
    }

    public class ShiftLeft
    {
        [Fact]
        public void Test_ShiftLeft_defaults_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();
            string[] expected = ["2", "3", "4", "5", "6", null];

            // Act
            var result = values.ShiftLeft();

            // Assert
            result.Length.Should().Be(values.Length);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftLeft_by_number_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();
            string[] expected = ["4", "5", "6", null, null, null];

            // Act
            var result = values.ShiftLeft(by: 3);

            // Assert
            result.Length.Should().Be(values.Length);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftLeft_with_fill_value_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();
            string[] expected = ["3", "4", "5", "6", "0", "0"];

            // Act
            var result = values.ShiftLeft(by: 2, fillValue: "0");

            // Assert
            result.Length.Should().Be(values.Length);
            result.Should().BeEquivalentTo(expected);
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


    public class ShiftRight
    {
        [Fact]
        public void Test_ShiftRight_defaults_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();
            string[] expected = [null, "1", "2", "3", "4", "5"];

            // Act
            var result = values.ShiftRight();

            // Assert
            result.Length.Should().Be(values.Length);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftRight_by_number_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();
            string[] expected = [null, null, null, "1", "2", "3"];

            // Act
            var result = values.ShiftRight(by: 3);

            // Assert
            result.Length.Should().Be(values.Length);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftRight_with_fill_value_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();
            string[] expected = ["0", "0", "1", "2", "3", "4"];

            // Act
            var result = values.ShiftRight(by: 2, fillValue: "0");

            // Assert
            result.Length.Should().Be(values.Length);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftRight_null_array()
        {
            string[] values = null;

            var result = values.ShiftRight();

            result.Should().NotBeNull();
            result.Length.Should().Be(0);
        }
    }
}
