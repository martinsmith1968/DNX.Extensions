using System.Reflection;
using DNX.Extensions.Reflection;
using Shouldly;
using Xunit;

#pragma warning disable CA1822 // Members can be static
#pragma warning disable IDE0051 // Unused private member

// ReSharper disable UnusedMember.Local

namespace DNX.Extensions.Tests.Reflection;

public class TestClass
{
    internal string MachineName => Environment.MachineName;
    internal string UserName => Environment.UserName;
    private string UserDomainName => Environment.UserDomainName;

    public string PublicSetOnly { private get; set; }

    private static string CurrentDirectory => Environment.CurrentDirectory;
}

public class ReflectionExtensionsTests
{
    public static TheoryData<string, string> GetPrivatePropertyValue_Private_Data()
    {
        return new TheoryData<string, string>
        {
            { "UserDomainName", Environment.UserDomainName },
        };
    }

    public static TheoryData<string, string> GetPrivatePropertyValue_Public_Data()
    {
        return new TheoryData<string, string>
        {
            { nameof(TestClass.MachineName), Environment.MachineName },
            { nameof(TestClass.UserName), Environment.UserName },
        };
    }

    [Theory]
    [MemberData(nameof(GetPrivatePropertyValue_Private_Data))]
    public void GetPrivatePropertyValue_can_read_private_values_successfully(string propertyName, string expected)
    {
        // Arrange
        var instance = new TestClass();

        // Act
        var result = instance.GetPrivatePropertyValue(propertyName);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(GetPrivatePropertyValue_Public_Data))]
    public void GetPrivatePropertyValue_can_read_non_private_values_successfully(string propertyName, string expected)
    {
        // Arrange
        var instance = new TestClass();

        // Act
        var result = instance.GetPrivatePropertyValue(propertyName);

        // Assert
        result.ShouldBe(expected);
    }

    [Fact]
    public void GetPropertyValueByName_can_read_private_static_values_successfully()
    {
        // Arrange
        var instance = new TestClass();

        // Act
        var result = instance.GetPropertyValueByName("CurrentDirectory", BindingFlags.Static | BindingFlags.NonPublic);

        // Assert
        result.ShouldBe(Environment.CurrentDirectory);
    }

    [Fact]
    public void GetPropertyValueByName_for_unknown_property_name_returns_null()
    {
        // Arrange
        var instance = new TestClass();

        // Act
        var result = instance.GetPropertyValueByName(Guid.NewGuid().ToString(), BindingFlags.Static | BindingFlags.NonPublic);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetPropertyValueByName_for_property_name_without_getter_returns_null()
    {
        // Arrange
        var instance = new TestClass();

        // Act
        var result = instance.GetPropertyValueByName(nameof(TestClass.PublicSetOnly), BindingFlags.Instance | BindingFlags.Public);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetPrivatePropertyValue_for_unknown_property_name_returns_null()
    {
        // Arrange
        var instance = new TestClass();

        // Act
        var result = instance.GetPrivatePropertyValue(Guid.NewGuid().ToString());

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetPrivatePropertyValue_for_unknown_property_name_returns_default_value()
    {
        // Arrange
        var instance = new TestClass();
        var defaultValue = Guid.NewGuid().ToString();

        // Act
        var result = instance.GetPrivatePropertyValue(Guid.NewGuid().ToString(), defaultValue);

        // Assert
        result.ShouldBe(defaultValue);
    }
}
