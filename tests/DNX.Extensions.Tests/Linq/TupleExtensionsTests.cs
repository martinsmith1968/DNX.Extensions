using Shouldly;
using Xunit;
using DNX.Extensions.Linq;

namespace DNX.Extensions.Tests.Linq;

public class TupleExtensionsTests
{
    [Fact]
    public void Tuple_IList_2_Test()
    {
        // Arrange

        // Act
        var result = new List<Tuple<int, int>>()
        {
#if CSHARP6
                { 1, 2 }
#endif
        };

        result.Add(1, 2);

        // Assert
#if CSHARP6
            result.Count.ShouldBe(2);
#else
        result.Count.ShouldBe(1);
#endif
    }

    [Fact]
    public void Tuple_IList_3_Test()
    {
        // Arrange

        // Act
        var result = new List<Tuple<int, int, int>>()
        {
#if CSHARP6
                { 1, 2, 3 }
#endif
        };

        result.Add(1, 2, 3);

        // Assert
#if CSHARP6
            result.Count.ShouldBe(2);
#else
        result.Count.ShouldBe(1);
#endif
    }

    [Fact]
    public void Tuple_IList_4_Test()
    {
        // Arrange

        // Act
        var result = new List<Tuple<int, int, int, int>>()
        {
#if CSHARP6
                { 1, 2, 3, 4 }
#endif
        };

        result.Add(1, 2, 3, 4);

        // Assert
#if CSHARP6
            result.Count.ShouldBe(2);
#else
        result.Count.ShouldBe(1);
#endif
    }

    [Fact]
    public void Tuple_IList_5_Test()
    {
        // Arrange

        // Act
        var result = new List<Tuple<int, int, int, int, int>>()
        {
#if CSHARP6
                { 1, 2, 3, 4, 5 }
#endif
        };

        result.Add(1, 2, 3, 4, 5);

        // Assert
#if CSHARP6
            result.Count.ShouldBe(2);
#else
        result.Count.ShouldBe(1);
#endif
    }

    [Fact]
    public void Tuple_IList_6_Test()
    {
        // Arrange

        // Act
        var result = new List<Tuple<int, int, int, int, int, int>>()
        {
#if CSHARP6
                { 1, 2, 3, 4, 5, 6 }
#endif
        };

        result.Add(1, 2, 3, 4, 5, 6);

        // Assert
#if CSHARP6
            result.Count.ShouldBe(2);
#else
        result.Count.ShouldBe(1);
#endif
    }
}
