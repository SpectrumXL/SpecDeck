using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using SpecDeck.CodeGen.ContextReceivers;
using SpecDeck.CodeGen.Models;

namespace SpecDeck.CodeGen
{
    /// <summary>
    /// A source generator that generates specifications for entities marked with the SpecDeckSpec attribute.
    /// </summary>
    [Generator]
    public class SpecificationGenerator : ISourceGenerator
    {
        private readonly SpecsFactoryCodeGenerator _specsFactoryCodeGenerator = new SpecsFactoryCodeGenerator();
        private readonly EntitySpecsCodeGenerator _entitySpecsCodeGenerator = new EntitySpecsCodeGenerator();

        private GeneratorExecutionContext _executionContext;

        /// <summary>
        /// Initializes the source generator and registers for syntax notifications.
        /// </summary>
        /// <param name="context">The generator initialization context.</param>
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ContextSyntaxReceiver());
        }

        /// <summary>
        /// Executes the source generator to generate specifications.
        /// </summary>
        /// <param name="context">The generator execution context.</param>
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is ContextSyntaxReceiver receiver))
                return;

            _executionContext = context;

            foreach (var classSymbol in receiver.SpecDeckEntities)
            {
                var entityGenerationContext = new EntitySpecsGenerationContext
                {
                    EntityName = classSymbol.Name,
                    EntityFullName = classSymbol.ToDisplayString(),
                    EntityNamespace = classSymbol.ContainingNamespace.ToDisplayString()
                };

                GenerateSpecifications(entityGenerationContext, classSymbol);
            }
        }

        /// <summary>
        /// Generates specifications for the specified class symbol.
        /// </summary>
        /// <param name="entityGenerationContext">The context for generating entity specifications.</param>
        /// <param name="classSymbol">The class symbol for which to generate specifications.</param>
        private void GenerateSpecifications(
            EntitySpecsGenerationContext entityGenerationContext,
            INamedTypeSymbol classSymbol)
        {
            var generatedSpecs = GeneratePropertySpecifications(classSymbol, entityGenerationContext);

            var factorySourceCode = _specsFactoryCodeGenerator
                .GenerateSpecificationsFactory(entityGenerationContext, generatedSpecs);

            _executionContext.AddSource($"{classSymbol.Name}Specs.g.cs", factorySourceCode);
        }

        /// <summary>
        /// Generates property specifications for the specified class symbol.
        /// </summary>
        /// <param name="classSymbol">The class symbol for which to generate property specifications.</param>
        /// <param name="entityGenerationContext">The context for generating entity specifications.</param>
        /// <returns>A read-only list of generated specification descriptors.</returns>
        private IReadOnlyList<SpecificationDescriptor> GeneratePropertySpecifications(
            INamedTypeSymbol classSymbol,
            EntitySpecsGenerationContext entityGenerationContext)
        {
            var propertyDescriptors = GetEntityPropertyDescriptors(classSymbol);
            var generatedClasses = _entitySpecsCodeGenerator
                .GetEntitySpecsCode(entityGenerationContext, propertyDescriptors);

            foreach (var kvp in generatedClasses)
            {
                SaveSpecificationClass(kvp);
            }

            return generatedClasses.Keys.ToList().AsReadOnly();
        }

        /// <summary>
        /// Saves the generated specification class to the source.
        /// </summary>
        /// <param name="kvp">The key-value pair containing the specification descriptor and the generated code.</param>
        private void SaveSpecificationClass(KeyValuePair<SpecificationDescriptor, string> kvp)
        {
            _executionContext.AddSource($"{kvp.Key.Name}.g.cs", kvp.Value);
        }

        /// <summary>
        /// Gets the property descriptors for the specified class symbol.
        /// </summary>
        /// <param name="classSymbol">The class symbol for which to get property descriptors.</param>
        /// <returns>A list of entity property descriptors.</returns>
        private static List<EntityPropertyDescriptor> GetEntityPropertyDescriptors(INamedTypeSymbol classSymbol)
        {
            var properties = classSymbol
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(x => x.DeclaredAccessibility == Accessibility.Public)
                .ToList();

            var propertyDescriptors = properties
                .Select(x => new EntityPropertyDescriptor
                {
                    Name = x.Name,
                    Type = x.Type.ToDisplayString(),
                    IsCollection = typeof(IEnumerable<>).IsInstanceOfType(x.Type),
                    IsNullable = x.NullableAnnotation == NullableAnnotation.Annotated
                }).ToList();
            return propertyDescriptors;
        }
    }
}
