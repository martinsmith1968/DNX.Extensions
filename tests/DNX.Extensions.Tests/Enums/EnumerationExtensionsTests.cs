using System.ComponentModel;
using DNX.Extensions.Enumerations;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Enums;

[AttributeUsage(AttributeTargets.Field)]
internal class MultiplierAttribute(int multiplier) : Attribute
{
    public int Multiplier { get; set; } = multiplier;
}

internal enum MyTestEnum1
{
    [Description("First")]
    One = 1,

    [Description("Second")]
    Two = 2,

    [Description("Third")]
    Three = 3,

    [Description("Fourth")]
    Four = 4,

    [Description("Fifth")]
    Five = 5
}

[Flags]
internal enum MyTestEnum2
{
    Flag1 = 1,
    Flag2 = 2,
    Flag3 = 4,
    Flag4 = 8,
    Flag5 = 16
}

internal enum MyTestEnum3
{
    [Multiplier(10)]
    Ten = 10,

    Twenty = 20,

    [Multiplier(15)]
    Thirty = 30,

    Fourty = 40,

    [Multiplier(1000)]
    Fifty = 50
}

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

public class EnumerationExtensionsTests
{
    [Theory]
    [InlineData("One", "One")]
    [InlineData("Four", "Four")]
    [InlineData("Five", "Five")]
    public void ParseEnumTest_can_successfully_parse_MyTestEnum1(string text, string expectedResult)
    {
        var result = text.ParseEnum<MyTestEnum1>();

        result.ToString().ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("ONE", false)]
    [InlineData("Twenty", false)]
    [InlineData("Six", false)]
    public void ParseEnumTest_fails_to_parse_MyTestEnum1(string text, bool expectedResult)
    {
        try
        {
            var result = text.ParseEnum<MyTestEnum1>();

            expectedResult.ShouldBeTrue();
        }
        catch (ArgumentException ex)
        {
            ex.Message.ShouldContain($"'{text}'");

            expectedResult.ShouldBeFalse();
        }
    }


    [Fact]
    public void ParseEnumTest1()
    {

    }

    [Fact]
    public void ParseEnumOrDefaultTest()
    {

    }

    [Fact]
    public void ParseEnumOrDefaultTest1()
    {

    }

    [Theory]
    [InlineData(typeof(MyTestEnum1), "One", false, true)]
    [InlineData(typeof(MyTestEnum1), "Three", false, true)]
    [InlineData(typeof(MyTestEnum1), "Five", false, true)]
    [InlineData(typeof(MyTestEnum1), "5", false, false)]
    [InlineData(typeof(MyTestEnum2), "Flag3", false, true)]
    [InlineData(typeof(MyTestEnum2), "Flag4", false, true)]
    [InlineData(typeof(MyTestEnum2), "Flag5", false, true)]
    [InlineData(typeof(MyTestEnum2), "Flag6", false, false)]
    [InlineData(typeof(MyTestEnum1), "ONE", false, false)]
    [InlineData(typeof(MyTestEnum1), "ThReE", false, false)]
    [InlineData(typeof(MyTestEnum1), "five", false, false)]
    [InlineData(typeof(MyTestEnum1), "OnE", true, true)]
    [InlineData(typeof(MyTestEnum1), "ThReE", true, true)]
    [InlineData(typeof(MyTestEnum1), "FIVE", true, true)]
    [InlineData(typeof(MyTestEnum1), "Five", true, true)]
    public void IsValidEnumTest_string(Type type, string text, bool ignoreCase, bool expectedResult)
    {
        var result = text.IsValidEnum(type, ignoreCase);

        result.ShouldBe(expectedResult);
    }

    [Fact]
    public void GetMaxValueTest()
    {
        var max1 = EnumerationExtensions.GetMaxValue<MyTestEnum1>();
        var max2 = EnumerationExtensions.GetMaxValue<MyTestEnum2>();

        ((int)max1).ShouldBe((int)MyTestEnum1.Five);
        ((int)max2).ShouldBe((int)MyTestEnum2.Flag5);
    }

    [Fact]
    public void GetMinValueTest()
    {
        var max1 = EnumerationExtensions.GetMinValue<MyTestEnum1>();
        var max2 = EnumerationExtensions.GetMinValue<MyTestEnum2>();

        ((int)max1).ShouldBe((int)MyTestEnum1.One);
        ((int)max2).ShouldBe((int)MyTestEnum2.Flag1);
    }

    [Fact]
    public void IsValueOneOfTest()
    {

    }

    [Fact]
    public void IsValueOneOfTest1()
    {

    }

    [Fact]
    public void ManipulateFlagTest()
    {
        // Arrange
        var flags1 = MyTestEnum2.Flag2 | MyTestEnum2.Flag4;
        var flags2 = MyTestEnum2.Flag2 | MyTestEnum2.Flag4;

        // Act
        flags1 = flags1.ManipulateFlag(MyTestEnum2.Flag3, true);
        flags2 = flags2.ManipulateFlag(MyTestEnum2.Flag4, false);

        // Assert
        flags1.ShouldBe(MyTestEnum2.Flag2 | MyTestEnum2.Flag3 | MyTestEnum2.Flag4);
        flags2.ShouldBe(MyTestEnum2.Flag2);
    }

    [Fact]
    public void SetFlagTest()
    {
        // Arrange
        var flags = MyTestEnum2.Flag2 | MyTestEnum2.Flag4;

        // Act
        flags = flags.SetFlag(MyTestEnum2.Flag3);

        // Assert
        flags.ShouldBe(MyTestEnum2.Flag2 | MyTestEnum2.Flag3 | MyTestEnum2.Flag4);
    }

    [Fact]
    public void UnsetFlagTest()
    {
        // Arrange
        var flags = MyTestEnum2.Flag2 | MyTestEnum2.Flag4 | MyTestEnum2.Flag5;

        // Act
        flags = flags.UnsetFlag(MyTestEnum2.Flag2);

        // Assert
        flags.ShouldBe(MyTestEnum2.Flag4 | MyTestEnum2.Flag5);
    }

    [Fact]
    public void GetSetValuesListTest()
    {
        // Arrange
        var flags = MyTestEnum2.Flag2 | MyTestEnum2.Flag4 | MyTestEnum2.Flag5;

        // Act
        var setFlags = EnumerationExtensions.GetSetValuesList<MyTestEnum2>(flags);

        // Assert
        setFlags.Count.ShouldBe(3);
        setFlags.Contains(MyTestEnum2.Flag2).ShouldBeTrue();
        setFlags.Contains(MyTestEnum2.Flag4).ShouldBeTrue();
        setFlags.Contains(MyTestEnum2.Flag5).ShouldBeTrue();
    }

    [Fact]
    public void ToDictionaryTest_MyTestEnum1()
    {
        var dict = EnumerationExtensions.ToDictionaryByName<MyTestEnum1>();

        dict.ShouldNotBeNull();
        dict.Count.ShouldBe(5);
        dict[MyTestEnum1.One.ToString()].ShouldBe(MyTestEnum1.One);
        dict[MyTestEnum1.Two.ToString()].ShouldBe(MyTestEnum1.Two);
        dict[MyTestEnum1.Three.ToString()].ShouldBe(MyTestEnum1.Three);
        dict[MyTestEnum1.Four.ToString()].ShouldBe(MyTestEnum1.Four);
        dict[MyTestEnum1.Five.ToString()].ShouldBe(MyTestEnum1.Five);
    }

    [Fact]
    public void ToDictionaryTest_MyTestEnum2()
    {
        var dict = EnumerationExtensions.ToDictionaryByName<MyTestEnum2>();

        dict.ShouldNotBeNull();
        dict.Count.ShouldBe(5);
        dict[MyTestEnum2.Flag1.ToString()].ShouldBe(MyTestEnum2.Flag1);
        dict[MyTestEnum2.Flag2.ToString()].ShouldBe(MyTestEnum2.Flag2);
        dict[MyTestEnum2.Flag3.ToString()].ShouldBe(MyTestEnum2.Flag3);
        dict[MyTestEnum2.Flag4.ToString()].ShouldBe(MyTestEnum2.Flag4);
        dict[MyTestEnum2.Flag5.ToString()].ShouldBe(MyTestEnum2.Flag5);
    }

    [Theory]
    [InlineData(MyTestEnum1.One, "First")]
    [InlineData(MyTestEnum1.Two, "Second")]
    [InlineData(MyTestEnum1.Three, "Third")]
    [InlineData(MyTestEnum1.Four, "Fourth")]
    [InlineData(MyTestEnum1.Five, "Fifth")]
    [InlineData(MyTestEnum2.Flag2, null)]
    public void GetDescriptionTest(Enum value, string expectedResult)
    {
        // Act
        var result = value.GetDescription();

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(MyTestEnum3.Ten, 10)]
    [InlineData(MyTestEnum3.Twenty, null)]
    [InlineData(MyTestEnum3.Thirty, 15)]
    [InlineData(MyTestEnum3.Fourty, null)]
    [InlineData(MyTestEnum3.Fifty, 1000)]
    public void GetAttributeTest(Enum value, int? expectedResult)
    {
        var attribute = value.GetAttribute<MultiplierAttribute>();

        (attribute?.Multiplier).ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData(MyType.One, null)]
    [InlineData(MyType.Two, "Number 2")]
    [InlineData(MyType.Three, "")]
    [InlineData(MyType.Four, null)]
    [InlineData(MyType.Five, "")]
    public void GetDescription_can_retrieve_value_correctly(MyType myType, string expectedResult)
    {
        // Act
        var result = myType.GetDescription();

        // Assert
        result.ShouldBe(expectedResult, $"{myType} has description: {result}");
    }

    [Theory]
    [InlineData(MyType.One, "One")]
    [InlineData(MyType.Two, "Number 2")]
    [InlineData(MyType.Three, "")]
    [InlineData(MyType.Four, "Four")]
    [InlineData(MyType.Five, "")]
    public void GetDescriptionOrName_can_retrieve_value_correctly(MyType myType, string expectedResult)
    {
        // Act
        var result = myType.GetDescriptionOrName();

        // Assert
        result.ShouldBe(expectedResult, $"{myType} has description: {result}");
    }
}
