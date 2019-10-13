using System;
using System.Reflection;

namespace Brochure.Core
{
    internal static class ConverUtil
    {
        public static IRecord ConverToRecord (object obj)
        {
            if (obj is IRecord record2)
                return record2;
            var type = obj.GetType ();
            var objStr = obj.ToString ();
            if (type.IsClass && type.Name != nameof (TypeCode.String))
                return new Record (obj);
            return objStr.AsObject<Record> ();
        }

        public static object ConverObj (IRecord record, Type type)
        {
            if (type == typeof (string))
                return record.ToString ();
            if (!type.IsClass)
                return null;
            if (type == null || string.IsNullOrWhiteSpace (type.FullName))
                return null;
            var properties = type.GetProperties ();
            //var construct = type.GetConstructor(Type.EmptyTypes);
            //if (construct == null)
            //    throw new Exception("类型必须包含无参构造函数");
            //var parameters = construct.GetParameters();
            var obj = type.Assembly.CreateInstance (type.FullName);
            foreach (var item in properties)
            {
                if (item.CanWrite)
                {
                    var value = record[item.Name];
                    if (value is IRecord trecord)
                        value = ConverObj (trecord, item.PropertyType);
                    value = As (value, item.PropertyType);
                    item.SetValue (obj, value);
                }
            }
            return obj;
        }

        internal static object As (object obj, Type type)
        {
            if (obj == null)
                return (object) null;
            if (obj is Enum)
                obj = (int) obj;
            if (type == typeof (Guid))
                return Guid.Parse (obj.ToString ());
            //如果是基本类型 使用基本类型转换
            {
                if (type.Name == nameof (TypeCode.String))
                    return obj.ToString ();
                else if (type.Name == nameof (TypeCode.Int32))
                    return int.Parse (obj.ToString ());
                else if (type.Name == nameof (TypeCode.Double))
                    return double.Parse (obj.ToString ());
                else if (type.Name == nameof (TypeCode.Single))
                    return float.Parse (obj.ToString ());
                else if (type.Name == nameof (TypeCode.DateTime))
                    return DateTime.Parse (obj.ToString ());
                else if (type.BaseType == typeof (Enum))
                    return Enum.Parse (type, obj.ToString ());
            }
            //IRecord 类型转换
            {
                if (type == typeof (IRecord) || type == typeof (Record))
                    return ConverToRecord (obj);
                if (obj is IRecord record)
                {
                    return ConverObj (record, type);
                }
            }
            //如果是系统其他类型  则使用系统的转换器
            return (object) Convert.ChangeType (obj, type);
        }
    }
}