using System;

namespace Brochure.ORM.Atrributes
{
    [AttributeUsage (AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class KeyAttribute : Attribute
    {
        public string Name = "";
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyAttribute"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public KeyAttribute (string key)
        {
            Name = key;
        }
    }
}