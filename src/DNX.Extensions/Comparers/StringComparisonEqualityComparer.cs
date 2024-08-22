using System;
using System.Collections.Generic;

#pragma warning disable IDE0290 // Use Primary Constructor

namespace DNX.Extensions.Comparers;

/// <summary>
/// String Equality Comparer based on <see cref="StringComparison"/>
/// </summary>
/// <seealso cref="string" />
public class StringComparisonEqualityComparer : IEqualityComparer<string>
{
    /// <summary>
    /// The string comparison method
    /// </summary>
    public StringComparison StringComparisonMethod { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringComparisonEqualityComparer"/> class.
    /// </summary>
    public StringComparisonEqualityComparer()
        : this(StringComparison.CurrentCulture)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringComparisonEqualityComparer"/> class.
    /// </summary>
    /// <param name="stringComparisonMethod">The string comparison.</param>
    public StringComparisonEqualityComparer(StringComparison stringComparisonMethod)
    {
        StringComparisonMethod = stringComparisonMethod;
    }

    /// <summary>
    /// Determines whether the specified objects are equal.
    /// </summary>
    /// <param name="x">The first object of type T to compare.</param>
    /// <param name="y">The second object of type T to compare.</param>
    /// <returns>
    /// true if the specified objects are equal; otherwise, false.
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public bool Equals(string x, string y)
    {
        return string.Equals(x, y, StringComparisonMethod);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public int GetHashCode(string obj)
    {
        if (Equals(obj?.ToLowerInvariant(), obj?.ToUpperInvariant()))
            return obj?.ToLowerInvariant().GetHashCode() ?? default;

        return obj.GetHashCode();
    }
}
