using System.Reflection;
using System.Resources;
using DNX.Extensions.Assemblies;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace DNX.Extensions.Tests.Assemblies;

public class AssemblyExtensionsTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void GetEmbeddedResourceText_can_read_resource_successfully()
    {
        // Arrange
        var name = "TestData.SampleData.json";

        // Act
        var result = Assembly.GetExecutingAssembly().GetEmbeddedResourceText(name);

        testOutputHelper.WriteLine("Result: {0}", result);

        // Assert
        result.ShouldNotBeNull();
    }

    [Fact]
    public void GetEmbeddedResourceText_throws_on_unknown_resource_name()
    {
        // Arrange
        var name = $"{Guid.NewGuid()}.json";

        // Act
        var ex = Assert.Throws<MissingManifestResourceException>(
            () => Assembly.GetExecutingAssembly().GetEmbeddedResourceText(name)
        );

        testOutputHelper.WriteLine("Exception Message: {0}", ex?.Message);

        // Assert
        ex.ShouldNotBeNull();
        ex.Message.ShouldContain(name);
    }

    [Fact]
    public void GetEmbeddedResourceText_can_read_resource_with_specific_namespace_successfully()
    {
        // Arrange
        var name = "SampleData.json";
        var nameSpace = $"DNX.Extensions.Tests.TestData";

        // Act
        var result = Assembly.GetExecutingAssembly().GetEmbeddedResourceText(name, nameSpace);

        testOutputHelper.WriteLine("Result: {0}", result);

        // Assert
        result.ShouldNotBeNull();
    }
}
