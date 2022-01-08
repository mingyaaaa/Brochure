using System;
using System.Collections.Generic;
using System.Linq;

namespace Brochure.Extensions
{
    /// <summary>
    /// The i enumerable extend.
    /// </summary>
    public static class IEnumerableExtend
    {
        /// <summary>
        /// Removes the.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="func">The func.</param>
        /// <returns>A list of TS.</returns>
        public static IEnumerable<T> Remove<T> (this IEnumerable<T> list, Func<T, bool> func)
        {
            var result = list.ToList ();
            var deleteList = result.Where (func);
            foreach (var item in deleteList)
                result.Remove (item);
            return result;
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="items">The items.</param>
        /// <returns>A list of TS.</returns>
        public static IEnumerable<T> RemoveRange<T> (this IEnumerable<T> list, IEnumerable<T> items)
        {
            var deleteItem = items.ToList ();
            var result = list.ToList ();
            deleteItem.ForEach (t => result.Remove (t));
            return result;
        }

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="fun">The fun.</param>
        /// <returns>A string.</returns>
        public static string Join<T> (this IEnumerable<T> list, string separator, Func<T, string> fun = null)
        {
            if (fun == null)
                fun = t => t.ToString ();
            return string.Join (separator, list.Select (fun));
        }

        /// <summary>
        /// Ofs the type.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="fun">The fun.</param>
        /// <returns>A list of T2S.</returns>
        public static List<T2> OfType<T1, T2> (this IEnumerable<T1> list, Func<T1, T2> fun)
        {
            if (fun == null)
                return list.OfType<T2> ().ToList ();
            var alist = list.ToList ();
            var result = new List<T2> ();
            foreach (var item in alist)
            {
                result.Add (fun (item));
            }
            return result;
        }

        /// <summary>
        /// As the dictionary.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="funcKey">The func key.</param>
        /// <param name="funcValues">The func values.</param>
        /// <returns>An IDictionary.</returns>
        public static IDictionary<T2, IEnumerable<T3>> AsDictionary<T1, T2, T3> (this IEnumerable<T1> list, Func<T1, T2> funcKey, Func<T1, T3> funcValues = null)
        where T1 : class
        where T3 : class
        {
            var dic = new Dictionary<T2, IEnumerable<T3>> ();
            var tlist = list.ToArray ();
            var keys = list.Select (funcKey).Distinct ().ToArray ();
            if (funcValues == null)
                funcValues = t => t as T3;
            foreach (var item in keys)
                dic[item] = list.Where (t => funcKey (t).Equals (item)).Select (funcValues).Distinct ();
            return dic;
        }
    }
}