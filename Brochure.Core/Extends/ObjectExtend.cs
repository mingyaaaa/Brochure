using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

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
        public static T As<T> (this object obj, bool isException = true)
        {
            // var logger = DI.ServiceProvider.GetService<ILoggerFactory>().CreateLogger("As");
            try
            {
                if (obj is T)
                    return (T) obj;
                var type = typeof (T);
                //实现接口IBConverable的类使用 接口转换器
                var interfaceType = obj.GetType ().GetInterface ($"IBConverables`1");
                if (interfaceType != null)
                {
                    if (obj is IBConverables<T> ibConver)
                        return ibConver.Conver ();
                }
                return (T) ConverUtil.As (obj, type);
            }
            catch (System.Exception e)
            {
                if (isException)
                    throw e;
                //   logger.LogError(e, e.Message);
                return (T) (object) default (T);
            }
        }

        public static IEnumerable<T> AsEnumerable<T> (this object obj, bool isException = true)
        {
            if (obj == null)
                return new List<T> ();
            if (obj is IEnumerable<T>)
                return (IEnumerable<T>) obj;
            var type = obj.GetType ();
            if (type.Name == BaseType.String)
            {
                return obj.ToString ().AsEnumerable<T> ();
            }
            if (isException)
                throw new Exception ($"{type.FullName}不是集合类型");
            return new List<T> ();
        }

        public static IDictionary<string, object> AsDictionary (this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException ("obj", "参数为空");
            if (obj is IDictionary<string, object>)
                return (IDictionary<string, object>) obj;
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
    }
}