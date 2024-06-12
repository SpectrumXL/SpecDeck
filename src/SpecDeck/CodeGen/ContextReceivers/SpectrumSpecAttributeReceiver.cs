using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SpecDeck.CodeGen.ContextReceivers
{
    /// <summary>
    /// A syntax context receiver that collects entities marked with the SpecDeckSpec attribute.
    /// </summary>
    internal class SpecDeckSpecAttributeReceiver : ISyntaxContextReceiver
    {
        /// <summary>
        /// Gets the list of entities marked with the SpecDeckSpec attribute.
        /// </summary>
        public List<INamedTypeSymbol> SpecDeckEntities { get; } = new List<INamedTypeSymbol>();

        /// <summary>
        /// Called when the generator visits a syntax node.
        /// </summary>
        /// <param name="context">The generator syntax context.</param>
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            var syntaxNode = context.Node;
            if (!(syntaxNode is ClassDeclarationSyntax classDeclaration))
            {
                return;
            }

            var containsSpecAttribute = classDeclaration.AttributeLists
                .SelectMany(al => al.Attributes)
                .Any(ad => ad.Name.ToString() == "SpecDeckSpec");

            if (!containsSpecAttribute)
            {
                return;
            }

            var namedTypeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
            if (namedTypeSymbol != null)
            {
                SpecDeckEntities.Add(namedTypeSymbol);
            }
        }
    }
}
