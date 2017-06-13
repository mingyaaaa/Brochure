using System;
using System.Collections.Generic;
using System.Text;
using Brochure.Core.Atrribute;

namespace Brochure.Core.Abstract
{
    public abstract class BaseEntrity : IEntrity
    {
        [Ingore]
        public abstract string TableName { get; }
        public Guid Id { get; set; }
        public abstract IModel ConverToDataModel();
    }
}
