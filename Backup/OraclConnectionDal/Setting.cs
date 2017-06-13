using System;
using System.Collections.Generic;
using System.Text;
using Brochure.Core;
using Brochure.Core.implement;
using Microsoft.Extensions.Configuration;

namespace ConnectionDal
{
    public class Setting : Singleton, ISetting
    {
        public string ConnectString { get; set; }
    }
}
