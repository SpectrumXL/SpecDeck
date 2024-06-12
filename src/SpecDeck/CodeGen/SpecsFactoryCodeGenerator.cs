using System.Text;
using System.Collections.Generic;
using System.Linq;
using SpecDeck.CodeGen.Models;

namespace SpecDeck.CodeGen
{
    /// <summary>
    /// This class generates the code for the entity specifications factory.
    /// </summary>
    internal class SpecsFactoryCodeGenerator
    {
        /// <summary>
        /// Generates a specification provider class for the specified entity context and generated specifications.
        /// </summary>
        /// <param name="context">The entity specifications factory generation context.</param>
        /// <param name="generatedSpecs">The list of generated specification descriptors.</param>
        /// <returns>The generated code for the specifications factory as a string.</returns>
        public string GenerateSpecificationsFactory(
            EntitySpecsGenerationContext context,
            IReadOnlyList<SpecificationDescriptor> generatedSpecs)
        {
            var entityName = context.EntityName;
            var entityFullName = context.EntityFullName;
            var entityNamespace = context.EntityNamespace;

            var sb = new StringBuilder();

            sb.AppendLine("using SpecDeck.Core;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine();
            sb.AppendLine($"namespace {entityNamespace}.Specs.{entityName};");
            sb.AppendLine();
            sb.AppendLine($"public static class {entityName}Specs");
            sb.AppendLine("{");
            sb.AppendLine(
                $"\tpublic static Specification<{entityFullName}> All<T>() => Specification<{entityFullName}>.True;");
            sb.AppendLine();

            foreach (var descriptor in generatedSpecs)
            {
                GenerateSpecificationMethod(sb, descriptor);
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        /// <summary>
        /// Generates a method for the specified specification descriptor.
        /// </summary>
        /// <param name="builder">The string builder to which to append the method.</param>
        /// <param name="descriptor">The specification descriptor.</param>
        private void GenerateSpecificationMethod(StringBuilder builder, SpecificationDescriptor descriptor)
        {
            var args = descriptor.Args ?? Constants.NoArgs;

            builder.Append($"\tpublic static {descriptor.Name} {descriptor.Name}(");
            foreach (var arg in args)
            {
                builder.Append($"{arg.Value} {arg.Key}");
            }

            builder.Append(") => new(");
            for (var i = 0; i < args.Count; i++)
            {
                builder.Append(args.ElementAt(i).Key);
                var isLast = i == args.Count - 1;
                if (!isLast) builder.Append(", ");
            }

            builder.Append(");");
            builder.AppendLine();
        }
    }
}
