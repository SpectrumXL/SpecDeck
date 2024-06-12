using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using SpecDeck.CodeGen.Models;

namespace SpecDeck.CodeGen
{
    internal class EntitySpecsCodeGenerator
    {
        /// <summary>
        /// Generates entity specifications code for the provided context and property descriptors.
        /// </summary>
        /// <param name="context">The generation context containing information about the entity.</param>
        /// <param name="propertyDescriptors">The list of property descriptors for which specifications will be generated.</param>
        /// <returns>A dictionary mapping specification descriptors to the generated code strings.</returns>
        public Dictionary<SpecificationDescriptor, string> GetEntitySpecsCode(
            EntitySpecsGenerationContext context, List<EntityPropertyDescriptor> propertyDescriptors)
        {
            var generatedSpecs = new Dictionary<SpecificationDescriptor, string>();
            foreach (var descriptor in propertyDescriptors)
            {
                var res = GeneratePropertySpecification(context, descriptor);
                generatedSpecs.Add(res.Item1, res.Item2);
            }

            return generatedSpecs;
        }

        /// <summary>
        /// Generates a property specification for the specified entity context and property descriptor.
        /// </summary>
        /// <param name="entityContext">The entity context containing information about the entity.</param>
        /// <param name="propertyDescriptor">The property descriptor for which to generate the specification.</param>
        /// <returns>A tuple containing the specification descriptor and the generated code string.</returns>
        private (SpecificationDescriptor, string) GeneratePropertySpecification(
            EntitySpecsGenerationContext entityContext,
            EntityPropertyDescriptor propertyDescriptor)
        {
            var sb = new StringBuilder();

            var entityNamespace = entityContext.EntityNamespace;
            var entityFullName = entityContext.EntityFullName;
            var entityName = entityContext.EntityName;

            var propertyName = propertyDescriptor.Name;
            var propertyType = propertyDescriptor.Type;

            sb.AppendLine("using SpecDeck.Core;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine($"using {entityNamespace};");
            sb.AppendLine();
            sb.AppendLine($"namespace {entityNamespace}.Specs.{entityName};");
            sb.AppendLine();
            sb.AppendLine(
                $"public class By{propertyName}Spec({propertyType} v) : Specification<{entityFullName}>");
            sb.AppendLine("{");
            sb.AppendLine($"\tprivate readonly {propertyType} _v = v;");
            sb.AppendLine();
            sb.AppendLine($"\tpublic override Expression<Func<{entityFullName}, bool>> ToExpression()");
            sb.AppendLine($"\t\t=> (t => t.{propertyName}.Equals(_v));");
            sb.AppendLine("}");

            return (new SpecificationDescriptor
            {
                Name = $"By{propertyDescriptor.Name}Spec",
                Args = new Dictionary<string, string>
                {
                    { "v", propertyType }
                }
            }, sb.ToString());
        }
    }
}
