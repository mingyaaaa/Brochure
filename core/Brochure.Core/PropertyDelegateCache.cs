using Brochure.Utils;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Brochure.Core
{
    /// <summary>
    /// The property delegate cache.
    /// </summary>
    public static class PropertyGetDelegateCache
    {
        private static readonly ConcurrentDictionary<string, Delegate> getPropertyFunCache = new ConcurrentDictionary<string, Delegate>();

        /// <summary>
        /// Gets the get action.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns>A Func.</returns>
        public static bool TryGetGetAction<T>(string key, out Func<T, object> func) where T : class
        {
            func = null;
            if (getPropertyFunCache.ContainsKey(key))
            {
                func = (Func<T, object>)getPropertyFunCache[key];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the get action.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="func">The func.</param>
        public static void AddGetAction<T>(string key, Func<T, object> func) where T : class
        {
            getPropertyFunCache.TryAdd(key, func);
        }

        /// <summary>
        /// Tries the invoke.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="obj">The obj.</param>
        /// <returns>An object? .</returns>
        public static object TryGet<T>(PropertyInfo propertyInfo, T obj) where T : class
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

    //public static class PropertyGetDelegateCache
    //{
    //    private static readonly ConcurrentDictionary<string, Func<object, object>> getPropertyFunCache = new ConcurrentDictionary<string, Func<object, object>>();

    //    /// <summary>
    //    /// Gets the get action.
    //    /// </summary>
    //    /// <param name="propertyName">The property name.</param>
    //    /// <returns>A Func.</returns>
    //    public static bool TryGetGetAction(string key, out Func<object, object> func)
    //    {
    //        func = null;
    //        if (getPropertyFunCache.ContainsKey(key))
    //        {
    //            func = getPropertyFunCache[key];
    //            return true;
    //        }
    //        return false;
    //    }

    //    /// <summary>
    //    /// Adds the get action.
    //    /// </summary>
    //    /// <param name="key">The key.</param>
    //    /// <param name="func">The func.</param>
    //    public static void AddGetAction(string key, Func<object, object> func)
    //    {
    //        getPropertyFunCache.TryAdd(key, func);
    //    }

    //    /// <summary>
    //    /// Tries the invoke.
    //    /// </summary>
    //    /// <param name="propertyName">The property name.</param>
    //    /// <param name="obj">The obj.</param>
    //    /// <returns>An object? .</returns>
    //    public static object TryGet(PropertyInfo propertyInfo, object obj)
    //    {
    //        var key = propertyInfo.PropertyType.FullName + propertyInfo.Name;
    //        if (TryGetGetAction(key, out Func<object, object> func))
    //        {
    //            return func.Invoke(obj);
    //        }
    //        else
    //        {
    //            var t_func = ReflectorUtil.Instance.GetPropertyValueFun(obj.GetType(), propertyInfo.Name);
    //            AddGetAction(key, t_func);
    //            return t_func(obj);
    //        }
    //    }
    //}

    /// <summary>
    /// The property set delegate cache.
    /// </summary>
    public static class PropertySetDelegateCache
    {
        private static readonly ConcurrentDictionary<string, Delegate> setPropertyFunCache = new ConcurrentDictionary<string, Delegate>();

        /// <summary>
        /// Adds the set action.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="action">The action.</param>
        public static void AddSetAction<T>(string key, Action<T, object> action) where T : class
        {
            setPropertyFunCache.TryAdd(key, action);
        }

        /// <summary>
        /// Gets the set action.
        /// </summary>
        /// <param name="key">The property name.</param>
        /// <returns>An Action.</returns>
        public static bool TryGetSetAction<T>(string key, out Action<T, object> action) where T : class
        {
            action = null;
            if (setPropertyFunCache.ContainsKey(key))
            {
                action = (Action<T, object>)setPropertyFunCache[key];
                return true;
            }
            return false;
        }

        public static void TrySet<T>(PropertyInfo propertyInfo, T obj, object value) where T : class
        {
            var key = propertyInfo.PropertyType.FullName + propertyInfo.Name;
            if (TryGetSetAction<T>(key, out var action))
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