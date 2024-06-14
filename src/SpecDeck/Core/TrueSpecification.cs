using System.Linq.Expressions;
using System;

namespace SpecDeck.Core
{
    /// <summary>
    /// A specification that always evaluates to true.
    /// </summary>
    /// <typeparam name="T">The type of entity that this specification applies to.</typeparam>
    public class TrueSpecification<T> : Specification<T>
    {
        /// <summary>
        /// Returns an expression that always evaluates to true.
        /// </summary>
        /// <returns>An expression that always evaluates to true.</returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            return x => true;
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

            return GetType() == obj.GetType();
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }
}
