using System;

namespace Brochure.ORM.Atrributes
{
    /// <summary>
    /// The column attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public int Length { get; set; } = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ColumnAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnAttribute"/> class.
        /// </summary>
        public ColumnAttribute()
        { }
    }
}