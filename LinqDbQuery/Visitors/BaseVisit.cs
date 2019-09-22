using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        protected ORMVisitor (IDbProvider dbProvider)
        {
            _dbPrivoder = dbProvider;
            this.Parameters = new List<IDbDataParameter> ();
        }
        protected IDbProvider _dbPrivoder;
        protected object sql;
        protected List<IDbDataParameter> Parameters;
        public object GetSql (Expression expression = null)
        {
            if (expression != null)
                Visit (expression);
            return sql;
        }

        public IEnumerable<IDbDataParameter> GetParameters ()
        {
            return Parameters;
        }

        protected override Expression VisitMember (MemberExpression node)
        {
            if (node.Member is FieldInfo)
            {
                var obj = (node.Member as FieldInfo).GetValue ((node.Expression as ConstantExpression).Value);
                this.sql = AddParamers (obj);

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
            else if (node.Method.Name == FuncName.StartsWith)
            {
                if (call is string)
                    sql = $"{member} like '%{call}'";
            }
            else if (node.Method.Name == FuncName.EndsWith)
            {
                if (call is string)
                    sql = $"{member} like '{call}%'";
            }
            return node;
        }
        protected override Expression VisitConstant (ConstantExpression node)
        {

            sql = AddParamers (node.Value);
            return node;
        }
        private object AddParamers (object obj)
        {
            if (!_dbPrivoder.IsUseParamers)
            {
                return obj;
            }
            var parms = _dbPrivoder.GetDbDataParameter ();
            parms.ParameterName = $"{_dbPrivoder.GetParamsSymbol()}p{Parameters.Count}";
            parms.Value = obj;
            Parameters.Add (parms);
            return parms.ParameterName;

        }
    }

}