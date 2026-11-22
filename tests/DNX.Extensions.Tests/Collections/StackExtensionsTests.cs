using DNX.Extensions.Collections;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Collections;

public class StackExtensionsTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData(new int[] { }, true)]
    [InlineData(new[] { 1, 2, 3 }, false)]
    public void Test_IsNullOrEmpty(int[] values, bool expectedResult)
    {
        var stack = values == null ? null : new Stack<int>(values);

        stack.IsNullOrEmpty().ShouldBe(expectedResult);
    }

    [Fact]
    public void Test_PushRange_pushes_items_so_the_last_one_ends_up_on_top()
    {
        var stack = new Stack<int>();

        stack.PushRange([1, 2, 3]);

        stack.Pop().ShouldBe(3);
        stack.Pop().ShouldBe(2);
        stack.Pop().ShouldBe(1);
    }

    [Fact]
    public void Test_PushRange_with_null_items_does_nothing()
    {
        var stack = new Stack<int>([1]);

        stack.PushRange(null);

        stack.Count.ShouldBe(1);
    }

    [Fact]
    public void Test_PushRange_with_null_stack_throws()
    {
        Stack<int> stack = null;

        Should.Throw<ArgumentNullException>(() => stack.PushRange([1, 2]));
    }
}
