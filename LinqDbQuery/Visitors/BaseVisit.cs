using System.Linq.Expressions;
using System.Reflection;

namespace LinqDbQuery.Visitors
{
    public abstract class NoSqlVisitor : ExpressionVisitor
    {

    }

    public abstract class ORMVisitor : ExpressionVisitor
    {
        protected object sql;
        public object GetSql (Expression expression = null)
        {
            if (expression != null)
                Visit (expression);
            return sql;
        }

        protected override Expression VisitMember (MemberExpression node)
        {
            if (node.Member is FieldInfo)
            {
                sql = (node.Member as FieldInfo).GetValue ((node.Expression as ConstantExpression).Value);
            }
            else if (node.Member is PropertyInfo)
            {
                var tableName = ReflectedUtli.GetTableName ((node.Member as PropertyInfo).DeclaringType);
                sql = $"[{tableName}].[{node.Member.Name}]";
            }
            return node;
        }

        protected override Expression VisitConstant (ConstantExpression node)
        {
            sql = node.Value;
            return node;

        }
    }

}