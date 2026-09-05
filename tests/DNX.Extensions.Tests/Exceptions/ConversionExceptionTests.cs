using DNX.Extensions.Exceptions;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Exceptions;

public class ConversionExceptionTests
{
    [Fact]
    public void Test_ConversionException_constructor_value_message()
    {
        // Arrange
        var value = "abc";
        var message = "Unable to convert value";

        // Act
        var ex = new ConversionException(value, message);

        // Assert
        ex.ShouldNotBeNull();
        value.ShouldBe(ex.Value);
        message.ShouldBe(ex.Message);
        ex.ConvertType.ShouldBeNull();
    }

    [Fact]
    public void Test_ConversionException_constructor_value_message_type()
    {
        // Arrange
        var value = "abc";
        var message = "Unable to convert value";
        var type = typeof(int);

        // Act
        var ex = new ConversionException(value, message, type);

        // Assert
        ex.ShouldNotBeNull();
        value.ShouldBe(ex.Value);
        message.ShouldBe(ex.Message);
        type.ShouldBe(ex.ConvertType);

        ex.ShouldNotBeNull();
        ex.Value.ShouldBe(value);
        ex.Message.ShouldBe(message);
        ex.ConvertType.ShouldBe(type);
    }
}
