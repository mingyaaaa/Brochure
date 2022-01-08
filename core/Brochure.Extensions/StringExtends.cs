using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Brochure.Extensions
{
    /// <summary>
    /// The string extends.
    /// </summary>
    public static class StringExtends
    {

        /// <summary>
        /// Replaces the reg.
        /// </summary>
        /// <param name="str">The str.</param>
        /// <param name="regStr">The reg str.</param>
        /// <param name="newStr">The new str.</param>
        /// <returns>A string.</returns>
        public static string ReplaceReg (this string str, string regStr, string newStr)
        {
            Regex rx = new Regex (regStr);
            var match = rx.Match (str);
            if (match.Length > 0)
            {
                str = str.Replace (match.Value, newStr);
            }
            return str;
        }

        /// <summary>
        /// Contains the reg.
        /// </summary>
        /// <param name="str">The str.</param>
        /// <param name="regStr">The reg str.</param>
        /// <returns>A bool.</returns>
        public static bool ContainsReg (this string str, string regStr)
        {
            Regex rx = new Regex (regStr);
            var match = rx.Match (str);
            return match.Length > 0;
        }
    }
}