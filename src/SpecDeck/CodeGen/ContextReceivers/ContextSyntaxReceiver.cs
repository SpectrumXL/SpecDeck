using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace SpecDeck.CodeGen.ContextReceivers
{
    /// <summary>
    /// A syntax context receiver that delegates to the SpecDeckSpecAttributeReceiver.
    /// </summary>
    internal class ContextSyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly SpecDeckSpecAttributeReceiver _SpecDeckEntitiesReceiver = new SpecDeckSpecAttributeReceiver();

        /// <summary>
        /// Gets the list of entities marked with the SpecDeckSpec attribute.
        /// </summary>
        public IReadOnlyList<INamedTypeSymbol> SpecDeckEntities
            => _SpecDeckEntitiesReceiver.SpecDeckEntities.AsReadOnly();

        /// <summary>
        /// Called when the generator visits a syntax node.
        /// </summary>
        /// <param name="context">The generator syntax context.</param>
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            _SpecDeckEntitiesReceiver.OnVisitSyntaxNode(context);
        }
    }
}
