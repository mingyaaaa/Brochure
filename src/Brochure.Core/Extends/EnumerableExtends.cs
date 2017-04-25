using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brochure.Core.Exceptions;

namespace Brochure.Core.Extends
{
    public static class EnumerableExtends
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Func<object, T> func)
        {
            var result = new List<T>();
            if (func == null)
                throw new ParameterException();
            foreach (var item in list)
            {
                var temp = func.Invoke(item);
                result.Add(temp);
            }
            return result;
        }

        public static string ToString<T>(this IEnumerable<T> list, string symbol)
        {
            return string.Join(symbol, list);
        }

        public static string ToSqlString<T>(this IEnumerable<T> list)
        {
            List<T> result = new List<T>();
            foreach (var item in list)
            {
                T temp;
                if (item is int)
                    temp = item;
                else if (item is string)
                    temp = (T)(object)$"'{item}'";
                else
                {
                    temp = (T)(object)null;
                }
                result.Add(temp);
            }
            return result.ToString(",");
        }
        public static IEnumerable<string> AddPreString(this IEnumerable<object> list, string symbol)
        {
            return list.Select(t => symbol + t.ToString());
        }
    }
}