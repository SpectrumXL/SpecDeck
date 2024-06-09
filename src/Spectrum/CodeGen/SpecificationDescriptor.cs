namespace Spectrum.CodeGen
{
    /// <summary>
    /// Descriptor for generated specifications.
    /// </summary>
    internal class SpecificationDescriptor
    {
        /// <summary>
        /// Gets or sets the name of the specification.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the arguments for the specification.
        /// </summary>
        public Dictionary<string, string> Args { get; set; }
    }
}
