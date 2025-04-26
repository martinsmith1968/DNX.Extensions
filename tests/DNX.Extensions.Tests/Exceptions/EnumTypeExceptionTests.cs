using DNX.Extensions.Exceptions;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Exceptions;

public class EnumTypeExceptionTests
{
    [Fact]
    public void Test_EnumTypeException_constructor_type()
    {
        // Arrange
        var type = typeof(int);

        // Act
        var ex = new EnumTypeException(type);

        // Assert
        ex.ShouldNotBeNull();
        ex.Type.ShouldBe(type);
    }

    [Fact]
    public void Test_EnumTypeException_constructor_type_messageTemplate()
    {
        // Arrange
        var messageTemplate = "Customer message about: {0}";
        var type = typeof(int);

        // Act
        var ex = new EnumTypeException(type, messageTemplate);

        // Assert
        ex.ShouldNotBeNull();
        ex.Type.ShouldBe(type);
        ex.Message.ShouldBe(ex.Message.Replace("{0}", type.Name));
    }
}
