using SpecDeck.CodeGen.Models;

namespace SpecDeck.CodeGen.Factories
{
    internal interface ISpecificationFactory
    {
        GeneratedSpecification GetSpecificationDescriptor(
            SpecificationGenerationTicket ticket,
            EntitySpecsGenerationContext context,
            EntityPropertyDescriptor propertyDescriptor);
    }
}
