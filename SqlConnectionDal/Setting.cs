using System;
using System.Collections.Generic;
using System.Text;
using Brochure.Core;
using Brochure.Core.implement;
using Microsoft.Extensions.Configuration;

namespace ConnectionDal
{
    public class Setting : Singleton<Setting>, ISetting
    {
        public string ConnectString { get; set; }
#if Sql
        public string PreParamString => "@";
#elif Oracle
        public string PreParamString => "@";
#elif MySql
        public string PreParamString => "@";
#endif
    }
}
