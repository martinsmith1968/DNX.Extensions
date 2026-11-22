using DNX.Extensions.IO;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.IO;

public class PathExtensionsTests
{
    [Fact]
    public void CombineWith_combines_the_path_with_the_additional_segments()
    {
        var result = "base".CombineWith("sub", "file.txt");

        result.ShouldBe(Path.Combine("base", "sub", "file.txt"));
    }

    [Fact]
    public void CombineWith_returns_the_original_path_when_no_additional_segments_are_supplied()
    {
        var result = "base".CombineWith();

        result.ShouldBe("base");
    }

    [Fact]
    public void CombineWith_treats_a_null_segments_array_the_same_as_none()
    {
        var result = "base".CombineWith(null);

        result.ShouldBe("base");
    }

    [Fact]
    public void CombineWith_throws_for_a_null_path()
    {
        string path = null;

        Should.Throw<ArgumentNullException>(() => path.CombineWith("sub"));
    }
}
