using System.Linq.Expressions;
using System;

namespace SpecDeck.Core
{
    /// <summary>
    /// Represents a specification that negates another specification.
    /// </summary>
    /// <typeparam name="T">The type of entity that this specification applies to.</typeparam>
    public class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _specification;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification{T}"/> class with the specified specification to negate.
        /// </summary>
        /// <param name="spec">The specification to negate.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="spec"/> is null.</exception>
        public NotSpecification(Specification<T> spec)
        {
            _specification = spec ?? throw new ArgumentNullException(nameof(spec));
        }

        /// <summary>
        /// Converts the specification to an expression.
        /// </summary>
        /// <returns>An expression that represents the specification.</returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            var expr = _specification.ToExpression();
            return Expression.Lambda<Func<T, bool>>(Expression.Not(expr.Body), expr.Parameters);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            if (ReferenceEquals(this, obj)) return true;

            if (obj is NotSpecification<T> otherSpec) return _specification.Equals(otherSpec._specification);

            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return _specification.GetHashCode() ^ GetType().GetHashCode();
        }
    }
}
