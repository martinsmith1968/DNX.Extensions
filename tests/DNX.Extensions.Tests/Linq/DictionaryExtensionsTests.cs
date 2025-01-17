using System.Collections.ObjectModel;
using DNX.Extensions.Exceptions;
using DNX.Extensions.Linq;
using DNX.Extensions.Strings;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Linq;

public class DictionaryExtensionsTests
{
    [Theory]
    [InlineData("a=1;b=2;c=3", "b", "2")]
    [InlineData("a=1;b=2;c=3", "a", "1")]
    [InlineData("a=1;b=2;c=3", "c", "3")]
    [InlineData("a=1;b=2;c=3", "z", null)]
    [InlineData("a=1;b=2;c=3", "", null)]
    public void Test_GetValue(string dictionaryText, string key, string expectedResult)
    {
        var dictionary = dictionaryText.Split(";")
            .Select(x => x.ParseFirstMatchToDictionary(@"([^=]+)=(.*)"))
            .ToDictionary(
                a => a["1"],
                a => a["2"]
            );

        // Act
        var result = dictionary.GetValue(key);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("blah")]
    public void Test_GetValue_does_not_allow_null_dictionary(string key)
    {
        Dictionary<string, string> dictionary = null;

        // Act
        var ex = Assert.Throws<ArgumentNullException>(
            () => dictionary.GetValue(key)
        );

        // Assert
        ex.ShouldNotBeNull();
        ex.ParamName.ShouldBe("dictionary");
    }

    [Theory]
    [InlineData("a=1;b=2;c=3", null, false)]
    public void Test_GetValue_does_not_allow_null_key_name(string dictionaryText, string key, bool expectedResult)
    {
        try
        {
            // Act
            Test_GetValue(dictionaryText, key, null);

            expectedResult.ShouldBeTrue();
        }
        catch (ArgumentNullException ex)
        {
            ex.ParamName.ShouldBe("keyName");

            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [InlineData("a=1;b=2;c=3", "b", "4", "a=1;b=4;c=3")]
    [InlineData("a=1;b=2;c=3", "a", "4", "a=4;b=2;c=3")]
    [InlineData("a=1;b=2;c=3", "c", "4", "a=1;b=2;c=4")]
    [InlineData("a=1;b=2;c=3", "z", "4", "a=1;b=2;c=3;z=4")]
    [InlineData("a=1;b=2;c=3", "", "4", "a=1;b=2;c=3;=4")]
    public void Test_SetValue(string dictionaryText, string key, string newValue, string expectedResult)
    {
        var dictionary = dictionaryText.Split(";")
            .Select(x => x.ParseFirstMatchToDictionary(@"([^=]+)=(.*)"))
            .ToDictionary(
                a => a["1"],
                a => a["2"]
            );

        dictionary.SetValue(key, newValue);

        // Act
        var result = string.Join(";", dictionary
            .Select(x => $"{x.Key}={x.Value}")
        );

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("blah", "blah")]
    public void Test_SetValue_does_not_allow_null_dictionary(string key, string value)
    {
        Dictionary<string, string> dictionary = null;

        // Act
        var ex = Assert.Throws<ArgumentNullException>(
            () => dictionary.SetValue(key, value)
        );

        // Assert
        ex.ShouldNotBeNull();
        ex.ParamName.ShouldBe("dictionary");
    }

    [Theory]
    [InlineData("a=1;b=2;c=3", null, "4", false)]
    public void Test_SetValue_does_not_allow_null_key_name(string dictionaryText, string key, string newValue, bool expectedResult)
    {
        try
        {
            // Act
            Test_SetValue(dictionaryText, key, newValue, null);

            // Assert
            expectedResult.ShouldBeTrue();
        }
        catch (ArgumentNullException ex)
        {
            // Assert
            ex.ParamName.ShouldBe("keyName");

            expectedResult.ShouldBeFalse();
        }
    }

    [Theory]
    [InlineData("a=1;b=2;c=3", "b", "q", "a=1;c=3;q=2")]
    [InlineData("a=1;b=2;c=3", "a", "q", "b=2;c=3;q=1")]
    [InlineData("a=1;b=2;c=3", "c", "q", "a=1;b=2;q=3")]
    [InlineData("a=1;b=2;c=3", "z", "q", "a=1;b=2;c=3")]
    [InlineData("a=1;b=2;c=3", "", "q", "a=1;b=2;c=3")]
    public void Test_RenameKey(string dictionaryText, string keyName, string newKeyName, string expectedResult)
    {
        var dictionary = dictionaryText.Split(";")
            .Select(x => x.ParseFirstMatchToDictionary(@"([^=]+)=(.*)"))
            .ToDictionary(
                a => a["1"],
                a => a["2"]
            );

        // Act
        dictionary.RenameKey(keyName, newKeyName);

        var result = string.Join(";", dictionary
            .OrderBy(d => d.Key)
            .Select(x => $"{x.Key}={x.Value}")
        );

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("a=1;b=2;c=3", "a", null, false)]
    [InlineData("a=1;b=2;c=3", null, "a", false)]
    public void Test_RenameKey_does_not_allow_null_keyname(string dictionaryText, string key, string newKeyName, bool expectedResult)
    {
        try
        {
            // Act
            Test_RenameKey(dictionaryText, key, newKeyName, null);

            // Assert
            expectedResult.ShouldBeTrue();
        }
        catch (ArgumentNullException ex)
        {
            // Assert
            ex.ParamName.EndsWith("KeyName").ShouldBeTrue();    // fromKeyName, toKeyName

            expectedResult.ShouldBeFalse();
        }
    }

    [Fact]
    public void MergeUnique_can_combine_dictionaries_successfully()
    {
        // Arrange
        var dict1 = new Dictionary<string, int>()
        {
            { "A1", 1 },
            { "A2", 2 },
            { "A3", 3 },
            { "A4", 4 },
            { "A5", 5 },
        };

        var dict2 = new Dictionary<string, int>()
        {
            { "B1", 1 },
            { "B2", 2 },
            { "B3", 3 },
        };

        var dict3 = new Dictionary<string, int>()
        {
            { "C1", 1 },
            { "C2", 2 },
            { "C3", 3 },
        };

        // Act
        var result1 = DictionaryExtensions.MergeUnique(dict1, dict2, dict3);
        var result2 = DictionaryExtensions.Merge(MergeTechnique.Unique, dict1, dict2, dict3);

        // Assert
        result1.ShouldNotBeNull();
        result1.Count.ShouldBe(dict1.Count + dict2.Count + dict3.Count);
        foreach (var kvp1 in dict1)
        {
            result1.ContainsKey(kvp1.Key).ShouldBeTrue();
            result1[kvp1.Key].ShouldBe(kvp1.Value);
        }
        foreach (var kvp2 in dict2)
        {
            result1.ContainsKey(kvp2.Key).ShouldBeTrue();
            result1[kvp2.Key].ShouldBe(kvp2.Value);
        }
        foreach (var kvp3 in dict3)
        {
            result1.ContainsKey(kvp3.Key).ShouldBeTrue();
            result1[kvp3.Key].ShouldBe(kvp3.Value);
        }

        result1.ShouldBeEquivalentTo(result2);
    }

    [Fact]
    public void MergeUnique_with_dictionaries_with_key_clashes_fails()
    {
        // Arrange
        var dict1 = new Dictionary<string, int>()
        {
            { "A1", 1 },
            { "A2", 2 },
            { "A3", 3 },
            { "A4", 4 },
            { "A5", 5 },
        };

        var dict2 = new Dictionary<string, int>()
        {
            { "B1", 1 },
            { "A2", 2 },
            { "B3", 3 },
        };

        // Act
        try
        {
            var result = DictionaryExtensions.MergeUnique(dict1, dict2);

            Assert.Fail("Expected exception not thrown");
        }
        catch (ArgumentException e)
        {
            e.Message.ShouldNotBeNull();
        }

        try
        {
            var result = DictionaryExtensions.Merge(MergeTechnique.Unique, dict1, dict2);

            Assert.Fail("Expected exception not thrown");
        }
        catch (ArgumentException e)
        {
            e.Message.ShouldNotBeNull();
        }
    }

    [Fact]
    public void MergeFirst_can_combine_unique_dictionaries_successfully()
    {
        // Arrange
        var dict1 = new Dictionary<string, int>()
        {
            { "A1", 1 },
            { "A2", 2 },
            { "A3", 3 },
            { "A4", 4 },
            { "A5", 5 },
        };

        var dict2 = new Dictionary<string, int>()
        {
            { "B1", 1 },
            { "B2", 2 },
            { "B3", 3 },
        };

        var dict3 = new Dictionary<string, int>()
        {
            { "C1", 1 },
            { "C2", 2 },
            { "C3", 3 },
        };

        // Act
        var result1 = DictionaryExtensions.MergeFirst(dict1, dict2, dict3);
        var result2 = DictionaryExtensions.Merge(MergeTechnique.TakeFirst, dict1, dict2, dict3);

        // Assert
        result1.ShouldNotBeNull();
        result1.Count.ShouldBe(dict1.Count + dict2.Count + dict2.Count);
        foreach (var kvp1 in dict1)
        {
            result1.ContainsKey(kvp1.Key).ShouldBeTrue();
            result1[kvp1.Key].ShouldBe(kvp1.Value);
        }
        foreach (var kvp2 in dict2)
        {
            result1.ContainsKey(kvp2.Key).ShouldBeTrue();
            result1[kvp2.Key].ShouldBe(kvp2.Value);
        }
        foreach (var kvp3 in dict3)
        {
            result1.ContainsKey(kvp3.Key).ShouldBeTrue();
            result1[kvp3.Key].ShouldBe(kvp3.Value);
        }

        result1.ShouldBeEquivalentTo(result2);
    }

    [Fact]
    public void MergeFirst_with_dictionaries_with_key_clashes_uses_first_found_values_successfully()
    {
        // Arrange
        var dict1 = new Dictionary<string, int>()
        {
            { "A1", 1 },
            { "A2", 2 },
            { "A3", 3 },
            { "A4", 4 },
            { "A5", 5 },
        };

        var dict2 = new Dictionary<string, int>()
        {
            { "A2", 12 },
            { "A4", 14 },
            { "A6", 16 },
        };

        var dict3 = new Dictionary<string, int>()
        {
            { "A1", 21 },
            { "A3", 23 },
            { "A5", 25 },
        };

        // Act
        var result1 = DictionaryExtensions.MergeFirst(dict1, dict2, dict3);
        var result2 = DictionaryExtensions.Merge(MergeTechnique.TakeFirst, dict1, dict2, dict3);

        // Assert
        result1.ShouldNotBeNull();
        result1.Count.ShouldBe(dict1.Select(d => d.Key).Union(dict2.Select(d => d.Key)).Union(dict3.Select(d => d.Key)).Distinct().Count());
        foreach (var kvp1 in dict1)
        {
            result1.ContainsKey(kvp1.Key).ShouldBeTrue();
        }
        foreach (var kvp2 in dict2)
        {
            result1.ContainsKey(kvp2.Key).ShouldBeTrue();
        }
        foreach (var kvp3 in dict3)
        {
            result1.ContainsKey(kvp3.Key).ShouldBeTrue();
        }
        result1["A1"].ShouldBe(1);
        result1["A2"].ShouldBe(2);
        result1["A3"].ShouldBe(3);
        result1["A4"].ShouldBe(4);
        result1["A5"].ShouldBe(5);
        result1["A6"].ShouldBe(16);

        result1.ShouldBeEquivalentTo(result2);
    }

    [Fact]
    public void MergeLast_can_combine_dictionaries_successfully()
    {
        // Arrange
        var dict1 = new Dictionary<string, int>()
        {
            { "A1", 1 },
            { "A2", 2 },
            { "A3", 3 },
            { "A4", 4 },
            { "A5", 5 },
        };

        var dict2 = new Dictionary<string, int>()
        {
            { "B1", 1 },
            { "B2", 2 },
            { "B3", 3 },
        };

        var dict3 = new Dictionary<string, int>()
        {
            { "C1", 1 },
            { "C2", 2 },
            { "C3", 3 },
        };

        // Act
        var result1 = DictionaryExtensions.MergeLast(dict1, dict2, dict3);
        var result2 = DictionaryExtensions.Merge(MergeTechnique.TakeLast, dict1, dict2, dict3);

        // Assert
        result1.ShouldNotBeNull();
        result1.Count.ShouldBe(dict1.Count + dict2.Count + dict2.Count);
        foreach (var kvp1 in dict1)
        {
            result1.ContainsKey(kvp1.Key).ShouldBeTrue();
            result1[kvp1.Key].ShouldBe(kvp1.Value);
        }
        foreach (var kvp2 in dict2)
        {
            result1.ContainsKey(kvp2.Key).ShouldBeTrue();
            result1[kvp2.Key].ShouldBe(kvp2.Value);
        }
        foreach (var kvp3 in dict3)
        {
            result1.ContainsKey(kvp3.Key).ShouldBeTrue();
            result1[kvp3.Key].ShouldBe(kvp3.Value);
        }

        result1.ShouldBeEquivalentTo(result2);
    }

    [Fact]
    public void MergeLast_with_dictionaries_with_key_clashes_uses_last_found_values_successfully()
    {
        // Arrange
        var dict1 = new Dictionary<string, int>()
        {
            { "A1", 1 },
            { "A2", 2 },
            { "A3", 3 },
            { "A4", 4 },
            { "A5", 5 },
        };

        var dict2 = new Dictionary<string, int>()
        {
            { "A2", 12 },
            { "A4", 14 },
            { "A6", 16 },
        };

        var dict3 = new Dictionary<string, int>()
        {
            { "A1", 21 },
            { "A3", 23 },
            { "A5", 25 },
        };

        // Act
        var result1 = DictionaryExtensions.MergeLast(dict1, dict2, dict3);
        var result2 = DictionaryExtensions.Merge(MergeTechnique.TakeLast, dict1, dict2, dict3);

        // Assert
        result1.ShouldNotBeNull();
        result1.Count.ShouldBe(dict1.Select(d => d.Key).Union(dict2.Select(d => d.Key)).Union(dict3.Select(d => d.Key)).Distinct().Count());
        foreach (var kvp1 in dict1)
        {
            result1.ContainsKey(kvp1.Key).ShouldBeTrue();
        }
        foreach (var kvp2 in dict2)
        {
            result1.ContainsKey(kvp2.Key).ShouldBeTrue();
        }
        foreach (var kvp3 in dict3)
        {
            result1.ContainsKey(kvp3.Key).ShouldBeTrue();
        }
        result1["A1"].ShouldBe(21);
        result1["A2"].ShouldBe(12);
        result1["A3"].ShouldBe(23);
        result1["A4"].ShouldBe(14);
        result1["A5"].ShouldBe(25);
        result1["A6"].ShouldBe(16);

        result1.ShouldBeEquivalentTo(result2);
    }

    [Fact]
    public void MergeWith_can_merge_a_target_dictionary_and_leave_source_and_target_untouched_successfully()
    {
        // Arrange
        var dict1 = new Dictionary<string, int>()
        {
            { "A1", 1 },
            { "A2", 2 },
            { "A3", 3 },
            { "A4", 4 },
            { "A5", 5 },
        };

        var dict2 = new Dictionary<string, int>()
        {
            { "B1", 11 },
            { "B2", 12 },
            { "B3", 13 },
        };

        // Act
        var result = dict1.MergeWith(dict2, MergeTechnique.Unique);

        // Assert
        dict1.Count.ShouldBe(5);
        dict2.Count.ShouldBe(3);
        result.ShouldNotBeNull();
        result.Count.ShouldBe(dict1.Count + dict2.Count);
        foreach (var kvp1 in dict1)
        {
            result.ContainsKey(kvp1.Key).ShouldBeTrue();
        }
        foreach (var kvp2 in dict2)
        {
            result.ContainsKey(kvp2.Key).ShouldBeTrue();
        }
        result["A1"].ShouldBe(1);
        result["A2"].ShouldBe(2);
        result["A3"].ShouldBe(3);
        result["A4"].ShouldBe(4);
        result["A5"].ShouldBe(5);
        result["B1"].ShouldBe(11);
        result["B2"].ShouldBe(12);
        result["B3"].ShouldBe(13);
    }

    [Fact]
    public void MergeWith_can_chain_merge_target_dictionaries_and_leave_sources_and_targets_untouched_successfully()
    {
        // Arrange
        var dict1 = new Dictionary<string, int>()
        {
            { "A1", 1 },
            { "A2", 2 },
            { "A3", 3 },
            { "A4", 4 },
            { "A5", 5 },
        };

        var dict2 = new Dictionary<string, int>()
        {
            { "B1", 11 },
            { "B2", 12 },
            { "B3", 13 },
        };

        var dict3 = new Dictionary<string, int>()
        {
            { "C1", 21 },
            { "C2", 22 },
        };

        // Act
        var result = dict1
                .MergeWith(dict2, MergeTechnique.Unique)
                .MergeWith(dict3, MergeTechnique.Unique)
            ;

        // Assert
        dict1.Count.ShouldBe(5);
        dict2.Count.ShouldBe(3);
        dict3.Count.ShouldBe(2);
        result.ShouldNotBeNull();
        result.Count.ShouldBe(dict1.Count + dict2.Count + dict3.Count);
        foreach (var kvp1 in dict1)
        {
            result.ContainsKey(kvp1.Key).ShouldBeTrue();
        }
        foreach (var kvp2 in dict2)
        {
            result.ContainsKey(kvp2.Key).ShouldBeTrue();
        }
        foreach (var kvp3 in dict3)
        {
            result.ContainsKey(kvp3.Key).ShouldBeTrue();
        }
        result["A1"].ShouldBe(1);
        result["A2"].ShouldBe(2);
        result["A3"].ShouldBe(3);
        result["A4"].ShouldBe(4);
        result["A5"].ShouldBe(5);
        result["B1"].ShouldBe(11);
        result["B2"].ShouldBe(12);
        result["B3"].ShouldBe(13);
        result["C1"].ShouldBe(21);
        result["C2"].ShouldBe(22);
    }

    [Fact]
    public void Merge_fails_when_invalid_or_unknown_MergeTechnique_specified()
    {
        // Arrange
        var dict1 = new Dictionary<string, int>()
        {
            { "A1", 1 },
            { "A2", 2 },
            { "A3", 3 },
            { "A4", 4 },
            { "A5", 5 },
        };

        var dict2 = new Dictionary<string, int>()
        {
            { "B1", 11 },
            { "B2", 12 },
            { "B3", 13 },
        };

        var mergeTechnique = (MergeTechnique)int.MaxValue;

        // Act
        var ex = Assert.Throws<EnumValueException<MergeTechnique>>(() =>
            {
                dict1.MergeWith(dict2, mergeTechnique);
            }
        );

        // Assert
        ex.ShouldNotBeNull();
        ex.Value.ShouldBe(mergeTechnique);
        ex.Type.ShouldBe(typeof(MergeTechnique));
    }

    [Fact]
    public void ToExpando_can_convert_simple_dictionary()
    {
        // Arrange
        var dict = new Dictionary<string, object>()
        {
            { "Key1", Guid.NewGuid() },
            { "Key2", Guid.NewGuid() },
            { "Key3", Guid.NewGuid() },
        };

        // Act
        dynamic expando = dict.ToExpando();

        // Assert
        ((object)expando).ShouldNotBeNull();
        ((object)expando.Key1).ShouldBe(dict["Key1"]);
        ((object)expando.Key2).ShouldBe(dict["Key2"]);
        ((object)expando.Key3).ShouldBe(dict["Key3"]);
    }

    [Fact]
    public void ToExpando_can_convert_dictionary_containing_dictionary_properties()
    {
        // Arrange
        var dict = new Dictionary<string, object>()
        {
            { "Key1", Guid.NewGuid() },
            {
                "Key2", new Dictionary<string, object>()
                {
                    { "Key21", Guid.NewGuid() },
                    { "Key22", Guid.NewGuid() },
                }
            },
            { "Key3", Guid.NewGuid() },
        };

        // Act
        dynamic expando = dict.ToExpando();

        // Assert
        ((object)expando).ShouldNotBeNull();
        ((object)expando.Key1).ShouldBe(dict["Key1"]);
        ((object)expando.Key2).ShouldBe(dict["Key2"]);
        ((object)expando.Key3).ShouldBe(dict["Key3"]);
        ((object)expando.Key2.Key21).ShouldBe(((Dictionary<string, object>)dict["Key2"])["Key21"]);
        ((object)expando.Key2.Key22).ShouldBe(((Dictionary<string, object>)dict["Key2"])["Key22"]);
    }

    [Fact]
    public void ToExpando_can_convert_dictionary_containing_collection_properties()
    {
        // Arrange
        var dict = new Dictionary<string, object>()
        {
            { "Key1", Guid.NewGuid() },
            {
                "Key2", new Collection<string>()
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                }
            },
            { "Key3", Guid.NewGuid() },
        };

        // Act
        dynamic expando = dict.ToExpando();

        // Assert
        ((object)expando).ShouldNotBeNull();
        ((object)expando.Key1).ShouldBe(dict["Key1"]);
        ((object)expando.Key2).ShouldBe(dict["Key2"]);
        ((object)expando.Key3).ShouldBe(dict["Key3"]);
        ((object)expando.Key2[0]).ShouldBe(((Collection<string>)dict["Key2"])[0]);
        ((object)expando.Key2[1]).ShouldBe(((Collection<string>)dict["Key2"])[1]);
    }
}
