using Brochure.Core.Extenstions;
using Brochure.Extensions;
using Brochure.ORM.Visitors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Brochure.ORM.Querys
{
    /// <summary>
    /// The query builder.
    /// </summary>
    public interface IQueryBuilder
    {
        /// <summary>
        /// Builds the.
        /// </summary>
        /// <param name="queryExpression">The query expression.</param>
        /// <returns>A ParmsSqlResult.</returns>
        ParmsSqlResult Build(IQuery queryExpression);

        IDbDataParameter[] RenameParameter(StringBuilder sql, IDictionary<string, IDbDataParameter> parDic);

        void RenameTableType(StringBuilder str, IDictionary<int, Type> tableType, IEnumerable<int> tempTable = null, IDictionary<string, string> groupDic = null);

        void RenameTempTableType(StringBuilder str, IEnumerable<int> tempType);
    }

    /// <summary>
    /// The query builder.
    /// </summary>
    internal class QueryBuilder : IQueryBuilder
    {
        private readonly IVisitProvider _visitProvider;
        private readonly IDbProvider _dbProvider;
        private readonly DbOption _dbOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
        /// </summary>
        /// <param name="visitProvider">The visit provider.</param>
        /// <param name="dbProvider">The db provider.</param>
        public QueryBuilder(IVisitProvider visitProvider, IDbProvider dbProvider, DbOption dbOption)
        {
            this._visitProvider = visitProvider;
            this._dbProvider = dbProvider;
            _dbOption = dbOption;
        }

        /// <summary>
        /// Builds the.
        /// </summary>
        /// <param name="queryExpression">The query expression.</param>
        /// <returns>A ParmsSqlResult.</returns>
        public ParmsSqlResult Build(IQuery queryExpression)
        {
            var result = new ParmsSqlResult();
            var tp = BuildSub(queryExpression);
            var t_sql = new StringBuilder(tp.SQL);
            var t_p = RenameParameter(t_sql, tp.Params);
            RenameTableType(t_sql, tp.TableTypeDic, tp.TempTable, tp.GroupDic);
            RenameTempTableType(t_sql, tp.TempTable);
            result.SQL = t_sql.ToString().Trim();
            result.Parameters.AddRange(t_p);
            return result;
        }

        private SqlParam BuildSub(IQuery queryExpression)
        {
            //  var tableNames = queryExpression.MainTables.Select(t => _dbProvider.FormatFieldName(TableUtlis.GetTableName(t)));
            Type type;
            var parm = new Dictionary<string, IDbDataParameter>();
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

        private SqlParam BuildTakeAndSkip(int take, int skip)
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
                r.Params.Add(parms.ParameterName, parms);
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
        /// <param name="tables">The tables.</param>
        /// <returns>A ParmsSqlResult.</returns>
        private SqlParam BuildFrom(bool hasSelectExpression, IEnumerable<BaseSubQueryType> subQueryTypes)
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
                var t_r = BuildSub(item);//处理子查询
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
        private SqlParam BuildSelect(Expression expression, string groupSql)
        {
            if (expression == null)
                return new SqlParam();
            var visitor = this._visitProvider.Builder<SelectVisitor>();
            var r = Build(visitor, expression);
            //由于Group的存在 会限制select 的查询内容
            //if (r.SQL.ContainsReg($@"{_dbProvider.FormatFieldName(@"<>f__AnonymousType[0-9]`[0-9]")}") && !string.IsNullOrWhiteSpace(groupSql))
            //{
            //    var groupField = groupSql.Replace("group by", "").Trim();
            //    var groupFields = groupField.Split(',');
            //    foreach (var item in groupFields)
            //    {
            //        var filed = item.Split('.')[1];
            //        r.SQL = r.SQL.ReplaceReg($@"{_dbProvider.FormatFieldName(@"<>f__AnonymousType[0-9]`[0-9]")}.{filed}", item);
            //    }
            //}
            //if (r.SQL.ContainsReg($@"{_dbProvider.FormatFieldName(@"IGrouping`[0-9]")}.{_dbProvider.FormatFieldName(@"Key")}") && !string.IsNullOrWhiteSpace(groupSql))
            //{
            //    var groupField = groupSql.Replace("group by", "").Trim();

            //    r.SQL = r.SQL.ReplaceReg($@"{_dbProvider.FormatFieldName(@"IGrouping`[0-9]")}.{_dbProvider.FormatFieldName(@"Key")}", groupField);
            //}
            r.Type = visitor.SelectType;
            return r;
        }

        /// <summary>
        /// Builds the where.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="whereList">The where list.</param>
        /// <returns>A ParmsSqlResult.</returns>
        private SqlParam BuildWhere(Expression expression, IEnumerable<(string, Expression)> whereList)
        {
            var result = new SqlParam();
            if (expression == null && whereList.Count() == 0)
                return result;
            var visitor = this._visitProvider.Builder<WhereVisitor>();
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
        private SqlParam BuildWhereList(WhereVisitor whereVisitor, IEnumerable<(string, Expression)> expression, string whereSql)
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
        private (SqlParam, Dictionary<string, string>) BuildGroup(Expression expression)
        {
            var visitor = this._visitProvider.Builder<GroupVisitor>();
            return (Build(visitor, expression), visitor.GroupDic);
        }

        /// <summary>
        /// Builds the order.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="isAes">If true, is aes.</param>
        /// <returns>A ParmsSqlResult.</returns>
        private SqlParam BuildOrder(Expression expression, bool isAes)
        {
            var visitor = this._visitProvider.Builder<OrderVisitor>();
            visitor.IsAes = isAes;
            return Build(visitor, expression);
        }

        /// <summary>
        /// Builds the join.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>A ParmsSqlResult.</returns>
        private SqlParam BuildJoin(IEnumerable<(BaseSubQueryType, Expression)> expression)
        {
            var result = new SqlParam();
            var visitor = this._visitProvider.Builder<JoinVisitor>();
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
                    var t_r = BuildSub(subQuery);
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
        public SqlParam BuildHaving(Expression expression)
        {
            var visitor = this._visitProvider.Builder<HavingVisitor>();
            return Build(visitor, expression);
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

        /// <summary>
        /// Adds the white space.
        /// </summary>
        /// <param name="str">The str.</param>
        /// <returns>A string.</returns>
        private string AddWhiteSpace(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
                return str + " ";
            return string.Empty;
        }

        public IDbDataParameter[] RenameParameter(StringBuilder stringBuilder, IDictionary<string, IDbDataParameter> parDic)
        {
            var name = string.Empty;
            var count = 0;
            foreach (var item in parDic)
            {
                name = $"{_dbProvider.GetParamsSymbol()}p{count}";
                stringBuilder.Replace(item.Key, name);
                item.Value.ParameterName = name;
                count++;
            }
            return parDic.Values.ToArray();
        }

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

        public void RenameTempTableType(StringBuilder sqlBuilder, IEnumerable<int> tempType)
        {
            var count = 0;
            foreach (var item in tempType)
            {
                sqlBuilder.Replace(item.ToString(), $"TEMP{count}");
                count++;
            }
        }

        internal class SqlParam
        {
            internal static SqlParam Empty => new SqlParam();

            public SqlParam() : this("", new Dictionary<string, IDbDataParameter>(), new Dictionary<int, Type>())
            {
            }

            public SqlParam(string sql, IDictionary<string, IDbDataParameter> pp, IDictionary<int, Type> tableType, HashSet<int> tempTableDic = null, IDictionary<string, string> groupDic = null, Type type = null)
            {
                SQL = sql;
                Params = pp;
                TableTypeDic = tableType;
                GroupDic = groupDic ?? new Dictionary<string, string>();
                TempTable = tempTableDic ?? new HashSet<int>();
                Type = type;
            }

            public Type Type { get; set; }

            internal string SQL { get; set; }
            internal IDictionary<string, IDbDataParameter> Params { get; }

            internal IDictionary<int, Type> TableTypeDic { get; }

            internal IDictionary<string, string> GroupDic { get; set; }

            internal HashSet<int> TempTable { get; }
        }
    }
}