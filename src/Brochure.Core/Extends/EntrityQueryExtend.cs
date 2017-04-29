using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Brochure.Core.Query;

namespace Brochure.Core.Extends
{
    public static class EntrityQueryExtend
    {
        private static readonly Random Random = new Random();
        public static EntrieyQuery Equal<T>(this T entrity, Expression<Func<T, object>> expr) where T : IEntrity
        {
            var randomstr = Random.Next(0, 9999);
            var tuple = entrity.GetPropertyValueTuple(expr);
            IDocument doc = new RecordDocument();
            var dicStr = tuple.Item1 + randomstr;
            string str = $" {tuple.Item1} = {ConstString.SqlServerPre + dicStr} ";//拼接字符串
            doc.Add(dicStr, tuple.Item2);//存储字符串参数
            EntrieyQuery entrieyQuery = new EntrieyQuery(str, doc, entrity);
            return entrieyQuery;
        }
        public static EntrieyQuery Equal<T>(this T entrity, Expression<Func<T, object>> expr, object value) where T : IEntrity
        {
            var randomstr = Random.Next(0, 9999);
            var property = entrity.GetPropertyName(expr);
            IDocument doc = new RecordDocument();
            var dicStr = property + randomstr;
            string str = $" {property} = {ConstString.SqlServerPre + dicStr} ";
            doc.Add(dicStr, value);
            EntrieyQuery entrieyQuery = new EntrieyQuery(str, doc, entrity);
            return entrieyQuery;
        }
        public static EntrieyQuery Between<T>(this T entrity, Expression<Func<T, object>> expr, double min, double max) where T : IEntrity
        {
            var randomstr = Random.Next(0, 9999);
            var property = entrity.GetPropertyName(expr);
            var dicStr = property + randomstr;
            string str = $" {property} between {ConstString.SqlServerPre + ConstString.Min + dicStr } and {ConstString.SqlServerPre + ConstString.Max + dicStr } ";
            IDocument doc = new RecordDocument();
            doc.Add(ConstString.Min + dicStr, min);
            doc.Add(ConstString.Max + dicStr, max);
            return new EntrieyQuery(str, doc, entrity);
        }

        public static EntrieyQuery In<T>(this T entrity, Expression<Func<T, object>> expr, params object[] array) where T : IEntrity
        {
            var property = entrity.GetPropertyName(expr);
            string str = " in [{0}] ";
            List<string> paramlist = new List<string>();
            IDocument doc = new RecordDocument();
            for (int i = 0; i < array.Length; i++)
            {
                var randomstr = Random.Next(0, 9999);
                string tstr = property + randomstr;
                doc.Add(tstr, array[i]);
                paramlist.Add(ConstString.SqlServerPre + tstr);
            }
            str = string.Format(str, paramlist.ToString(","));
            return new EntrieyQuery(str, doc, entrity);
        }
    }
}
