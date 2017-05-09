using System;
using Brochure.Core.Abstract;
using Brochure.Core.Atrribute;

namespace Brochure.Core
{
    public class UserDatabase : BaseEntrity
    {
        public override string TableName => "user_info";
        public string Name { get; set; }
        public int Age { get; set; }
        public override IModel ConverToDataModel()
        {
            throw new NotImplementedException();
        }

    }
}