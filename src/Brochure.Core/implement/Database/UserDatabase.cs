using System;

namespace Brochure.Core
{
    public class UserDatabase : IEntrity
    {
        public string TableName => "user_info";
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public IModel ConverToDataModel()
        {
            throw new NotImplementedException();
        }

    }
}