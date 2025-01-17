using System.Text;
using Bogus;

namespace DNX.Extensions.Tests.Streams;

[TestFixture]
public class StreamExtensionsTests
{
    private Faker _faker;

    [SetUp]
    public void TestSetup()
    {
        _faker = new Faker();
    }

    [Fact]
    public void ReadAllText_should_read_text_successfully()
    {
        // Arrange
        var text = _faker.Lorem.Sentences(_faker.Random.Int(5, 10));
        var bytes = Encoding.UTF8.GetBytes(text);

        var stream = new MemoryStream(bytes);

        // Act
        var result = stream.ReadAllText();
        stream.Dispose();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.ShouldBe(text);
    }

    [Fact]
    public void ReadAllText_handles_null_streams_appropriately()
    {
        // Arrange
        var stream = (MemoryStream)null;

        // Act
        var result = stream.ReadAllText();

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void ReadAllLines_should_read_lines_successfully()
    {
        // Arrange
        var textLines = Enumerable.Range(5, 10)
            .Select(f => _faker.Lorem.Slug(_faker.Random.Int(5, 10)))
            .ToList();
        var bytes = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, textLines));

        var stream = new MemoryStream(bytes);

        // Act
        var result = stream.ReadAllLines();
        stream.Dispose();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBeGreaterThan(0);
        result.Count.ShouldBe(textLines.Count);
        result.ShouldBe(textLines);
    }

    [Fact]
    public void ReadAllLines_handles_null_streams_appropriately()
    {
        // Arrange
        var stream = (MemoryStream)null;

        // Act
        var result = stream.ReadAllLines();

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void ReadAllBytes_should_read_bytes_successfully()
    {
        // Arrange
        var text = _faker.Lorem.Sentences(_faker.Random.Int(5, 10));
        var bytes = Encoding.UTF8.GetBytes(text);

        var stream = new MemoryStream(bytes);

        // Act
        var result = stream.ReadAllBytes();
        stream.Dispose();

        // Assert
        result.ShouldNotBeNull();
        result.Length.ShouldBeGreaterThan(0);
        result.ShouldBe(bytes);
    }

    [Fact]
    public void ReadAllBytes_handles_null_streams_appropriately()
    {
        // Arrange
        var stream = (MemoryStream)null;

        // Act
        var result = stream.ReadAllBytes();

        // Assert
        result.ShouldBeNull();
    }
}
