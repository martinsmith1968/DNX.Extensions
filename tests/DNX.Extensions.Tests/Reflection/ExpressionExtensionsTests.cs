using System.Linq.Expressions;
using DNX.Extensions.Reflection;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Reflection;

public class ExpressionExtensionsTests
{
    private static int StaticProperty => 42;
    private static int SomeMethod() => 5;

    public class IsMemberExpressionTests
    {
        [Fact]
        public void Returns_true_for_a_captured_local_variable()
        {
            var localVar = 5;

            ExpressionExtensions.IsMemberExpression(() => localVar).ShouldBeTrue();
        }

        [Fact]
        public void Returns_true_for_a_property_access()
        {
            ExpressionExtensions.IsMemberExpression(() => StaticProperty).ShouldBeTrue();
        }

        [Fact]
        public void Returns_false_for_a_method_call()
        {
            ExpressionExtensions.IsMemberExpression(() => SomeMethod()).ShouldBeFalse();
        }
    }

    public class IsLambdaExpressionTests
    {
        [Fact]
        public void Returns_true_when_the_body_is_itself_a_lambda()
        {
            Expression<Func<Func<int>>> exp = () => () => 5;

            ExpressionExtensions.IsLambdaExpression(exp).ShouldBeTrue();
        }

        [Fact]
        public void Returns_false_for_a_member_access()
        {
            var localVar = 5;

            ExpressionExtensions.IsLambdaExpression(() => localVar).ShouldBeFalse();
        }

        [Fact]
        public void Returns_false_for_a_method_call()
        {
            ExpressionExtensions.IsLambdaExpression(() => SomeMethod()).ShouldBeFalse();
        }
    }

    public class IsUnaryExpressionTests
    {
        [Fact]
        public void Returns_true_for_a_boxing_conversion()
        {
            var localVar = 5;

            ExpressionExtensions.IsUnaryExpression(() => (object)localVar).ShouldBeTrue();
        }

        [Fact]
        public void Returns_false_for_a_member_access()
        {
            var localVar = 5;

            ExpressionExtensions.IsUnaryExpression(() => localVar).ShouldBeFalse();
        }

        [Fact]
        public void Returns_false_for_a_method_call()
        {
            ExpressionExtensions.IsUnaryExpression(() => SomeMethod()).ShouldBeFalse();
        }
    }

    public class GetMemberNameTests
    {
        [Fact]
        public void Returns_the_name_of_a_captured_local_variable()
        {
            var localVar = 5;

            ExpressionExtensions.GetMemberName(() => localVar).ShouldBe(nameof(localVar));
        }

        [Fact]
        public void Returns_the_name_of_a_property_access()
        {
            ExpressionExtensions.GetMemberName(() => StaticProperty).ShouldBe(nameof(StaticProperty));
        }

        [Fact]
        public void Returns_null_for_a_method_call()
        {
            ExpressionExtensions.GetMemberName(() => SomeMethod()).ShouldBeNull();
        }
    }

    public class GetLambdaNameTests
    {
        [Fact]
        public void Returns_the_name_of_a_manually_named_lambda()
        {
            var innerLambda = Expression.Lambda<Func<int>>(Expression.Constant(5), "MyNamedLambda", []);
            var outerLambda = Expression.Lambda<Func<Func<int>>>(innerLambda);

            ExpressionExtensions.GetLambdaName(outerLambda).ShouldBe("MyNamedLambda");
        }

        [Fact]
        public void Returns_null_for_a_compiler_generated_nested_lambda_since_it_has_no_name()
        {
            Expression<Func<Func<int>>> exp = () => () => 5;

            ExpressionExtensions.GetLambdaName(exp).ShouldBeNull();
        }

        [Fact]
        public void Returns_null_for_a_method_call()
        {
            ExpressionExtensions.GetLambdaName(() => SomeMethod()).ShouldBeNull();
        }
    }

    public class GetUnaryNameTests
    {
        [Fact]
        public void Returns_the_member_name_for_a_boxed_member_access()
        {
            var localVar = 5;

            ExpressionExtensions.GetUnaryName(() => (object)localVar).ShouldBe(nameof(localVar));
        }

        [Fact]
        public void Returns_null_when_the_boxed_operand_is_not_a_member_access()
        {
            ExpressionExtensions.GetUnaryName(() => (object)SomeMethod()).ShouldBeNull();
        }

        [Fact]
        public void Returns_null_for_a_method_call()
        {
            ExpressionExtensions.GetUnaryName(() => SomeMethod()).ShouldBeNull();
        }
    }

    public class GetExpressionNameTests
    {
        [Fact]
        public void Delegates_to_GetMemberName_for_a_member_access()
        {
            var localVar = 5;

            ExpressionExtensions.GetExpressionName(() => localVar).ShouldBe(nameof(localVar));
        }

        [Fact]
        public void Delegates_to_GetLambdaName_for_a_nested_lambda_body()
        {
            Expression<Func<Func<int>>> exp = () => () => 5;

            ExpressionExtensions.GetExpressionName(exp).ShouldBeNull();
        }

        [Fact]
        public void Delegates_to_GetUnaryName_for_a_boxed_member_access()
        {
            var localVar = 5;

            ExpressionExtensions.GetExpressionName(() => (object)localVar).ShouldBe(nameof(localVar));
        }

        [Fact]
        public void Returns_null_for_a_method_call()
        {
            ExpressionExtensions.GetExpressionName(() => SomeMethod()).ShouldBeNull();
        }
    }
}
