namespace Spectrum
{
    /// <summary>
    /// This is a marker attribute used to identify an entity to generate specifications for.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SpectrumSpecAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpectrumSpecAttribute"/> class.
        /// </summary>
        public SpectrumSpecAttribute()
        {
        }
    }
}
