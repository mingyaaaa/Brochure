using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Brochure.Core
{
    public static class StringExtends
    {
        public static T AsObject<T> (this string str)
        {
            if (string.IsNullOrWhiteSpace (str))
                return (T) (object) null;
            var type = typeof (T);
            object result = null;
            List<Type> types = null;
            if (type.IsInterface)
                types = ReflectorUtil.GetTypeByInterface (type.Assembly, type);
            else if (type.IsAbstract)
                types = ReflectorUtil.GetTypeByClass (type.Assembly, type);
            if (types != null)
            {
                var first = types.FirstOrDefault ();
                if (first == null)
                    throw new Exception ($"{type.FullName}没有实现 无法转化");
                result = JsonSerializer.Deserialize (str, first);
            }
            else
            {
                result = JsonSerializer.Deserialize<T> (str);
            }

            return (T) result;
        }
        public static IEnumerable<T> AsEnumerable<T> (this string str)
        {
            if (string.IsNullOrWhiteSpace (str))
                return new List<T> ();
            var type = typeof (T);
            List<Type> types = null;
            object result;
            if (type.IsInterface)
                types = ReflectorUtil.GetTypeByInterface (type.Assembly, type);
            else if (type.IsAbstract)
                types = ReflectorUtil.GetTypeByClass (type.Assembly, type);
            if (types != null)
            {
                var first = types.FirstOrDefault ();
                if (first == null)
                    throw new Exception ($"{type.FullName}没有实现 无法转化");
                var enumerableType = typeof (IEnumerable<>);
                var gtype = enumerableType.MakeGenericType (first);
                result = JsonSerializer.Deserialize (str, gtype);
            }
            else
            {
                result = JsonSerializer.Deserialize<IEnumerable<T>> (str);
            }
            return (IEnumerable<T>) result;
        }
    }
}