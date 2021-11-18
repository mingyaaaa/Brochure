using Brochure.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core
{
    /// <summary>
    /// The property delegate cache.
    /// </summary>
    public static class PropertyGetDelegateCache<T> where T : class
    {
        private static readonly ConcurrentDictionary<string, Func<T, object>> getPropertyFunCache = new ConcurrentDictionary<string, Func<T, object>>();

        /// <summary>
        /// Gets the get action.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A Func.</returns>
        public static bool TryGetGetAction(string key, out Func<T, object> func)
        {
            func = null;
            if (getPropertyFunCache.ContainsKey(key))
            {
                func = getPropertyFunCache[key];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the get action.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="func">The func.</param>
        public static void AddGetAction(string key, Func<T, object> func)
        {
            getPropertyFunCache.TryAdd(key, func);
        }

        /// <summary>
        /// Tries the invoke.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>An object? .</returns>
        public static object TryGet(PropertyInfo propertyInfo, T obj)
        {
            var key = propertyInfo.PropertyType.FullName + propertyInfo.Name;
            if (TryGetGetAction(key, out Func<T, object> func))
            {
                return func.Invoke(obj);
            }
            else
            {
                var t_func = ReflectorUtil.Instance.GetPropertyValueFun<T>(propertyInfo.Name);
                AddGetAction(key, t_func);
                return t_func(obj);
            }
        }
    }

    /// <summary>
    /// The property set delegate cache.
    /// </summary>
    public static class PropertySetDelegateCache<T>
    {
        private static readonly ConcurrentDictionary<string, Action<T, object>> setPropertyFunCache = new ConcurrentDictionary<string, Action<T, object>>();

        /// <summary>
        /// Adds the set action.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="action">The action.</param>
        public static void AddSetAction(string key, Action<T, object> action)
        {
            setPropertyFunCache.TryAdd(key, action);
        }

        /// <summary>
        /// Gets the set action.
        /// </summary>
        /// <param name="key">The property name.</param>
        /// <returns>An Action.</returns>
        public static bool TryGetSetAction(string key, out Action<T, object> action)
        {
            action = null;
            if (setPropertyFunCache.ContainsKey(key))
            {
                action = setPropertyFunCache[key];
                return true;
            }
            return false;
        }

        public static void TrySet(PropertyInfo propertyInfo, T obj, object value)
        {
            var key = propertyInfo.PropertyType.FullName + propertyInfo.Name;
            if (TryGetSetAction(key, out var action))
            {
                action?.Invoke(obj, value);
            }
            else
            {
                var t_action = ReflectorUtil.Instance.GetSetPropertyValueFun<T>(propertyInfo.PropertyType, propertyInfo.Name);
                t_action.Invoke(obj, value);
                AddSetAction(key, t_action);
            }
        }
    }
}