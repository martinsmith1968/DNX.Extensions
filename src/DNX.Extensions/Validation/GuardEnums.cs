using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DNX.Extensions.Enumerations;
using DNX.Extensions.Reflection;

namespace DNX.Extensions.Validation
{
    public static partial class Guard
    {
        /// <summary>
        /// Ensures the expression is a valid Enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">The exp.</param>
        public static void IsValidEnum<T>(this Expression<Func<T>> exp)
            where T : struct
        {
            IsValidEnum(exp, exp.Compile().Invoke());
        }

        /// <summary>
        /// Ensures the expression is a valid Enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">The exp.</param>
        /// <param name="val">The value.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void IsValidEnum<T>(this Expression<Func<T>> exp, T val)
            where T : struct
        {
            if (!val.IsValidEnum())
            {
                var memberName = ExpressionExtensions.GetMemberName(exp);

                throw new ArgumentException(
                    $"{memberName} must be a valid {typeof(T).Name} value", memberName
                );
            }
        }

        /// <summary>
        /// Ensures the expression is a valid Enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">The exp.</param>
        /// <param name="allowed">The allowed.</param>
        public static void IsEnumOneOf<T>(this Expression<Func<T>> exp, params T[] allowed)
            where T : struct
        {
            IsEnumOneOf(exp, exp.Compile().Invoke(), allowed);
        }

        /// <summary>
        /// Ensures the expression is a valid Enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">The exp.</param>
        /// <param name="allowed">The allowed.</param>
        public static void IsEnumOneOf<T>(this Expression<Func<T>> exp, IList<T> allowed)
            where T : struct
        {
            IsEnumOneOf(exp, exp.Compile().Invoke(), allowed);
        }

        /// <summary>
        /// Ensures the expression is a valid Enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">The exp.</param>
        /// <param name="val">The value.</param>
        /// <param name="allowed">The allowed.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void IsEnumOneOf<T>(this Expression<Func<T>> exp, T val, IList<T> allowed)
            where T : struct
        {
            if (!val.IsValueOneOf(allowed))
            {
                var memberName = ExpressionExtensions.GetMemberName(exp);

                var allowedValues = allowed
                    .Select(a => Convert.ToString(a));

                throw new ArgumentException(
                    $"{memberName} must be an allowed {typeof(T).Name} value: {string.Join(",", allowedValues)}",
                    memberName
                );
            }
        }
    }
}
