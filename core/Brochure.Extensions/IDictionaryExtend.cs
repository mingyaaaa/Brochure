using System;
using System.Collections.Generic;
using System.Linq;

namespace Brochure.Extensions
{
    /// <summary>
    /// The i dictionary extend.
    /// </summary>
    public static class IDictionaryExtend
    {
        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <param name="dic">The dic.</param>
        /// <param name="tlist">The tlist.</param>
        /// <param name="funcValues">The func values.</param>
        /// <returns>A list of T3S.</returns>
        public static IEnumerable<T3> GetValues<T1, T2, T3> (this IDictionary<T1, IEnumerable<T2>> dic, IEnumerable<T1> tlist, Func<T2, T3> funcValues = null)
        where T1 : class
        where T3 : class
        {
            var list = new List<T3> ();
            if (funcValues == null)
                funcValues = t => t as T3;
            var ll = tlist.ToList ();
            foreach (var item in ll)
            {
                if (dic.ContainsKey (item))
                    list.AddRange (dic[item].Select (funcValues));
            }
            return list;
        }
        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <param name="dic">The dic.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddValue<T1, T2> (this IDictionary<T1, List<T2>> dic, T1 key, T2 value)
        {
            if (dic.ContainsKey (key))
            {
                dic[key].Add (value);
            }
            else
            {
                dic.Add (key, new List<T2> () { value });
            }
        }
    }
}