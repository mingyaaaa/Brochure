using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Brochure.Abstract;
using Brochure.Utils;

namespace Brochure.Core
{
    /// <summary>
    /// 默认使用的是jsonConver
    /// </summary>
    public class DefaultConverPolicy<T1, T2> : IConverPolicy<T1, T2> where T1 : class where T2 : class
    {
        private static ConcurrentDictionary<Type, Action<T2, object>> setPropertyFunCache;
        private static ConcurrentDictionary<Type, Func<T1, object>> getPropertyFunCache;
        public DefaultConverPolicy ()
        {
            setPropertyFunCache = new ConcurrentDictionary<Type, Action<T2, object>> ();
            getPropertyFunCache = new ConcurrentDictionary<Type, Func<T1, object>> ();
        }
        public T2 ConverTo (T1 model)
        {
            var properties = model.GetType ().GetProperties ();
            var t2 = ReflectorUtil.Instance.CreateInstance<T2> ();
            foreach (var item in properties)
            {
                var propertyType = item.PropertyType;
                object value = null;
                if (getPropertyFunCache.ContainsKey (propertyType))
                {
                    value = getPropertyFunCache[propertyType].Invoke (model);
                }
                else
                {
                    var func = ReflectorUtil.Instance.GetPropertyValueFun<T1> (item.Name);
                    value = func.Invoke (model);
                    getPropertyFunCache.TryAdd (propertyType, func);
                }
                if (setPropertyFunCache.ContainsKey (propertyType))
                {
                    setPropertyFunCache[propertyType].Invoke (t2, value);
                }
                else
                {
                    var action = ReflectorUtil.Instance.GetSetPropertyValueFun<T2> (propertyType, item.Name);
                    action.Invoke (t2, value);
                    setPropertyFunCache.TryAdd (propertyType, action);
                }
            }
            return t2;
        }
    }
}