using System.Linq.Expressions;

namespace SpecDeck.Core
{
    /// <summary>
    /// Represents a specification that combines two specifications using a logical AND operation.
    /// </summary>
    /// <typeparam name="T">The type of entity that this specification applies to.</typeparam>
    public class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndSpecification{T}"/> class with the specified left and right specifications.
        /// </summary>
        /// <param name="left">The left specification.</param>
        /// <param name="right">The right specification.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="left"/> or <paramref name="right"/> is null.</exception>
        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }

        /// <summary>
        /// Converts the specification to an expression.
        /// </summary>
        /// <returns>An expression that represents the specification.</returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpr = _left.ToExpression();
            var rightExpr = _right.ToExpression();

            return leftExpr.AndAlso(rightExpr);
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

            if (obj is AndSpecification<T> otherSpec)
                return _left.Equals(otherSpec._left) &&
                       _right.Equals(otherSpec._right);

            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return _left.GetHashCode() ^ _right.GetHashCode() ^ GetType().GetHashCode();
        }
    }
}
