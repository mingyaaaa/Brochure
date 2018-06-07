using System;

namespace Brochure.Core.Atrributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class IngoreAttribute : Attribute
    {
    }
}