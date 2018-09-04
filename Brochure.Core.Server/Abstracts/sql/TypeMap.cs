using System;
using System.Collections.Generic;

namespace Brochure.Core.Server
{
    public abstract class TypeMap
    {
        protected TypeMap()
        {
            MapDic = new Dictionary<string, string>();
            InitMap();
        }
        protected static IDictionary<string, string> MapDic;
        public abstract void InitMap();
        public string GetSqlType(string type)
        {
            if (MapDic == null)
                InitMap();
            if (MapDic == null || !MapDic.ContainsKey(type))
                throw new InvalidProgramException("无法匹配数据库类型");
            return MapDic[type];
        }
    }
}
