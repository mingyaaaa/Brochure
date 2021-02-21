using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Brochure.Extensions
{
    public static class StringExtends
    {

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

        public static bool ContainsReg (this string str, string regStr)
        {
            Regex rx = new Regex (regStr);
            var match = rx.Match (str);
            return match.Length > 0;
        }
    }
}