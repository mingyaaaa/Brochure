using System;
using System.Collections.Generic;
using System.Reflection;

namespace Brochure.Utils
{
    public interface IReflectorUtil
    {
        List<object> GetObjectByInterface (Assembly assembly, Type type);
        List<Type> GetTypeByInterface (Assembly assembly, Type type);
        List<object> GetObjectByClass (Assembly assembly, Type type);

        List<Type> GetTypeByClass (Assembly assembly, Type type);
        T CreateInstance<T> (params object[] parms) where T : class;
    }
}