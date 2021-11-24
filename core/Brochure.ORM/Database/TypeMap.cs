using System;
using System.Collections.Generic;

namespace Brochure.ORM
{
    public abstract class TypeMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMap"/> class.
        /// </summary>
        protected TypeMap ()
        {
            MapDic = new Dictionary<string, string> ();
            InitMap ();
        }

        protected static IDictionary<string, string> MapDic;
        /// <summary>
        /// Inits the map.
        /// </summary>
        public abstract void InitMap ();

        /// <summary>
        /// Gets the sql type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A string.</returns>
        public string GetSqlType (string type)
        {
            if (MapDic == null)
                InitMap ();
            if (MapDic?.ContainsKey (type) != true)
                throw new InvalidProgramException ("无法匹配数据库类型");
            return MapDic[type];
        }
    }
}