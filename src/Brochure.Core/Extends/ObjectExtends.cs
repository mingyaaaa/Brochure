using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Brochure.Core.Atrribute;
using Brochure.Core.implement;

namespace Brochure.Core.Extends
{
    public static class ObjectExtends
    {
        public static T To<T>(this object obj)
        {
            if (obj is T)
                return (T)obj;
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static IEnumerable<T> ToEnumerable<T>(this object obj)
        {
            var ir = obj as IEnumerable<T>;
            if (ir == null)
                throw new Exception("无法转化为集合类型");
            var result = ir.ForEach(t => t.To<T>());
            return result;
        }
        public static IDocument AsDocument(this object obj)
        {
            if (obj is IDictionary<string, object>)
                return new RecordDocument(obj as IDictionary<string, object>);
            return new RecordDocument(obj);
        }
        public static IDocument AsDocument<T>(this T obj, params Expression<Func<T, object>>[] array)
        {
            IDocument result;
            if (obj is IDictionary<string, object>)
                result = new RecordDocument(obj as IDictionary<string, object>);
            else
            {
                result = new RecordDocument(obj);
            }
            foreach (var item in array)
            {
                var property = obj.GetPropertyName(item);
                result.Remove(property);
            }
            return result;
        }
        public static IDictionary<string, object> AsDictionary(this object obj)
        {
            var result = new Dictionary<string, object>();
            if (obj == null)
                return null;
            var type = obj.GetType();
            var properties = type.GetRuntimeProperties();
            foreach (var item in properties)
            {
                var attribute = item.GetCustomAttribute(typeof(IngoreAttribute), true);
                if (attribute != null)
                    continue;
                result.Add(item.Name, item.GetValue(obj));
            }
            return result;
        }

        //深度转换
        public static IDictionary<string, object> AsDepDictionary(this object obj)
        {
            var result = new Dictionary<string, object>();
            if (obj == null)
                return null;
            var type = obj.GetType();
            var properties = type.GetRuntimeProperties();
            foreach (var item in properties)
                result.Add(item.Name, item.GetValue(obj));
            return result;
        }

        public static Tuple<string, object> GetPropertyValueTuple<T>(this T obj, Expression<Func<T, object>> expr)
        {
            if (obj == null)
                throw new Exception("对象为null");
            var type = obj.GetType();
            var properties = type.GetRuntimeProperties();
            var property = properties.FirstOrDefault(t => t.Name == obj.GetPropertyName(expr));
            return Tuple.Create(property.Name, property.GetValue(obj));
        }
        public static string GetPropertyName<T>(this T obj, Expression<Func<T, object>> expr)
        {
            var rtn = "";
            if (expr.Body is UnaryExpression)
            {
                rtn = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                rtn = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                rtn = ((ParameterExpression)expr.Body).Type.Name;
            }
            return rtn;
        }
    }
}