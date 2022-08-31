using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Extensions
{
    public static class EnumExtensions
    {
        public static ConcurrentDictionary<Type, ConcurrentDictionary<int, string>> EnumDesDic = new ConcurrentDictionary<Type, ConcurrentDictionary<int, string>>();

        public static string GetDescript(this Enum e)
        {
            var type = e.GetType();
            var value = Convert.ToInt32(e);

            if (EnumDesDic.TryGetValue(type, out var dic))
            {
                if (dic.TryGetValue(value, out var des))
                {
                    return des;
                }
            }
            var str = string.Empty;
            var enumDesDic = new ConcurrentDictionary<int, string>();
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var att = field.GetCustomAttribute<DescriptionAttribute>(false);
                var des = att == null ? "" : att.Description;
                var v = (int)field.GetValue(e)!;
                enumDesDic.TryAdd(v, des);
                if (v == value)
                    str = des;
            }
            EnumDesDic.TryAdd(type, enumDesDic);
            return str;
        }
    }
}