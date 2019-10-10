using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspectCore.Extensions.Reflection;

namespace Brochure.Utils
{
    public class ReflectedUtli
    {
        /// <summary>
        /// 根据接口获取指定的类型
        /// </summary>
        /// <returns></returns>
        public static List<object> GetObjectByInterface (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var listobject = new List<object> ();
            foreach (var item in types)
            {
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
        public static List<Type> GetTypeByInterface (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var list = new List<Type> ();
            foreach (var item in types)
            {
                var tinterfaces = item.GetInterfaces ();
                if (!string.IsNullOrWhiteSpace (item.FullName) && tinterfaces.Any (t => t.FullName == type.FullName))
                    list.Add (item);
            }

            return list;
        }

        public static List<object> GetObjectByClass (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var listobject = new List<object> ();

            foreach (var item in types)
            {
                if (!string.IsNullOrWhiteSpace (item.FullName) && item.BaseType?.FullName == type.FullName)
                    listobject.Add (assembly.CreateInstance (item.FullName));
            }

            return listobject;
        }

        public static List<Type> GetTypeByClass (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var list = new List<Type> ();

            foreach (var item in types)
            {
                if (!string.IsNullOrWhiteSpace (item.FullName) && item.BaseType?.FullName == type.FullName)
                    list.Add (item);
            }

            return list;
        }

        public static T CreateInstance<T> (params object[] parms) where T : class
        {
            var type = typeof (T);
            var typeinfo = type.GetTypeInfo ();
            var paramsTypes = new List<Type> ();
            foreach (var o in parms)
            {
                paramsTypes.Add (o.GetType ());
            }
            var constructor = typeinfo.GetConstructor (paramsTypes.ToArray ());
            var reflector = constructor.GetReflector ();
            return (T) reflector?.Invoke (parms);
        }
    }
}