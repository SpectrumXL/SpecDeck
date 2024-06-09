using System.Linq.Expressions;

namespace SpecDeck.Core
{
    /// <summary>
    /// Represents a specification created from an ad-hoc expression.
    /// </summary>
    /// <typeparam name="T">The type of entity that this specification applies to.</typeparam>
    public class AdHocSpecification<T> : Specification<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdHocSpecification{T}"/> class with the specified expression.
        /// </summary>
        /// <param name="expression">The expression that represents the specification.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="expression"/> is null.</exception>
        public AdHocSpecification(Expression<Func<T, bool>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>
        /// Converts the specification to an expression.
        /// </summary>
        /// <returns>An expression that represents the specification.</returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            return _expression;
        }
    }
}
