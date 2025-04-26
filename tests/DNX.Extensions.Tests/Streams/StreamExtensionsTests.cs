using System.Text;
using Bogus;
using DNX.Extensions.Streams;
using Shouldly;
using Xunit;

// ReSharper disable ExpressionIsAlwaysNull

namespace DNX.Extensions.Tests.Streams;

public class StreamExtensionsTests
{
    private static readonly Faker Faker = new();

    [Fact]
    public void ReadAllText_should_read_text_successfully()
    {
        // Arrange
        var text = Faker.Lorem.Sentences(Faker.Random.Int(5, 10));
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
        MemoryStream stream = null;

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
            .Select(_ => Faker.Lorem.Slug(Faker.Random.Int(5, 10)))
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
        var text = Faker.Lorem.Sentences(Faker.Random.Int(5, 10));
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
