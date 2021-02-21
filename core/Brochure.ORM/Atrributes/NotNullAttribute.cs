using System;

namespace Brochure.ORM.Atrributes
{

    [AttributeUsage (AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NotNullAttribute : Attribute { }
}