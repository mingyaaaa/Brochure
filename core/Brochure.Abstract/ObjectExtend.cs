using Brochure.Abstract.Models;
using Mapster;
using System.Reflection;

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
        public static T As<T>(this object obj)
        {
            if (obj is T t)
                return t;
            return (T)As(obj, typeof(T));
        }

        /// <summary>
        /// As the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>A T.</returns>
        public static T AsDefault<T>(this object obj, T defaultValue)
        {
            try
            {
                return As<T>(obj);
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
            var objType = obj.GetType();
            if (type.IsAssignableFrom(objType))
                return obj;
            if (type.IsEnum)
                return Enum.Parse(type, obj.ToString());
            if (typeof(IRecord).IsAssignableFrom(type))
                return (object)new Record(obj.Adapt<IDictionary<string, object>>());
            return obj.Adapt(objType, type);
        }
    }
}