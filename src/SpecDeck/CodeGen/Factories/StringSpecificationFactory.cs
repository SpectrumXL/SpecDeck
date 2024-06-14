using System.Collections.Generic;
using SpecDeck.CodeGen.Models;

namespace SpecDeck.CodeGen.Factories
{
    internal class StringSpecificationFactory : ISpecificationFactory
    {
        private static readonly Dictionary<string, string> CodeTemplates = new Dictionary<string, string>()
        {
            { "Equals", SpecificationsTemplates.EqualsTemplate },
            { "Contains", SpecificationsTemplates.ContainsTemplate},
            { "EndsWith", SpecificationsTemplates.EndsWithTemplate },
            { "StartsWith", SpecificationsTemplates.StartsWithTemplate }
        };

        public GeneratedSpecification GetSpecificationDescriptor(
            SpecificationGenerationTicket ticket,
            EntitySpecsGenerationContext context,
            EntityPropertyDescriptor propertyDescriptor)
        {
            var codeTemplate = CodeTemplates[ticket.Name];

            var code = string.Format(
                codeTemplate,
                context.EntityNamespace,
                context.EntityName,
                context.EntityFullName,
                propertyDescriptor.Type,
                propertyDescriptor.Name,
                ticket.Operation,
                ticket.Name);

            return new GeneratedSpecification
            {
                Code = code,
                Descriptor = new SpecificationDescriptor
                {
                    Name = $"{propertyDescriptor.Name}{ticket.Name}Spec",
                    Args = new Dictionary<string, string>()
                    {
                        { "v", propertyDescriptor.Type }
                    }
                }
            };
        }
    }
}
