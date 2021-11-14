﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.Extenstions
{
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="dic">The dic.</param>
        /// <param name="list">The list.</param>
        /// <returns>An IDictionary.</returns>
        public static IDictionary<T1, T2> AddRange<T1, T2>(this IDictionary<T1, T2> dic, IDictionary<T1, T2> list)
        {
            foreach (var item in list)
            {
                dic.TryAdd(item.Key, item.Value);
            }
            return dic;
        }

        public static HashSet<T> AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                hashSet.Add(item);
            }
            return hashSet;
        }
    }
}