using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Brochure.Utils
{
    public class ReflectorUtil : IReflectorUtil
    {
        public static IReflectorUtil Instance => new ReflectorUtil ();

        public IEnumerable<object> GetObjectOfBase (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var listobject = new List<object> ();
            foreach (var item in types)
            {
                if (item.IsAssignableFrom (type))
                    listobject.Add (assembly.CreateInstance (item.FullName));
            }
            return listobject;
        }

        public IEnumerable<T> GetObjectOfBase<T> (Assembly assembly)
        {
            var type = typeof (T);
            var objs = GetObjectOfBase (assembly, type);
            return objs.OfType<T> ();
        }

        public IEnumerable<Type> GetTypeOfBase (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var list = new List<Type> ();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (item.IsAssignableFrom (type))
                    list.Add (item);
            }
            return list;
        }

        public IEnumerable<Type> GetTypeOfBase<T> (Assembly assembly)
        {
            var type = typeof (T);
            return GetTypeOfBase (assembly, type);
        }

        public T CreateInstance<T> (params object[] parms) where T : class
        {
            var type = typeof (T);
            return (T) CreateInstance (type, parms);
        }

        public object CreateInstance (Type type, params object[] parms)
        {
            var typeinfo = type.GetTypeInfo ();
            var paramsTypes = new List<Type> ();
            foreach (var o in parms)
            {
                paramsTypes.Add (o.GetType ());
            }
            var constructor = typeinfo.GetConstructor (paramsTypes.ToArray ());
            return constructor?.Invoke (parms);
        }

        public IEnumerable<object> GetObjectOfAbsoluteBase (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var listobject = new List<object> ();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                var ii = item.GetInterfaces ();
                if (ii.Any (t => t.FullName == type.FullName))
                    listobject.Add (assembly.CreateInstance (item.FullName));
            }
            return listobject;
        }

        public IEnumerable<T> GetObjectOfAbsoluteBase<T> (Assembly assembly)
        {
            var type = typeof (T);
            var listobject = GetObjectOfAbsoluteBase (assembly, type);
            return listobject.OfType<T> ();
        }

        public IEnumerable<Type> GetTypeOfAbsoluteBase (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var listobject = new List<Type> ();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                var ii = item.GetInterfaces ();
                if (ii.Any (t => t.FullName == type.FullName))
                    listobject.Add (item);
            }
            return listobject;
        }

        public IEnumerable<Type> GetTypeOfAbsoluteBase<T> (Assembly assembly)
        {
            var type = typeof (T);
            return GetTypeOfAbsoluteBase (assembly, type);
        }
    }
}