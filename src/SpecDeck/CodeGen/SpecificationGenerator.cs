using System.Text;
using Microsoft.CodeAnalysis;
using SpecDeck.CodeGen.ContextReceivers;

namespace SpecDeck.CodeGen
{
    /// <summary>
    /// A source generator that generates specifications for entities marked with the SpecDeckSpec attribute.
    /// </summary>
    [Generator]
    public class SpecificationGenerator : ISourceGenerator
    {
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

            foreach (var classSymbol in receiver.SpecDeckEntities)
            {
                GenerateSpecifications(context, classSymbol);
            }
        }

        /// <summary>
        /// Generates specifications for the specified class symbol.
        /// </summary>
        /// <param name="context">The generator execution context.</param>
        /// <param name="classSymbol">The class symbol for which to generate specifications.</param>
        private void GenerateSpecifications(GeneratorExecutionContext context, INamedTypeSymbol classSymbol)
        {
            var generatedSpecs = GeneratePropertySpecifications(context, classSymbol);
            GenerateSpecificationsProvider(context, classSymbol, generatedSpecs);
        }

        /// <summary>
        /// Generates property specifications for the specified class symbol.
        /// </summary>
        /// <param name="context">The generator execution context.</param>
        /// <param name="classSymbol">The class symbol for which to generate property specifications.</param>
        /// <returns>A read-only list of generated specification descriptors.</returns>
        private IReadOnlyList<SpecificationDescriptor> GeneratePropertySpecifications(
            GeneratorExecutionContext context,
            INamedTypeSymbol classSymbol)
        {
            var properties = classSymbol
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(x => x.DeclaredAccessibility == Accessibility.Public)
                .ToList();

            var generatedSpecs = new List<SpecificationDescriptor>();
            foreach (var propertySymbol in properties)
            {
                var descriptor = GeneratePropertySpecification(context, classSymbol, propertySymbol);
                generatedSpecs.Add(descriptor);
            }

            return generatedSpecs.AsReadOnly();
        }

        /// <summary>
        /// Generates a property specification for the specified class and property symbols.
        /// </summary>
        /// <param name="context">The generator execution context.</param>
        /// <param name="classSymbol">The class symbol for which to generate the specification.</param>
        /// <param name="propertySymbol">The property symbol for which to generate the specification.</param>
        /// <returns>A descriptor of the generated specification.</returns>
        private SpecificationDescriptor GeneratePropertySpecification(
            GeneratorExecutionContext context,
            INamedTypeSymbol classSymbol,
            IPropertySymbol propertySymbol)
        {
            var sb = new StringBuilder();

            var entityNamespace = classSymbol.ContainingNamespace.ToDisplayString();
            var entityFullName = classSymbol.ToDisplayString();

            sb.AppendLine("using SpecDeck.Core;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine($"using {entityNamespace};");
            sb.AppendLine();
            sb.AppendLine($"namespace {entityNamespace}.Specs.{classSymbol.Name};");
            sb.AppendLine();
            sb.AppendLine(
                $"public class By{propertySymbol.Name}Spec({propertySymbol.Type.ToDisplayString()} v) : Specification<{entityFullName}>");
            sb.AppendLine("{");
            sb.AppendLine($"\tprivate readonly {propertySymbol.Type.ToDisplayString()} _v = v;");
            sb.AppendLine();
            sb.AppendLine($"\tpublic override Expression<Func<{entityFullName}, bool>> ToExpression()");
            sb.AppendLine($"\t\t=> (t => t.{propertySymbol.Name}.Equals(_v));");
            sb.AppendLine("}");

            context.AddSource($"By{propertySymbol.Name}Spec.g.cs", sb.ToString());
            return new SpecificationDescriptor()
            {
                Name = $"By{propertySymbol.Name}Spec",
                Args = new Dictionary<string, string>
                {
                    { "v", propertySymbol.Type.ToDisplayString() }
                }
            };
        }

        /// <summary>
        /// Generates a specifications provider class for the specified class symbol and generated specifications.
        /// </summary>
        /// <param name="context">The generator execution context.</param>
        /// <param name="classSymbol">The class symbol for which to generate the provider.</param>
        /// <param name="generatedSpecs">The list of generated specification descriptors.</param>
        private void GenerateSpecificationsProvider(
            GeneratorExecutionContext context,
            INamedTypeSymbol classSymbol,
            IReadOnlyList<SpecificationDescriptor> generatedSpecs)
        {
            var sb = new StringBuilder();

            var entityFullName = classSymbol.ToDisplayString();
            sb.AppendLine("using SpecDeck.Core;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine();
            var entityNamespace = classSymbol.ContainingNamespace.ToDisplayString();
            sb.AppendLine($"namespace {entityNamespace}.Specs.{classSymbol.Name};");
            sb.AppendLine();
            sb.AppendLine($"public static class {classSymbol.Name}Specs");
            sb.AppendLine("{");
            sb.AppendLine(
                $"\tpublic static Specification<{entityFullName}> All<T>() => Specification<{entityFullName}>.True;");
            sb.AppendLine();

            foreach (var descriptor in generatedSpecs)
            {
                GenerateSpecificationMethod(sb, descriptor);
            }

            sb.AppendLine("}");
            context.AddSource($"{classSymbol.Name}Specs.g.cs", sb.ToString());
        }

        /// <summary>
        /// Generates a method for the specified specification descriptor.
        /// </summary>
        /// <param name="builder">The string builder to which to append the method.</param>
        /// <param name="descriptor">The specification descriptor.</param>
        private void GenerateSpecificationMethod(StringBuilder builder, SpecificationDescriptor descriptor)
        {
            builder.Append($"\tpublic static {descriptor.Name} {descriptor.Name}(");
            foreach (var arg in descriptor.Args)
            {
                builder.Append($"{arg.Value} {arg.Key}");
            }
            builder.Append(") => new(");
            foreach (var arg in descriptor.Args)
            {
                var isLastArg = arg.Key == descriptor.Args.Last().Key;
                var argDefinition = isLastArg ? $"{arg.Key}" : $"{arg.Key}, ";
                builder.Append(argDefinition);
            }
            builder.Append(");");
            builder.AppendLine();
        }
    }
}
