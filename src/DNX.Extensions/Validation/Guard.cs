using System;
using System.Linq.Expressions;
using DNX.Extensions.Reflection;

// ReSharper disable InvertIf

namespace DNX.Extensions.Validation;

/// <summary>
/// Guard Extensions.
/// </summary>
public static partial class Guard
{
    /// <summary>
    /// Ensures the specified exp is true.
    /// </summary>
    /// <param name="exp">The exp.</param>
    public static void IsTrue(Expression<Func<bool>> exp)
    {
        IsTrue(exp, exp.Compile().Invoke());
    }

    /// <summary>
    /// Ensures the specified exp is true.
    /// </summary>
    /// <param name="exp">The exp.</param>
    /// <param name="val">The value.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void IsTrue(Expression<Func<bool>> exp, bool val)
    {
        if (val)
        {
            return;
        }

        var memberName = ExpressionExtensions.GetMemberName(exp);

        throw new ArgumentOutOfRangeException(
            memberName,
            false,
            $"{memberName} must be true"
        );
    }

    /// <summary>
    /// Ensures the specified exp is false.
    /// </summary>
    /// <param name="exp">The exp.</param>
    public static void IsFalse(Expression<Func<bool>> exp)
    {
        IsFalse(exp, exp.Compile().Invoke());
    }

    /// <summary>
    /// Ensures the specified exp is false.
    /// </summary>
    /// <param name="exp">The exp.</param>
    /// <param name="val">The value.</param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static void IsFalse(Expression<Func<bool>> exp, bool val)
    {
        if (!val)
        {
            return;
        }

        var memberName = ExpressionExtensions.GetMemberName(exp);

        throw new ArgumentOutOfRangeException(
            memberName,
            true,
            $"{memberName} must be false"
        );
    }

    /// <summary>
    /// Ensures the specified exp is not null
    /// </summary>
    /// <typeparam name="T">Any reference type</typeparam>
    /// <param name="exp">The linq expression of the argument to check</param>
    public static void IsNotNull<T>(Expression<Func<T>> exp)
        where T : class
    {
        var val = exp.Compile().Invoke();

        IsNotNull(exp, val);
    }

    /// <summary>
    /// Ensures the specified exp is not null
    /// </summary>
    /// <typeparam name="T">Any reference type</typeparam>
    /// <param name="exp">The linq expression of the argument to check</param>
    /// <param name="val">value of argument in exp</param>
    /// <remarks>Use this if you are not happy that the expression exp will be invoked more than once by your method.</remarks>
    public static void IsNotNull<T>(Expression<Func<T>> exp, T val)
        where T : class
    {
        if (val == null)
        {
            var memberName = ExpressionExtensions.GetMemberName(exp);

            throw new ArgumentNullException(
                memberName,
                $"{memberName} must not be null"
            );
        }
    }

    /// <summary>
    /// Ensures the specified exp is not null or empty
    /// </summary>
    /// <param name="exp">The linq expression of the argument to check</param>
    public static void IsNotNullOrEmpty(Expression<Func<string>> exp)
    {
        IsNotNullOrEmpty(exp, exp.Compile().Invoke());
    }

    /// <summary>
    /// Ensures the specified exp is not null or empty
    /// </summary>
    /// <param name="exp">The linq expression of the argument to check</param>
    /// <param name="val">value of argument in exp</param>
    /// <remarks>Use this if you are not happy that the expression exp will be invoked more than once by your method.</remarks>
    public static void IsNotNullOrEmpty(Expression<Func<string>> exp, string val)
    {
        if (string.IsNullOrEmpty(val))
        {
            var memberName = ExpressionExtensions.GetMemberName(exp);

            throw new ArgumentException(
                $"{memberName} must not be null or empty", memberName
            );
        }
    }

    /// <summary>
    /// Ensures the specified exp is not null or whitespace
    /// </summary>
    /// <param name="exp">The linq expression of the argument to check</param>
    public static void IsNotNullOrWhitespace(Expression<Func<string>> exp)
    {
        IsNotNullOrWhitespace(exp, exp.Compile().Invoke());
    }

    /// <summary>
    /// Ensures the specified exp is not null or whitespace
    /// </summary>
    /// <param name="exp">The linq expression of the argument to check</param>
    /// <param name="val">value of argument in exp</param>
    /// <remarks>Use this if you are not happy that the expression exp will be invoked more than once by your method.</remarks>
    public static void IsNotNullOrWhitespace(Expression<Func<string>> exp, string val)
    {
        if (string.IsNullOrWhiteSpace(val))
        {
            var memberName = ExpressionExtensions.GetMemberName(exp);

            throw new ArgumentException(
                $"{memberName} must not be null or whitespace", memberName
            );
        }
    }
}
