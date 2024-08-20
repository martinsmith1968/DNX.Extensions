using DNX.Extensions.Comparers;
using FluentAssertions;
using Xunit;

namespace DNX.Extensions.Tests.Comparers
{
    public class StringComparisonEqualityComparerTests
    {
        private StringComparison _comparisonMethod = StringComparison.CurrentCultureIgnoreCase;
        private StringComparisonEqualityComparer Sut => new(_comparisonMethod);

        [Fact]
        public void DefaultConstructor_works_as_expected()
        {
            var sut = new StringComparisonEqualityComparer();

            sut.StringComparisonMethod.Should().Be(StringComparison.CurrentCulture);
        }

        [Theory]
        [MemberData(nameof(StringComparisonValues_Data))]
        public void Constructor_for_StringComparison_works_as_expected(StringComparison stringComparison)
        {
            var sut = new StringComparisonEqualityComparer(stringComparison);

            sut.StringComparisonMethod.Should().Be(stringComparison);
        }

        [Theory]
        [MemberData(nameof(Equals_Data))]
        public void Equals_compares_as_expected(string x, string y, StringComparison stringComparison, bool expectedResult)
        {
            // Arrange
            _comparisonMethod = stringComparison;

            // Act
            var result = Sut.Equals(x, y);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [MemberData(nameof(Equals_Data))]
        public void GetHashCode_compares_as_expected(string x, string y, StringComparison stringComparison, bool expectedResult)
        {
            // Arrange
            _comparisonMethod = stringComparison;

            // Act
            var resultX = Sut.GetHashCode(x);
            var resultY = Sut.GetHashCode(y);

            // Assert
            (resultX == resultY).Should().Be(expectedResult);
        }

        #region TestData

        public static IEnumerable<object[]> StringComparisonValues_Data()
        {
            return Enum.GetValues(typeof(StringComparison))
                    .Cast<StringComparison>()
                    .Select(x => new object[] { x });
        }

        public static IEnumerable<object[]> Equals_Data()
        {
            return new List<object[]>()
            {
                new object[] { null, null, StringComparison.CurrentCulture, true },
                new object[] { "", "", StringComparison.CurrentCulture, true },
                new object[] { "ClearBank", "", StringComparison.CurrentCulture, false },
                new object[] { "ClearBank", "", StringComparison.CurrentCulture, false },
                new object[] { "", "ClearBank", StringComparison.CurrentCulture, false },
                new object[] { "ClearBank", null, StringComparison.CurrentCulture, false },
                new object[] { null, "ClearBank", StringComparison.CurrentCulture, false },

                new object[] { "Clear", "Bank", StringComparison.CurrentCulture, false },
                new object[] { "Clear", "Bank", StringComparison.CurrentCultureIgnoreCase, false },
                new object[] { "Clear", "Bank", StringComparison.InvariantCulture, false },
                new object[] { "Clear", "Bank", StringComparison.InvariantCultureIgnoreCase, false },
                new object[] { "Clear", "Bank", StringComparison.Ordinal, false },
                new object[] { "Clear", "Bank", StringComparison.OrdinalIgnoreCase, false },

                new object[] { "ClearBank", "ClearBank", StringComparison.CurrentCulture, true },
                new object[] { "ClearBank", "ClearBank", StringComparison.CurrentCultureIgnoreCase, true },
                new object[] { "ClearBank", "ClearBank", StringComparison.InvariantCulture, true },
                new object[] { "ClearBank", "ClearBank", StringComparison.InvariantCultureIgnoreCase, true },
                new object[] { "ClearBank", "ClearBank", StringComparison.Ordinal, true },
                new object[] { "ClearBank", "ClearBank", StringComparison.OrdinalIgnoreCase, true },

                new object[] { "ClearBank", "CLEARBANK", StringComparison.CurrentCulture, false },
                new object[] { "ClearBank", "CLEARBANK", StringComparison.CurrentCultureIgnoreCase, true },
                new object[] { "ClearBank", "CLEARBANK", StringComparison.InvariantCulture, false },
                new object[] { "ClearBank", "CLEARBANK", StringComparison.InvariantCultureIgnoreCase, true },
                new object[] { "ClearBank", "CLEARBANK", StringComparison.Ordinal, false },
                new object[] { "ClearBank", "CLEARBANK", StringComparison.OrdinalIgnoreCase, true },
            };
        }

        #endregion
    }
}
