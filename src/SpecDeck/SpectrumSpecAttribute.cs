using System;

namespace SpecDeck
{
    /// <summary>
    /// This is a marker attribute used to identify an entity to generate specifications for.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SpecDeckSpecAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecDeckSpecAttribute"/> class.
        /// </summary>
        public SpecDeckSpecAttribute()
        {
        }
    }
}
