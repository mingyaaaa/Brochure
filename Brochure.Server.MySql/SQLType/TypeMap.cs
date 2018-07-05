using System;
using System.Collections.Generic;
using System.Reflection;
using Brochure.Core.Interfaces;
using Brochure.Core.Server.Abstracts;

namespace Brochure.Core.Server.SQLMap
{
    public class MySqlTypeMap : AbTypeMap
    {
        public override void InitMap ()
        {
            MapDic = new Dictionary<string, string> ();
            MapDic.Add (typeof (int).Name, "decimal");
            MapDic.Add (typeof (double).Name, "decimal");
            MapDic.Add (typeof (string).Name, "nvarchar");
            MapDic.Add (typeof (DateTime).Name, "datetime");
            MapDic.Add (typeof (Guid).Name, "nvarchar(36)");
        }
    }
}