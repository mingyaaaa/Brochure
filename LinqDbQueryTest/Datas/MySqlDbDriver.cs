using System;
using System.Data;
using System.Linq.Expressions;
using LinqDbQuery;
using MySql.Data.MySqlClient;
namespace LinqDbQueryTest
{
    public class MySqlDbProvider : IDbProvider
    {
        public MySqlDbProvider ()
        {
            IsUseParamers = true;
        }
        public bool IsUseParamers { get; set; }

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
            string str = string.Empty;
            if (IsUseParamers)
            {
                str = obj.ToString ();
            }
            else
            {
                if (obj is string)
                    str = $"'{obj.ToString()}'";
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
            return "@";
        }

        public ExpressionVisitor GetVisitor ()
        {
            throw new NotImplementedException ();
        }
    }
}