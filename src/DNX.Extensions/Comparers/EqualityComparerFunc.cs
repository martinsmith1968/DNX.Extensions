using System;
using System.Collections.Generic;

namespace DNX.Extensions.Comparers;

/// <summary>
/// Class EqualityComparerFunc.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.Collections.Generic.IEqualityComparer{T}" />
public class EqualityComparerFunc<T> : IEqualityComparer<T>
{
    private readonly Func<T, T, bool> _func;
    private readonly Func<T, int> _hash;

    private EqualityComparerFunc(Func<T, T, bool> func, Func<T, int> hash)
    {
        _func = func;
        _hash = hash;
    }

    /// <summary>
    /// Determines whether the specified objects are equal.
    /// </summary>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>true if the specified objects are equal; otherwise, false.</returns>
    public bool Equals(T x, T y)
    {
        return _func(x, y);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public int GetHashCode(T obj)
    {
        return _hash(obj);
    }

    /// <summary>
    /// Creates a comparer for the specified type, using the type's default hash code.
    /// </summary>
    /// <param name="func">The function.</param>
    /// <returns>ActionEqualityComparer&lt;T&gt;.</returns>
    /// <remarks>
    /// If <paramref name="func"/> considers values equal that the default <see cref="EqualityComparer{T}"/>
    /// would hash differently, use the <see cref="Create(Func{T, T, bool}, Func{T, int})"/> overload with a
    /// matching hash function instead, to satisfy the Equals/GetHashCode contract.
    /// </remarks>
    public static EqualityComparerFunc<T> Create(Func<T, T, bool> func)
    {
        return new EqualityComparerFunc<T>(func, obj => obj == null ? 0 : EqualityComparer<T>.Default.GetHashCode(obj));
    }

    /// <summary>
    /// Creates a comparer for the specified type, with an explicit hash function matching the equality function.
    /// </summary>
    /// <param name="func">The equality function.</param>
    /// <param name="hash">The hash function.</param>
    /// <returns>ActionEqualityComparer&lt;T&gt;.</returns>
    public static EqualityComparerFunc<T> Create(Func<T, T, bool> func, Func<T, int> hash)
    {
        return new EqualityComparerFunc<T>(func, hash);
    }
}
