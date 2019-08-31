using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqDbQuery
{
    public interface IDbProvider
    {
        string GetParamsSymbol ();
        string GetOperateSymbol (object left, ExpressionType expressionType, object right);
        string GetObjectType (object type);
        ExpressionVisitor GetVisitor ();
    }
    public class MySqlDbProvider : IDbProvider
    {

        public string GetObjectType (object obj)
        {
            string str = string.Empty;
            if (obj is string)
                str = $"'{obj.ToString()}'";
            else if (obj is int || obj is double || obj is float)
                str = obj.ToString ();
            return str;
        }

        public string GetOperateSymbol (object left, ExpressionType expressionType, object right)
        {
            var str = string.Empty;
            string leftStr = left.ToString ();
            var rightObject = GetObjectType (right);
            if (expressionType == ExpressionType.Equal)
            {
                if (right == null)
                    return $"{leftStr} is null";
                else
                    return $"{leftStr} = {rightObject}";
            }
            if (expressionType == ExpressionType.NotEqual)
            {
                if (right == null)
                    return $"{leftStr} is not null";
                else
                    return $"{leftStr} < {rightObject} and {leftStr} > {rightObject}";
            }
            return str;
        }

        public string GetParamsSymbol ()
        {
            throw new NotImplementedException ();
        }

        public ExpressionVisitor GetVisitor ()
        {
            throw new NotImplementedException ();
        }
    }

    public abstract class DbSource
    {
        public DbSource ()
        {

        }
        public abstract T GetDbConnection<T> ();
    }

}