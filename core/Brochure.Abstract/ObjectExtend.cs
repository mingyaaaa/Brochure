using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Brochure.Abstract;

namespace Brochure.Abstract.Extensions
{
    /// <summary>
    /// The object extend.
    /// </summary>
    public static class ObjectExtend
    {
        /// <summary>
        /// 类型转换
        /// 转换不成功 回返回默认值
        /// </summary>
        /// <param name="obj">转化对象</param>
        /// <param name="isException"></param>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns></returns>
        public static T As<T>(this object obj, bool isException = true)
        {
            try
            {
                if (obj is T t)
                    return t;
                var type = typeof(T);
                return (T)obj.As(type);
            }
            catch (System.Exception) when (!isException)
            {
                return (T)(object)default(T);
            }
        }

        /// <summary>
        /// As the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>A T.</returns>
        public static T As<T>(this object obj, T defaultValue)
        {
            try
            {
                return As<T>(obj, false);
            }
            catch (System.Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 可以通过 ObjectConverCollection 注册需要转换的类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object As(this object obj, Type type)
        {
            if (type.IsAssignableFrom(obj.GetType()))
            {
                return obj;
            }
            if (type.IsEnum)
            {
                return Enum.Parse(type, obj.ToString());
            }
            //处理IObjectConver
            if (ObjectConverCollection.TryGetConverFunc(type, out var fun))
            {
                return fun(obj);
            }
            //如果是系统其他类型  则使用系统的转换器
            return Convert.ChangeType(obj, type);
        }

        /// <summary>
        /// As the dictionary.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An IDictionary.</returns>
        public static IDictionary<string, object> AsDictionary(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "参数为空");
            if (obj is IDictionary<string, object> dictionary)
                return dictionary;
            var result = new Dictionary<string, object>();
            if (obj is IRecord record)
            {
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