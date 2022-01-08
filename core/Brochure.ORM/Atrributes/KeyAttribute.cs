using System;

namespace Brochure.ORM.Atrributes
{
    /// <summary>
    /// The key attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class KeyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyAttribute"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public KeyAttribute(string key)
        {
            Name = key;
        }
    }
}