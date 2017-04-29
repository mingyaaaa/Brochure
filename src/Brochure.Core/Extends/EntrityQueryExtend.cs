using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Brochure.Core.Query;

namespace Brochure.Core.Extends
{
    public static class EntrityQueryExtend
    {
        public static EntrieyQuery Equal<T>(this T entrity, Expression<Func<T, object>> expr) where T : IEntrity
        {
            var tuple = entrity.GetPropertyValueTuple(expr);
            IDocument doc = new RecordDocument();
            string str = $" {tuple.Item1} = {ConstString.SqlServerPre + tuple.Item1}";
            doc.Add(tuple.Item1, tuple.Item2);
            EntrieyQuery entrieyQuery = new EntrieyQuery(str, doc, entrity);
            return entrieyQuery;
        }
        public static EntrieyQuery Equal<T>(this T entrity, Expression<Func<T, object>> expr, object value) where T : IEntrity
        {
            var property = entrity.GetPropertyName(expr);
            IDocument doc = new RecordDocument();
            string str = $" {property} = {ConstString.SqlServerPre + value}";
            doc.Add(property, value);
            EntrieyQuery entrieyQuery = new EntrieyQuery(str, doc, entrity);
            return entrieyQuery;
        }
        public static EntrieyQuery Between<T>(this T entrity, Expression<Func<T, object>> expr, double min, double max) where T : IEntrity
        {
            var property = entrity.GetPropertyName(expr);
            string str = $" {property} between {ConstString.SqlServerPre + ConstString.Min + property} and {ConstString.SqlServerPre + ConstString.Max + property}";
            IDocument doc = new RecordDocument();
            doc.Add(ConstString.Min + property, min);
            doc.Add(ConstString.Max + property, max);
            return new EntrieyQuery(str, doc, entrity);
        }

        public static EntrieyQuery In<T>(this T entrity, Expression<Func<T, object>> expr, params object[] array) where T : IEntrity
        {
            var property = entrity.GetPropertyName(expr);
            string str = " in [{0}]";
            List<string> paramlist = new List<string>();
            IDocument doc = new RecordDocument();
            for (int i = 0; i < array.Length; i++)
            {
                string tstr = property + i;
                doc.Add(tstr, array[i]);
                paramlist.Add(ConstString.SqlServerPre + tstr);
            }
            str = string.Format(str, paramlist.ToString(","));
            return new EntrieyQuery(str, doc, entrity);
        }
    }
}
