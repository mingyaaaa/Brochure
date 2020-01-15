using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Brochure.Utils
{
    public interface IReflectorUtil
    {
        IEnumerable<object> GetObjectByInterface(Assembly assembly, Type type);
        IEnumerable<T> GetObjectByInterface<T>(Assembly assembly);
        IEnumerable<Type> GetTypeByInterface(Assembly assembly, Type type);
        IEnumerable<object> GetObjectByClass(Assembly assembly, Type type);
        IEnumerable<T> GetObjectByClass<T>(Assembly assembly);
        IEnumerable<Type> GetTypeByClass(Assembly assembly, Type type);
        T CreateInstance<T>(params object[] parms) where T : class;
    }
}