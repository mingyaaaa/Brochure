using System;

namespace LinqDbQuery.Atrributes
{

    [AttributeUsage (AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NotNullAttribute : Attribute { }
}