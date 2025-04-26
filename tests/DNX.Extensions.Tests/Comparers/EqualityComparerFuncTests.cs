using DNX.Extensions.Comparers;
using Shouldly;
using Xunit;

// ReSharper disable ConvertToLambdaExpression
// ReSharper disable ConvertToLocalFunction

namespace DNX.Extensions.Tests.Comparers;

public class EqualityComparerFuncTests
{
    [Theory]
    [InlineData(1, 2, -2, 3, -5, 3, true)]
    [InlineData(3, 3, -4, 3, -3, 4, true)]
    [InlineData(1, 2, -2, 3, -5, 0, false)]
    [InlineData(3, 3, -4, 3, -3, 2, false)]
    public void Compare_Contains_Tests(int a, int b, int c, int d, int e, int search, bool expectedResult)
    {
        Func<int, int, bool> absoluteEqualityFunc = (x, y) =>
        {
            return Math.Abs(x) == Math.Abs(y);
        };

        var list = new[] { a, b, c, d, e };

        // Act
        var contains = list.Contains(search, EqualityComparerFunc<int>.Create(absoluteEqualityFunc));

        // Assert
        contains.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("1,2,3,4,5")]
    [InlineData("2,-2,2,-2,2")]
    public void GetHashCode_generates_constant_values(string valueList)
    {
        Func<int, int, bool> absoluteEqualityFunc = (x, y) =>
        {
            return x == y;
        };

        var comparer = EqualityComparerFunc<int>.Create(absoluteEqualityFunc);

        var list = valueList.Split(",".ToCharArray())
            .Select(x => Convert.ToInt32(x))
            .ToList();

        var hashCodes = list
            .Select(x => comparer.GetHashCode(x));

        hashCodes
            .All(hc => hc.Equals(0)).ShouldBeTrue();
    }
}
