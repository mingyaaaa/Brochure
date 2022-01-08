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
    public abstract class NoSqlVisitor : ExpressionVisitor
    { }

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
        /// <param name="funList">The fun list.</param>
        /// <param name="serviceProvider">The service provider.</param>
        protected ORMVisitor(IDbProvider dbProvider, DbOption dbOption, IEnumerable<IFuncVisit> funList, IServiceProvider serviceProvider = null)
        {
            _dbPrivoder = dbProvider;
            Parameters = new List<IDbDataParameter>();
            TableTypeDic = new Dictionary<int, Type>();
            this.serviceProvider = serviceProvider;
            _funList = funList;
            this.dbOption = dbOption;
        }

        /// <summary>
        ///
        /// </summary>
        protected IDbProvider _dbPrivoder;

        /// <summary>
        ///
        /// </summary>
        protected object sql;

        /// <summary>
        ///
        /// </summary>
        protected List<IDbDataParameter> Parameters;

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<int, Type> TableTypeDic;

        private readonly IEnumerable<IFuncVisit> _funList;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        ///
        /// </summary>
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

        /// <summary>
        /// Gets the sql base.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An object.</returns>
        private object GetSqlBase(Expression expression = null)
        {
            if (expression != null)
                Visit(expression);
            return sql;
        }

        /// <summary>
        /// Gets the constant express value.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An object.</returns>
        protected object GetConstantExpressValue(MemberExpression expression = null)
        {
            if (expression == null)
                return null;
            if (expression.Expression is MemberExpression memberExpression)
            {
                var obj = GetConstantExpressValue(memberExpression);
                if (expression.Member is PropertyInfo propertyInfo)
                {
                    return propertyInfo.GetGetMethod().Invoke(obj, null);
                }
            }
            if (expression.Member is FieldInfo)
            {
                return (expression.Member as FieldInfo)?.GetValue((expression.Expression as ConstantExpression)?.Value);
            }
            return null;
        }

        /// <summary>
        /// Gets the root express value.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An object.</returns>
        private object GetRootExpressValue(MemberExpression expression)
        {
            if (expression.Expression is MemberExpression memberExpression)
            {
                var obj = GetRootExpressValue(memberExpression);
                //   return obj;
                if (expression.Member is PropertyInfo propertyInfo && obj is ObjectValue valueObj)
                {
                    return new ObjectValue(propertyInfo.GetGetMethod().Invoke(valueObj.Value, null));
                }
            }
            else if (expression.Expression is ParameterExpression parameterExpression)
            {
                return parameterExpression.Type;
            }
            else if (expression.Expression is ConstantExpression constantExpression)
            {
                if (expression.Member is FieldInfo)
                {
                    return new ObjectValue((expression.Member as FieldInfo)?.GetValue(constantExpression.Value));
                }
            }
            else if (expression.Expression is UnaryExpression unary && unary.Operand is ParameterExpression unaryParamExpression)
            {
                string tableName = TableUtlis.GetTableName(unaryParamExpression.Type);
                return $"{_dbPrivoder.FormatFieldName(tableName)}.{_dbPrivoder.FormatFieldName(expression.Member.Name)}";
            }
            return null;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <returns>A list of IDbDataParameters.</returns>
        public List<IDbDataParameter> GetParameters()
        {
            return Parameters;
        }

        /// <summary>
        /// Gets the table dic.
        /// </summary>
        /// <returns>A Dictionary.</returns>
        public Dictionary<int, Type> GetTableDic()
        {
            return TableTypeDic;
        }

        /// <summary>
        /// Visits the member.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>An Expression.</returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            var obj = GetRootExpressValue(node);
            if (obj is Type type)
            {
                var tableKey = type.GetHashCode();
                TableTypeDic.TryAdd(tableKey, type);
                sql = $"{tableKey}.{_dbPrivoder.FormatFieldName(node.Member.Name)}";
            }
            else if (obj is ObjectValue valueObj)
            {
                sql = AddParamers(valueObj.Value);
            }
            else
            {
                sql = obj;
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
            parms.ParameterName = Guid.NewGuid().ToString();
            parms.Value = obj;
            Parameters.Add(parms);
            return parms.ParameterName;
        }

        /// <summary>
        /// The object value.
        /// </summary>
        private class ObjectValue
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ObjectValue"/> class.
            /// </summary>
            /// <param name="obj">The obj.</param>
            internal ObjectValue(object obj)
            {
                Value = obj;
            }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            public object Value { get; set; }
        }
    }
}