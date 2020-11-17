using System;
using System.Collections.Concurrent;
using System.Linq;
using Brochure.Utils;

namespace Brochure.Core
{
    /// <summary>
    /// 默认使用的是jsonConver
    /// </summary>
    public class PropertyDelegate<T1, T2> where T1 : class where T2 : class
    {
        private static readonly ConcurrentDictionary<string, Action<T2, object>> setPropertyFunCache = new ConcurrentDictionary<string, Action<T2, object>> ();
        private static readonly ConcurrentDictionary<string, Func<T1, object>> getPropertyFunCache = new ConcurrentDictionary<string, Func<T1, object>> ();
        private Type t1Type;
        private Type t2Type;
        public PropertyDelegate ()
        {
            t1Type = typeof (T1);
            t2Type = typeof (T2);
        }
        public T2 ConverTo (T1 model)
        {
            var properties = t1Type.GetProperties ();
            var t2Properties = t2Type.GetProperties ();
            var t2 = ReflectorUtil.Instance.CreateInstance<T2> ();
            foreach (var item in properties)
            {
                var propertyType = item.PropertyType;
                object value = null;
                var key = propertyType.FullName + item.Name;
                if (!t2Properties.Any (t => t.Name == item.Name))
                {
                    continue;
                }
                if (getPropertyFunCache.ContainsKey (key))
                {
                    value = getPropertyFunCache[key].Invoke (model);
                }
                else
                {
                    var func = ReflectorUtil.Instance.GetPropertyValueFun<T1> (item.Name);
                    value = func.Invoke (model);
                    getPropertyFunCache.TryAdd (key, func);
                }
                if (setPropertyFunCache.ContainsKey (key))
                {
                    setPropertyFunCache[key].Invoke (t2, value);
                }
                else
                {
                    var action = ReflectorUtil.Instance.GetSetPropertyValueFun<T2> (propertyType, item.Name);
                    action.Invoke (t2, value);
                    setPropertyFunCache.TryAdd (key, action);
                }
            }
            return t2;
        }
    }

}