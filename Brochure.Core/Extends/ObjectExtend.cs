using System;
using System.Collections.Generic;
using System.Reflection;

namespace Brochure.Core
{
    public static class ObjectExtend
    {
        /// <summary>
        /// 类型转换  
        /// 转换不成功 回返回默认值 
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="exc">异常，如果部位null 则会throw 异常</param>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns></returns>
        public static T As<T>(this object obj, Exception exc = null)
        {

            if (obj == null)
                return (T)(object)default(T);
            var type = typeof(T);
            var objType = obj.GetType();
            if (type == objType || obj is T)
                return (T)obj;
            //如果是枚举类型 着转化为整形
            if (obj is Enum)
                obj = (int)obj;
            if (type.BaseType == typeof(Enum))
                return (T)Enum.Parse(type, obj.ToString());
            if (type == typeof(IRecord) || type == typeof(Record))
                return (T)ConverUtil.ConverToRecord(obj);
            if (obj is IRecord)
            {
                var record = (IRecord)obj;
                return (T)ConverUtil.ConverObj(record, type);
            }
            try
            {
                //如果是基本类型 使用基本类型转换
                if (type.Name == BaseType.String)
                    return (T)(object)obj.ToString();
                else if (type.Name == BaseType.Int)
                    return (T)(object)int.Parse(obj.ToString());
                else if (type.Name == BaseType.Double)
                    return (T)(object)double.Parse(obj.ToString());
                else if (type.Name == BaseType.Float)
                    return (T)(object)float.Parse(obj.ToString());
                else if (type.Name == BaseType.DateTime)
                    return (T)(object)DateTime.Parse(obj.ToString());
                //实现接口IBConverable的类使用 接口转换器
                var interfaceType = objType.GetInterface($"IBConverables`1");
                var ibConver = obj as IBConverables;
                if (ibConver != null)
                    return ibConver.Conver<T>();
                //如果是系统其他类型  则使用系统的转换器
                return (T)(object)Convert.ChangeType(obj, type);
            }
            catch (Exception)
            {
                if (exc != null)
                    throw exc;
                return (T)(object)default(T);
            }
        }
        public static IDictionary<string, object> AsDictionary(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj", "参数为空");
            if (obj is IDictionary<string, object>)
                return (IDictionary<string, object>)obj;
            var result = new Dictionary<string, object>();
            if (obj is IRecord)
            {
                var record = (IRecord)obj;
                foreach (var keys in record.Keys)
                {
                    result[keys] = record[keys];
                }
                return result;
            }
            var type = obj.GetType();
            var typeInfo = type.GetTypeInfo();
            var props = typeInfo.GetRuntimeProperties();
            foreach (var item in props)
            {
                var attribute = item.GetCustomAttribute(typeof(IngoreAttribute), true);
                if (attribute != null)
                    continue;
                result.Add(item.Name, item.GetValue(obj));
            }
            return result;
        }
    }
}
