namespace DNX.Extensions.Tests.Maths;

[TestFixture]
public class IsBetweenTypeExtensionsTests
{
    [InlineData(IsBetweenBoundsType.Inclusive, "between {0} and {1}")]
    [InlineData(IsBetweenBoundsType.Exclusive, "between but not including {0} and {1}")]
    [InlineData(IsBetweenBoundsType.GreaterThanLowerLessThanOrEqualToUpper, "greater than {0} but less than or equal to {1}")]
    [InlineData(IsBetweenBoundsType.GreaterThanOrEqualToLowerLessThanUpper, "greater than or equal to {0} but less than {1}")]
    [InlineData(IsBetweenBoundsType.IncludeLowerAndUpper, "between {0} and {1}")]
    [InlineData(IsBetweenBoundsType.ExcludeLowerAndUpper, "between but not including {0} and {1}")]
    [InlineData(IsBetweenBoundsType.ExcludeLowerIncludeUpper, "greater than {0} but less than or equal to {1}")]
    [InlineData(IsBetweenBoundsType.IncludeLowerExcludeUpper, "greater than or equal to {0} but less than {1}")]
    [InlineData((IsBetweenBoundsType)999, null)]
    public string GetLimitDescriptionFormatTest(IsBetweenBoundsType boundsType)
    {
        return boundsType.GetLimitDescriptionFormat();
    }
}
