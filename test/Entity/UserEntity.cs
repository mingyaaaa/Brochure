using System;
using System.Collections.Generic;
using System.Text;
using Brochure.Core;
using Brochure.Core.Abstract;

namespace test.Entity
{
    public class UserEntity : BaseEntrity
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
