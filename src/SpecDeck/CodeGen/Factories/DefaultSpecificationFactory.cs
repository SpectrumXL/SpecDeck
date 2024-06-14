using System.Collections.Generic;
using SpecDeck.CodeGen.Models;

namespace SpecDeck.CodeGen.Factories
{
    internal class DefaultSpecificationFactory : ISpecificationFactory
    {
        public GeneratedSpecification GetSpecificationDescriptor(
            SpecificationGenerationTicket ticket,
            EntitySpecsGenerationContext context,
            EntityPropertyDescriptor propertyDescriptor)
        {
            var code = string.Format(
                SpecificationsTemplates.EqualsTemplate,
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
