using System;
using System.Collections.Generic;

namespace DNX.Extensions.Exceptions;

/// <summary>
/// Class ReadOnlyListException.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="System.Exception" />
public class ReadOnlyListException<T> : Exception
{
    private const string MessageTemplate = "The list of type {0} is read-only and cannot be modified";

    /// <summary>
    /// Gets the list.
    /// </summary>
    /// <value>The list.</value>
    public IList<T> List { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyListException{T}" /> class.
    /// </summary>
    /// <param name="list">The list.</param>
    public ReadOnlyListException(IList<T> list)
        : base(string.Format(MessageTemplate, list?.GetType().Name))
    {
        List = list;
    }
}
