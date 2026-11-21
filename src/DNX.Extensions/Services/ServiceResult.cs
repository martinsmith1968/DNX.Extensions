using System;

namespace DNX.Extensions.Services;

/// <summary>
/// A container for the result of a decoupled service operation.
/// </summary>
/// <remarks>
/// Inspired by: https://codingbolt.net/2023/10/06/the-serviceresult-pattern/
/// </remarks>
public readonly struct ServiceResult<T>
{
    public T Data { get; }
    public Exception Error { get; }
    public bool IsSuccess => Error == null;

    public ServiceResult(T data)
    {
        Data = data;
        Error = null;
    }

    public ServiceResult(Exception error)
    {
        Data = default;
        Error = error;
    }

    /// <summary>
    /// Creates a successful result. The implicit conversion from <typeparamref name="T"/> is
    /// ambiguous when <typeparamref name="T"/> is itself <see cref="Exception"/> - use this
    /// factory method instead in that case.
    /// </summary>
    public static ServiceResult<T> Success(T data) => new(data);

    /// <summary>
    /// Creates a failed result. The implicit conversion from <see cref="Exception"/> is
    /// ambiguous when <typeparamref name="T"/> is itself <see cref="Exception"/> - use this
    /// factory method instead in that case.
    /// </summary>
    public static ServiceResult<T> Failure(Exception error) => new(error);

    // Implicit operator for data
    public static implicit operator ServiceResult<T>(T data) => new(data);

    // Implicit operator for exception
    public static implicit operator ServiceResult<T>(Exception ex) => new(ex);
}
