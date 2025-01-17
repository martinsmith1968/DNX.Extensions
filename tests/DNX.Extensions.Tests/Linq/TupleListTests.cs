using DNX.Extensions.Linq;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Linq;

public class TupleListTests
{
    [Fact]
    public void TupleList_2_can_construct_easily()
    {
        // Arrange


        // Act
        // ReSharper disable once UseObjectOrCollectionInitializer
        var list = new TupleList<int, int>
        {
            { 1, 2 }
        };

        list.Add(1, 2);

        // Assert
        list.ShouldNotBeNull();
        list.Count.ShouldBe(2);
    }

    [Fact]
    public void TupleList_3_can_construct_easily()
    {
        // Arrange


        // Act
        // ReSharper disable once UseObjectOrCollectionInitializer
        var list = new TupleList<int, int, int>
        {
            { 1, 2, 3 }
        };

        list.Add(1, 2, 3);

        // Assert
        list.ShouldNotBeNull();
        list.Count.ShouldBe(2);
    }

    [Fact]
    public void TupleList_4_can_construct_easily()
    {
        // Arrange


        // Act
        // ReSharper disable once UseObjectOrCollectionInitializer
        var list = new TupleList<int, int, int, int>
        {
            { 1, 2, 3, 4 }
        };

        list.Add(1, 2, 3, 4);

        // Assert
        list.ShouldNotBeNull();
        list.Count.ShouldBe(2);
    }

    [Fact]
    public void TupleList_5_can_construct_easily()
    {
        // Arrange


        // Act
        // ReSharper disable once UseObjectOrCollectionInitializer
        var list = new TupleList<int, int, int, int, int>
        {
            { 1, 2, 3, 4, 5 }
        };

        list.Add(1, 2, 3, 4, 5);

        // Assert
        list.ShouldNotBeNull();
        list.Count.ShouldBe(2);
    }

    [Fact]
    public void TupleList_6_can_construct_easily()
    {
        // Arrange


        // Act
        // ReSharper disable once UseObjectOrCollectionInitializer
        var list = new TupleList<int, int, int, int, int, int>
        {
            { 1, 2, 3, 4, 5, 6 }
        };

        list.Add(1, 2, 3, 4, 5, 6);

        // Assert
        list.ShouldNotBeNull();
        list.Count.ShouldBe(2);
    }
}
