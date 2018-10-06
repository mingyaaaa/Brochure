using System;
using System.Collections.Generic;
using System.Linq;

namespace Brochure.Core
{
    public static class IEnumerableExtend
    {
        public static IEnumerable<T> Remove<T>(this IEnumerable<T> list, Func<T, bool> func)
        {
            var result = list.ToList();
            var deleteList = result.Where(func);
            foreach (var item in deleteList)
                result.Remove(item);
            return result;
        }

        public static IEnumerable<T> RemoveRange<T>(this IEnumerable<T> list, IEnumerable<T> items)
        {
            var deleteItem = items.ToList();
            var result = list.ToList();
            deleteItem.ForEach(t => result.Remove(t));
            return result;
        }

        public static string Join<T>(this IEnumerable<T> list, string separator, Func<T, string> fun)
        {
            if (fun == null)
                fun = t => t.ToString();
            return string.Join(separator, list.Select(fun));
        }

        public static List<T2> OfType<T1, T2>(this IEnumerable<T1> list, Func<T1, T2> fun)
        {
            if (fun == null)
                return list.OfType<T2>().ToList();
            var alist = list.ToList();
            var result = new List<T2>();
            foreach (var item in alist)
            {
                result.Add(fun(item));
            }
            return result;
        }
    }
}
