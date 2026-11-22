using DNX.Extensions.Collections;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Collections;

public class QueueExtensionsTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData(new int[] { }, true)]
    [InlineData(new[] { 1, 2, 3 }, false)]
    public void Test_IsNullOrEmpty(int[] values, bool expectedResult)
    {
        var queue = values == null ? null : new Queue<int>(values);

        queue.IsNullOrEmpty().ShouldBe(expectedResult);
    }

    [Fact]
    public void Test_EnqueueRange_enqueues_items_in_order_supplied()
    {
        var queue = new Queue<int>([1, 2]);

        queue.EnqueueRange([3, 4]);

        queue.ToArray().ShouldBe([1, 2, 3, 4]);
    }

    [Fact]
    public void Test_EnqueueRange_with_null_items_does_nothing()
    {
        var queue = new Queue<int>([1, 2]);

        queue.EnqueueRange(null);

        queue.ToArray().ShouldBe([1, 2]);
    }

    [Fact]
    public void Test_EnqueueRange_with_null_queue_throws()
    {
        Queue<int> queue = null;

        Should.Throw<ArgumentNullException>(() => queue.EnqueueRange([1, 2]));
    }
}
