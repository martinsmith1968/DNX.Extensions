using DNX.Extensions.Strings.Interpolation;
using DNX.Extensions.Tests.Strings.Interpolation.Data;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Strings.Interpolation;
public class NamedInstanceTests
{
    [Fact]
    public void ToDictionary_can_convert_a_NamedInstance_successfully()
    {
        var instanceName = "SpitTheDogOwner";
        var person = new Person()
        {
            FirstName = "Bob",
            LastName = "Carolgees",
            DateOfBirth = new DateTime(1940, 05, 01),
            AgeInYears = DateTime.UtcNow.Year - 1940

        };
        var instance = new NamedInstance(person, instanceName);

        // Act
        var result = instance.ToDictionary();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(6);
        result.ContainsKey($"{instanceName}.{nameof(Person.FirstName)}").ShouldBeTrue();
        result.ContainsKey($"{instanceName}.{nameof(Person.LastName)}").ShouldBeTrue();
        result.ContainsKey($"{instanceName}.{nameof(Person.DateOfBirth)}").ShouldBeTrue();
        result.ContainsKey($"{instanceName}.{nameof(Person.AgeInYears)}").ShouldBeTrue();
        result.ContainsKey($"{instanceName}.{nameof(Person.YearOfBirth)}").ShouldBeTrue();
        result.ContainsKey($"{instanceName}.{nameof(Person.Number)}").ShouldBeTrue();
        result[$"{instanceName}.{nameof(Person.FirstName)}"].ShouldBe(person.FirstName);
        result[$"{instanceName}.{nameof(Person.LastName)}"].ShouldBe(person.LastName);
        result[$"{instanceName}.{nameof(Person.DateOfBirth)}"].ShouldBe(person.DateOfBirth);
        result[$"{instanceName}.{nameof(Person.AgeInYears)}"].ShouldBe(person.AgeInYears);
        result[$"{instanceName}.{nameof(Person.YearOfBirth)}"].ShouldBe(person.YearOfBirth);
        result[$"{instanceName}.{nameof(Person.Number)}"].ShouldNotBe(0);
    }

    [Fact]
    public void ToDictionary_can_handle_a_null_NamedInstance_successfully()
    {
        var instance = new NamedInstance(null, "Missing");

        // Act
        var result = instance.ToDictionary();

        // Assert
        result.ShouldBeNull();
    }
}
