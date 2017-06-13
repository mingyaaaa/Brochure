using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Brochure.Core.Helper
{
    public class ObjectHelper
    {
        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
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
        public static string GetPropertyName(Expression<Func<object>> expr)
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
        public static T CreateInstance<T>()
        {

            var type = typeof(T).GetTypeInfo();
            var assembly = type.Assembly;
            if (assembly == null)
                throw new Exception("无法转发为具体对象");
            return (T)assembly.CreateInstance(type.FullName);
        }
    }
}
