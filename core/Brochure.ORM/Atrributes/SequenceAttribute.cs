using System;

namespace Brochure.ORM.Atrributes
{
    /// <summary>
    /// The sequence attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SequenceAttribute : Attribute
    { }
}