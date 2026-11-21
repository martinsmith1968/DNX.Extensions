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
    public void Converting_null_NameValueCollection_with_explicit_merge_technique_throws()
    {
        NameValueCollection collection = null;

        var ex = Should.Throw<ArgumentNullException>(() => collection.ToDictionary(MergeTechnique.TakeFirst));

        ex.ParamName.ShouldBe("collection");
    }

    [Fact]
    public void Converting_an_empty_NameValueCollection_returns_an_empty_dictionary()
    {
        // Arrange
        var collection = new NameValueCollection();

        // Act
        var result = collection.ToDictionary();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Fact]
    public void Converting_a_NameValueCollection_with_a_null_valued_key_drops_that_key()
    {
        // Arrange
        var collection = new NameValueCollection
        {
            { "WithNull", null },
            { "Normal", "x" }
        };

        // Act
        var result = collection.ToDictionary();

        // Assert
        // NameValueCollection.GetValues() returns null (not a single-null-element array) for a
        // key added with a null value, so the production code's fallback to an empty sequence
        // means that key never gets a dictionary entry at all.
        result.ShouldNotBeNull();
        result.ShouldNotContainKey("WithNull");
        result["Normal"].ShouldBe("x");
        result.Count.ShouldBe(1);
    }

    [Fact]
    public void Converting_a_NameValueCollection_with_a_null_named_entry_drops_that_entry()
    {
        // Arrange
        var collection = new NameValueCollection
        {
            { null, "orphan-value" },
            { "Normal", "x" }
        };

        // Act
        var result = collection.ToDictionary();

        // Assert
        // Dictionary<string, string> doesn't allow a null key, so a null-named
        // NameValueCollection entry (e.g. a bare value with no "=key" in a query string) is
        // dropped rather than carried through - symmetric with how a null-valued entry is
        // already dropped.
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result["Normal"].ShouldBe("x");
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
