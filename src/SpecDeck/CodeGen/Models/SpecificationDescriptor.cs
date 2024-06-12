using System.Collections.Generic;

namespace SpecDeck.CodeGen.Models
{
    /// <summary>
    /// Descriptor for generated specifications.
    /// </summary>
    internal class SpecificationDescriptor
    {
        /// <summary>
        /// Gets or sets the name of the specification.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the arguments for the specification.
        /// </summary>
        public Dictionary<string, string>? Args { get; set; }
    }
}
