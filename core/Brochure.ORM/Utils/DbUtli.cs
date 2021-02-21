using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Brochure.ORM
{
    public static class DbUtlis
    {
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