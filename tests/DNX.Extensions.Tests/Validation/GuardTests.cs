using DNX.Extensions.Maths;
using DNX.Extensions.Validation.BuiltInTypes;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Validation;

public class GuardTests
{
    private enum Status
    {
        Pending = 0,
        Active = 1,
        Disabled = 2
    }

    [Fact]
    public void IsTrue_throws_when_expression_evaluates_to_false()
    {
        var isEnabled = true;

        Guard.IsTrue(() => isEnabled);

        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsTrue(() => false));
    }

    [Fact]
    public void IsTrue_with_precomputed_value_throws_when_false()
    {
        var isEnabled = true;

        Guard.IsTrue(() => isEnabled, isEnabled);

        var ex = Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsTrue(() => isEnabled, false));
        ex.ParamName.ShouldBe("isEnabled");
    }

    [Fact]
    public void IsFalse_throws_when_expression_evaluates_to_true()
    {
        var isEnabled = false;

        Guard.IsFalse(() => isEnabled);

        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsFalse(() => true));
    }

    [Fact]
    public void IsFalse_with_precomputed_value_throws_when_true()
    {
        var isEnabled = false;

        Guard.IsFalse(() => isEnabled, isEnabled);

        var ex = Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsFalse(() => isEnabled, true));
        ex.ParamName.ShouldBe("isEnabled");
    }

    [Fact]
    public void IsNotNull_throws_when_reference_is_null()
    {
        var value = new object();

        Guard.IsNotNull(() => value);

        var ex = Should.Throw<ArgumentNullException>(() => Guard.IsNotNull(() => (object)null));
        ex.Message.ShouldContain("must not be null");
    }

    [Fact]
    public void IsNotNull_with_precomputed_value_throws_when_null()
    {
        var value = new object();

        Guard.IsNotNull(() => value, value);

        var ex = Should.Throw<ArgumentNullException>(() => Guard.IsNotNull(() => (object)null, (object)null));
        ex.Message.ShouldContain("must not be null");
    }

    [Fact]
    public void IsNotNullOrEmpty_throws_when_string_is_null_or_empty()
    {
        var value = "hello";

        Guard.IsNotNullOrEmpty(() => value);

        Should.Throw<ArgumentException>(() => Guard.IsNotNullOrEmpty(() => string.Empty));
        Should.Throw<ArgumentException>(() => Guard.IsNotNullOrEmpty(() => (string)null));
    }

    [Fact]
    public void IsNotNullOrEmpty_with_precomputed_value_throws_when_null_or_empty()
    {
        var value = "hello";

        Guard.IsNotNullOrEmpty(() => value, value);

        Should.Throw<ArgumentException>(() => Guard.IsNotNullOrEmpty(() => value, string.Empty));
        Should.Throw<ArgumentException>(() => Guard.IsNotNullOrEmpty(() => value, null));
    }

    [Fact]
    public void IsNotNullOrWhitespace_throws_when_string_is_null_or_whitespace()
    {
        var value = "hello";

        Guard.IsNotNullOrWhitespace(() => value);

        Should.Throw<ArgumentException>(() => Guard.IsNotNullOrWhitespace(() => " "));
        Should.Throw<ArgumentException>(() => Guard.IsNotNullOrWhitespace(() => (string)null));
    }

    [Fact]
    public void IsNotNullOrWhitespace_with_precomputed_value_throws_when_null_or_whitespace()
    {
        var value = "hello";

        Guard.IsNotNullOrWhitespace(() => value, value);

        Should.Throw<ArgumentException>(() => Guard.IsNotNullOrWhitespace(() => value, " "));
        Should.Throw<ArgumentException>(() => Guard.IsNotNullOrWhitespace(() => value, null));
    }

    [Fact]
    public void IsValidEnum_throws_when_value_is_not_a_member()
    {
        var status = Status.Active;

        Guard.IsValidEnum(() => status);

        Should.Throw<ArgumentException>(() => Guard.IsValidEnum(() => (Status)99));
    }

    [Fact]
    public void IsValidEnum_with_precomputed_value_throws_when_not_a_member()
    {
        var status = Status.Active;

        Guard.IsValidEnum(() => status, status);

        var ex = Should.Throw<ArgumentException>(() => Guard.IsValidEnum(() => status, (Status)99));
        ex.ParamName.ShouldBe("status");
    }

    [Fact]
    public void IsEnumOneOf_accepts_and_rejects_values()
    {
        var status = Status.Active;

        Guard.IsEnumOneOf(() => status, Status.Pending, Status.Active);
        Should.Throw<ArgumentException>(() => Guard.IsEnumOneOf(() => Status.Disabled, new[] { Status.Pending, Status.Active }));

        IList<Status> allowed = new[] { Status.Pending, Status.Active };
        Guard.IsEnumOneOf(() => status, allowed);
        Guard.IsEnumOneOf(() => status, status, allowed);
        Should.Throw<ArgumentException>(() => Guard.IsEnumOneOf(() => Status.Disabled, Status.Disabled, allowed));
    }

    [Fact]
    public void Numeric_guard_methods_verify_bounds()
    {
        var score = 7;

        Guard.IsGreaterThan(() => score, 5);
        Guard.IsGreaterThanOrEqualTo(() => score, 7);
        Guard.IsLessThan(() => score, 10);
        Guard.IsLessThanOrEqualTo(() => score, 7);
        Guard.IsBetween(() => score, 5, 10);
        Guard.IsBetween(() => score, 10, 5, true, IsBetweenBoundsType.Inclusive);

        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsGreaterThan(() => score, 10));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsGreaterThanOrEqualTo(() => score, 8));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsLessThan(() => score, 7));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsLessThanOrEqualTo(() => score, 6));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsBetween(() => score, 8, 10));
    }

    [Fact]
    public void Numeric_guard_methods_report_the_parameter_name_and_value()
    {
        var score = 7;

        var ex = Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsGreaterThan(() => score, 10));
        ex.ParamName.ShouldBe("score");
        ex.ActualValue.ShouldBe(score);
        ex.Message.ShouldContain("score must be greater than 10");
    }

    [Fact]
    public void Numeric_guard_methods_support_the_precomputed_value_overloads()
    {
        var score = 7;

        Guard.IsGreaterThan(() => score, score, 5);
        Guard.IsGreaterThanOrEqualTo(() => score, score, 7);
        Guard.IsLessThan(() => score, score, 10);
        Guard.IsLessThanOrEqualTo(() => score, score, 7);

        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsGreaterThan(() => score, score, 10));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsGreaterThanOrEqualTo(() => score, score, 8));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsLessThan(() => score, score, 7));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsLessThanOrEqualTo(() => score, score, 6));
    }

    [Fact]
    public void IsBetween_respects_the_bounds_type()
    {
        var score = 5;

        // Boundary value: included under Inclusive, rejected under Exclusive
        Guard.IsBetween(() => score, 5, 10, IsBetweenBoundsType.Inclusive);

        var ex = Should.Throw<ArgumentOutOfRangeException>(
            () => Guard.IsBetween(() => score, 5, 10, IsBetweenBoundsType.Exclusive));
        ex.ParamName.ShouldBe("score");
    }

    [Fact]
    public void IsBetween_only_normalizes_bound_order_when_allowed()
    {
        var score = 7;

        // Reversed bounds (10, 5) are only valid when allowEitherOrder is true
        Guard.IsBetween(() => score, 10, 5, true, IsBetweenBoundsType.Inclusive);

        Should.Throw<ArgumentOutOfRangeException>(
            () => Guard.IsBetween(() => score, 10, 5, false, IsBetweenBoundsType.Inclusive));
    }

    [Fact]
    public void IsBetween_with_precomputed_value_verifies_bounds()
    {
        var score = 7;

        Guard.IsBetween(() => score, score, 5, 10, true, IsBetweenBoundsType.Inclusive);

        var ex = Should.Throw<ArgumentOutOfRangeException>(
            () => Guard.IsBetween(() => score, 12, 5, 10, true, IsBetweenBoundsType.Inclusive));
        ex.ParamName.ShouldBe("score");
    }
}
