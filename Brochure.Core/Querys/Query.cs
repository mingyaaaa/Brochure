using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brochure.Core.Interfaces;
using Brochure.Core.System;

namespace Brochure.Core.Querys
{
    public class Query
    {
        public string Key;
        private string _queryString;
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
            _queryString = $"{_queryString} {QueryOperationType.And} { query.ToString()}";
            return this;
        }
        public Query Or (Query query)
        {
            _queryString = $"{_queryString} {QueryOperationType.Or} {query.ToString()}";
            return this;
        }
        public static Query Eq (string key, object value)
        {
            if (value == null)
                return Query.IsNull (key);
            var str = $"{key} {QueryOperationType.Eq} {GetString(value)}";
            return new Query (str);
        }

        public static Query In<T> (string key, IEnumerable<T> values)
        {
            var a = new List<List<string>> ();
            var b = new List<string> ();
            var type = typeof (T);
            var list = values.Select (t => GetString (type.Name, t)).ToList ();
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 1000 == 0 && i != 0)
                {
                    a.Add (b);
                    b = new List<string> ();
                }
                else
                {
                    b.Add (list[i]);
                }
            }
            a.Add (b);
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
        public static Query NotIn (string key, IEnumerable values)
        {
            var a = new List<List<string>> ();
            var b = new List<string> ();
            var list = values.Cast<object> ().ToList ();
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 1000 == 0 && i != 0)
                {
                    a.Add (b);
                    b.Clear ();
                }
                else
                {
                    b.Add (GetString (list[i]));
                }
            }
            a.Add (b);
            var valueStr = string.Empty;
            var count = 1;
            foreach (var item in a)
            {
                valueStr = $"{key}  {QueryOperationType.NotIn} ({string.Join(",",item)})";
                if (count > 2)
                {
                    valueStr = $"{valueStr} and {key} {QueryOperationType.NotIn} ({string.Join(",",item)})";
                }
                count++;
            }
            return new Query ($"({valueStr})");
        }
        public static Query NotEq (string key, object value)
        {
            var str = $"({key} {QueryOperationType.NotEq} {GetString(value)})";
            return new Query (str);
        }

        public static Query IsNull (string key)
        {
            var str = $"({key} is null)";
            return new Query (str);
        }
        public static Query IsNotNull (string key)
        {
            var str = $"({key} is not null)";
            return new Query (str);
        }
        public static Query Like (string key, string value)
        {
            var str = $"{key} {QueryOperationType.Like} '{value}'";
            return new Query (str);
        }
        public static Query NotLike (string key, string value)
        {
            var str = $"{key} {QueryOperationType.NotLike} '{value}'";
            return new Query (str);
        }
        public static Query Between (string key, object value, object value1)
        {
            var str = $"{key} {QueryOperationType.Betweent}";
            var aa = string.Format (str, GetString (value), GetString (value1));
            return new Query (aa);
        }
        public static Query NotBetween (string key, object value, object value1)
        {
            var str = $"{key} {QueryOperationType.NotBetweent}";
            var aa = string.Format (str, GetString (value), GetString (value1));
            return new Query (aa);
        }
        public static Query Gt (string key, object value)
        {
            var str = $"{key} {QueryOperationType.Gt} {GetString(value)}";
            return new Query (str);
        }
        public static Query Gte (string key, object value)
        {
            var str = $"{key} {QueryOperationType.Gte} {GetString(value)}";
            return new Query (str);
        }
        public static Query Lt (string key, object value)
        {
            var str = $"{key} {QueryOperationType.Lt} {GetString(value)}";
            return new Query (str);
        }
        public static Query Lte (string key, object value)
        {
            var str = $"{key} {QueryOperationType.Lte} {GetString(value)}";
            return new Query (str);
        }
        private static string GetString (object a)
        {
            var type = a.GetType ();
            if (type.Name == BaseType.Int || type.Name == BaseType.Double || type.Name == BaseType.Float)
                return a.ToString ();
            else if (type.Name == BaseType.String)
                return $"'{a.ToString()}'";
            else if (type.Name == BaseType.DateTime)
                return $"Date({a.ToString()})";
            return string.Empty;
        }
        private static string GetString (string typeName, object a)
        {
            if (typeName == BaseType.Int || typeName == BaseType.Double || typeName == BaseType.Float)
                return a.ToString ();
            else if (typeName == BaseType.String)
                return $"'{a.ToString()}'";
            else if (typeName == BaseType.DateTime)
                return $"Date({a.ToString()})";
            return string.Empty;
        }
    }
}