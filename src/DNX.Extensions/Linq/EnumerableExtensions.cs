using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable PossibleMultipleEnumeration

namespace DNX.Extensions.Linq;

/// <summary>
/// Enumerable Extensions
/// </summary>
public static class EnumerableExtensions
{
    private static readonly Random Randomizer = new();

    /// <summary>
    /// Determines whether the specified enumerable has any elements and is not null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <returns><c>true</c> if the specified enumerable has any elements; otherwise, <c>false</c>.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static bool HasAny<T>(this IEnumerable<T> enumerable)
    {
        return enumerable != null && enumerable.Any();
    }

    /// <summary>
    /// Determines whether the specified enumerable has any elements that match the predicate and is not null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns><c>true</c> if the specified predicate has any elements that match the predicate; otherwise, <c>false</c>.</returns>
    /// <remarks>Also available as an extension method</remarks>
    public static bool HasAny<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        return enumerable != null && enumerable.Any(predicate);
    }

    /// <summary>
    /// Determine if enumerable contains any of the specified candidates
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <param name="candidates">The candidates.</param>
    /// <returns>
    ///   <c>true</c> if input is one of the specified candidates; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOneOf<T>(this T value, params T[] candidates)
        => value.IsOneOf(candidates?.ToList());

    /// <summary>
    /// Determine if enumerable contains any of the specified candidates, using a Comparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <param name="comparer">The comparer.</param>
    /// <param name="candidates">The candidates.</param>
    /// <returns>
    ///   <c>true</c> if input is one of the specified candidates; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOneOf<T>(this T value, IEqualityComparer<T> comparer, params T[] candidates)
        => value.IsOneOf(candidates?.ToList(), comparer);

    /// <summary>
    /// Determines whether input is one of the specified candidates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input">The input.</param>
    /// <param name="candidates">The candidates.</param>
    /// <returns>
    ///   <c>true</c> if input is one of the specified candidates; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOneOf<T>(this T input, IEnumerable<T> candidates)
    {
        return candidates != null && candidates.Any() && candidates.Contains(input);
    }

    /// <summary>
    /// Determines whether input is one of the specified candidates, according to Comparison Method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input">The input.</param>
    /// <param name="candidates">The candidates.</param>
    /// <param name="comparer">The comparer.</param>
    /// <returns>
    ///   <c>true</c> if input is one of the specified candidates; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOneOf<T>(this T input, IEnumerable<T> candidates, IEqualityComparer<T> comparer)
    {
        return candidates != null && candidates.Any() && candidates.Contains(input, comparer);
    }

    /// <summary>
    /// Gets the random item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">The items.</param>
    /// <param name="randomizer">The randomizer to use (optional)</param>
    /// <returns></returns>
    public static T GetRandomItem<T>(this IEnumerable<T> items, Random randomizer = null)
    {
        // ReSharper disable PossibleMultipleEnumeration
        if (!items.HasAny())
            return default;

        var list = items.ToArray();
        // ReSharper restore PossibleMultipleEnumeration

        var index = (randomizer ?? Randomizer).Next(list.Length);

        return list[index];
    }

    /// <summary>
    /// Gets the item at an index position, or default
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">The items.</param>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public static T GetAt<T>(this IList<T> items, int index)
    {
        return items != null && index >= 0 && index < items.Count
            ? items[index]
            : default;
    }
}
