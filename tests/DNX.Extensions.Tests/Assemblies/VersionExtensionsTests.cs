using DNX.Extensions.Assemblies;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Assemblies
{
    public class VersionExtensionsTests
    {
        [Theory]
        [InlineData(1, 0, 0, 0, 1, "1")]
        [InlineData(1, 2, 0, 0, 1, "1.2")]
        [InlineData(1, 0, 3, 0, 1, "1.0.3")]
        [InlineData(1, 0, 0, 4, 4, "1.0.0.4")]
        [InlineData(1, 2, 3, 4, 1, "1.2.3.4")]
        [InlineData(1, 2, 3, 4, 2, "1.2.3.4")]
        [InlineData(1, 2, 3, 4, 3, "1.2.3.4")]
        [InlineData(1, 2, 3, 4, 4, "1.2.3.4")]
        public void SimplifyTests(int major, int minor, int build, int revision, int minimumPositions, string expectedResult)
        {
            // Act
            var version = new Version(major, minor, build, revision);

            // Assert
            version.Simplify(minimumPositions).ShouldBe(expectedResult);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, 0, false)]
        [InlineData(1, 2, 3, 4, 5, false)]
        [InlineData(1, 2, 3, 4, 1, true)]
        [InlineData(1, 2, 3, 4, 4, true)]
        public void Simplify_GuardException_Tests(int major, int minor, int build, int revision, int minimumPositions, bool expectedResult)
        {
            // TODO: Refactor to separate tests


            // Act
            var version = new Version(major, minor, build, revision);

            // Act
            try
            {
                var text = version.Simplify(minimumPositions);

                // Assert
                expectedResult.ShouldBeTrue();
            }
            catch (ArgumentOutOfRangeException)
            {
                // Assert
                expectedResult.ShouldBeFalse();
            }
        }

        [Fact]
        public void Simplify_for_null_returns_expected()
        {
            // Arrange
            Version version = null;

            // Act
            var result = version.Simplify();

            // Assert
            result.ShouldBeNull();
        }
    }
}
