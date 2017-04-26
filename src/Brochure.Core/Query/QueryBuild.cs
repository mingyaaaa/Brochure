using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Brochure.Core.Extends;
using Brochure.Core.Helper;

namespace Brochure.Core.Query
{
    public class QueryBuild
    {
        public static QueryBuild Ins => new QueryBuild();
        private IDictionary<string, object> dic = new Dictionary<string, object>();
        private string _resultStr = "1=1";
        private const string PreStr = ConstString.SqlServerPre;

        public QueryBuild And<T>(T entrity, Expression<Func<T, object>> expr)
        {
            var tuple = entrity.GetPropertyValueTuple(expr);
            return And(tuple.Item1, tuple.Item2);
        }
        public QueryBuild And(Tuple<string, object> tuple)
        {
            return And(tuple.Item1, tuple.Item2);
        }
        public QueryBuild And(string name, object value)
        {
            _resultStr = _resultStr + $" and {name} = {PreStr + name}";
            dic.Add(name, value);
            return this;
        }
        public QueryBuild Or<T>(T entrity, Expression<Func<T, object>> expr)
        {
            var tuple = entrity.GetPropertyValueTuple(expr);
            return Or(tuple.Item1, tuple.Item2);
        }
        public QueryBuild Or(Tuple<string, object> tuple)
        {
            return Or(tuple.Item1, tuple.Item2);
        }
        public QueryBuild Or(string name, object value)
        {
            _resultStr = _resultStr + $" or {name} = {PreStr + name}";
            dic.Add(name, value);
            return this;
        }
        public QueryBuild AndBetweeen<T>(Expression<Func<T, object>> expr, double min, double max)
        {
            var property = ObjectHelper.GetPropertyName(expr);
            return AndBetweeen(property, min, max);
        }

        public QueryBuild AndBetweeen(string name, double min, double max)
        {
            _resultStr = _resultStr + $" and {name} between {min} and {max}";
            dic.Add(ConstString.Min + name, min);
            dic.Add(ConstString.Max + name, max);
            return this;
        }
        public QueryBuild OrBetweeen<T>(Expression<Func<T, object>> expr, double min, double max)
        {
            var property = ObjectHelper.GetPropertyName(expr);
            return OrBetweeen(property, min, max);
        }

        public QueryBuild OrBetweeen(string name, double min, double max)
        {
            _resultStr = _resultStr + $" or {name} between {min} and {max}";
            dic.Add(ConstString.Min + name, min);
            dic.Add(ConstString.Max + name, max);
            return this;
        }
        public QueryBuild AndIn<T>(Expression<Func<T, object>> expr, params object[] array)
        {
            var property = ObjectHelper.GetPropertyName(expr);
            return AndIn(property, array);
        }
        public QueryBuild AndIn(string name, params object[] array)
        {
            if (array.Length >= 20)
                throw new Exception("In参数最多支持20个值");
            _resultStr = _resultStr + $" and {name} in ({array.ToSqlString()})";
            return this;
        }

        public QueryBuild OrIn<T>(Expression<Func<T, object>> expr, params object[] array)
        {
            var property = ObjectHelper.GetPropertyName(expr);
            return OrIn(property, array);
        }

        public QueryBuild OrIn(string name, params object[] array)
        {
            if (array.Length >= 20)
                throw new Exception("In参数最多支持20个值");
            _resultStr = _resultStr + $" or {name} in ({array.ToSqlString()})";
            return this;
        }
        public override string ToString() => _resultStr;

        public IDictionary<string, object> GetDictionary()
        {
            return dic;
        }
    }
}
