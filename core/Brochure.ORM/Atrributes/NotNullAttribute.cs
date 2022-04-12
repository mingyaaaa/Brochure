using System;

namespace Brochure.ORM.Atrributes
{
    /// <summary>
    /// The not null attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NotNullAttribute : Attribute
    { }
}