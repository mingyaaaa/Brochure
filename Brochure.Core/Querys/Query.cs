using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brochure.Core.Interfaces;
namespace Brochure.Core.Querys
{
    public class Query
    {
        public string Key;
        private string _queryString;
        public IBDocument Params;
        public Query (string queryString)
        {
            _queryString = queryString;
        }

        public override string ToString ()
        {
            return _queryString;
        }

        public Query And (Query query)
        {
            _queryString = $"{QueryOperationType.And} {query.ToString()}";
            return this;
        }
        public Query Or (Query query)
        {
            _queryString = $"{QueryOperationType.Or} {query.ToString()}";
            return this;
        }
        public static Query Eq (string key, object value)
        {
            var str = $"({key} {QueryOperationType.Eq} {value})";
            return new Query (str);
        }
        public static Query In (string key, object[] values)
        {
            var a = new List<List<object>> ();
            var b = new List<object> ();
            for (int i = 0; i < values.Length; i++)
            {
                b.Add (values[i]);
                if (i / 1000 == 0)
                {
                    a.Add (b);
                    b.Clear ();
                }
            }
            var valueStr = string.Empty;
            var count = 1;
            foreach (var item in a)
            {
                valueStr = $"{key} {QueryOperationType.In} ({string.Join(",",item)})";
                if (count > 2)
                {
                    valueStr = $"{valueStr} or {key} {QueryOperationType.In} ({string.Join(",",item)})";
                }
                count++;
            }
            return new Query ($"({valueStr})");
        }
        public static Query NotIn (string key, object[] values)
        {
            var a = new List<List<object>> ();
            var b = new List<object> ();
            for (int i = 0; i < values.Length; i++)
            {
                b.Add (values[i]);
                if (i / 1000 == 0)
                {
                    a.Add (b);
                    b.Clear ();
                }
            }
            var valueStr = string.Empty;
            var count = 1;
            foreach (var item in a)
            {
                valueStr = $"{key} {QueryOperationType.NotIn} ({string.Join(",",item)})";
                if (count > 2)
                {
                    valueStr = $"{valueStr} or {key} {QueryOperationType.NotIn} ({string.Join(",",item)})";
                }
                count++;
            }
            return new Query ($"({valueStr})");
        }
        public static Query NotEq (string key, object value)
        {
            var str = $"({key} {QueryOperationType.NotEq} {value})";
            return new Query (str);
        }
        public static Query Like (string key, string value)
        {
            var str = $"({key} {QueryOperationType.Like} {value})";
            return new Query (str);
        }
        public static Query NotLike (string key, string value)
        {
            var str = $"({key}  {QueryOperationType.Like} {value})";
            return new Query (str);
        }
        public static Query Between (string key, object value, object value1)
        {
            var str = $"({key} {QueryOperationType.Betweent} )";
            var aa = string.Format (str, value, value1);
            return new Query (aa);
        }
        public static Query Gt (string key, object value)
        {
            var str = $"({key} {QueryOperationType.Gt} {value})";
            return new Query (str);
        }
        public static Query Gte (string key, object value)
        {
            var str = $"({key} {QueryOperationType.Gte} {value})";
            return new Query (str);
        }
        public static Query Lt (string key, object value)
        {
            var str = $"({key} {QueryOperationType.Lt} {value})";
            return new Query (str);
        }
        public static Query Lte (string key, object value)
        {
            var str = $"({key} {QueryOperationType.Lte} {value})";
            return new Query (str);
        }
    }
}