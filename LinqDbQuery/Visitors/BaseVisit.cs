using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqDbQuery.Visitors
{
    public abstract class NoSqlVisitor : ExpressionVisitor
    {

    }

    public abstract class ORMVisitor : ExpressionVisitor
    {
        protected IDbProvider _dbPrivoder;
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
                //  sql = GetSql (node.Expression);
            }
            else if (node.Member is PropertyInfo)
            {
                var tableName = ReflectedUtli.GetTableName ((node.Member as PropertyInfo).DeclaringType);
                sql = $"[{tableName}].[{node.Member.Name}]";
            }
            return node;
        }

        protected override Expression VisitMethodCall (MethodCallExpression node)
        {
            var argument0 = node.Arguments[0];
            var call = GetSql (argument0);
            object member = null;
            if (node.Object == null && node.Arguments.Count == 2)
            {
                member = GetSql (node.Arguments[1]);
            }
            else
            {
                member = GetSql (node.Object);
            }
            if (node.Method.Name == FuncName.Contains)
            {

                if (call is string)
                    sql = $"{member} like '%{call}%'";
                else if (call is IEnumerable)
                {
                    var listStr = string.Join (',', (call as IEnumerable).OfType<object> ().Select (t => _dbPrivoder.GetObjectType (t)));
                    sql = $"{member} in ({listStr})";
                }
            }
            else if (node.Method.Name == FuncName.StartWith)
            {
                if (call is string)
                    sql = $"{call} like '%{member}'";
            }
            else if (node.Method.Name == FuncName.EndWith)
            {
                if (call is string)
                    sql = $"{call} like '{member}%'";
            }
            return node;
        }
        protected override Expression VisitConstant (ConstantExpression node)
        {
            sql = node.Value;
            return node;
        }
        // protected override Expression
    }

}