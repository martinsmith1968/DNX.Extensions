namespace DNX.Extensions.Strings
{
    /// <summary>
    /// How the delimiter is to be treated when splitting text
    /// </summary>
    public enum SplitDelimiterType
    {
        /// <summary>
        /// Any specified value can be a delimiter
        /// </summary>
        Any = 0,

        /// <summary>
        /// All specified values are the delimiter
        /// </summary>
        All
    }
}