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
    }
}
