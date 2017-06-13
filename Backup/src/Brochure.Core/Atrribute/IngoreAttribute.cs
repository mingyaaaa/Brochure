using System;
using System.Collections.Generic;
using System.Text;

namespace Brochure.Core.Atrribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class IngoreAttribute : Attribute
    {
    }
}
