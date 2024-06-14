using System.Collections.Generic;
using SpecDeck.CodeGen.Models;

namespace SpecDeck.CodeGen.Factories
{
    internal class NumericSpecificationFactory : ISpecificationFactory
    {
        private static readonly Dictionary<string, string> CodeTemplates = new Dictionary<string, string>()
        {
            { "Default", SpecificationsTemplates.GeneralTemplate },
            { "InRange", SpecificationsTemplates.InRangeTemplate }
        };

        public GeneratedSpecification GetSpecificationDescriptor(
            SpecificationGenerationTicket ticket,
            EntitySpecsGenerationContext context,
            EntityPropertyDescriptor propertyDescriptor)
        {
            var codeTemplateKey = ticket.Name == "InRange" ? "InRange" : "Default";
            var codeTemplate = CodeTemplates[codeTemplateKey];

            var code = string.Format(
                codeTemplate,
                context.EntityNamespace,
                context.EntityName,
                context.EntityFullName,
                propertyDescriptor.Type,
                propertyDescriptor.Name,
                ticket.Operation,
                ticket.Name);

            var specArgs = GetSpecificationArgs(ticket, propertyDescriptor);

            return new GeneratedSpecification
            {
                Code = code,
                Descriptor = new SpecificationDescriptor
                {
                    Name = $"{propertyDescriptor.Name}{ticket.Name}Spec",
                    Args = specArgs
                }
            };
        }

        private static Dictionary<string, string> GetSpecificationArgs(
            SpecificationGenerationTicket ticket,
            EntityPropertyDescriptor propertyDescriptor)
        {
            var specArgs = new Dictionary<string, string>();

            if (ticket.Name == "InRange")
            {
                specArgs.Add("min", propertyDescriptor.Type);
                specArgs.Add("max", propertyDescriptor.Type);
            }
            else
            {
                specArgs.Add("v", propertyDescriptor.Type);
            }

            return specArgs;
        }
    }
}
