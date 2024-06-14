using System.Collections.Generic;
using System.Linq;
using SpecDeck.CodeGen.Factories;
using SpecDeck.CodeGen.Models;

namespace SpecDeck.CodeGen
{
    internal class EntitySpecsCodeGenerator
    {
        private readonly SpecificationsCodeFactory _specificationsCodeFactory = new SpecificationsCodeFactory();

        /// <summary>
        /// Generates entity specifications code for the provided context and property descriptors.
        /// </summary>
        /// <param name="context">The generation context containing information about the entity.</param>
        /// <param name="propertyDescriptors">The list of property descriptors for which specifications will be generated.</param>
        /// <returns>A dictionary mapping specification descriptors to the generated code strings.</returns>
        public List<GeneratedSpecification> GetEntitySpecsCode(
            EntitySpecsGenerationContext context, List<EntityPropertyDescriptor> propertyDescriptors)
        {
            var generatedSpecs = new List<GeneratedSpecification>();
            foreach (var descriptor in propertyDescriptors)
            {
                var tickets = SpecificationsTypeResolver.Resolve(descriptor.Type);

                var specs = tickets.Select(ticket => _specificationsCodeFactory
                    .GetSpecificationDescriptor(ticket, context, descriptor)).ToList();
                generatedSpecs.AddRange(specs);
            }

            return generatedSpecs;
        }
    }
}
