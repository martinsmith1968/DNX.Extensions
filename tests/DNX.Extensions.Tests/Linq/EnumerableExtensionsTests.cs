using System.Collections;
using System.Collections.Generic;
using DNX.Extensions.Comparers;
using DNX.Extensions.Linq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace DNX.Extensions.Tests.Linq;

internal enum OneToFive
{
    One,
    Two,
    Three,
    Four,
    Five
}

public class EnumerableExtensionsTests
{
    public class HasAny
    {
        [Theory]
        [InlineData("1,2,3,4,5")]
        [InlineData("Dave,Bob,Steve")]
        public void Populated_Enumerable_returns_successfully_as_True(string itemList)
        {
            // Arrange
            var enumerable = itemList.Split(",");

            // Act
            var result = enumerable.HasAny();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Empty_Enumerable_returns_successfully_as_False()
        {
            // Arrange
            var enumerable = new List<string>();

            // Act
            var result = enumerable.HasAny();

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Null_Enumerable_returns_successfully_as_False()
        {
            // Arrange
            List<string> enumerable = null;

            // Act
            var result = enumerable.HasAny();

            // Assert
            result.ShouldBeFalse();
        }
    }

    public class HasAny_Predicate
    {
        [Theory]
        [InlineData("", "1", false)]
        [InlineData(null, "1", false)]
        [InlineData("a1,b2,c1,d2,e1,f2,g1,h2,i1,j2", "1", true)]
        [InlineData("a1,b2,c1,d2,e1,f2,g1,h2,i1,j2", "2", true)]
        [InlineData("a1,b2,c1,d2,e1,f2,g1,h2,i1,j2", "0", false)]
        public void Test_HasAny_predicate(string commaDelimitedArray, string suffix, bool expectedResult)
        {
            var enumerable = commaDelimitedArray?
                    .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                ;

            var result = enumerable.HasAny(s => s.EndsWith(suffix));

            result.ShouldBe(expectedResult);
        }
    }

    public class IsOneOf_Tests
    {
        [Theory]
        [MemberData(nameof(IsOneOf_string_Data))]
        public void IsOneOf_with_string_data_can_operate_as_expected(string[] list, string candidate, bool expectedResult)
        {
            var result = candidate.IsOneOf(list);

            result.ShouldBe(expectedResult);
        }

        [Theory]
        [MemberData(nameof(IsOneOf_string_comparer_Data))]
        public void IsOneOf_with_string_data_and_comparer_can_operate_as_expected(string[] list, string candidate, IEqualityComparer<string> comparer, bool expectedResult)
        {
            var result = candidate.IsOneOf(list, comparer);

            result.ShouldBe(expectedResult);
        }

        [Fact]
        public void IsOneOf_with_params_can_operate_as_expected()
        {
            ((string)null).IsOneOf().ShouldBe(false);
            ((string)null).IsOneOf((string[])null).ShouldBe(false);
            "Hello".IsOneOf((string[])null).ShouldBeFalse();
            "Hello".IsOneOf().ShouldBe(false);
            "3".IsOneOf("1", "2", "3", "4", "5").ShouldBe(true);
            "6".IsOneOf("1", "2", "3", "4", "5").ShouldBe(false);
            "One".IsOneOf("One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Two".IsOneOf("One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Three".IsOneOf("One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Four".IsOneOf("One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Five".IsOneOf("One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "five".IsOneOf("One", "Two", "Three", "Four", "Five").ShouldBe(false);
            "FIVE".IsOneOf("One", "Two", "Three", "Four", "Five").ShouldBe(false);
            "Six".IsOneOf("One", "Two", "Three", "Four", "Five").ShouldBe(false);
            1.IsOneOf(1, 2, 3, 4, 5).ShouldBe(true);
            3.IsOneOf(1, 2, 3, 4, 5).ShouldBe(true);
            5.IsOneOf(1, 2, 3, 4, 5).ShouldBe(true);
            6.IsOneOf(1, 2, 3, 4, 5).ShouldBe(false);
            OneToFive.One.IsOneOf(OneToFive.One, OneToFive.Three, OneToFive.Five).ShouldBe(true);
            OneToFive.Three.IsOneOf(OneToFive.One, OneToFive.Three, OneToFive.Five).ShouldBe(true);
            OneToFive.Five.IsOneOf(OneToFive.One, OneToFive.Three, OneToFive.Five).ShouldBe(true);
            OneToFive.Two.IsOneOf(OneToFive.One, OneToFive.Three, OneToFive.Five).ShouldBe(false);
            ((OneToFive)100).IsOneOf(OneToFive.One, OneToFive.Three, OneToFive.Five).ShouldBe(false);
        }

        [Fact]
        public void IsOneOf_with_params_and_Comparer_can_operate_as_expected()
        {
            var comparer = StringComparer.FromComparison(StringComparison.CurrentCulture);
            ((string)null).IsOneOf(comparer).ShouldBe(false);
            ((string)null).IsOneOf(comparer, null).ShouldBe(false);
            "Hello".IsOneOf((string[])null).ShouldBeFalse();
            "Hello".IsOneOf().ShouldBe(false);
            "Hello".IsOneOf(comparer).ShouldBe(false);
            "3".IsOneOf(comparer, "1", "2", "3", "4", "5").ShouldBe(true);
            "6".IsOneOf(comparer, "1", "2", "3", "4", "5").ShouldBe(false);
            "One".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Two".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Three".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Four".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Five".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "five".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(false);
            "FIVE".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(false);
            "Six".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(false);

            comparer = StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase);
            "Hello".IsOneOf(comparer).ShouldBe(false);
            "3".IsOneOf(comparer, "1", "2", "3", "4", "5").ShouldBe(true);
            "6".IsOneOf(comparer, "1", "2", "3", "4", "5").ShouldBe(false);
            "One".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Two".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Three".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Four".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Five".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "five".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "FIVE".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(true);
            "Six".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").ShouldBe(false);
        }

        #region Test Data

        public static TheoryData<string[], string, bool> IsOneOf_string_Data()
        {
            return new TheoryData<string[], string, bool>
            {
                { null, "3", false },
                { Enumerable.Empty<string>().ToArray(), "3", false },
                { "1,2,3,4,5".Split(','), "3", true },
                { "1,2,3,4,5".Split(','), "6", false },
                { "One,Two,Three,Four,Five".Split(','), "One", true },
                { "One,Two,Three,Four,Five".Split(','), "Two", true },
                { "One,Two,Three,Four,Five".Split(','), "Three", true },
                { "One,Two,Three,Four,Five".Split(','), "Four", true },
                { "One,Two,Three,Four,Five".Split(','), "Five", true },
                { "One,Two,Three,Four,Five".Split(','), "five", false },
                { "One,Two,Three,Four,Five".Split(','), "Six", false },
            };
        }

        public static TheoryData<string[], string, IEqualityComparer<string>, bool> IsOneOf_string_comparer_Data()
        {
            return new TheoryData<string[], string, IEqualityComparer<string>, bool>
            {
                { "1,2,3,4,5".Split(','), "3", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                { "1,2,3,4,5".Split(','), "6", StringComparer.FromComparison(StringComparison.CurrentCulture), false },
                { "1,2,3,4,5".Split(','), "3", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                { "1,2,3,4,5".Split(','), "6", StringComparer.FromComparison(StringComparison.CurrentCulture), false },

                { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), false },
                { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.CurrentCulture), true },
                { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.CurrentCulture), true },
                { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.CurrentCulture), true },
                { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.CurrentCulture), true },
                { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.CurrentCulture), true },
                { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.CurrentCulture), false },
                { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.CurrentCulture), false },

                { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), false },
                { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.Ordinal), true },
                { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.Ordinal), true },
                { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.Ordinal), true },
                { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.Ordinal), true },
                { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.Ordinal), true },
                { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.Ordinal), false },
                { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.Ordinal), false },

                { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), false },
                { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.InvariantCulture), true },
                { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.InvariantCulture), true },
                { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.InvariantCulture), true },
                { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.InvariantCulture), true },
                { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.InvariantCulture), true },
                { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.InvariantCulture), false },
                { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.InvariantCulture), false },
            };
        }

        #endregion
    }

    public class IsIn_Tests
    {
        [Theory]
        [InlineData("1,2,3,4,5", "1", true)]
        [InlineData("1,2,3,4,5", "3", true)]
        [InlineData("1,2,3,4,5", "5", true)]
        [InlineData("1,2,3,4,5", "0", false)]
        [InlineData("1,2,3,4,5", "", false)]
        [InlineData("1,2,3,4,5", null, false)]
        [InlineData(null, "1", false)]
        public void IsIn_returns_as_expected(string itemList, string item, bool expectedResult)
        {
            // Arrange
            var enumerable = itemList?.Split(",");

            // Act
            var result = item.IsIn(enumerable);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("a,b,c,d,e", "a", StringComparison.Ordinal, true)]
        [InlineData("a,b,c,d,e", "A", StringComparison.Ordinal, false)]
        [InlineData("a,b,c,d,e", "A", StringComparison.OrdinalIgnoreCase, true)]
        [InlineData(null, "1", StringComparison.OrdinalIgnoreCase, false)]
        public void IsIn_EqualityComparer_returns_as_expected(string itemList, string item, StringComparison comparison, bool expectedResult)
        {
            // Arrange
            var enumerable = itemList?.Split(",");

            // Act
            var result = item.IsIn(enumerable, StringComparer.FromComparison(comparison));

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class IsNotIn_Tests
    {
        [Theory]
        [InlineData("1,2,3,4,5", "1", false)]
        [InlineData("1,2,3,4,5", "3", false)]
        [InlineData("1,2,3,4,5", "5", false)]
        [InlineData("1,2,3,4,5", "0", true)]
        [InlineData("1,2,3,4,5", "", true)]
        [InlineData("1,2,3,4,5", null, true)]
        [InlineData(null, "1", true)]
        public void IsNotIn_returns_as_expected(string itemList, string item, bool expectedResult)
        {
            // Arranger
            var enumerable = itemList?.Split(",");

            // Act
            var result = item.IsNotIn(enumerable);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("a,b,c,d,e", "a", StringComparison.Ordinal, false)]
        [InlineData("a,b,c,d,e", "A", StringComparison.Ordinal, true)]
        [InlineData("a,b,c,d,e", "A", StringComparison.OrdinalIgnoreCase, false)]
        [InlineData(null, "1", StringComparison.CurrentCultureIgnoreCase, true)]
        public void IsNotIn_EqualityComparer_returns_as_expected(string itemList, string item, StringComparison comparison, bool expectedResult)
        {
            // Arrange
            var enumerable = itemList?.Split(",");

            // Act
            var result = item.IsNotIn(enumerable, StringComparer.FromComparison(comparison));

            // Assert
            result.ShouldBe(expectedResult);
        }
    }

    public class GetRandomItem(ITestOutputHelper testOutputHelper)
    {
        [Theory]
        [InlineData("1,2,3,4,5", 10000)]
        [InlineData("A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z", 10000)]
        [InlineData("1,2", 100)]
        [InlineData("1", 100)]
        public void Populated_Enumerable_repeatedly_returns_an_item_successfully(string itemList, int count)
        {
            // Arrange
            var enumerable = itemList.Split(",", StringSplitOptions.RemoveEmptyEntries);

            var hitCounts = enumerable
                .ToDictionary(x => x, x => 0);

            Enumerable.Range(1, count)
                .ToList()
                .ForEach(x =>
                {
                    // Act
                    var result = enumerable.GetRandomItem();

                    hitCounts[result]++;

                    // Assert
                    result.ShouldNotBeNullOrEmpty();
                });

            // Assert
            foreach (var kvp in hitCounts)
            {
                testOutputHelper.WriteLine("HitCount [{0}]: {1}", kvp.Key, kvp.Value);
                kvp.Value.ShouldBeGreaterThan(0);
            }
        }

        [Fact]
        public void Empty_string_Enumerable_returns_default()
        {
            // Arrange
            var enumerable = Enumerable.Empty<string>();

            // Act
            var result = enumerable.GetRandomItem();

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Null_string_Enumerable_returns_default()
        {
            // Arrange
            IEnumerable<string> enumerable = null;

            // Act
            var result = enumerable.GetRandomItem();

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Empty_int_Enumerable_returns_default()
        {
            // Arrange
            var enumerable = Enumerable.Empty<int>();

            // Act
            var result = enumerable.GetRandomItem();

            // Assert
            result.ShouldBe(default);
        }

        [Fact]
        public void Null_int_Enumerable_returns_default()
        {
            // Arrange
            IEnumerable<int> enumerable = null;

            // Act
            var result = enumerable.GetRandomItem();

            // Assert
            result.ShouldBe(default);
        }
    }

    public class GetAt
    {
        [Theory]
        [InlineData("1,2,3,4,5", 0, "1")]
        [InlineData("1,2,3,4,5", 2, "3")]
        [InlineData("1,2,3,4,5", 4, "5")]
        [InlineData("1,2,3,4,5", -1, "5")]
        [InlineData("1,2,3,4,5", 5, null)]
        [InlineData("1,2,3,4,5", 6, null)]
        [InlineData("1,2,3,4,5", 100, null)]
        [InlineData("1,2,3,4,5", -100, null)]
        public void Populated_List_repeatedly_returns_an_item_successfully(string itemList, int index, string expected)
        {
            // Arrange
            var items = itemList.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            // Act
            var result = items.GetAt(index);

            // Assert
            result.ShouldBe(expected);
        }

        [Theory]
        [InlineData("1,2,3,4,5", 0, "1")]
        [InlineData("1,2,3,4,5", 2, "3")]
        [InlineData("1,2,3,4,5", 4, "5")]
        [InlineData("1,2,3,4,5", -1, "5")]
        [InlineData("1,2,3,4,5", 5, null)]
        [InlineData("1,2,3,4,5", 6, null)]
        [InlineData("1,2,3,4,5", 100, null)]
        [InlineData("1,2,3,4,5", -100, null)]
        public void Populated_Array_repeatedly_returns_an_item_successfully(string itemList, int index, string expected)
        {
            // Arrange
            var items = itemList.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            // Act
            var result = items.GetAt(index);

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void Empty_IList_returns_default()
        {
            // Arrange
            var items = Enumerable.Empty<string>()
                .ToList();

            // Act
            var result = items.GetAt(0);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Empty_Array_returns_default()
        {
            // Arrange
            var items = Array.Empty<string>();

            // Act
            var result = items.GetAt(0);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Null_IList_returns_default()
        {
            // Arrange
            List<string> items = null;

            // Act
            var result = items.GetAt(0);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Null_Array_returns_default()
        {
            // Arrange
            string[] items = null;

            // Act
            var result = items.GetAt(0);

            // Assert
            result.ShouldBeNull();
        }
    }

    public class AppendItem
    {
        [Theory]
        [InlineData("a,b,c", "d", "a,b,c,d")]
        [InlineData("a,b,c", "a", "a,b,c,a")]
        [InlineData(null, "a", "a")]
        [InlineData("", "a", "a")]
        public void Test_AppendItem(string commaDelimitedArray, string value, string expectedResult)
        {
            var enumerable = commaDelimitedArray?
                .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            // Act
            var result = enumerable.AppendItem(value);

            // Assert
            string.Join(",", result).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData("a,b,c", "d", "a,b,c,d")]
        [InlineData("a,b,c,d", "D", "a,b,c,d")]
        [InlineData("a,b,c", "a", "a,b,c")]
        [InlineData(null, "a", "a")]
        [InlineData("", "a", "a")]
        public void Test_Append_CaseInsensitiveComparer(string commaDelimitedArray, string value, string expectedResult)
        {
            var comparer = EqualityComparerFunc<string>.Create(
                (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase)
            );

            var enumerable = commaDelimitedArray?
                .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            // Act
            var result = enumerable.AppendItem(value, comparer);

            // Assert
            string.Join(",", result).ShouldBe(expectedResult);
        }
    }
}
