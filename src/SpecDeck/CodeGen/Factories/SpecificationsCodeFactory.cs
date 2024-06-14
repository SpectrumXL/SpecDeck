using SpecDeck.CodeGen.Models;

namespace SpecDeck.CodeGen.Factories
{
    internal class SpecificationsCodeFactory : ISpecificationFactory
    {
        private readonly ISpecificationFactory _numericsSpecificationFactory = new NumericSpecificationFactory();
        private readonly ISpecificationFactory _defaultSpecificationFactory = new DefaultSpecificationFactory();
        private readonly ISpecificationFactory _stringSpecificationFactory = new StringSpecificationFactory();
        private readonly ISpecificationFactory _dateSpecificationFactory = new DateSpecificationFactory();

        public GeneratedSpecification GetSpecificationDescriptor(SpecificationGenerationTicket ticket,
            EntitySpecsGenerationContext context, EntityPropertyDescriptor propertyDescriptor)
        {
            return ticket.Type switch
            {
                "numeric" => _numericsSpecificationFactory
                    .GetSpecificationDescriptor(ticket, context, propertyDescriptor),
                "date" => _dateSpecificationFactory
                    .GetSpecificationDescriptor(ticket, context, propertyDescriptor),
                "string" => _stringSpecificationFactory
                    .GetSpecificationDescriptor(ticket, context, propertyDescriptor),

                _ => _defaultSpecificationFactory.GetSpecificationDescriptor(ticket, context, propertyDescriptor)
            };
        }
    }
}
