using System;
using Brochure.Core.Enums;

namespace Brochure.Core.Atrribute

{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ModeLAttribute : Attribute
    {
        private AtrributeType _type;

        public ModeLAttribute(AtrributeType type)
        {
            _type = type;
        }

        public AtrributeType Type { get => _type; set => _type = value; }
    }
}