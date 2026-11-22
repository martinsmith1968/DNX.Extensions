using System.Collections.Generic;
using DNX.Extensions.Validation.BuiltInTypes;

namespace DNX.Extensions.Collections;

/// <summary>
/// Queue Extensions
/// </summary>
public static class QueueExtensions
{
    /// <summary>
    /// Determines whether the specified queue is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queue">The queue.</param>
    /// <returns><c>true</c> if the queue is null or has no elements; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty<T>(this Queue<T> queue)
    {
        return queue == null || queue.Count == 0;
    }

    /// <summary>
    /// Enqueues each of the specified items, in the order supplied.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queue">The queue.</param>
    /// <param name="items">The items.</param>
    public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
    {
        Guard.IsNotNull(() => queue);

        if (items == null)
        {
            return;
        }

        foreach (var item in items)
        {
            queue.Enqueue(item);
        }
    }
}
