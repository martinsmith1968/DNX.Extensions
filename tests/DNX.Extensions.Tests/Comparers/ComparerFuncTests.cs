using DNX.Extensions.Comparers;
using Shouldly;
using Xunit;

// ReSharper disable ConvertToLocalFunction

namespace DNX.Extensions.Tests.Comparers;

public class ComparerFuncTests
{
    [Theory]
    [InlineData(1, 2, 3, 4, -5, -5)]
    [InlineData(3, -3, -4, -3, -3, -4)]
    public void Compare_Absolute_Tests(int a, int b, int c, int d, int e, int expectedResult)
    {
        Func<int, int, int> absoluteComparerFunc = (x, y) =>
        {
            if (Math.Abs(x) > Math.Abs(y)) return 1;
            if (Math.Abs(x) < Math.Abs(y)) return -1;
            return 0;
        };

        var list = new[] { a, b, c, d, e };

        // Act
        var sortedList = list.OrderBy(z => z, ComparerFunc<int>.Create(absoluteComparerFunc))
            .ToList();

        // Assert
        sortedList.Last().ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 6, "2,4,6,1,3")]
    [InlineData(64, 25, 90, 17, 4, "4,64,90,17,25")]
    public void Compare_OrderBy_Tests(int a, int b, int c, int d, int e, string expectedResult)
    {
        Func<int, int, int> evensFirst = (x, y) =>
        {
            if (x == y) return 0;

            if (x % 2 == y % 2)
            {
                return x < y ? -1 : 1;
            }

            return (x % 2 == 0) ? -1 : 1;
        };

        var list = new[] { a, b, c, d, e };

        // Act
        var sortedList = list.OrderBy(z => z, ComparerFunc<int>.Create(evensFirst))
            .ToList();

        // Assert
        string.Join(",", sortedList.Select(x => x.ToString())).ShouldBe(expectedResult);
    }
}
