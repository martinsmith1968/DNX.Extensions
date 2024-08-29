using System;
using System.Linq.Expressions;

namespace DNX.Extensions.Reflection
{
    /// <summary>
    /// Class ExpressionExtensions.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Determines whether the func is a member expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static bool IsMemberExpression<T>(this Expression<Func<T>> func)
        {
            return func.Body is MemberExpression;
        }

        /// <summary>
        /// Determines whether the action is a member expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static bool IsMemberExpression<T>(this Expression<Action> action)
        {
            return action.Body is MemberExpression;
        }

        /// <summary>
        /// Determines whether the func is a lambda expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static bool IsLambdaExpression<T>(Expression<Func<T>> func)
        {
            return func.Body is LambdaExpression;
        }

        /// <summary>
        /// Determines whether the action is a lambda expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static bool IsLambdaExpression<T>(Expression<Action<T>> action)
        {
            return action.Body is LambdaExpression;
        }

        /// <summary>
        /// Determines whether the func is a unary expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static bool IsUnaryExpression<T>(Expression<Func<T>> func)
        {
            var unaryExpression = func.Body as UnaryExpression;

            return unaryExpression != null;
        }

        /// <summary>
        /// Determines whether the action is a unary expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static bool IsUnaryExpression<T>(Expression<Action<T>> action)
        {
            var unaryExpression = action.Body as UnaryExpression;

            return unaryExpression != null;
        }

        /// <summary>
        /// Gets the name of the member expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static string GetMemberName<T>(Expression<Func<T>> func)
        {
            var memberExpression = func.Body as MemberExpression;

            return memberExpression?.Member.Name;
        }

        /// <summary>
        /// Gets the name of the lambda expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">The exp.</param>
        /// <returns></returns>
        public static string GetLambdaName<T>(Expression<Func<T>> exp)
        {
            var lambdaExpression = exp.Body as LambdaExpression;

            return lambdaExpression?.Name;
        }

        /// <summary>
        /// Gets the name of the unary expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">The exp.</param>
        /// <returns></returns>
        public static string GetUnaryName<T>(Expression<Func<T>> exp)
        {
            var unaryExpression = exp.Body as UnaryExpression; //((UnaryExpression)exp.Body).Operand;

            var operand = unaryExpression?.Operand as MemberExpression;

            return operand?.Member.Name;
        }

        /// <summary>
        /// Gets the name of the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">The exp.</param>
        /// <returns></returns>
        public static string GetExpressionName<T>(Expression<Func<T>> exp)
        {
            if (IsMemberExpression(exp))
            {
                return GetMemberName(exp);
            }

            if (IsLambdaExpression(exp))
            {
                return GetLambdaName(exp);
            }

            if (IsUnaryExpression(exp))
            {
                return GetUnaryName(exp);
            }

            return null;
        }
    }
}
