using System;
using System.Collections.Concurrent;
using System.Linq;
using Brochure.Abstract;

namespace Brochure.Abstract
{
    public static class ObjectConverCollection
    {
        private static readonly ConcurrentDictionary<Type, IObjectConver> concurrentDictionary = new ConcurrentDictionary<Type, IObjectConver> ();

        public static T ConvertFromObject<T> (object obj)
        {
            var type = typeof (T);
            var first = concurrentDictionary.FirstOrDefault (t => type.IsAssignableFrom (t.Key) || type.IsSubclassOf (t.Key) || t.Key.Equals (type)).Value;
            if (first != null)
            {
                return (T) first.ConvertFromObject (obj);
            }
            return (T) (object) null;
        }

        public static void RegistObjectConver<T> () where T : IObjectConver
        {
            var type = typeof (T);
            concurrentDictionary.TryAdd (type, Activator.CreateInstance<T> ());
        }

        public static void Remove<T> ()
        {
            var type = typeof (T);
            concurrentDictionary.TryRemove (type, out var _);
        }
    }
}