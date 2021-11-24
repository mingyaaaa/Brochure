using System;

namespace Brochure.ORM.Atrributes
{
    [AttributeUsage (AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        public string Name = "";
        public int Length = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ColumnAttribute (string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute"/> class.
        /// </summary>
        public ColumnAttribute () { }
    }
}