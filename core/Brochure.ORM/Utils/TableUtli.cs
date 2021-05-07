using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Brochure.ORM
{
    /// <summary>
    /// The table utlis.
    /// </summary>
    public static class TableUtlis
    {
        private static ConcurrentDictionary<string, string> _tableNameCache = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// Gets the table name.
        /// </summary>
        /// <returns>A string.</returns>
        public static string GetTableName<T>()
        {
            var type = typeof(T);
            return GetTableName(type);
        }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A string.</returns>
        public static string GetTableName(Type type)
        {
            if (type == null)
                throw new Exception("");
            var tableName = type.Name;
            if (_tableNameCache.TryGetValue(type.FullName, tableName))
                return tableName;
            if (type.GetCustomAttribute(typeof(TableAttribute)) is TableAttribute tableAttribute)
                tableName = tableAttribute.Name;
            _tableNameCache.TryAdd(type.FullName, tableName);
            return tableName;
        }
    }
}