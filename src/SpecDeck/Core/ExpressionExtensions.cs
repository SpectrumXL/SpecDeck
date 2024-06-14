using System.Linq.Expressions;
using System;
using System.Linq;

namespace SpecDeck.Core
{
    /// <summary>
    /// Provides extension methods for combining expressions.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Combines two expressions using a logical AND operation.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="left">The left expression.</param>
        /// <param name="right">The right expression.</param>
        /// <returns>An expression that represents the logical AND of the input expressions.</returns>
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return left.Compose(right, Expression.AndAlso);
        }

        /// <summary>
        /// Combines two expressions using a logical OR operation.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="left">The left expression.</param>
        /// <param name="right">The right expression.</param>
        /// <returns>An expression that represents the logical OR of the input expressions.</returns>
        public static Expression<Func<T, bool>> OrElse<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return left.Compose(right, Expression.OrElse);
        }

        /// <summary>
        /// Composes two expressions using a specified merge function.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="left">The left expression.</param>
        /// <param name="right">The right expression.</param>
        /// <param name="merge">The function to merge the expressions.</param>
        /// <returns>An expression that represents the merged expressions.</returns>
        private static Expression<T> Compose<T>(
            this Expression<T> left,
            Expression<T> right,
            Func<Expression, Expression, Expression> merge)
        {
            var map = left.Parameters
                .Select((expr, index) => new { Expression = expr, Parameter = right.Parameters[index] })
                .ToDictionary(p => p.Parameter, p => p.Expression);

            var rightBody = ParameterRebinder.ReplaceParameters(map, right.Body);

            return Expression.Lambda<T>(merge(left.Body, rightBody), left.Parameters);
        }
    }
}
