namespace DNX.Extensions.Maths
{
    /// <summary>
    /// Enum IsBetweenBoundsType
    /// </summary>
    public enum IsBetweenBoundsType
    {
        /// <summary>
        /// Include Lower and Upper bounds when determining IsBetween
        /// </summary>
        IncludeLowerAndUpper = 1,
        /// <summary>
        /// Include Lower and Upper bounds when determining IsBetween
        /// </summary>
        Inclusive = IncludeLowerAndUpper,

        /// <summary>
        /// Exclude Lower and Upper bounds when determining IsBetween
        /// </summary>
        ExcludeLowerAndUpper = 2,
        /// <summary>
        /// Exclude Lower and Upper bounds when determining IsBetween
        /// </summary>
        Exclusive = ExcludeLowerAndUpper,

        /// <summary>
        /// Include Lower but Exclude Upper bounds when determining IsBetween
        /// </summary>
        IncludeLowerExcludeUpper = 3,
        /// <summary>
        /// Include Lower but Exclude Upper bounds when determining IsBetween
        /// </summary>
        GreaterThanOrEqualToLowerLessThanUpper = IncludeLowerExcludeUpper,

        /// <summary>
        /// Exclude Lower but Include Upper bounds when determining IsBetween
        /// </summary>
        ExcludeLowerIncludeUpper = 4,
        /// <summary>
        /// Exclude Lower but Include Upper bounds when determining IsBetween
        /// </summary>
        GreaterThanLowerLessThanOrEqualToUpper = ExcludeLowerIncludeUpper
    }

    /// <summary>
    /// Class IsBetweenTypeExtensions.
    /// </summary>
    public static class IsBetweenTypeExtensions
    {
        /// <summary>
        /// Gets the limit description format.
        /// </summary>
        /// <param name="boundsType">Type of the bounds.</param>
        /// <returns>System.String.</returns>
        public static string GetLimitDescriptionFormat(this IsBetweenBoundsType boundsType)
        {
            return boundsType switch
            {
                IsBetweenBoundsType.IncludeLowerAndUpper => "between {0} and {1}",
                IsBetweenBoundsType.ExcludeLowerAndUpper => "between but not including {0} and {1}",
                IsBetweenBoundsType.IncludeLowerExcludeUpper => "greater than or equal to {0} but less than {1}",
                IsBetweenBoundsType.ExcludeLowerIncludeUpper => "greater than {0} but less than or equal to {1}",
                _ => null
            };
        }
    }
}
