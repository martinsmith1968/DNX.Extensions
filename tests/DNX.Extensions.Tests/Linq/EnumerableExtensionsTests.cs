using DNX.Extensions.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace DNX.Extensions.Tests.Linq
{
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
                result.Should().BeTrue();
            }

            [Fact]
            public void Empty_Enumerable_returns_successfully_as_False()
            {
                // Arrange
                var enumerable = new List<string>();

                // Act
                var result = enumerable.HasAny();

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public void Null_Enumerable_returns_successfully_as_False()
            {
                // Arrange
                List<string> enumerable = null;

                // Act
                var result = enumerable.HasAny();

                // Assert
                result.Should().BeFalse();
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

                result.Should().Be(expectedResult);
            }
        }

        public class IsOneOf_Tests
        {
            [Theory]
            [MemberData(nameof(IsOneOf_string_Data))]
            public void IsOneOf_with_string_data_can_operate_as_expected(IEnumerable<string> list, string candidate, bool expectedResult)
            {
                var result = candidate.IsOneOf(list);

                result.Should().Be(expectedResult);
            }

            [Theory]
            [MemberData(nameof(IsOneOf_string_comparer_Data))]
            public void IsOneOf_with_string_data_and_comparer_can_operate_as_expected(IEnumerable<string> list, string candidate, IEqualityComparer<string> comparer, bool expectedResult)
            {
                var result = candidate.IsOneOf(list, comparer);

                result.Should().Be(expectedResult);
            }

            [Fact]
            public void IsOneOf_with_params_can_operate_as_expected()
            {
                ((string)null).IsOneOf().Should().Be(false);
                ((string)null).IsOneOf((string[])null).Should().Be(false);
                "Hello".IsOneOf((string[])null).Should().BeFalse();
                "Hello".IsOneOf().Should().Be(false);
                "3".IsOneOf("1", "2", "3", "4", "5").Should().Be(true);
                "6".IsOneOf("1", "2", "3", "4", "5").Should().Be(false);
                "One".IsOneOf("One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Two".IsOneOf("One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Three".IsOneOf("One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Four".IsOneOf("One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Five".IsOneOf("One", "Two", "Three", "Four", "Five").Should().Be(true);
                "five".IsOneOf("One", "Two", "Three", "Four", "Five").Should().Be(false);
                "FIVE".IsOneOf("One", "Two", "Three", "Four", "Five").Should().Be(false);
                "Six".IsOneOf("One", "Two", "Three", "Four", "Five").Should().Be(false);
                1.IsOneOf(1, 2, 3, 4, 5).Should().Be(true);
                3.IsOneOf(1, 2, 3, 4, 5).Should().Be(true);
                5.IsOneOf(1, 2, 3, 4, 5).Should().Be(true);
                6.IsOneOf(1, 2, 3, 4, 5).Should().Be(false);
                OneToFive.One.IsOneOf(OneToFive.One, OneToFive.Three, OneToFive.Five).Should().Be(true);
                OneToFive.Three.IsOneOf(OneToFive.One, OneToFive.Three, OneToFive.Five).Should().Be(true);
                OneToFive.Five.IsOneOf(OneToFive.One, OneToFive.Three, OneToFive.Five).Should().Be(true);
                OneToFive.Two.IsOneOf(OneToFive.One, OneToFive.Three, OneToFive.Five).Should().Be(false);
                ((OneToFive)100).IsOneOf(OneToFive.One, OneToFive.Three, OneToFive.Five).Should().Be(false);
            }

            [Fact]
            public void IsOneOf_with_params_and_Comparer_can_operate_as_expected()
            {
                var comparer = StringComparer.FromComparison(StringComparison.CurrentCulture);
                ((string)null).IsOneOf(comparer).Should().Be(false);
                ((string)null).IsOneOf(comparer, null).Should().Be(false);
                "Hello".IsOneOf((string[])null).Should().BeFalse();
                "Hello".IsOneOf().Should().Be(false);
                "Hello".IsOneOf(comparer).Should().Be(false);
                "3".IsOneOf(comparer, "1", "2", "3", "4", "5").Should().Be(true);
                "6".IsOneOf(comparer, "1", "2", "3", "4", "5").Should().Be(false);
                "One".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Two".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Three".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Four".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Five".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "five".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(false);
                "FIVE".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(false);
                "Six".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(false);

                comparer = StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase);
                "Hello".IsOneOf(comparer).Should().Be(false);
                "3".IsOneOf(comparer, "1", "2", "3", "4", "5").Should().Be(true);
                "6".IsOneOf(comparer, "1", "2", "3", "4", "5").Should().Be(false);
                "One".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Two".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Three".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Four".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Five".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "five".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "FIVE".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(true);
                "Six".IsOneOf(comparer, "One", "Two", "Three", "Four", "Five").Should().Be(false);
            }

            #region Test Data

            public static IEnumerable<object[]> IsOneOf_string_Data()
            {
                return new List<object[]>()
                {
                    new object[] { null, "3", false },
                    new object[] { Enumerable.Empty<string>().ToArray(), "3", false },
                    new object[] { "1,2,3,4,5".Split(','), "3", true },
                    new object[] { "1,2,3,4,5".Split(','), "6", false },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "One", true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Two", true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Three", true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Four", true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Five", true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "five", false },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Six", false },
                };
            }

            public static IEnumerable<object[]> IsOneOf_string_comparer_Data()
            {
                return new List<object[]>()
                {
                    new object[] { "1,2,3,4,5".Split(','), "3", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                    new object[] { "1,2,3,4,5".Split(','), "6", StringComparer.FromComparison(StringComparison.CurrentCulture), false },
                    new object[] { "1,2,3,4,5".Split(','), "3", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                    new object[] { "1,2,3,4,5".Split(','), "6", StringComparer.FromComparison(StringComparison.CurrentCulture), false },

                    new object[] { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.CurrentCultureIgnoreCase), false },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.CurrentCulture), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.CurrentCulture), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.CurrentCulture), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.CurrentCulture), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.CurrentCulture), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.CurrentCulture), false },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.CurrentCulture), false },

                    new object[] { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.OrdinalIgnoreCase), false },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.Ordinal), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.Ordinal), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.Ordinal), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.Ordinal), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.Ordinal), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.Ordinal), false },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.Ordinal), false },

                    new object[] { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.InvariantCultureIgnoreCase), false },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "One", StringComparer.FromComparison(StringComparison.InvariantCulture), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Two", StringComparer.FromComparison(StringComparison.InvariantCulture), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Three", StringComparer.FromComparison(StringComparison.InvariantCulture), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Four", StringComparer.FromComparison(StringComparison.InvariantCulture), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Five", StringComparer.FromComparison(StringComparison.InvariantCulture), true },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "five", StringComparer.FromComparison(StringComparison.InvariantCulture), false },
                    new object[] { "One,Two,Three,Four,Five".Split(','), "Six", StringComparer.FromComparison(StringComparison.InvariantCulture), false },
                };
            }

            #endregion
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
                        result.Should().NotBeNullOrEmpty();
                    });

                // Assert
                foreach (var kvp in hitCounts)
                {
                    testOutputHelper.WriteLine("HitCount [{0}]: {1}", kvp.Key, kvp.Value);
                    kvp.Value.Should().BeGreaterThan(0);
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
                result.Should().BeNull();
            }

            [Fact]
            public void Null_string_Enumerable_returns_default()
            {
                // Arrange
                IEnumerable<string> enumerable = null;

                // Act
                var result = enumerable.GetRandomItem();

                // Assert
                result.Should().BeNull();
            }

            [Fact]
            public void Empty_int_Enumerable_returns_default()
            {
                // Arrange
                var enumerable = Enumerable.Empty<int>();

                // Act
                var result = enumerable.GetRandomItem();

                // Assert
                result.Should().Be(default);
            }

            [Fact]
            public void Null_int_Enumerable_returns_default()
            {
                // Arrange
                IEnumerable<int> enumerable = null;

                // Act
                var result = enumerable.GetRandomItem();

                // Assert
                result.Should().Be(default);
            }
        }

        public class GetAt(ITestOutputHelper testOutputHelper)
        {
            private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

            [Theory]
            [InlineData("1,2,3,4,5", 0, "1")]
            [InlineData("1,2,3,4,5", 2, "3")]
            [InlineData("1,2,3,4,5", 4, "5")]
            [InlineData("1,2,3,4,5", -1, null)]
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
                result.Should().Be(expected);
            }

            [Theory]
            [InlineData("1,2,3,4,5", 0, "1")]
            [InlineData("1,2,3,4,5", 2, "3")]
            [InlineData("1,2,3,4,5", 4, "5")]
            [InlineData("1,2,3,4,5", -1, null)]
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
                result.Should().Be(expected);
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
                result.Should().BeNull();
            }

            [Fact]
            public void Empty_Array_returns_default()
            {
                // Arrange
                var items = Array.Empty<string>();

                // Act
                var result = items.GetAt(0);

                // Assert
                result.Should().BeNull();
            }

            [Fact]
            public void Null_IList_returns_default()
            {
                // Arrange
                List<string> items = null;

                // Act
                var result = items.GetAt(0);

                // Assert
                result.Should().BeNull();
            }

            [Fact]
            public void Null_Array_returns_default()
            {
                // Arrange
                string[] items = null;

                // Act
                var result = items.GetAt(0);

                // Assert
                result.Should().BeNull();
            }
        }
    }
}
