using Brochure.Core.Server.Abstracts;
using System;

namespace Brochure.Core.Server.SQLMap
{
    public class MySqlTypeMap : TypeMap
    {
        public override void InitMap()
        {
            MapDic.Add(typeof(int).Name, "decimal");
            MapDic.Add(typeof(double).Name, "decimal");
            MapDic.Add(typeof(long).Name, "decimal");
            MapDic.Add(typeof(string).Name, "nvarchar");
            MapDic.Add(typeof(DateTime).Name, "datetime");
            MapDic.Add(typeof(Guid).Name, "nvarchar(36)");
        }
    }
}
