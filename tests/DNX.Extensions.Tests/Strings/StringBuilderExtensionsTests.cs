using System.Text;
using DNX.Extensions.Strings;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Strings;
public class StringBuilderExtensionsTests
{
    private StringBuilder Sut { get; set; } = new();

    [Fact]
    public void AppendIfNotPresent_does_not_append_to_null_instance()
    {
        const string suffix = "Bob";
        Sut = null;

        // Act
        var result = Sut.AppendIfNotPresent(suffix);

        // Assert
        Sut.ShouldBeNull();
        result.ShouldBe(Sut);
    }

    [Fact]
    public void AppendIfNotPresent_appends_to_empty_instance_without_suffix()
    {
        const string suffix = "Bob";

        // Act
        var result = Sut.AppendIfNotPresent(suffix);

        // Assert
        Sut.ToString().ShouldBe(suffix);
        result.ShouldNotBeNull();
        result.ShouldBe(Sut);
    }

    [Fact]
    public void AppendIfNotPresent_appends_to_existing_instance_without_suffix()
    {
        const string initial = "Hello";
        const string suffix = "Bob";
        Sut.Append(initial);

        // Act
        var result = Sut.AppendIfNotPresent(suffix);

        // Assert
        Sut.ToString().ShouldBe(initial + suffix);
        result.ShouldNotBeNull();
        result.ShouldBe(Sut);
    }

    [Fact]
    public void AppendIfNotPresent_does_not_append_to_existing_instance_with_suffix()
    {
        const string initial = "HelloBob";
        const string suffix = "Bob";
        Sut.Append(initial);

        // Act
        var result = Sut.AppendIfNotPresent(suffix);

        // Assert
        Sut.ToString().ShouldBe(initial);
        result.ShouldNotBeNull();
        result.ShouldBe(Sut);
    }

    [Fact]
    public void AppendSeparator_does_not_append_to_null_instance()
    {
        Sut = null;

        // Act
        var result = Sut.AppendSeparator();

        // Assert
        Sut.ShouldBeNull();
        result.ShouldBe(Sut);
    }

    [Fact]
    public void AppendSeparator_does_not_append_default_separator_to_empty_instance()
    {
        // Act
        var result = Sut.AppendSeparator();

        // Assert
        Sut.ToString().ShouldBe("");
        result.ShouldNotBeNull();
        result.ShouldBe(Sut);
    }

    [Fact]
    public void AppendSeparator_appends_default_separator_to_existing_instance()
    {
        const string initial = "Hello";
        Sut.Append(initial);

        // Act
        var result = Sut.AppendSeparator();

        // Assert
        Sut.ToString().ShouldBe(initial + " ");
        result.ShouldNotBeNull();
        result.ShouldBe(Sut);
    }

    [Fact]
    public void AppendSeparator_does_not_append_default_separator_to_existing_instance_with_suffix()
    {
        const string initial = "Hello ";
        Sut.Append(initial);

        // Act
        var result = Sut.AppendSeparator();

        // Assert
        Sut.ToString().ShouldBe(initial);
        result.ShouldNotBeNull();
        result.ShouldBe(Sut);
    }

    [Fact]
    public void AppendSeparator_does_not_append_custom_separator_to_empty_instance()
    {
        // Act
        var separator = ",";
        var result = Sut.AppendSeparator(separator);

        // Assert
        Sut.ToString().ShouldBe("");
        result.ShouldNotBeNull();
        result.ShouldBe(Sut);
    }

    [Fact]
    public void AppendSeparator_appends_custom_separator_to_existing_instance()
    {
        const string initial = "Hello";
        var separator = ",";
        Sut.Append(initial);

        // Act
        var result = Sut.AppendSeparator(separator);

        // Assert
        Sut.ToString().ShouldBe(initial + separator);
        result.ShouldNotBeNull();
        result.ShouldBe(Sut);
    }

    [Fact]
    public void AppendSeparator_does_not_append_custom_separator_to_existing_instance_with_suffix()
    {
        const string initial = "Hello,";
        var separator = ",";
        Sut.Append(initial);

        // Act
        var result = Sut.AppendSeparator(separator);

        // Assert
        Sut.ToString().ShouldBe(initial);
        result.ShouldNotBeNull();
        result.ShouldBe(Sut);
    }
}
