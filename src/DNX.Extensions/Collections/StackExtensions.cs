using System.Collections.Generic;
using DNX.Extensions.Validation.BuiltInTypes;

namespace DNX.Extensions.Collections;

/// <summary>
/// Stack Extensions
/// </summary>
public static class StackExtensions
{
    /// <summary>
    /// Determines whether the specified stack is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stack">The stack.</param>
    /// <returns><c>true</c> if the stack is null or has no elements; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty<T>(this Stack<T> stack)
    {
        return stack == null || stack.Count == 0;
    }

    /// <summary>
    /// Pushes each of the specified items onto the stack, in the order supplied (so the last
    /// item in the sequence ends up on top).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stack">The stack.</param>
    /// <param name="items">The items.</param>
    public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
    {
        Guard.IsNotNull(() => stack);

        if (items == null)
        {
            return;
        }

        foreach (var item in items)
        {
            stack.Push(item);
        }
    }
}
