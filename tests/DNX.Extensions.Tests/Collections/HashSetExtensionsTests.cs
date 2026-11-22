using DNX.Extensions.Collections;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Collections;

public class HashSetExtensionsTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData(new int[] { }, true)]
    [InlineData(new[] { 1, 2, 3 }, false)]
    public void Test_IsNullOrEmpty(int[] values, bool expectedResult)
    {
        var set = values == null ? null : new HashSet<int>(values);

        set.IsNullOrEmpty().ShouldBe(expectedResult);
    }

    [Fact]
    public void Test_AddRange_adds_only_new_items_and_returns_the_number_added()
    {
        var set = new HashSet<int> { 1, 2 };

        var addedCount = set.AddRange([2, 3, 4]);

        addedCount.ShouldBe(2);
        set.ShouldBe([1, 2, 3, 4], ignoreOrder: true);
    }

    [Fact]
    public void Test_AddRange_with_null_items_adds_nothing()
    {
        var set = new HashSet<int> { 1, 2 };

        var addedCount = set.AddRange(null);

        addedCount.ShouldBe(0);
        set.ShouldBe([1, 2], ignoreOrder: true);
    }

    [Fact]
    public void Test_AddRange_with_null_set_throws()
    {
        HashSet<int> set = null;

        Should.Throw<ArgumentNullException>(() => set.AddRange([1, 2]));
    }

    [Fact]
    public void Test_ToHashSet_creates_a_set_from_an_enumerable()
    {
        IEnumerable<int> source = [1, 2, 2, 3];

        var result = source.ToHashSet();

        result.ShouldBeOfType<HashSet<int>>();
        result.ShouldBe([1, 2, 3], ignoreOrder: true);
    }

    [Fact]
    public void Test_ToHashSet_uses_the_supplied_comparer()
    {
        IEnumerable<string> source = ["a", "A", "b"];

        var result = source.ToHashSet(StringComparer.OrdinalIgnoreCase);

        result.Count.ShouldBe(2);
    }
}
