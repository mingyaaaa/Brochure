using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Brochure.Core.Utils
{
    public class ReflectorUtil
    {
        /// <summary>
        /// 根据接口获取指定的类型
        /// </summary>
        /// <returns></returns>
        public static List<object> GetObjectByInterface(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var listobject = new List<object>();
            foreach (var item in types)
            {
                var tinterfaces = item.GetInterfaces();
                if (!string.IsNullOrWhiteSpace(item.FullName) && tinterfaces.Any(t => t.FullName == type.FullName))
                    listobject.Add(assembly.CreateInstance(item.FullName));
            }

            return listobject;
        }
        public static List<object> GetObjectByClass(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var listobject = new List<object>();

            foreach (var item in types)
            {
                if (!string.IsNullOrWhiteSpace(item.FullName) && item.BaseType?.FullName == type.FullName)
                    listobject.Add(assembly.CreateInstance(item.FullName));
            }

            return listobject;
        }
    }
}
