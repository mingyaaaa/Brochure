using System;

namespace Brochure.Core
{
    internal class ConverUtil
    {
        public static IRecord ConverToRecord(object obj)
        {
            if (obj is IRecord)
                return (IRecord)obj;
            return new Record(obj);
        }
        public static object ConverObj(IRecord record, Type type)
        {
            if (type == null || string.IsNullOrWhiteSpace(type.FullName))
                return null;
            var properties = type.GetProperties();
            //var construct = type.GetConstructor(Type.EmptyTypes);
            //if (construct == null)
            //    throw new Exception("类型必须包含无参构造函数");
            //var parameters = construct.GetParameters();
            var obj = type.Assembly.CreateInstance(type.FullName);
            foreach (var item in properties)
            {
                if (item.CanWrite)
                {
                    var value = record[item.Name];
                    var trecord = value as IRecord;
                    if (trecord != null)
                        value = ConverObj(trecord, type);
                    item.SetValue(obj, value);
                }
            }
            return obj;
        }
    }
}
