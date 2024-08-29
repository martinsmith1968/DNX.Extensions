using DNX.Extensions.Reflection;
using FluentAssertions;
using Xunit;

#pragma warning disable xUnit1044   // TestData not serializable

namespace DNX.Extensions.Tests.Reflection;

public class ExpressionExtensionsTests
{
    // TODO: Not sure how to test these




    [Fact]
    public void IsMemberExpression_func_Tests()
    {
        ExpressionExtensions.IsMemberExpression(() => BooleanFunc()).Should().BeFalse();
        ExpressionExtensions.IsMemberExpression(() => StringFunc()).Should().BeFalse();
        ExpressionExtensions.IsMemberExpression(() => Int32Func()).Should().BeFalse();
        ExpressionExtensions.IsMemberExpression(() => DoubleFunc()).Should().BeFalse();
    }

    [Fact]
    public void IsMemberExpression_action_Tests()
    {
        // TODO: This looks wrong
        ExpressionExtensions.IsMemberExpression<object>(() => Action1()).Should().BeFalse();
    }

    [Fact]
    public void IsLambdaExpression_func_Tests()
    {
        ExpressionExtensions.IsLambdaExpression(() => BooleanFunc()).Should().BeFalse();
        ExpressionExtensions.IsLambdaExpression(() => StringFunc()).Should().BeFalse();
        ExpressionExtensions.IsLambdaExpression(() => Int32Func()).Should().BeFalse();
        ExpressionExtensions.IsLambdaExpression(() => DoubleFunc()).Should().BeFalse();
    }

    [Fact]
    public void IsUnaryExpression_func_Tests()
    {
        ExpressionExtensions.IsUnaryExpression(() => BooleanFunc()).Should().BeFalse();
        ExpressionExtensions.IsUnaryExpression(() => StringFunc()).Should().BeFalse();
        ExpressionExtensions.IsUnaryExpression(() => Int32Func()).Should().BeFalse();
        ExpressionExtensions.IsUnaryExpression(() => DoubleFunc()).Should().BeFalse();
    }




    private static bool BooleanFunc() => DateTime.UtcNow.Millisecond % 2 == 0;
    private static string StringFunc() => Environment.MachineName;
    private static int Int32Func() => DateTime.UtcNow.Second;
    private static double DoubleFunc() => DateTime.UtcNow.Millisecond;

    private static void Action1() { }
}
