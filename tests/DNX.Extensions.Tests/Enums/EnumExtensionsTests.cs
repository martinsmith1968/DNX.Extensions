using System.ComponentModel;
using DNX.Extensions.Enums;
using FluentAssertions;
using Xunit;

namespace DNX.Extensions.Tests.Enums
{
    public enum MyType
    {
        One = 1,

        [Description("Number 2")]
        Two = 2,

        [Description]
        Three = 3,

        [Description(null)]
        Four = 4,

        [Description("")]
        Five = 5
    }

    public class EnumExtensionsTests
    {
        [Theory]
        [InlineData(MyType.One, "One")]
        [InlineData(MyType.Two, "Number 2")]
        [InlineData(MyType.Three, "")]
        [InlineData(MyType.Four, null)]
        [InlineData(MyType.Five, "")]
        public void GetDescription_can_retrieve_value_correctly(MyType myType, string expectedResult)
        {
            // Act
            var result = myType.GetDescription();

            // Assert
            result.Should().Be(expectedResult, $"{myType} has description: {result}");
        }
    }
}
