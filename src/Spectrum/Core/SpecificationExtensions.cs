namespace Spectrum.Core
{
    /// <summary>
    /// Provides extension methods for working with specifications.
    /// </summary>
    public static class SpecificationExtensions
    {
        /// <summary>
        /// Maps an array of values to a collection of specifications.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="values">The array of values to map.</param>
        /// <param name="selector">The function to create specifications from values.</param>
        /// <param name="defaultValue">The default value if the array is empty.</param>
        /// <returns>A collection of specifications mapped from the values.</returns>
        public static IEnumerable<Specification<TEntity>> Map<TEntity, TValue>(
            this TValue[] values,
            Func<TValue, Specification<TEntity>> selector,
            bool defaultValue = true)
        {
            return values.Length is 0
                ? Enumerable.Repeat(FromBool<TEntity>(defaultValue), 1)
                : values.Select(selector);
        }

        /// <summary>
        /// Combines a collection of specifications using logical AND.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="specs">The collection of specifications to combine.</param>
        /// <returns>A single specification representing the logical AND of the input specifications.</returns>
        public static Specification<T> CombineAnd<T>(this IEnumerable<Specification<T>> specs)
        {
            return specs.Aggregate((seed, spec) => seed &= spec);
        }

        /// <summary>
        /// Combines a collection of specifications using logical OR.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="specs">The collection of specifications to combine.</param>
        /// <returns>A single specification representing the logical OR of the input specifications.</returns>
        public static Specification<T> CombineOr<T>(this IEnumerable<Specification<T>> specs)
        {
            return specs.Aggregate((seed, spec) => seed |= spec);
        }

        /// <summary>
        /// Creates a specification from a boolean value.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="defaultValue">The boolean value to create the specification from.</param>
        /// <returns>A specification that evaluates to the given boolean value.</returns>
        private static Specification<T> FromBool<T>(bool defaultValue)
        {
            return defaultValue
                ? Specification<T>.True
                : Specification<T>.False;
        }
    }
}
