using Microsoft.CodeAnalysis;

namespace Spectrum.CodeGen.ContextReceivers
{
    /// <summary>
    /// A syntax context receiver that delegates to the SpectrumSpecAttributeReceiver.
    /// </summary>
    internal class ContextSyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly SpectrumSpecAttributeReceiver _spectrumEntitiesReceiver = new SpectrumSpecAttributeReceiver();

        /// <summary>
        /// Gets the list of entities marked with the SpectrumSpec attribute.
        /// </summary>
        public IReadOnlyList<INamedTypeSymbol> SpectrumEntities
            => _spectrumEntitiesReceiver.SpectrumEntities.AsReadOnly();

        /// <summary>
        /// Called when the generator visits a syntax node.
        /// </summary>
        /// <param name="context">The generator syntax context.</param>
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            _spectrumEntitiesReceiver.OnVisitSyntaxNode(context);
        }
    }
}
