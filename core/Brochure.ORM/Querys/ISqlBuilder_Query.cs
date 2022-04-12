using Brochure.Core.Extenstions;
using Brochure.ORM.Querys;
using Brochure.ORM.Visitors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Brochure.ORM
{
    /// <summary>
    /// The sql builder.
    /// </summary>
    public partial class SqlBuilder
    {
        /// <summary>
        /// Builds the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>An ISqlResult.</returns>
        protected virtual ISqlResult BuildQuery(IQuery query)
        {
            var result = new ParmsSqlResult();
            var sql = BuildSubQuery(query);
            var strBuild = new StringBuilder(sql.SQL);
            RenameTableType(strBuild, sql.TableTypeDic, sql.TempTable, sql.GroupDic);
            RenameTempTableType(strBuild, sql.TempTable);

            result.SQL = strBuild.ToString();
            result.Parameters.AddRange(sql.Params);
            return result;
        }

        /// <summary>
        /// Builds the sub.
        /// </summary>
        /// <param name="queryExpression">The query expression.</param>
        /// <returns>A SqlParam.</returns>
        protected virtual SqlParam BuildSubQuery(IQuery queryExpression)
        {
            Type type;
            var parm = new List<IDbDataParameter>();
            var tableTypeDic = new Dictionary<int, Type>();
            var tempTableDic = new HashSet<int>();

            var fromSqlResult = BuildFrom(queryExpression.SelectExpression != null, queryExpression.MainTables);
            parm.AddRange(fromSqlResult.Params);
            tableTypeDic.AddRange(fromSqlResult.TableTypeDic);
            tempTableDic.AddRange(fromSqlResult.TempTable);
            type = fromSqlResult.Type;

            var joinParams = BuildJoin(queryExpression.JoinExpression);
            parm.AddRange(joinParams.Params);
            tableTypeDic.AddRange(joinParams.TableTypeDic);
            tempTableDic.AddRange(joinParams.TempTable);

            var whereParams = BuildWhere(queryExpression.WhereExpression, queryExpression.WhereListExpression);
            parm.AddRange(whereParams.Params);
            tableTypeDic.AddRange(whereParams.TableTypeDic);
            tempTableDic.AddRange(whereParams.TempTable);

            (var groupParms, var groupDic) = BuildGroup(queryExpression.GroupExpress);
            parm.AddRange(groupParms.Params);
            tableTypeDic.AddRange(groupParms.TableTypeDic);
            tempTableDic.AddRange(groupParms.TempTable);

            var orderParams = BuildOrder(queryExpression.OrderExpression, true);
            parm.AddRange(orderParams.Params);
            tableTypeDic.AddRange(orderParams.TableTypeDic);
            tempTableDic.AddRange(orderParams.TempTable);

            var orderDesParams = BuildOrder(queryExpression.OrderDescExpression, false);
            parm.AddRange(orderDesParams.Params);
            tableTypeDic.AddRange(orderDesParams.TableTypeDic);
            tempTableDic.AddRange(orderDesParams.TempTable);

            var selectParams = BuildSelect(queryExpression.SelectExpression, groupParms.SQL);
            parm.AddRange(selectParams.Params);
            tableTypeDic.AddRange(selectParams.TableTypeDic);
            tempTableDic.AddRange(selectParams.TempTable);
            var limitParams = BuildTakeAndSkip(queryExpression.TakeCount, queryExpression.SkipCount);
            parm.AddRange(limitParams.Params);
            tableTypeDic.AddRange(limitParams.TableTypeDic);
            tempTableDic.AddRange(limitParams.TempTable);
            if (selectParams.Type != null)
                type = selectParams.Type;
            var sql = $"{selectParams.SQL}{fromSqlResult.SQL}{joinParams.SQL}{groupParms.SQL}{whereParams.SQL}{orderParams.SQL}{orderDesParams.SQL}{limitParams.SQL}";
            return new SqlParam(sql, parm, tableTypeDic, tempTableDic, groupDic, type);
        }

        /// <summary>
        /// Builds the take and skip.
        /// </summary>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <returns>A SqlParam.</returns>
        protected virtual SqlParam BuildTakeAndSkip(int take, int skip)
        {
            if (take == 0 && skip == 0)
                return SqlParam.Empty;
            var r = new SqlParam();
            Func<int, string> fun = t =>
            {
                if (!_dbOption.IsUseParamers)
                    return t.ToString();
                var parms = _dbProvider.GetDbDataParameter();
                parms.ParameterName = Guid.NewGuid().ToString();
                parms.Value = t;
                r.Params.Add(parms);
                return parms.ParameterName;
            };
            var takeStr = take == 0 ? string.Empty : fun(take);
            var skipStr = skip == 0 ? string.Empty : $"{fun(skip)},";
            r.SQL = AddWhiteSpace($"limit {skipStr}{takeStr}");
            return r;
        }

        /// <summary>
        /// Builds the from.
        /// </summary>
        /// <param name="hasSelectExpression">If true, has select expression.</param>
        /// <param name="subQueryTypes"></param>
        /// <returns>A ParmsSqlResult.</returns>
        protected virtual SqlParam BuildFrom(bool hasSelectExpression, IEnumerable<BaseSubQueryType> subQueryTypes)
        {
            var r = new SqlParam();
            var stringBuilder = new StringBuilder();
            if (subQueryTypes.Count() == 0)
                return r;
            if (!hasSelectExpression)
                stringBuilder.Append(AddWhiteSpace("select * from"));
            var typeObjList = subQueryTypes.Select(t => t.GetSubQueryType()).ToList();
            var typeList = typeObjList.OfType<Type>();
            if (typeList.Any())//当前查询返回的类型
            {
                r.Type = typeList.First();
            }
            var tableNames = typeList.Select(t => _dbProvider.FormatFieldName(TableUtlis.GetTableName(t)));
            stringBuilder.Append(AddWhiteSpace(string.Join(',', tableNames)));
            var queryList = typeObjList.OfType<IQuery>();
            foreach (var item in queryList)
            {
                var t_r = BuildSubQuery(item);//处理子查询
                r.Type = t_r.Type;
                r.GroupDic.AddRange(t_r.GroupDic);
                r.TableTypeDic.AddRange(t_r.TableTypeDic);
                r.Params.AddRange(t_r.Params);
                //如果Type为Null 表示没有只有条件查询语句
                if (r.Type == null)
                {
                    stringBuilder.Append(AddWhiteSpace(t_r.SQL));
                }
                else
                {
                    var tableKey = r.Type.GetHashCode();
                    r.TempTable.Add(tableKey);
                    stringBuilder.Append($"({t_r.SQL}) {tableKey}");
                }
            }
            r.SQL = AddWhiteSpace(stringBuilder.ToString().Trim());
            //if (!hasSelectExpression)
            //    r.SQL = AddWhiteSpace($"select * from {string.Join(",", tables)}");
            //else
            //    r.SQL = AddWhiteSpace(string.Join(",", tables));
            return r;
        }

        /// <summary>
        /// Builds the select.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="groupSql">The group sql.</param>
        /// <returns>A ParmsSqlResult.</returns>
        protected virtual SqlParam BuildSelect(Expression expression, string groupSql)
        {
            if (expression == null)
                return new SqlParam();
            var visitor = _visitProvider.Builder<SelectVisitor>();
            var r = Build(visitor, expression);
            r.Type = visitor.SelectType;
            return r;
        }

        /// <summary>
        /// Builds the where.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="whereList">The where list.</param>
        /// <returns>A ParmsSqlResult.</returns>
        protected virtual SqlParam BuildWhere(Expression expression, IEnumerable<(string, Expression)> whereList)
        {
            var result = new SqlParam();
            if (expression == null && whereList.Count() == 0)
                return result;
            var visitor = _visitProvider.Builder<WhereVisitor>();
            var r = Build(visitor, expression);
            var whereSqlResult = BuildWhereList(visitor, whereList, r.SQL);
            result.SQL = $"where {r.SQL}{whereSqlResult.SQL}";
            result.Params.AddRange(visitor.GetParameters());
            result.TableTypeDic.AddRange(visitor.GetTableDic());
            return result;
        }

        /// <summary>
        /// Builds the where list.
        /// </summary>
        /// <param name="whereVisitor">The where visitor.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="whereSql">The where sql.</param>
        /// <returns>A ParmsSqlResult.</returns>
        protected virtual SqlParam BuildWhereList(WhereVisitor whereVisitor, IEnumerable<(string, Expression)> expression, string whereSql)
        {
            var result = new SqlParam();
            var visitor = whereVisitor;//此处里面有参数 确保 参数话查询 不会有重复的符号
            foreach (var item in expression)
            {
                var r = Build(visitor, item.Item2);
                if (string.IsNullOrWhiteSpace(whereSql + result.SQL))
                    result.SQL = $"{r.SQL}";
                else
                    result.SQL = $"{result.SQL}{AddWhiteSpace(item.Item1)}({r.SQL.TrimEnd()})";
            }
            result.Params.AddRange(visitor.GetParameters());
            result.SQL = AddWhiteSpace(result.SQL);
            return result;
        }

        /// <summary>
        /// Builds the group.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>A ParmsSqlResult.</returns>
        protected virtual (SqlParam, Dictionary<string, string>) BuildGroup(Expression expression)
        {
            var visitor = _visitProvider.Builder<GroupVisitor>();
            return (Build(visitor, expression), visitor.GroupDic);
        }

        /// <summary>
        /// Builds the order.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="isAes">If true, is aes.</param>
        /// <returns>A ParmsSqlResult.</returns>
        protected virtual SqlParam BuildOrder(Expression expression, bool isAes)
        {
            var visitor = _visitProvider.Builder<OrderVisitor>();
            visitor.IsAes = isAes;
            return Build(visitor, expression);
        }

        /// <summary>
        /// Builds the join.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>A ParmsSqlResult.</returns>
        protected virtual SqlParam BuildJoin(IEnumerable<(BaseSubQueryType, Expression)> expression)
        {
            var result = new SqlParam();
            var visitor = _visitProvider.Builder<JoinVisitor>();
            foreach (var item in expression)
            {
                (var subQueryType, var joinExpression) = item;
                var joinType = subQueryType.GetSubQueryType();
                if (joinType is Type pType)
                {
                    visitor.SetTableName(pType);
                }
                else if (joinType is IQuery subQuery)
                {
                    var t_r = BuildSubQuery(subQuery);
                    var tableKey = t_r.Type.GetHashCode();
                    var joinTable = $"({t_r.SQL}) {tableKey}";
                    result.TempTable.Add(tableKey);
                    visitor.SetTableName(joinTable);
                }

                var r = Build(visitor, joinExpression);
                result.SQL = result.SQL + r.SQL;
                result.Params.AddRange(r.Params);
                result.TableTypeDic.AddRange(r.TableTypeDic);
            }
            return result;
        }

        /// <summary>
        /// Builds the having.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>A ParmsSqlResult.</returns>
        protected virtual SqlParam BuildHaving(Expression expression)
        {
            var visitor = _visitProvider.Builder<HavingVisitor>();
            return Build(visitor, expression);
        }

        /// <summary>
        /// Builds the other query.
        /// </summary>
        /// <param name="queries">The queries.</param>
        /// <returns>A SqlParam.</returns>
        protected virtual SqlParam BuildOtherQuery(IList<IQuery> queries)
        {
            var result = new SqlParam();
            var str = new StringBuilder();
            foreach (var item in queries)
            {
                var r = BuildSubQuery(item);
                str.Append($";{r.SQL}");
                result.TableTypeDic.AddRange(r.TableTypeDic);
                result.Params.AddRange(r.Params);
                result.TempTable.AddRange(r.TempTable);
                result.GroupDic.AddRange(r.GroupDic);
            }
            result.SQL = str.ToString();
            return result;
        }

        /// <summary>
        /// Renames the table type.
        /// </summary>
        /// <param name="sqlBuilder">The sql builder.</param>
        /// <param name="tableType">The table type.</param>
        /// <param name="tempTable">The temp table.</param>
        /// <param name="groupDic">The group dic.</param>
        public void RenameTableType(StringBuilder sqlBuilder, IDictionary<int, Type> tableType, IEnumerable<int> tempTable = null, IDictionary<string, string> groupDic = null)
        {
            if (groupDic != null)
            {
                foreach (var item in groupDic)
                {
                    sqlBuilder.Replace(item.Key, item.Value);
                }
            }
            foreach (var item in tableType)
            {
                if (tempTable != null && tempTable.Any(t => t == item.Key))
                {
                    continue;
                }
                var tableName = TableUtlis.GetTableName(item.Value);
                sqlBuilder.Replace(item.Key.ToString(), _dbProvider.FormatFieldName(tableName));
            }
        }

        /// <summary>
        /// Renames the temp table type.
        /// </summary>
        /// <param name="sqlBuilder">The sql builder.</param>
        /// <param name="tempType">The temp type.</param>
        public void RenameTempTableType(StringBuilder sqlBuilder, IEnumerable<int> tempType)
        {
            var count = 0;
            foreach (var item in tempType)
            {
                sqlBuilder.Replace(item.ToString(), $"TEMP{count}");
                count++;
            }
        }

        /// <summary>
        /// Builds the.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>A ParmsSqlResult.</returns>
        private SqlParam Build(ORMVisitor visitor, Expression expression)
        {
            if (expression == null)
                return SqlParam.Empty;
            var sql = visitor.GetSql(expression).ToString();
            var parList = visitor.GetParameters();
            return new SqlParam(AddWhiteSpace(sql), parList, visitor.GetTableDic());
        }
    }

    /// <summary>
    /// The sql param.
    /// </summary>
    public class SqlParam
    {
        /// <summary>
        /// Gets the empty.
        /// </summary>
        internal static SqlParam Empty => new SqlParam();

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlParam"/> class.
        /// </summary>
        public SqlParam() : this("", new List<IDbDataParameter>(), new Dictionary<int, Type>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlParam"/> class.
        /// </summary>
        /// <param name="sql">The sql.</param>
        /// <param name="pp">The pp.</param>
        /// <param name="tableType">The table type.</param>
        /// <param name="tempTableDic">The temp table dic.</param>
        /// <param name="groupDic">The group dic.</param>
        /// <param name="type">The type.</param>
        public SqlParam(string sql, List<IDbDataParameter> pp, IDictionary<int, Type> tableType, HashSet<int> tempTableDic = null, IDictionary<string, string> groupDic = null, Type type = null)
        {
            SQL = sql;
            Params = pp;
            TableTypeDic = tableType;
            GroupDic = groupDic ?? new Dictionary<string, string>();
            TempTable = tempTableDic ?? new HashSet<int>();
            Type = type;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the s q l.
        /// </summary>
        internal string SQL { get; set; }

        /// <summary>
        /// Gets the params.
        /// </summary>
        internal List<IDbDataParameter> Params { get; }

        /// <summary>
        /// Gets the table type dic.
        /// </summary>
        internal IDictionary<int, Type> TableTypeDic { get; }

        /// <summary>
        /// Gets or sets the group dic.
        /// </summary>
        internal IDictionary<string, string> GroupDic { get; set; }

        /// <summary>
        /// Gets the temp table.
        /// </summary>
        internal HashSet<int> TempTable { get; }
    }
}