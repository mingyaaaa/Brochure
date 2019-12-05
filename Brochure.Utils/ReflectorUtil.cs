using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Brochure.Utils
{
    public class ReflectorUtil : IReflectorUtil
    {
        /// <summary>
        /// 根据接口获取指定的类型
        /// </summary>
        /// <returns></returns>
        public List<object> GetObjectByInterface (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var listobject = new List<object> ();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                var tinterfaces = item.GetInterfaces ();
                if (!string.IsNullOrWhiteSpace (item.FullName) && tinterfaces.Any (t => t.FullName == type.FullName))
                    listobject.Add (assembly.CreateInstance (item.FullName));
            }
            return listobject;
        }

        /// <summary>
        /// 根据接口获取指定的类型
        /// </summary>
        /// <returns></returns>
        public List<Type> GetTypeByInterface (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var list = new List<Type> ();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                var tinterfaces = item.GetInterfaces ();
                if (!string.IsNullOrWhiteSpace (item.FullName) && tinterfaces.Any (t => t.FullName == type.FullName))
                    list.Add (item);
            }
            return list;
        }

        public List<object> GetObjectByClass (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var listobject = new List<object> ();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (HasTargetType (item, type.FullName))
                {
                    listobject.Add (assembly.CreateInstance (item.FullName));
                }
            }
            return listobject;
        }

        public List<Type> GetTypeByClass (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var list = new List<Type> ();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (HasTargetType (item, type.FullName))
                {
                    list.Add (item);
                }
            }
            return list;
        }

        public T CreateInstance<T> (params object[] parms) where T : class
        {
            var type = typeof (T);
            var typeinfo = type.GetTypeInfo ();
            var paramsTypes = new List<Type> ();
            foreach (var o in parms)
            {
                paramsTypes.Add (o.GetType ());
            }
            var constructor = typeinfo.GetConstructor (paramsTypes.ToArray ());
            return (T) constructor?.Invoke (parms);
        }

        private bool HasTargetType (Type type, string targetTypeFullName)
        {
            if (type.BaseType?.FullName == targetTypeFullName)
                return true;
            var interfaces = type.GetInterfaces ();
            foreach (var item in interfaces)
            {
                if (item.FullName == targetTypeFullName)
                    return true;
                if (HasTargetType (item, targetTypeFullName))
                    return true;
            }
            return false;
        }

    }
}