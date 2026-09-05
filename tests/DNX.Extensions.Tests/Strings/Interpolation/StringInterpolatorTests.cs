using DNX.Extensions.Enumerations;
using DNX.Extensions.Strings.Interpolation;
using DNX.Extensions.Tests.Strings.Interpolation.Data;
using Shouldly;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace DNX.Extensions.Tests.Strings.Interpolation;

public class StringInterpolatorTests
{
    [Theory]
    [InlineData("Hello {FirstName}", "Martin", "Smith", "2017-08-11", null, "Hello Martin")]
    [InlineData("{FirstName} {LastName} {DateOfBirth:MMM-dd} {YearOfBirth}", "Martin", "Smith", "2017-08-11", null, "Martin Smith Aug-11 2017")]
    [InlineData("Hello {user.FirstName}", "Martin", "Smith", "2017-08-11", "user", "Hello Martin")]
    [InlineData("Hello {person.FirstName} {person.LastName}", "Martin", "Smith", "2017-08-11", "person", "Hello Martin Smith")]
    [InlineData("{FirstName} {LastName} <> {LastName} {FirstName}", "Martin", "Smith", "2017-08-11", null, "Martin Smith <> Smith Martin")]
    public void InterpolateWith_SinglePerson_Test(string format, string firstName, string lastName, string dateOfBirth, string instanceName, string expectedResult)
    {
        var person = new Person()
        {
            FirstName   = firstName,
            LastName    = lastName,
            DateOfBirth = DateTime.Parse(dateOfBirth)
        };

        // Act
        var result = format.InterpolateWith(person, instanceName);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("{FirstName} was born in {DateOfBirth:MMMM yyyy} and is {AgeInYears:0000}", "Martin", "Smith", "1968-08-11", 29, null, "Martin was born in August 1968 and is 0029")]
    public void InterpolateWith_SinglePerson_WithFormatModifiers_Test(string format, string firstName, string lastName, string dateOfBirth, int? fakeAge, string instanceName, string expectedResult)
    {
        var person = new Person()
        {
            FirstName   = firstName,
            LastName    = lastName,
            DateOfBirth = DateTime.Parse(dateOfBirth),
        };

        if (fakeAge.HasValue)
            person.AgeInYears = fakeAge.Value;

        // Act
        var result = format.InterpolateWith(person, instanceName);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Fact]
    public void InterpolateWithAll_Environment_SpecialFolders()
    {
        // Arrange
        var text = "{SpecialFolder.System}\\Shell32.dll";

        var dict = new Dictionary<string, object>()
        {
            { $"{nameof(Environment.SpecialFolder)}.{nameof(Environment.SpecialFolder.System)}", Environment.GetFolderPath(Environment.SpecialFolder.System) }
        };

        // Act
        var result = text.InterpolateWithAll(dict);

        // Assert
        result.ShouldBe(string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.System), "\\Shell32.dll"));
    }

    [Fact]
    public void InterpolateWithAll_Environment_Folders()
    {
        // Arrange
        var folderLocations = new Dictionary<string, string>()
        {
            { nameof(Environment.SystemDirectory), Environment.SystemDirectory },
            { nameof(Environment.CurrentDirectory), Environment.CurrentDirectory },
            { $"{nameof(Environment.SpecialFolder)}.{nameof(Environment.SpecialFolder.Windows)}", Environment.GetFolderPath(Environment.SpecialFolder.Windows) },
            { $"{nameof(Environment.SpecialFolder)}.{nameof(Environment.SpecialFolder.Personal)}", Environment.GetFolderPath(Environment.SpecialFolder.Personal) }
        };

        var interpolationInstances = new[]
        {
            new NamedInstance(folderLocations, nameof(Environment)),
        };

        var text = "{Environment.SystemDirectory}\\Shell32.dll";

        // Act
        var result = text.InterpolateWithAll(interpolationInstances);

        // Assert
        result.ShouldBe(string.Concat(Environment.SystemDirectory, "\\Shell32.dll"));
    }

    [Theory]
    [InlineData("{One}{Two}{Three}{Four}{Five}", "12345")]
    public void InterpolateWithAll_Dictionary(string text, string expectedValue)
    {
        // Arrange
        var dict = Enum.GetNames(typeof(OneToTen))
                .ToDictionary(x => x, x => (int) Enum.Parse(typeof(OneToTen), x))
            ;

        // Act
        var result = text.InterpolateWithAll(dict);

        // Assert
        result.ShouldBe(expectedValue);
    }

    [Theory]
    [InlineData("{person1.FirstName} {person1.LastName} and {person2.FirstName} {person2.LastName}", "Tommy", "Cannon", "1935-10-01", "person1", "Bobby", "Ball", "1936-07-04", "person2", "Tommy Cannon and Bobby Ball")]
    [InlineData("{artist.FirstName} {artist.LastName} and {puppet.FirstName} {puppet.LastName}", "Bob", "Carolgees", "1935-10-01", "artist", "Spit", "the Dog", "1900-01-01", "puppet", "Bob Carolgees and Spit the Dog")]
    public void InterpolateWithAll_MultiplePersons_Test(string format, string firstName1, string lastName1, string dateOfBirth1, string instanceName1, string firstName2, string lastName2, string dateOfBirth2, string instanceName2, string expectedResult)
    {
        var person1 = new Person()
        {
            FirstName   = firstName1,
            LastName    = lastName1,
            DateOfBirth = DateTime.Parse(dateOfBirth1)
        };
        var person2 = new Person()
        {
            FirstName   = firstName2,
            LastName    = lastName2,
            DateOfBirth = DateTime.Parse(dateOfBirth2)
        };

        var instanceList = new List<NamedInstance>()
        {
            new NamedInstance(person1, instanceName1),
            new NamedInstance(person2, instanceName2),
        };

        // Act
        var result = format.InterpolateWithAll(instanceList);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("{player.FirstName} {player.LastName} plays for {club.Name}", "Harry", "Kane", "1990-05-06", "player", "Spurs", "club", "Harry Kane plays for Spurs")]
    public void InterpolateWithAll_PersonAndClub_Test(string format, string firstName, string lastName, string dateOfBirth, string instanceName1, string clubName, string instanceName2, string expectedResult)
    {
        var person = new Person()
        {
            FirstName   = firstName,
            LastName    = lastName,
            DateOfBirth = DateTime.Parse(dateOfBirth)
        };
        var club = new Club()
        {
            Name = clubName
        };

        var instanceList = new List<NamedInstance>()
        {
            new NamedInstance(person, instanceName1),
            new NamedInstance(club, instanceName2),
        };

        // Act
        var result = format.InterpolateWithAll(instanceList);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Fact]
    public void InterpolateWithAll_FileName_Test()
    {
        // Arrange
        const string myFileName = "myFileName.txt";
        var specialFolder = Environment.SpecialFolder.CommonProgramFiles;
        var fileName = $"{{Environment.{specialFolder}}}\\{myFileName}";

        var values = EnumerationExtensions.ToDictionaryByName<Environment.SpecialFolder>()
            .ToDictionary(x => x.Key, x => Environment.GetFolderPath(x.Value));
        values.Add(nameof(Environment.SystemDirectory), Environment.SystemDirectory);
        values.Add(nameof(Environment.CurrentDirectory), Environment.CurrentDirectory);

        var namedInstance = new NamedInstance(values, nameof(Environment));

        // Act
        var result = fileName.InterpolateWith(namedInstance);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(string.Concat(Environment.GetFolderPath(specialFolder), "\\", myFileName));
    }

    [Fact]
    public void InterpolateWithAll_null_dictionary()
    {
        // Arrange
        const string myFileName = "myFileName.txt";
        var specialFolder = Environment.SpecialFolder.CommonProgramFiles;
        var fileName = $"{{Environment.{specialFolder}}}\\{myFileName}";

        IDictionary<string, object> dict = null;

        // Act
        var result = fileName.InterpolateWithAll(dict);

        // Assert
        result.ShouldBe(fileName);
    }

    [Fact]
    public void InterpolateWithAll_empty_dictionary()
    {
        // Arrange
        const string myFileName = "myFileName.txt";
        var specialFolder = Environment.SpecialFolder.CommonProgramFiles;
        var fileName = $"{{Environment.{specialFolder}}}\\{myFileName}";

        var dict = new Dictionary<string, object>();

        // Act
        var result = fileName.InterpolateWithAll(dict);

        // Assert
        result.ShouldBe(fileName);
    }
}
