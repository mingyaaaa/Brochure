using System;
using System.Collections.Generic;
using Brochure.Core.Abstracts;

namespace Brochure.Core.Server.Abstracts
{
    public abstract class ATypeMap : Singleton
    {
        protected ATypeMap ()
        {
            InitMap ();
        }
        protected static IDictionary<string, string> MapDic;
        public abstract void InitMap ();
        public string GetSqlType (string type)
        {
            if (MapDic == null)
                InitMap ();
            if (MapDic == null || !MapDic.ContainsKey (type))
                throw new InvalidProgramException ("无法匹配数据库类型");
            return MapDic[type];
        }
    }
}