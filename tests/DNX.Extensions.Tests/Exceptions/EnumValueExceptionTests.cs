using DNX.Extensions.Exceptions;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Exceptions;

internal enum MyEnumValueTestEnum
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5
}

public class EnumValueExceptionTests
{
    [Fact]
    public void Test_EnumValueException_constructor_valid_value()
    {
        // Arrange
        var value = MyEnumValueTestEnum.Four;

        // Act
        var ex = new EnumValueException<MyEnumValueTestEnum>(value);

        // Assert
        ex.ShouldNotBeNull();
        ex.Type.ShouldBe(value.GetType());
        ex.Value.ShouldBe(value);
        ex.Message.ShouldContain(typeof(MyEnumValueTestEnum).Name);
        ex.Message.ShouldContain(value.ToString());
    }

    [Fact]
    public void Test_EnumValueException_constructor_valid_value_messageTemplate()
    {
        // Arrange
        var messageTemplate = "Customer message about: {0}";
        var value = MyEnumValueTestEnum.Three;

        // Act
        var ex = new EnumValueException<MyEnumValueTestEnum>(value, messageTemplate);

        // Assert
        ex.ShouldNotBeNull();
        ex.Type.ShouldBe(value.GetType());
        ex.Message.ShouldBe(ex.Message.Replace("{0}", value.ToString()).Replace("{1}", value.GetType().Name));
    }

    [Fact]
    public void Test_EnumValueException_constructor_invalid_value()
    {
        // Arrange
        var value = (MyEnumValueTestEnum)int.MaxValue;

        // Act
        var ex = new EnumValueException<MyEnumValueTestEnum>(value);

        // Assert
        ex.ShouldNotBeNull();
        ex.Type.ShouldBe(value.GetType());
        ex.Value.ShouldBe(value);
        ex.Message.ShouldContain(typeof(MyEnumValueTestEnum).Name);
        ex.Message.ShouldContain(value.ToString());
    }

    [Fact]
    public void Test_EnumValueException_constructor_invalid_value_messageTemplate()
    {
        // Arrange
        var messageTemplate = "Customer message about: {0} - {1}";
        var value = (MyEnumValueTestEnum) int.MaxValue;

        // Act
        var ex = new EnumValueException<MyEnumValueTestEnum>(value, messageTemplate);

        // Assert
        ex.ShouldNotBeNull();
        ex.Type.ShouldBe(value.GetType());
        ex.Message.ShouldBe(ex.Message.Replace("{0}", value.ToString()).Replace("{1}", value.GetType().Name));
    }
}
