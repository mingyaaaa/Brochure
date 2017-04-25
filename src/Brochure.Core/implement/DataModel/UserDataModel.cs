using System;

namespace Brochure.Core
{
    public class UserDataModel : IModel
    {
        public string a = "aaaa";
        public IEntrity ConverToDatabase()
        {          
            return new UserDatabase();
        }
        public IView ConverToDataModel()
        {          
            return new UserDataView();
        }
    }
}