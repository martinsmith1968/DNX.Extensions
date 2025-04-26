using DNX.Extensions.Exceptions;
using Shouldly;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace DNX.Extensions.Tests.Exceptions;

public class ReadOnlyListExceptionTests
{
    [Fact]
    public void Test_ReadOnlyListException_constructor_list()
    {
        // Arrange
        var list = "abcdefghij".ToCharArray().ToList();

        // Act
        var ex = new ReadOnlyListException<char>(list);

        // Assert
        ex.ShouldNotBeNull();
        ex.List.ShouldBe(list);
    }
}
