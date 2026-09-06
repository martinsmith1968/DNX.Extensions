using System;
using DNX.Extensions.Exceptions;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Exceptions;

public class ParameterExceptionTests
{
    [Fact]
    public void Constructor_with_name_and_message_sets_expected_properties()
    {
        var exception = new ParameterException("name", "message");

        exception.ParamName.ShouldBe("name");
        exception.ParamValue.ShouldBeNull();
        exception.Message.ShouldBe("message");
        exception.InnerException.ShouldBeNull();
    }

    [Fact]
    public void Constructor_with_name_value_and_message_sets_expected_properties()
    {
        var value = 42;
        var exception = new ParameterException("name", value, "message");

        exception.ParamName.ShouldBe("name");
        exception.ParamValue.ShouldBe(value);
        exception.Message.ShouldBe("message");
        exception.InnerException.ShouldBeNull();
    }

    [Fact]
    public void Constructor_with_name_message_and_inner_exception_sets_expected_properties()
    {
        var inner = new InvalidOperationException("inner");
        var exception = new ParameterException("name", "message", inner);

        exception.ParamName.ShouldBe("name");
        exception.ParamValue.ShouldBeNull();
        exception.Message.ShouldBe("message");
        exception.InnerException.ShouldBe(inner);
    }

    [Fact]
    public void Constructor_with_name_value_message_and_inner_exception_sets_expected_properties()
    {
        var value = new object();
        var inner = new InvalidOperationException("inner");
        var exception = new ParameterException("name", value, "message", inner);

        exception.ParamName.ShouldBe("name");
        exception.ParamValue.ShouldBe(value);
        exception.Message.ShouldBe("message");
        exception.InnerException.ShouldBe(inner);
    }
}
