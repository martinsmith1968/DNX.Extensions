using System.Collections.Generic;
using DNX.Extensions.Validation.BuiltInTypes;

namespace DNX.Extensions.Collections;

/// <summary>
/// HashSet Extensions
/// </summary>
public static class HashSetExtensions
{
    /// <summary>
    /// Determines whether the specified set is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="set">The set.</param>
    /// <returns><c>true</c> if the set is null or has no elements; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty<T>(this HashSet<T> set)
    {
        return set == null || set.Count == 0;
    }

    /// <summary>
    /// Adds each of the specified items to the set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="set">The set.</param>
    /// <param name="items">The items.</param>
    /// <returns>The number of items that were not already present and were added.</returns>
    public static int AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
    {
        Guard.IsNotNull(() => set);

        if (items == null)
        {
            return 0;
        }

        var addedCount = 0;

        foreach (var item in items)
        {
            if (set.Add(item))
            {
                ++addedCount;
            }
        }

        return addedCount;
    }

#if NETSTANDARD2_0
    /// <summary>
    /// Creates a <see cref="HashSet{T}"/> from an <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    /// <param name="comparer">The comparer.</param>
    /// <returns>HashSet&lt;T&gt;.</returns>
    /// <remarks>
    /// netstandard2.0 has no built-in Enumerable.ToHashSet - this fills that gap. Deliberately
    /// excluded on later targets, where the BCL already provides it, to avoid an ambiguous-call
    /// error between two identical extension methods.
    /// </remarks>
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
    {
        Guard.IsNotNull(() => source);

        return new HashSet<T>(source, comparer);
    }
#endif
}
