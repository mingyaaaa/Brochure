using System;
using System.Reflection;
using System.Collections.Generic;
using Brochure.Core.Server.Abstracts;
using Brochure.Core.Interfaces;

namespace Brochure.Core.Server.SQLMap
{
    public class SqlTypeMap : AbTypeMap
    {
        public override void InitMap()
        {
            MapDic = new Dictionary<string, string>();
            MapDic.Add(typeof(int).Name, "sqlint");
            MapDic.Add(typeof(double).Name, "sqldouble");
            MapDic.Add(typeof(string).Name, "sqlstring");
            MapDic.Add(typeof(DateTime).Name, "sqlDateTime");
            MapDic.Add(typeof(Guid).Name, "sqlGuid");
        }
    }
    public class OracleTypeMap : AbTypeMap
    {
        public override void InitMap()
        {
            MapDic = new Dictionary<string, string>();
            MapDic.Add(typeof(int).Name, "oracleint");
            MapDic.Add(typeof(double).Name, "oracledouble");
            MapDic.Add(typeof(string).Name, "oraclestring");
            MapDic.Add(typeof(DateTime).Name, "oracleDateTime");
            MapDic.Add(typeof(Guid).Name, "oracleGuid");
        }
    }
}