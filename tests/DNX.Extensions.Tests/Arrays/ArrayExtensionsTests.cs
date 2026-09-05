using DNX.Extensions.Arrays;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable UseUtf8StringLiteral

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

        bytes.IsNullOrEmpty().ShouldBe(isEmpty);
    }

    [Fact]
    public void Test_PadLeft_when_greater_than_existing_array_length()
    {
        var values = "1,2,3,4,5,6".Split(",").ToArray();

        var result = values.PadLeft(8);

        values.Length.ShouldBe(6);
        result.Length.ShouldBe(8);

        for (var i = 1; i <= result.Length; ++i)
        {
            outputHelper.WriteLine($"Index: {i}");
            result[^i].ShouldBe(i > values.Length ? default : values[^i]);
        }
    }

    [Fact]
    public void Test_PadLeft_when_less_than_existing_array_length()
    {
        var values = "1,2,3,4,5,6".Split(",").ToArray();

        // Act
        var result = values.PadLeft(4);

        // Assert
        values.Length.ShouldBe(6);
        result.Length.ShouldBe(4);

        for (var i = 1; i <= result.Length; ++i)
        {
            outputHelper.WriteLine($"Index: {i}");
            result[^i].ShouldBe(values[^i]);
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
            result.Length.ShouldBe(values.Length);
            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftLeft_by_number_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();
            string[] expected = ["4", "5", "6", null, null, null];

            // Act
            var result = values.ShiftLeft(by: 3);

            // Assert
            result.Length.ShouldBe(values.Length);
            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftLeft_with_fill_value_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();
            string[] expected = ["3", "4", "5", "6", "0", "0"];

            // Act
            var result = values.ShiftLeft(by: 2, fillValue: "0");

            // Assert
            result.Length.ShouldBe(values.Length);
            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftLeft_null_array()
        {
            string[] values = null;

            var result = values.ShiftLeft();

            result.ShouldNotBeNull();
            result.Length.ShouldBe(0);
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
            result.Length.ShouldBe(values.Length);
            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftRight_by_number_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();
            string[] expected = [null, null, null, "1", "2", "3"];

            // Act
            var result = values.ShiftRight(by: 3);

            // Assert
            result.Length.ShouldBe(values.Length);
            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftRight_with_fill_value_populated_array()
        {
            var values = "1,2,3,4,5,6".Split(",").ToArray();
            string[] expected = ["0", "0", "1", "2", "3", "4"];

            // Act
            var result = values.ShiftRight(by: 2, fillValue: "0");

            // Assert
            result.Length.ShouldBe(values.Length);
            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Test_ShiftRight_null_array()
        {
            string[] values = null;

            var result = values.ShiftRight();

            result.ShouldNotBeNull();
            result.Length.ShouldBe(0);
        }
    }

    public class Reduce
    {
        [Theory]
        [MemberData(nameof(Reduce_Data_byte))]
        public void Reduce_can_produce_an_appropriate_output_for_bytes(byte[] input, int targetSize, Func<byte, byte, byte> method, byte[] expectedResult)
        {
            // Act
            var result = input.Reduce(targetSize, method);

            // Assert
            result.Length.ShouldBeLessThanOrEqualTo(targetSize);
            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(Reduce_Data_string))]
        public void Reduce_can_produce_an_appropriate_output_for_strings(string[] input, int targetSize, Func<string, string, string> method, string[] expectedResult)
        {
            // Act
            var result = input.Reduce(targetSize, method);

            // Assert
            result.Length.ShouldBeLessThanOrEqualTo(targetSize);
            result.ShouldBeEquivalentTo(expectedResult);
        }

        // Useful : https://www.compscilib.com/calculate/binaryxor?variation=default
        public static TheoryData<byte[], int, Func<byte, byte, byte>, byte[]> Reduce_Data_byte()
        {
            var data = new TheoryData<byte[], int, Func<byte, byte, byte>, byte[]>();

            data.Add(
                [1, 2, 3, 4, 5, 6],
                6,
                Or,
                [1, 2, 3, 4, 5, 6]
            );
            data.Add(
                [1, 2, 3, 4, 5, 6],
                8,
                Or,
                [1, 2, 3, 4, 5, 6]
            );
            data.Add(
                [1, 2, 3, 4, 5, 6],
                3,
                Or,
                [5, 7, 7]
            );
            data.Add(
                [1, 2, 3, 4, 5, 6],
                3,
                XOr,
                [5, 7, 5]
            );
            data.Add(
                [10, 20, 30, 40, 50, 60],
                3,
                XOr,
                [34, 38, 34]
            );

            return data;
        }

        public static TheoryData<string[], int, Func<string, string, string>, string[]> Reduce_Data_string()
        {
            var data = new TheoryData<string[], int, Func<string, string, string>, string[]>();

            data.Add(
                ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J"],
                5,
                Concat,
                ["AF", "BG", "CH", "DI", "EJ"]
            );

            return data;
        }

        public static byte Or(byte value1, byte value2) => (byte)(value1 | value2);
        public static byte XOr(byte value1, byte value2) => (byte)(value1 ^ value2);
        public static byte And(byte value1, byte value2) => (byte)(value1 & value2);
        public static byte Complement1(byte value1, byte value2) => (byte)(~value1);
        public static byte Complement2(byte value1, byte value2) => (byte)(~value2);

        public static string Concat(string value1, string value2) => value1 + value2;
    }
}
