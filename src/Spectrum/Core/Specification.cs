using System.Linq.Expressions;

namespace Spectrum.Core
{
    /// <summary>
    /// Represents a specification pattern for defining business rules.
    /// </summary>
    /// <typeparam name="T">The type of entity that this specification applies to.</typeparam>
    public abstract class Specification<T>
    {
        /// <summary>
        /// A specification that always evaluates to true.
        /// </summary>
        public static readonly Specification<T> True = new TrueSpecification<T>();

        /// <summary>
        /// A specification that always evaluates to false.
        /// </summary>
        public static readonly Specification<T> False = new NotSpecification<T>(new TrueSpecification<T>());

        /// <summary>
        /// Converts the specification to an expression.
        /// </summary>
        /// <returns>An expression that represents the specification.</returns>
        public abstract Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Converts the specification to a predicate function.
        /// </summary>
        /// <returns>A function that represents the specification.</returns>
        public virtual Func<T, bool> ToPredicate()
        {
            return ToExpression().Compile();
        }

        /// <summary>
        /// Determines whether the specified object satisfies the specification.
        /// </summary>
        /// <param name="obj">The object to evaluate.</param>
        /// <returns>true if the object satisfies the specification; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the object is null.</exception>
        public bool IsSatisfiedBy(T obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var predicate = ToPredicate();
            return predicate(obj);
        }

        /// <summary>
        /// Implements the logical AND operator for specifications.
        /// </summary>
        /// <param name="left">The left specification.</param>
        /// <param name="right">The right specification.</param>
        /// <returns>A specification that represents the logical AND of the input specifications.</returns>
        public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        {
            return new AndSpecification<T>(left, right);
        }

        /// <summary>
        /// Implements the logical OR operator for specifications.
        /// </summary>
        /// <param name="left">The left specification.</param>
        /// <param name="right">The right specification.</param>
        /// <returns>A specification that represents the logical OR of the input specifications.</returns>
        public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        {
            return new OrSpecification<T>(left, right);
        }

        /// <summary>
        /// Implements the logical NOT operator for specifications.
        /// </summary>
        /// <param name="spec">The specification to negate.</param>
        /// <returns>A specification that represents the logical NOT of the input specification.</returns>
        public static Specification<T> operator !(Specification<T> spec)
        {
            return new NotSpecification<T>(spec);
        }
    }
}
