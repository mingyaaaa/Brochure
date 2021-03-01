using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        public static bool TryGetGetAction(string propertyName, out Func<T, object> func)
        {
            func = null;
            if (getPropertyFunCache.ContainsKey(propertyName))
            {
                func = getPropertyFunCache[propertyName];
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
    }
}
