using System;
using Brochure.ORM;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlTypeMap : TypeMap
    {
        public override void InitMap()
        {
            MapDic.Add(typeof(int).Name, "decimal");
            MapDic.Add(typeof(double).Name, "decimal");
            MapDic.Add(typeof(float).Name, "decimal");
            MapDic.Add(typeof(long).Name, "decimal");
            MapDic.Add(typeof(string).Name, "nvarchar");
            MapDic.Add(typeof(DateTime).Name, "datetime");
            MapDic.Add(typeof(Guid).Name, "nvarchar");
            //-1代表false 1代表true
            MapDic.Add(typeof(bool).Name, "tinyint");
            MapDic.Add(typeof(byte).Name, "tinyint");
            MapDic.Add(typeof(decimal).Name, "decimal");
        }
    }
}