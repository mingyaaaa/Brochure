using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Brochure.ORM
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

        public static T ConverFromIDataReader<T> (IDataReader reader)
        {
            var obj = Activator.CreateInstance<T> ();
            PropertyInfo[] propertys = obj.GetType ().GetProperties ();
            List<string> fieldNameList = new List<string> ();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                fieldNameList.Add (reader.GetName (i));
            }
            foreach (PropertyInfo property in propertys)
            {
                if (!property.CanWrite)
                    continue;
                string fieldName = property.Name;
                if (fieldNameList.Contains (fieldName))
                {
                    object value = reader[fieldName];
                    if (value is DBNull)
                        continue;
                    try
                    {
                        property.SetValue (obj, value);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return obj;
        }
    }
}