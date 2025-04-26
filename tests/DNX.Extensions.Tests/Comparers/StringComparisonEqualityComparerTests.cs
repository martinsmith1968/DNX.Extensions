using DNX.Extensions.Comparers;
using Shouldly;
using Xunit;

namespace DNX.Extensions.Tests.Comparers;

public class StringComparisonEqualityComparerTests
{
    private StringComparison _comparisonMethod = StringComparison.CurrentCultureIgnoreCase;
    private StringComparisonEqualityComparer Sut => new(_comparisonMethod);

    [Fact]
    public void DefaultConstructor_works_as_expected()
    {
        var sut = new StringComparisonEqualityComparer();

        sut.StringComparisonMethod.ShouldBe(StringComparison.CurrentCulture);
    }

    [Theory]
    [MemberData(nameof(StringComparisonValues_Data))]
    public void Constructor_for_StringComparison_works_as_expected(StringComparison stringComparison)
    {
        var sut = new StringComparisonEqualityComparer(stringComparison);

        sut.StringComparisonMethod.ShouldBe(stringComparison);
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
        result.ShouldBe(expectedResult);
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
        (resultX == resultY).ShouldBe(expectedResult);
    }

    #region TestData

    public static TheoryData<StringComparison> StringComparisonValues_Data()
    {
        return new TheoryData<StringComparison>(
            Enum.GetValues(typeof(StringComparison))
                .Cast<StringComparison>()
        );
    }

    public static TheoryData<string, string, StringComparison, bool> Equals_Data()
    {
        return new TheoryData<string, string, StringComparison, bool>
        {
            { null, null, StringComparison.CurrentCulture, true },
            { "", "", StringComparison.CurrentCulture, true },
            { "ClearBank", "", StringComparison.CurrentCulture, false },
            { "ClearBank", "", StringComparison.CurrentCulture, false },
            { "", "ClearBank", StringComparison.CurrentCulture, false },
            { "ClearBank", null, StringComparison.CurrentCulture, false },
            { null, "ClearBank", StringComparison.CurrentCulture, false },

            { "Clear", "Bank", StringComparison.CurrentCulture, false },
            { "Clear", "Bank", StringComparison.CurrentCultureIgnoreCase, false },
            { "Clear", "Bank", StringComparison.InvariantCulture, false },
            { "Clear", "Bank", StringComparison.InvariantCultureIgnoreCase, false },
            { "Clear", "Bank", StringComparison.Ordinal, false },
            { "Clear", "Bank", StringComparison.OrdinalIgnoreCase, false },

            { "ClearBank", "ClearBank", StringComparison.CurrentCulture, true },
            { "ClearBank", "ClearBank", StringComparison.CurrentCultureIgnoreCase, true },
            { "ClearBank", "ClearBank", StringComparison.InvariantCulture, true },
            { "ClearBank", "ClearBank", StringComparison.InvariantCultureIgnoreCase, true },
            { "ClearBank", "ClearBank", StringComparison.Ordinal, true },
            { "ClearBank", "ClearBank", StringComparison.OrdinalIgnoreCase, true },

            { "ClearBank", "CLEARBANK", StringComparison.CurrentCulture, false },
            { "ClearBank", "CLEARBANK", StringComparison.CurrentCultureIgnoreCase, true },
            { "ClearBank", "CLEARBANK", StringComparison.InvariantCulture, false },
            { "ClearBank", "CLEARBANK", StringComparison.InvariantCultureIgnoreCase, true },
            { "ClearBank", "CLEARBANK", StringComparison.Ordinal, false },
            { "ClearBank", "CLEARBANK", StringComparison.OrdinalIgnoreCase, true },
        };
    }

    #endregion
}