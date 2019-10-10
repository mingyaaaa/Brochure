using System;
using System.Reflection;

namespace LinqDbQuery
{
    public static class TableUtlis
    {
        public static string GetTableName<T> ()
        {
            var type = typeof (T);
            return GetTableName (type);
        }

        public static string GetTableName (Type type)
        {
            if (type == null)
                throw new Exception ("");
            var tableName = type.Name;
            if (type.GetCustomAttribute (typeof (TableAttribute)) is TableAttribute tableAttribute)
                tableName = tableAttribute.Name;
            return tableName;
        }
    }
}