using System.Collections.Specialized;
using DNX.Extensions.Linq;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Linq;

public class NameValueExtensionsTests
{
    [Fact]
    public void Can_convert_a_NameValueCollection_using_default_ToDictionary_successfully()
    {
        // Arrange
        var collection = new NameValueCollection
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" }
        };

        // Act
        var result = collection.ToDictionary();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(collection.Count);
        result["One"].ShouldBe("1");
        result["Two"].ShouldBe("2");
        result["Three"].ShouldBe("3");
    }

    [Fact]
    public void Converting_null_NameValueCollection_with_ToDictionary_throws()
    {
        NameValueCollection collection = null;

        var ex = Should.Throw<ArgumentNullException>(() => collection.ToDictionary());

        ex.ParamName.ShouldBe("collection");
    }

    [Fact]
    public void Can_convert_a_NameValueCollection_using_Unique_successfully()
    {
        // Arrange
        var collection = new NameValueCollection
        {
            { "One", "1" },
            { "Two", "2" },
            { "Three", "3" },
            { "Four", "4" },
            { "Five", "5" },
        };

        // Act
        var result = collection.ToDictionary(MergeTechnique.Unique);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(collection.Count);

        foreach (var key in collection.AllKeys)
        {
            result.ContainsKey(key).ShouldBeTrue();
            result[key].ShouldBe(collection[key]);
        }
    }

    [Fact]
    public void Can_convert_a_NameValueCollection_using_First_successfully()
    {
        // Arrange
        var collection = new NameValueCollection
        {
            { "One", "1" },
            { "Two", "2" },
            { "One", "3" },
            { "Two", "4" },
            { "Three", "5" },
        };

        // Act
        var result = collection.ToDictionary(MergeTechnique.TakeFirst);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(collection.AllKeys.Distinct().Count());

        result["One"].ShouldBe("1");
        result["Two"].ShouldBe("2");
        result["Three"].ShouldBe("5");
    }

    [Fact]
    public void Can_convert_a_NameValueCollection_using_Last_successfully()
    {
        // Arrange
        var collection = new NameValueCollection
        {
            { "One", "1" },
            { "Two", "2" },
            { "One", "3" },
            { "Two", "4" },
            { "Three", "5" },
        };

        // Act
        var result = collection.ToDictionary(MergeTechnique.TakeLast);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(collection.AllKeys.Distinct().Count());

        result["One"].ShouldBe("3");
        result["Two"].ShouldBe("4");
        result["Three"].ShouldBe("5");
    }

    [Fact]
    public void Converting_a_NameValueCollection_using_Unique_with_duplictes_fails()
    {
        // Arrange
        var collection = new NameValueCollection
        {
            { "One", "1" },
            { "Two", "2" },
            { "One", "3" },
            { "Two", "4" },
            { "Three", "5" },
        };

        try
        {
            // Act
            var result = collection.ToDictionary(MergeTechnique.Unique);

            Assert.Fail("Expected exception not thrown");
        }
        catch (ArgumentException ex)
        {
            ex.Message.ShouldNotBeNull();
            ex.Message.ShouldNotBeEmpty();
        }
    }
}
