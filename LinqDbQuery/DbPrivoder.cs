using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqDbQuery
{
    public interface IDbProvider
    {
        string GetParamsSymbol();
        string GetOperateSymbol(object left, ExpressionType expressionType, object right);

        string GetObjectType(object type);

        ExpressionVisitor GetVisitor();
    }
    public class MySqlDbProvider : IDbProvider
    {
        public string GetObjectType(object obj)
        {
            string str = string.Empty;
            if (obj is string)
                str = $"'{obj.ToString()}'";
            else if (obj is int || obj is double || obj is float)
                str = obj.ToString();
            return str;
        }

        public string GetOperateSymbol(object left, ExpressionType expressionType, object right)
        {
            var str = string.Empty;
            if (expressionType == ExpressionType.Equal)
                return $"{left.ToString()} = {GetObjectType(right)}";
            return str;
        }


        public string GetParamsSymbol()
        {
            throw new NotImplementedException();
        }

        public ExpressionVisitor GetVisitor()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class DbSource
    {
        public DbSource()
        {

        }
        public abstract T GetDbConnection<T>();
    }
    public abstract class ORMVisitor : ExpressionVisitor
    {
        protected object sql;
        public object GetSql(Expression expression = null)
        {
            if (expression != null)
                Visit(expression);
            return sql;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member is FieldInfo)
            {
                sql = (node.Member as FieldInfo).GetValue((node.Expression as ConstantExpression).Value);
            }
            else if (node.Member is PropertyInfo)
            {
                var tableName = ReflectedUtli.GetTableName((node.Member as PropertyInfo).DeclaringType);
                sql = $"[{tableName}].[{node.Member.Name}]";
            }
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            sql = node.Value;
            return node;

        }
    }
    public abstract class NoSqlVisitor : ExpressionVisitor
    {

    }
}