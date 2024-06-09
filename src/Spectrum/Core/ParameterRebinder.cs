using System.Linq.Expressions;

namespace Spectrum.Core
{
    /// <summary>
    /// Rebinds parameters in expressions to new parameters.
    /// </summary>
    public sealed class ParameterRebinder : ExpressionVisitor
    {
        private readonly IDictionary<ParameterExpression, ParameterExpression> _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterRebinder"/> class with the specified parameter map.
        /// </summary>
        /// <param name="map">A dictionary that maps original parameters to replacement parameters.</param>
        private ParameterRebinder(IDictionary<ParameterExpression, ParameterExpression>? map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// Replaces the parameters in the given expression according to the specified map.
        /// </summary>
        /// <param name="map">A dictionary that maps original parameters to replacement parameters.</param>
        /// <param name="expression">The expression in which to replace parameters.</param>
        /// <returns>An expression with the parameters replaced.</returns>
        public static Expression ReplaceParameters(
            IDictionary<ParameterExpression, ParameterExpression> map,
            Expression expression)
        {
            return new ParameterRebinder(map).Visit(expression);
        }

        /// <summary>
        /// Visits the <see cref="ParameterExpression"/> nodes in the expression tree and replaces them according to the map.
        /// </summary>
        /// <param name="node">The parameter expression to visit.</param>
        /// <returns>The modified expression, if the parameter is replaced; otherwise, the original expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_map.TryGetValue(node, out var replacement)) node = replacement;

            return base.VisitParameter(node);
        }
    }
}
