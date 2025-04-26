using System.Reflection;
using DNX.Extensions.Assemblies;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Assemblies;

public class AssemblyDetailsTests
{
    [Fact]
    public void AssemblyDetails_created_from_current_assembly()
    {
        // Arrange
        var targetAssembly = Assembly.GetExecutingAssembly();

        // Act
        var assemblyDetails = new AssemblyDetails();

        // Assert
        assemblyDetails.ShouldNotBeNull();
        assemblyDetails.AssemblyName.Name.ShouldBe(targetAssembly.GetName().Name);
        assemblyDetails.Name.ShouldBe(targetAssembly.GetName().Name);
        assemblyDetails.Location.ShouldBe(targetAssembly.Location);
        assemblyDetails.FileName.ShouldNotBeNull();
        assemblyDetails.FileName.ShouldNotBeEmpty();
        assemblyDetails.Title.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyTitleAttribute>().First().Title);
        assemblyDetails.Product.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyProductAttribute>().First().Product);
        assemblyDetails.Copyright.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyCopyrightAttribute>().First().Copyright);
        assemblyDetails.Company.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyCompanyAttribute>().First().Company);
        assemblyDetails.Description.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyDescriptionAttribute>().First().Description);
        assemblyDetails.Trademark.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyTrademarkAttribute>().FirstOrDefault()?.Trademark);
        assemblyDetails.Configuration.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyConfigurationAttribute>().First().Configuration);
        assemblyDetails.Version.ShouldBe(targetAssembly.GetName().Version);
        assemblyDetails.FileVersion.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyFileVersionAttribute>().First().Version);
        assemblyDetails.InformationalVersion.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>().First().InformationalVersion);
        assemblyDetails.SimplifiedVersion.ShouldBe(assemblyDetails.Version.Simplify());
    }

    [Fact]
    public void AssemblyDetails_created_from_specific_assembly()
    {
        // Arrange
        var targetAssembly = typeof(AssemblyDetails).Assembly;

        // Act
        var assemblyDetails = new AssemblyDetails(targetAssembly);

        // Assert
        assemblyDetails.ShouldNotBeNull();
        assemblyDetails.AssemblyName.Name.ShouldBe(targetAssembly.GetName().Name);
        assemblyDetails.Name.ShouldBe(targetAssembly.GetName().Name);
        assemblyDetails.Location.ShouldBe(targetAssembly.Location);
        assemblyDetails.FileName.ShouldNotBeNull();
        assemblyDetails.FileName.ShouldNotBeEmpty();
        assemblyDetails.Title.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyTitleAttribute>().First().Title);
        assemblyDetails.Product.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyProductAttribute>().First().Product);
        assemblyDetails.Copyright.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyCopyrightAttribute>().First().Copyright);
        assemblyDetails.Company.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyCompanyAttribute>().First().Company);
        assemblyDetails.Description.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyDescriptionAttribute>().First().Description);
        assemblyDetails.Trademark.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyTrademarkAttribute>().FirstOrDefault()?.Trademark);
        assemblyDetails.Configuration.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyConfigurationAttribute>().First().Configuration);
        assemblyDetails.Version.ShouldBe(targetAssembly.GetName().Version);
        assemblyDetails.FileVersion.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyFileVersionAttribute>().First().Version);
        assemblyDetails.InformationalVersion.ShouldBe(targetAssembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>().First().InformationalVersion);
        assemblyDetails.SimplifiedVersion.ShouldBe(assemblyDetails.Version.Simplify());
    }

    [Fact]
    public void AssemblyDetails_ForAssembly_accesses_the_appropriate_assembly()
    {
        var type = typeof(AssemblyDetails);

        // Act
        var result = AssemblyDetails.ForAssembly(type.Assembly);

        // Assert
        result.ShouldNotBeNull();
        result.AssemblyName.Name.ShouldBe(type.Assembly.GetName().Name);
    }

    [Fact]
    public void Assembly_GetAssemblyDetails_accesses_the_appropriate_assembly()
    {
        var type = typeof(AssemblyDetails);

        // Act
        var result = type.Assembly.GetAssemblyDetails();

        // Assert
        result.ShouldNotBeNull();
        result.AssemblyName.Name.ShouldBe(type.Assembly.GetName().Name);
    }

    [Fact]
    public void AssemblyDetails_ForMe_accesses_the_appropriate_assembly()
    {
        var type = GetType();

        // Act
        var result = AssemblyDetails.ForMe();

        // Assert
        result.ShouldNotBeNull();
        result.AssemblyName.Name.ShouldBe(type.Assembly.GetName().Name);
    }

    [Fact]
    public void AssemblyDetails_ForEntryPoint_accesses_the_appropriate_assembly()
    {
        var type = GetType();

        // Act
        var result = AssemblyDetails.ForEntryPoint();

        // Assert
        result.ShouldNotBeNull();
        result.AssemblyName.Name.ShouldNotBe(type.Assembly.GetName().Name);
    }

    [Fact]
    public void AssemblyDetails_ForAssemblyContainingType_accesses_the_appropriate_assembly()
    {
        var type = typeof(AssemblyDetails);

        // Act
        var result = AssemblyDetails.ForAssemblyContaining(type);

        // Assert
        result.ShouldNotBeNull();
        result.AssemblyName.Name.ShouldBe(type.Assembly.GetName().Name);
    }

    [Fact]
    public void AssemblyDetails_ForAssemblyContainingType_T_accesses_the_appropriate_assembly()
    {
        var type = typeof(AssemblyDetails);

        // Act
        var result = AssemblyDetails.ForAssemblyContaining<AssemblyDetails>();

        // Assert
        result.ShouldNotBeNull();
        result.AssemblyName.Name.ShouldBe(type.Assembly.GetName().Name);
    }

    [Fact]
    public void AssemblyDetails_ForAssemblyContaining_methods_work_consistently()
    {
        var type = typeof(AssemblyDetails);

        // Act
        var result1 = AssemblyDetails.ForAssemblyContaining(type);
        var result2 = AssemblyDetails.ForAssemblyContaining<AssemblyDetails>();

        // Assert
        result1.Location.ShouldBe(result2.Location);
    }
}
