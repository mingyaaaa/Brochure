using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspectCore.Extensions.Reflection;

namespace Brochure.Utils
{
    public class ReflectorUtil : IReflectorUtil
    {
        public static IReflectorUtil Instance => new ReflectorUtil();
        /// <summary>
        /// 根据接口获取指定的类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetObjectByInterface(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var listobject = new List<object>();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                var tinterfaces = item.GetInterfaces();
                if (!string.IsNullOrWhiteSpace(item.FullName) && tinterfaces.Any(t => t.FullName == type.FullName))
                    listobject.Add(assembly.CreateInstance(item.FullName));
            }
            return listobject;
        }

        public IEnumerable<T> GetObjectByInterface<T>(Assembly assembly)
        {
            var type = typeof(T);
            var objs = GetObjectByInterface(assembly, type);
            return objs.OfType<T>();
        }


        /// <summary>
        /// 根据接口获取指定的类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetTypeByInterface(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var list = new List<Type>();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                var tinterfaces = item.GetInterfaces();
                if (!string.IsNullOrWhiteSpace(item.FullName) && tinterfaces.Any(t => t.FullName == type.FullName))
                    list.Add(item);
            }
            return list;
        }

        public IEnumerable<object> GetObjectByClass(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var listobject = new List<object>();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (HasTargetType(item, type.FullName))
                {
                    listobject.Add(assembly.CreateInstance(item.FullName));
                }
            }
            return listobject;
        }

        public IEnumerable<Type> GetTypeByClass(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var list = new List<Type>();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (HasTargetType(item, type.FullName))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public IEnumerable<T> GetObjectByClass<T>(Assembly assembly)
        {
            var type = typeof(T);
            var objs = GetObjectByClass(assembly, type);
            return objs.OfType<T>();
        }

        public T CreateInstance<T>(params object[] parms) where T : class
        {
            var type = typeof(T);
            return (T)CreateInstance(type, parms);
        }

        public object CreateInstance(Type type, params object[] parms)
        {
            var typeinfo = type.GetTypeInfo();
            var paramsTypes = new List<Type>();
            foreach (var o in parms)
            {
                paramsTypes.Add(o.GetType());
            }
            var constructor = typeinfo.GetConstructor(paramsTypes.ToArray());
            return constructor?.Invoke(parms);
        }

        private bool HasTargetType(Type type, string targetTypeFullName)
        {
            if (type.BaseType?.FullName == targetTypeFullName)
                return true;
            var interfaces = type.GetInterfaces();
            foreach (var item in interfaces)
            {
                if (item.FullName == targetTypeFullName)
                    return true;
                if (HasTargetType(item, targetTypeFullName))
                    return true;
            }
            return false;
        }


    }
}