using System;

namespace Brochure.ORM.Atrributes
{
    [AttributeUsage (AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        public string Name = "";
        public int Length = -1;

        public ColumnAttribute (string name)
        {
            this.Name = name;
        }

        public ColumnAttribute () { }
    }
}