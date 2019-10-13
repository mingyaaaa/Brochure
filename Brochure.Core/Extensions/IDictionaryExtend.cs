using System;
using System.Collections.Generic;
using System.Linq;

namespace Brochure.Core.Extends
{
    public static class IDictionaryExtend
    {
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
    }
}