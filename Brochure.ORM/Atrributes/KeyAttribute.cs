using System;

namespace LinqDbQuery.Atrributes
{
    [AttributeUsage (AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class KeyAttribute : Attribute
    {
        public string Name = "";
        public KeyAttribute (string key)
        {
            Name = key;
        }
    }
}