using System;
using System.Reflection;

namespace LinqDbQuery
{
    public static class ReflectedUtli
    {
        public static string GetTableName<T>()
        {
            var type = typeof(T);
            return GetTableName(type);
        }
        public static string GetTableName(Type type)
        {
            if (type == null)
                throw new Exception("");
            var tableName = type.Name;
            var tableAttribute = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            if (tableAttribute != null)
                tableName = tableAttribute.Name;
            return tableName;
        }
    }
}
