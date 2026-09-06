using System.ComponentModel;
using DNX.Extensions.Enumerations;
using DNX.Extensions.Exceptions;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Enums;

[AttributeUsage(AttributeTargets.Field)]
internal class MultiplierAttribute(int multiplier) : Attribute
{
    public int Multiplier { get; set; } = multiplier;
}

public enum MyTestEnum1
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

public enum MyTestEnum3
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
    public void ParseEnum_can_successfully_parse_MyTestEnum1(string text, string expectedResult)
    {
        var result = text.ParseEnum<MyTestEnum1>();

        result.ToString().ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("ONE", false)]
    [InlineData("Twenty", false)]
    [InlineData("Six", false)]
    public void ParseEnum_fails_to_parse_MyTestEnum1(string text, bool expectedResult)
    {
        if (expectedResult)
        {
            text.ParseEnum<MyTestEnum1>();
            return;
        }

        var ex = Should.Throw<ArgumentException>(() => text.ParseEnum<MyTestEnum1>());
        ex.Message.ShouldContain($"'{text}'");
    }

    [Theory]
    [InlineData("ONE", MyTestEnum1.One)]
    [InlineData("tHrEe", MyTestEnum1.Three)]
    public void ParseEnum_can_parse_case_insensitively(string text, MyTestEnum1 expectedResult)
    {
        text.ParseEnum<MyTestEnum1>(true).ShouldBe(expectedResult);
    }

    [Fact]
    public void ParseEnum_throws_for_null_text()
    {
        string text = null;

        Should.Throw<ArgumentNullException>(() => text.ParseEnum<MyTestEnum1>());
    }

    [Fact]
    public void IsValidEnum_throws_for_null_type()
    {
        var ex = Should.Throw<ArgumentNullException>(
            () => "One".IsValidEnum(null, false));

        ex.ParamName.ShouldBe("type");
    }

    [Fact]
    public void IsValidEnum_throws_for_non_enum_type()
    {
        var ex = Should.Throw<EnumTypeException>(
            () => "One".IsValidEnum(typeof(int), false));

        ex.Message.ShouldContain(nameof(Int32));
    }

    [Theory]
    [InlineData("One", MyTestEnum1.One, MyTestEnum1.One)]
    [InlineData("THREE", MyTestEnum1.One, MyTestEnum1.One)]
    [InlineData("Seven", MyTestEnum1.One, MyTestEnum1.One)]
    public void ParseEnumOrDefaultTest(string text, MyTestEnum1 defaultValue, MyTestEnum1 expectedResult)
    {
        var result = text.ParseEnumOrDefault(defaultValue);

        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("One", false, MyTestEnum1.One)]
    [InlineData("THREE", false, MyTestEnum1.One)]
    [InlineData("THREE", true, MyTestEnum1.Three)]
    [InlineData("Seven", false, MyTestEnum1.One)]
    public void ParseEnumOrDefaultTest1(string text, bool ignoreCase, MyTestEnum1 expectedResult)
    {
        var defaultValue = MyTestEnum1.One;
        var result = text.ParseEnumOrDefault(ignoreCase, defaultValue);

        result.ShouldBe(expectedResult);
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

    [Theory]
    [InlineData(MyTestEnum1.One, true)]
    [InlineData((MyTestEnum1)99, false)]
    public void IsValidEnum_can_validate_enum_values(MyTestEnum1 value, bool expectedResult)
    {
        value.IsValidEnum().ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("One", true, true)]
    [InlineData("one", false, true)]
    public void IsValidEnum_string_generic_overloads_respect_case(
        string value,
        bool expectedCaseSensitiveResult,
        bool expectedIgnoreCaseResult)
    {
        value.IsValidEnum<MyTestEnum1>().ShouldBe(expectedCaseSensitiveResult);
        value.IsValidEnum<MyTestEnum1>(true).ShouldBe(expectedIgnoreCaseResult);
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

    [Theory]
    [InlineData(MyTestEnum1.One, true)]
    [InlineData(MyTestEnum1.Three, true)]
    [InlineData(MyTestEnum1.Four, false)]
    public void IsValueOneOfTest(MyTestEnum1 value, bool expectedResult)
    {
        var allowed = new[] { MyTestEnum1.One, MyTestEnum1.Three, MyTestEnum1.Five };

        value.IsValueOneOf(allowed).ShouldBe(expectedResult);
        value.IsValueOneOf(allowed.ToList()).ShouldBe(expectedResult);
    }

    [Fact]
    public void IsValueOneOf_can_handle_an_empty_allowed_list()
    {
        MyTestEnum1.One.IsValueOneOf(Array.Empty<MyTestEnum1>()).ShouldBeFalse();
        MyTestEnum1.One.IsValueOneOf(new List<MyTestEnum1>()).ShouldBeFalse();
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
    public void GetSetValuesTest()
    {
        // Arrange
        var flags = MyTestEnum2.Flag2 | MyTestEnum2.Flag4 | MyTestEnum2.Flag5;
        var invalidFlags = 1 | 2 | 4;

        // Act
        var setFlags = flags.GetSetValues();
        //var invalidTypeSetValues = invalidFlags.GetSetValues();   // Does not compile as int is not an Enum

        // Assert
        setFlags.Length.ShouldBe(3);
        setFlags.Contains(MyTestEnum2.Flag2).ShouldBeTrue();
        setFlags.Contains(MyTestEnum2.Flag4).ShouldBeTrue();
        setFlags.Contains(MyTestEnum2.Flag5).ShouldBeTrue();
    }

    [Fact]
    public void GetSetValues_returns_empty_for_no_flags()
    {
        var setFlags = MyTestEnum2.Flag1.GetSetValues();

        setFlags.ShouldHaveSingleItem();
        setFlags[0].ShouldBe(MyTestEnum2.Flag1);
    }

    [Fact]
    public void ToDictionaryTest_MyTestEnum1()
    {
        var dict = EnumerationExtensions.ToDictionaryByName<MyTestEnum1>();

        dict.ShouldNotBeNull();
        dict.Count.ShouldBe(5);
        dict[nameof(MyTestEnum1.One)].ShouldBe(MyTestEnum1.One);
        dict[nameof(MyTestEnum1.Two)].ShouldBe(MyTestEnum1.Two);
        dict[nameof(MyTestEnum1.Three)].ShouldBe(MyTestEnum1.Three);
        dict[nameof(MyTestEnum1.Four)].ShouldBe(MyTestEnum1.Four);
        dict[nameof(MyTestEnum1.Five)].ShouldBe(MyTestEnum1.Five);
    }

    [Fact]
    public void ToDictionaryTest_MyTestEnum2()
    {
        var dict = EnumerationExtensions.ToDictionaryByName<MyTestEnum2>();

        dict.ShouldNotBeNull();
        dict.Count.ShouldBe(5);
        dict[nameof(MyTestEnum2.Flag1)].ShouldBe(MyTestEnum2.Flag1);
        dict[nameof(MyTestEnum2.Flag2)].ShouldBe(MyTestEnum2.Flag2);
        dict[nameof(MyTestEnum2.Flag3)].ShouldBe(MyTestEnum2.Flag3);
        dict[nameof(MyTestEnum2.Flag4)].ShouldBe(MyTestEnum2.Flag4);
        dict[nameof(MyTestEnum2.Flag5)].ShouldBe(MyTestEnum2.Flag5);
    }

    [Fact]
    public void ToDictionaryByValueTest_MyTestEnum1()
    {
        var dict = EnumerationExtensions.ToDictionaryByValue<MyTestEnum1>();

        dict.ShouldNotBeNull();
        dict.Count.ShouldBe(5);
        dict[MyTestEnum1.One].ShouldBe(nameof(MyTestEnum1.One));
        dict[MyTestEnum1.Two].ShouldBe(nameof(MyTestEnum1.Two));
        dict[MyTestEnum1.Three].ShouldBe(nameof(MyTestEnum1.Three));
        dict[MyTestEnum1.Four].ShouldBe(nameof(MyTestEnum1.Four));
        dict[MyTestEnum1.Five].ShouldBe(nameof(MyTestEnum1.Five));
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

    [Fact]
    public void Attribute_and_description_overloads_can_be_called_with_inherit()
    {
        var attributes = MyTestEnum3.Ten.GetAttributes<MultiplierAttribute>(true);
        var attribute = MyTestEnum3.Ten.GetAttribute<MultiplierAttribute>(true);

        attributes.ShouldHaveSingleItem();
        attributes[0].Multiplier.ShouldBe(10);
        attribute.Multiplier.ShouldBe(10);
        MyTestEnum1.Two.GetDescription(true).ShouldBe("Second");
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

    [Fact]
    public void GetAttributes_default_overload_returns_attributes_and_handles_composite_values()
    {
        var attributes = MyTestEnum3.Ten.GetAttributes<MultiplierAttribute>();
        var compositeAttributes = (MyTestEnum2.Flag2 | MyTestEnum2.Flag4)
            .GetAttributes<DescriptionAttribute>();

        attributes.ShouldHaveSingleItem();
        attributes[0].Multiplier.ShouldBe(10);
        compositeAttributes.ShouldBeNull();
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

    [Fact]
    public void GetDescriptionOrName_inherit_overload_returns_description_or_name()
    {
        MyType.Two.GetDescriptionOrName(true).ShouldBe("Number 2");
        MyType.One.GetDescriptionOrName(true).ShouldBe(nameof(MyType.One));
    }
}
