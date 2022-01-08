using System;
using System.Collections.Concurrent;
using System.Linq;
using Brochure.Abstract;

namespace Brochure.Abstract
{
    /// <summary>
    /// 用于处理 Object类型像其他类型转换的处理
    /// </summary>
    public static class ObjectConverCollection
    {
        private static readonly ConcurrentDictionary<Type, Func<object, object>> concurrentDictionary = new ConcurrentDictionary<Type, Func<object, object>> ();

        /// <summary>
        /// Tries the get conver func.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="func">The func.</param>
        /// <returns>A bool.</returns>
        public static bool TryGetConverFunc (Type type, out Func<object, object> func)
        {
            return concurrentDictionary.TryGetValue (type, out func);
        }

        /// <summary>
        /// Tries the get conver func.
        /// </summary>
        /// <param name="func">The func.</param>
        /// <returns>A bool.</returns>
        public static bool TryGetConverFunc<T1> (out Func<object, object> func)
        {
            var type = typeof (T1);
            return concurrentDictionary.TryGetValue (type, out func);
        }

        /// <summary>
        /// Regists the object conver.
        /// </summary>
        /// <param name="func">The func.</param>
        public static void RegistObjectConver<T1> (Func<object, T1> func)
        {
            var type = typeof (T1);
            concurrentDictionary.TryAdd (type, t => func (t));
        }

        /// <summary>
        /// Regists the object conver.
        /// </summary>
        /// <param name="func">The func.</param>
        public static void RegistObjectConver<T1, T2> (Func<T1, T2> func)
        {
            var type = typeof (T2);
            concurrentDictionary.TryAdd (type, t =>
            {
                if (!(t is T1))
                    throw new Exception ($"{t}的类型不正确 无法转化为");
                return func ((T1) t);
            });
        }

        /// <summary>
        /// Removes the.
        /// </summary>
        public static void Remove<T> ()
        {
            var type = typeof (T);
            concurrentDictionary.TryRemove (type, out var _);
        }
    }

}