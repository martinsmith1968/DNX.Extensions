using DNX.Extensions.Enumerations;
using FluentAssertions;
using Xunit;

namespace DNX.Extensions.Tests.Enumerations;

public class EnumerableExtensionsTests
{
    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("a,b,c,d,e,f,g,h,i,j", true)]
    public void Test_HasAny(string commaDelimitedArray, bool expectedResult)
    {
        var enumerable = commaDelimitedArray?
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        // Act
        var result = enumerable.HasAny();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("", "1", false)]
    [InlineData(null, "1", false)]
    [InlineData("a1,b2,c1,d2,e1,f2,g1,h2,i1,j2", "1", true)]
    [InlineData("a1,b2,c1,d2,e1,f2,g1,h2,i1,j2", "2", true)]
    [InlineData("a1,b2,c1,d2,e1,f2,g1,h2,i1,j2", "0", false)]
    public void Test_HasAny_predicate(string commaDelimitedArray, string suffix, bool expectedResult)
    {
        var enumerable = commaDelimitedArray?
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        // Act
        var result = enumerable.HasAny(s => s.EndsWith(suffix));

        // Assert
        result.Should().Be(expectedResult);
    }
}
