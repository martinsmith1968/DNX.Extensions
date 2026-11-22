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
    public void GetHashCode_uses_the_default_equality_comparers_hash_code_by_default(string valueList)
    {
        Func<int, int, bool> equalityFunc = (x, y) =>
        {
            return x == y;
        };

        var comparer = EqualityComparerFunc<int>.Create(equalityFunc);

        var list = valueList.Split(",".ToCharArray())
            .Select(x => Convert.ToInt32(x))
            .ToList();

        foreach (var value in list)
        {
            comparer.GetHashCode(value).ShouldBe(value.GetHashCode());
        }
    }

    [Fact]
    public void GetHashCode_can_use_an_explicit_hash_function_matching_a_custom_equality()
    {
        // The equality func treats a value and its negation as equal, so a matching hash
        // function must agree, or callers using this comparer in a HashSet/Dictionary would
        // violate the Equals/GetHashCode contract.
        var comparer = EqualityComparerFunc<int>.Create(
            (x, y) => Math.Abs(x) == Math.Abs(y),
            x => Math.Abs(x).GetHashCode());

        comparer.GetHashCode(5).ShouldBe(comparer.GetHashCode(-5));
    }
}
