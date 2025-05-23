using System;
using System.Collections.Generic;
using DNX.Extensions.Exceptions;

namespace DNX.Extensions.Linq;

/// <summary>
/// List Extensions.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Determines whether the index is valid for the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="index">The index.</param>
    /// <returns><c>true</c> if [is index valid] [the specified index]; otherwise, <c>false</c>.</returns>
    public static bool IsIndexValid<T>(this IList<T> list, int index)
    {
        return list.HasAny() && index >= 0 && index < list.Count;
    }

    /// <summary>
    /// Gets the absolute index value for the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="index">The index.</param>
    /// <returns>System.Int32.</returns>
    public static int GetAbsoluteIndex<T>(this IList<T> list, int index)
    {
        if (list.HasAny())
        {
            if (index < 0)
            {
                index = (list.Count + index) % list.Count;
            }
        }

        return index;
    }

    /// <summary>
    /// Gets at.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="index">The index.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>T.</returns>
    public static T GetItemAt<T>(this IList<T> list, int index, T defaultValue = default)
    {
        index = list.GetAbsoluteIndex(index);

        return !list.IsIndexValid(index)
            ? defaultValue
            : list[index];
    }

    /// <summary>
    /// Moves an item to the new specified index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="oldIndex">The old index.</param>
    /// <param name="newIndex">The new index.</param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// oldIndex
    /// or
    /// newIndex
    /// </exception>
    /// <exception cref="ReadOnlyListException{T}"></exception>
    public static void Move<T>(this IList<T> list, int oldIndex, int newIndex)
    {
        oldIndex = list.GetAbsoluteIndex(oldIndex);
        newIndex = list.GetAbsoluteIndex(newIndex);

        if (!list.IsIndexValid(oldIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(oldIndex));
        }

        if (!list.IsIndexValid(newIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(newIndex));
        }

        if (list.IsReadOnly)
        {
            throw new ReadOnlyListException<T>(list);
        }

        var item = list[oldIndex];
        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
    }

    /// <summary>
    /// Swaps the items at the 2 specified indexes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="oldIndex">The old index.</param>
    /// <param name="newIndex">The new index.</param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// oldIndex
    /// or
    /// newIndex
    /// </exception>
    /// <exception cref="ReadOnlyListException{T}"></exception>
    public static void Swap<T>(this IList<T> list, int oldIndex, int newIndex)
    {
        oldIndex = list.GetAbsoluteIndex(oldIndex);
        newIndex = list.GetAbsoluteIndex(newIndex);

        if (!list.IsIndexValid(oldIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(oldIndex));
        }

        if (!list.IsIndexValid(newIndex))
        {
            throw new ArgumentOutOfRangeException(nameof(newIndex));
        }

        if (list.IsReadOnly)
        {
            throw new ReadOnlyListException<T>(list);
        }

        (list[newIndex], list[oldIndex]) = (list[oldIndex], list[newIndex]);
    }

    /// <summary>
    /// Create a list from an arbitrary supplied list of arguments
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    public static IList<T> CreateList<T>(params T[] values)
    {
        return values.ToConcreteList();
    }
}
