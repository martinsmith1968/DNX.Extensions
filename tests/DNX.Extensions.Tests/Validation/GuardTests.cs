using System;
using DNX.Extensions.Validation;
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
    public void IsFalse_throws_when_expression_evaluates_to_true()
    {
        var isEnabled = false;

        Guard.IsFalse(() => isEnabled);

        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsFalse(() => true));
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
    public void IsNotNullOrEmpty_throws_when_string_is_null_or_empty()
    {
        var value = "hello";

        Guard.IsNotNullOrEmpty(() => value);

        Should.Throw<ArgumentException>(() => Guard.IsNotNullOrEmpty(() => string.Empty));
        Should.Throw<ArgumentException>(() => Guard.IsNotNullOrEmpty(() => (string)null));
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
    public void IsValidEnum_throws_when_value_is_not_a_member()
    {
        var status = Status.Active;

        Guard.IsValidEnum(() => status);

        Should.Throw<ArgumentException>(() => Guard.IsValidEnum(() => (Status)99));
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
        Guard.IsBetween(() => score, 10, 5, true, DNX.Extensions.Maths.IsBetweenBoundsType.Inclusive);

        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsGreaterThan(() => score, 10));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsGreaterThanOrEqualTo(() => score, 8));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsLessThan(() => score, 7));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsLessThanOrEqualTo(() => score, 6));
        Should.Throw<ArgumentOutOfRangeException>(() => Guard.IsBetween(() => score, 8, 10));
    }
}
