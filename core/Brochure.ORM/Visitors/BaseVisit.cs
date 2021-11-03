using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Brochure.ORM.Visitors
{
    /// <summary>
    /// The no sql visitor.
    /// </summary>
    public abstract class NoSqlVisitor : ExpressionVisitor { }

    /// <summary>
    /// The o r m visitor.
    /// </summary>
    public abstract class ORMVisitor : ExpressionVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ORMVisitor"/> class.
        /// </summary>
        /// <param name="dbProvider">The db provider.</param>
        /// <param name="dbOption">The db option.</param>
        /// <param name="">The .</param>
        /// <param name="funList">The fun list.</param>
        /// <param name="serviceProvider">The service provider.</param>
        protected ORMVisitor(IDbProvider dbProvider, DbOption dbOption, IEnumerable<IFuncVisit> funList, IServiceProvider serviceProvider = null)
        {
            _dbPrivoder = dbProvider;
            this.Parameters = new List<IDbDataParameter>();
            this.serviceProvider = serviceProvider;
            _funList = funList;
            this.dbOption = dbOption;
        }

        protected IDbProvider _dbPrivoder;
        protected object sql;
        protected List<IDbDataParameter> Parameters;
        private readonly IEnumerable<IFuncVisit> _funList;
        private readonly IServiceProvider serviceProvider;
        protected readonly DbOption dbOption;

        /// <summary>
        /// Gets the sql.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An object.</returns>
        public virtual object GetSql(Expression expression = null)
        {
            return GetSqlBase(expression);
        }

        private object GetSqlBase(Expression expression = null)
        {
            if (expression != null)
                Visit(expression);
            return sql;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <returns>A list of IDbDataParameters.</returns>
        public IEnumerable<IDbDataParameter> GetParameters()
        {
            return Parameters;
        }

        /// <summary>
        /// Visits the member.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member is FieldInfo)
            {
                var obj = (node.Member as FieldInfo)?.GetValue((node.Expression as ConstantExpression)?.Value);
                this.sql = AddParamers(obj);
            }
            else if (node.Member is PropertyInfo)
            {
                string tableName = TableUtlis.GetTableName((node.Member as PropertyInfo)?.DeclaringType);
                sql = $"{_dbPrivoder.FormatFieldName(tableName)}.{_dbPrivoder.FormatFieldName(node.Member.Name)}";
            }
            return node;
        }

        /// <summary>
        /// Visits the method call.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var argument0 = node.Arguments[0];
            var call = this.GetSqlBase(argument0);
            object member = null;
            if (node.Object == null && node.Arguments.Count == 2)
            {
                member = this.GetSqlBase(node.Arguments[1]);
            }
            else
            {
                member = this.GetSqlBase(node.Object);
            }

            switch (node.Method.Name)
            {
                case FuncName.Contains:
                    if (call is string)
                    {
                        sql = $"{member} like '%{call}%'";
                    }
                    else if (call is IEnumerable)
                    {
                        var listStr = string.Join(',', (call as IEnumerable).OfType<object>().Select(t => _dbPrivoder.GetObjectType(t)));
                        sql = $"{member} in ({listStr})";
                    }
                    break;
                case FuncName.StartsWith:
                    if (call is string)
                        sql = $"{member} like '%{call}'";
                    break;
                case FuncName.EndsWith:
                    if (call is string)
                        sql = $"{member} like '{call}%'";
                    break;
                case FuncName.Count:
                    sql = $"count({call})";
                    break;
                case FuncName.Sum:
                    sql = $"sum({member})";
                    break;
                case FuncName.Min:
                    sql = $"min({member})";
                    break;
                case FuncName.Max:
                    sql = $"max({member})";
                    break;
                default:
                    var fun = _funList?.FirstOrDefault(t => t.FuncName == node.Method.Name);
                    if (fun != null)
                    {
                        sql = fun.GetExcuteSql(call, member);
                    }
                    break;
            }
            return node;
        }

        /// <summary>
        /// Visits the constant.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            sql = AddParamers(node.Value);
            return node;
        }

        /// <summary>
        /// Adds the paramers.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An object.</returns>
        private object AddParamers(object obj)
        {
            if (!dbOption.IsUseParamers)
            {
                return obj;
            }
            var parms = _dbPrivoder.GetDbDataParameter();
            parms.ParameterName = $"{_dbPrivoder.GetParamsSymbol()}p{Parameters.Count}";
            parms.Value = obj;
            Parameters.Add(parms);
            return parms.ParameterName;
        }
    }
}