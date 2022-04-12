using Brochure.Core;
using Brochure.ORM.Atrributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Brochure.ORM.Utils
{
    /// <summary>
    /// The entity util.
    /// </summary>
    public static class EntityUtil
    {
        /// <summary>
        /// As the table record.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An IDictionary.</returns>
        public static IDictionary<string, object> AsTableRecord<T>(T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "参数为空");
            if (obj is IDictionary<string, object> dictionary)
                return dictionary;
            var result = new Dictionary<string, object>();
            if (obj is IDictionary<string, object>)
            {
                return (IDictionary<string, object>)obj;
            }
            var type = obj.GetType();
            var typeInfo = type.GetTypeInfo();
            var props = typeInfo.GetRuntimeProperties();
            foreach (var item in props)
            {
                var attribute = item.GetCustomAttribute(typeof(IngoreAttribute), true);
                if (attribute != null)
                    continue;
                attribute = item.GetCustomAttribute(typeof(SequenceAttribute), true);
                if (attribute != null)
                    continue;
                var value = PropertyGetDelegateCache.TryGet(item, obj);
                result.Add(item.Name, value);
            }
            return result;
        }

        /// <summary>
        /// As the table record.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An IDictionary.</returns>
        public static IDictionary<string, object> AsTableRecord(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "参数为空");
            if (obj is IDictionary<string, object> dictionary)
                return dictionary;
            var result = new Dictionary<string, object>();
            if (obj is IDictionary<string, object>)
            {
                return (IDictionary<string, object>)obj;
            }
            var type = obj.GetType();
            var typeInfo = type.GetTypeInfo();
            var props = typeInfo.GetRuntimeProperties();
            foreach (var item in props)
            {
                var attribute = item.GetCustomAttribute(typeof(IngoreAttribute), true);
                if (attribute != null)
                    continue;
                attribute = item.GetCustomAttribute(typeof(SequenceAttribute), true);
                if (attribute != null)
                    continue;
                var value = PropertyGetDelegateCache.TryGet(item, obj);
                result.Add(item.Name, value);
            }
            return result;
        }
    }
}