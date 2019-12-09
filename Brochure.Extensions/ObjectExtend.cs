using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Brochure.Abstract;
namespace Brochure.Extensions
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
        public static T As<T> (this object obj, bool isException = true)
        {
            try
            {
                if (obj is T t)
                    return t;
                var type = typeof (T);
                return (T) As (obj, type);
            }
            catch (System.Exception) when (!isException)
            {
                return (T) (object) default (T);
            }
        }

        public static IDictionary<string, object> AsDictionary (this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException (nameof (obj), "参数为空");
            if (obj is IDictionary<string, object> dictionary)
                return dictionary;
            var result = new Dictionary<string, object> ();
            if (obj is IRecord record)
            {
                foreach (var keys in record.Keys)
                {
                    result[keys] = record[keys];
                }
                return result;
            }
            var type = obj.GetType ();
            var typeInfo = type.GetTypeInfo ();
            var props = typeInfo.GetRuntimeProperties ();
            foreach (var item in props)
            {
                var attribute = item.GetCustomAttribute (typeof (IngoreAttribute), true);
                if (attribute != null)
                    continue;
                result.Add (item.Name, item.GetValue (obj));
            }
            return result;
        }

        /// <summary>
        /// 可以通过 ObjectConverCollection 注册需要转换的类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object As (object obj, Type type)
        {
            //处理IObjectConver
            if (ObjectConverCollection.TryGetConverFunc (type, out var fun))
            {
                return fun (obj);
            }
            //如果是系统其他类型  则使用系统的转换器
            return (object) Convert.ChangeType (obj, type);
        }
    }
}