using System;
using System.Data;
using System.Linq.Expressions;
using LinqDbQuery;
using MySql.Data.MySqlClient;

namespace Brochure.LinqDbQuery.MySql
{
    public class MySqlDbProvider : IDbProvider
    {
        public MySqlDbProvider ()
        {
            IsUseParamers = true;
        }

        public bool IsUseParamers { get; set; }
        public Func<DbOption> CreateOption { get; set; }

        public IDbConnection GetDbConnection ()
        {
            return new MySqlConnection ();
        }

        public IDbDataParameter GetDbDataParameter ()
        {
            return new MySqlParameter ();
        }

        public string GetObjectType (object obj)
        {
            if (obj == null)
                return null;
            string str = string.Empty;
            if (IsUseParamers)
            {
                str = obj.ToString ();
            }
            else
            {
                if (obj is string)
                    str = $"'{obj}'";
                else if (obj is int || obj is double || obj is float)
                    str = obj.ToString ();
            }
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
            else if (expressionType == ExpressionType.NotEqual)
            {
                if (right == null)
                    return $"{leftStr} is not null";
                else
                    return $"{leftStr} != {rightObject}";
            }
            else if (expressionType == ExpressionType.AndAlso)
            {
                return $"{left} and {right}";
            }
            else if (expressionType == ExpressionType.OrElse)
            {
                return $"{left} or {right}";
            }
            return str;
        }

        public string GetParamsSymbol ()
        {
            return "@";
        }

        public TypeMap GetTypeMap ()
        {
            return new MySqlTypeMap ();
        }

        public ExpressionVisitor GetVisitor ()
        {
            throw new NotImplementedException ();
        }
    }
}