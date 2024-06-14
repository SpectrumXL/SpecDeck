using System.Collections.Generic;

namespace SpecDeck.CodeGen.Models
{
    internal class EntitySpecsGenerationContext
    {
        /// <summary>
        /// Represents the name of the entity with the namespace.
        /// Fullname space required to avoid conflicts with Specs namespace.
        /// </summary>
        public string EntityFullName { get; set; } = null!;

        /// <summary>
        ///
        /// </summary>
        public string EntityName { get; set; } = null!;

        /// <summary>
        ///
        /// </summary>
        public string EntityNamespace { get; set; } = null!;

        /// <summary>
        ///
        /// </summary>
        public List<SpecificationDescriptor> GeneratedSpecs { get; set; } = new List<SpecificationDescriptor>();
    }
}
