using Brochure.Core;
using Brochure.Core.implement;

namespace ConnectionDal
{
    public class Setting : Singleton, ISetting
    {
        public string ConnectString { get; set; }
    }
}