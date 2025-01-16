// Code generated by a Template
using System;
using System.Linq.Expressions;
using DNX.Extensions.Maths;
using DNX.Extensions.Reflection;
using DNX.Extensions.Maths.BuiltInTypes;

namespace DNX.Extensions.Validation
{
    /// <summary>
    /// Guard Extensions.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Ensures the expression evaluates to greater than the specified minimum
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="min">The minimum.</param>
        public static void IsGreaterThan(Expression<Func<ushort>> exp, ushort min)
        {
            IsGreaterThan(exp, exp.Compile().Invoke(), min);
        }

        /// <summary>
        /// Ensures the expression and corresponding value evaluates to greater than the specified minimum
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="val">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void IsGreaterThan(Expression<Func<ushort>> exp, ushort val, ushort min)
        {
            if (val > min)
            {
                return;
            }

            var memberName = ExpressionExtensions.GetMemberName(exp);

            throw new ArgumentOutOfRangeException(
                memberName,
                val,
                string.Format("{0} must be greater than {1}",
                    memberName,
                    min
                )
            );
        }

        /// <summary>
        /// Ensures the expression evaluates to greater than or equal to the specified minimum
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="min">The minimum.</param>
        public static void IsGreaterThanOrEqualTo(Expression<Func<ushort>> exp, ushort min)
        {
            IsGreaterThanOrEqualTo(exp, exp.Compile().Invoke(), min);
        }

        /// <summary>
        /// Ensures the expression and corresponding value evaluates to greater than or equal to the specified minimum
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="val">The value.</param>
        /// <param name="min">The minimum.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void IsGreaterThanOrEqualTo(Expression<Func<ushort>> exp, ushort val, ushort min)
        {
            if (val >= min)
            {
                return;
            }

            var memberName = ExpressionExtensions.GetMemberName(exp);

            throw new ArgumentOutOfRangeException(
                memberName,
                val,
                string.Format("{0} must be greater than or equal to {1}",
                    memberName,
                    min
                )
            );
        }

        /// <summary>
        /// Ensures the expression evaluates to less than the specified minimum
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="max">The maximum.</param>
        public static void IsLessThan(Expression<Func<ushort>> exp, ushort max)
        {
            IsLessThan(exp, exp.Compile().Invoke(), max);
        }

        /// <summary>
        /// Ensures the expression and corresponding value evaluates to less than the specified minimum
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="val">The value.</param>
        /// <param name="max">The minimum.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void IsLessThan(Expression<Func<ushort>> exp, ushort val, ushort max)
        {
            if (val < max)
            {
                return;
            }

            var memberName = ExpressionExtensions.GetMemberName(exp);

            throw new ArgumentOutOfRangeException(
                memberName,
                val,
                string.Format("{0} must be less than {1}",
                    memberName,
                    max
                )
            );
        }

        /// <summary>
        /// Ensures the expression evaluates to less than or equal to the specified minimum
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="max">The maximum.</param>
        public static void IsLessThanOrEqualTo(Expression<Func<ushort>> exp, ushort max)
        {
            IsLessThanOrEqualTo(exp, exp.Compile().Invoke(), max);
        }

        /// <summary>
        /// Ensures the expression and corresponding value evaluates to less than or equal to the specified minimum
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="val">The value.</param>
        /// <param name="max">The maximum.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void IsLessThanOrEqualTo(Expression<Func<ushort>> exp, ushort val, ushort max)
        {
            if (val <= max)
            {
                return;
            }

            var memberName = ExpressionExtensions.GetMemberName(exp);

            throw new ArgumentOutOfRangeException(
                memberName,
                val,
                string.Format("{0} must be less than or equal to {1}",
                    memberName,
                    max
                )
            );
        }

        /// <summary>
        /// Ensures the expression evaluates to between the specified values
        /// </summary>
        /// <param name="exp">The linq expression of the argument to check</param>
        /// <param name="min">minimum allowed value</param>
        /// <param name="max">maximum allowed value</param>
        public static void IsBetween(Expression<Func<ushort>> exp, ushort min, ushort max)
        {
            IsBetween(exp, min, max, IsBetweenBoundsType.Inclusive);
        }

        /// <summary>
        /// Ensures the expression and corresponding value evaluates to between the specified values
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="boundsType">Type of the bounds.</param>
        public static void IsBetween(Expression<Func<ushort>> exp, ushort min, ushort max, IsBetweenBoundsType boundsType)
        {
            IsBetween(exp, min, max, false, boundsType);
        }

        /// <summary>
        /// Ensures the expression evaluates to between the specified values
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="bound1">The bound1.</param>
        /// <param name="bound2">The bound2.</param>
        /// <param name="allowEitherOrder">if set to <c>true</c> [allow either order].</param>
        /// <param name="boundsType">Type of the bounds.</param>
        public static void IsBetween(Expression<Func<ushort>> exp, ushort bound1, ushort bound2, bool allowEitherOrder, IsBetweenBoundsType boundsType)
        {
            IsBetween(exp, exp.Compile().Invoke(), bound1, bound2, allowEitherOrder, boundsType);
        }

        /// <summary>
        /// Ensures the expression and corresponding value evaluates to between the specified values
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="val">The value.</param>
        /// <param name="bound1">The bound1.</param>
        /// <param name="bound2">The bound2.</param>
        /// <param name="allowEitherOrder">if set to <c>true</c> [allow either order].</param>
        /// <param name="boundsType">Type of the bounds.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static void IsBetween(Expression<Func<ushort>> exp, ushort val, ushort bound1, ushort bound2, bool allowEitherOrder, IsBetweenBoundsType boundsType)
        {
            if (val.IsBetween(bound1, bound2, allowEitherOrder, boundsType))
            {
                return;
            }

            var memberName = ExpressionExtensions.GetMemberName(exp);

            throw new ArgumentOutOfRangeException(
                memberName,
                val,
                string.Format("{0} must be {1}",
                    memberName,
                    string.Format(boundsType.GetLimitDescriptionFormat(),
                        MathsUInt16Extensions.GetLowerBound(bound1, bound2, allowEitherOrder),
                        MathsUInt16Extensions.GetUpperBound(bound1, bound2, allowEitherOrder)
                        )
                    )
                );
        }
    }
}
