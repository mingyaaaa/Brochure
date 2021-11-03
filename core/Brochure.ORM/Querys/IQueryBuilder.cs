using Brochure.Extensions;
using Brochure.ORM.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    public interface IQueryBuilder
    {
        ParmsSqlResult Build(IQuery queryExpression);
    }

    internal class QueryBuilder : IQueryBuilder
    {
        private readonly IVisitProvider visitProvider;
        private readonly IDbProvider dbProvider;
        public QueryBuilder(IVisitProvider visitProvider, IDbProvider dbProvider)
        {
            this.visitProvider = visitProvider;
            this.dbProvider = dbProvider;
        }

        public ParmsSqlResult Build(IQuery queryExpression)
        {
            var tableNames = queryExpression.MainTables.Select(t => dbProvider.FormatFieldName(TableUtlis.GetTableName(t)));
            var result = new ParmsSqlResult();
            var fromSqlResult = BuildFrom(queryExpression.SelectExpression != null, tableNames);
            var joinParams = BuildJoin(queryExpression.JoinExpression);
            var whereParams = BuildWhere(queryExpression.WhereExpression, queryExpression.WhereListExpression);
            var groupParms = BuildGroup(queryExpression.GroupExpress);
            var orderParams = BuildOrder(queryExpression.OrderExpression, true);
            var orderDesParams = BuildOrder(queryExpression.OrderDescExpression, false);
            var selectParasm = BuildSelect(queryExpression.SelectExpression, groupParms.SQL);
            var sql = $"{selectParasm.SQL}{fromSqlResult.SQL}{joinParams.SQL}{groupParms.SQL}{whereParams.SQL}{orderParams.SQL}{orderDesParams.SQL}";
            result.SQL = sql.Trim();
            result.Parameters.AddRange(whereParams.Parameters);
            return result;
        }

        private ParmsSqlResult BuildFrom(bool hasSelectExpression, IEnumerable<string> tables)
        {
            var r = new ParmsSqlResult();
            if (tables.Count() == 0)
                return r;
            if (!hasSelectExpression)
                r.SQL = AddWhiteSpace($"select * from {string.Join(",", tables)}");
            else
                r.SQL = AddWhiteSpace(string.Join(",", tables));
            return r;
        }

        private ParmsSqlResult BuildSelect(Expression expression, string groupSql)
        {
            if (expression == null)
                return new ParmsSqlResult();
            var visitor = this.visitProvider.Builder<SelectVisitor>();
            var r = Build(visitor, expression);
            //由于Group的存在 会限制select 的查询内容
            if (r.SQL.ContainsReg($@"{dbProvider.FormatFieldName(@"<>f__AnonymousType[0-9]`[0-9]")}") && !string.IsNullOrWhiteSpace(groupSql))
            {
                var groupField = groupSql.Replace("group by", "").Trim();
                var groupFields = groupField.Split(',');
                foreach (var item in groupFields)
                {
                    var filed = item.Split('.')[1];
                    r.SQL = r.SQL.ReplaceReg($@"{dbProvider.FormatFieldName(@"<>f__AnonymousType[0-9]`[0-9]")}.{filed}", item);
                }
            }
            if (r.SQL.ContainsReg($@"{dbProvider.FormatFieldName(@"IGrouping`[0-9]")}.{dbProvider.FormatFieldName(@"Key")}") && !string.IsNullOrWhiteSpace(groupSql))
            {
                var groupField = groupSql.Replace("group by", "").Trim();

                r.SQL = r.SQL.ReplaceReg($@"{dbProvider.FormatFieldName(@"IGrouping`[0-9]")}.{dbProvider.FormatFieldName(@"Key")}", groupField);
            }
            return r;
        }



        private ParmsSqlResult BuildWhere(Expression expression, IEnumerable<(string, Expression)> whereList)
        {
            var result = new ParmsSqlResult();
            if (expression == null && whereList.Count() == 0)
                return result;
            var visitor = this.visitProvider.Builder<WhereVisitor>();
            var r = Build(visitor, expression);
            var whereSqlResult = BuildWhereList(visitor, whereList, r.SQL);
            result.SQL = $"where {r.SQL}{whereSqlResult.SQL}";
            result.Parameters.AddRange(visitor.GetParameters());
            return result;
        }

        private ParmsSqlResult BuildWhereList(WhereVisitor whereVisitor, IEnumerable<(string, Expression)> expression, string whereSql)
        {
            var result = new ParmsSqlResult();
            var visitor = whereVisitor;//此处里面有参数 确保 参数话查询 不会有重复的符号
            foreach (var item in expression)
            {
                var r = Build(visitor, item.Item2);
                if (string.IsNullOrWhiteSpace(whereSql + result.SQL))
                    result.SQL = $"{r.SQL}";
                else
                    result.SQL = $"{result.SQL}{AddWhiteSpace(item.Item1)}({r.SQL.TrimEnd()})";
            }
            result.Parameters.AddRange(visitor.GetParameters());
            result.SQL = AddWhiteSpace(result.SQL);
            return result;
        }

        private ParmsSqlResult BuildGroup(Expression expression)
        {
            var visitor = this.visitProvider.Builder<GroupVisitor>();
            return Build(visitor, expression);
        }

        private ParmsSqlResult BuildOrder(Expression expression, bool isAes)
        {
            var visitor = this.visitProvider.Builder<OrderVisitor>();
            visitor.IsAes = isAes;
            return Build(visitor, expression);
        }

        private ParmsSqlResult BuildJoin(IEnumerable<(Type, Expression)> expression)
        {
            var result = new ParmsSqlResult();
            var visitor = this.visitProvider.Builder<JoinVisitor>();
            foreach (var item in expression)
            {
                (var joinTableType, var joinExpression) = item;
                visitor.SetTableName(joinTableType);
                var t_r = Build(visitor, joinExpression);
                result.SQL = result.SQL + t_r.SQL;
                result.Parameters.AddRange(t_r.Parameters);
            }
            return result;
        }

        public ParmsSqlResult BuildHaving(Expression expression)
        {
            var visitor = this.visitProvider.Builder<HavingVisitor>();
            return Build(visitor, expression);
        }

        private ParmsSqlResult Build(ORMVisitor visitor, Expression expression)
        {
            var result = new ParmsSqlResult();
            if (expression == null)
                return result;
            var sql = visitor.GetSql(expression).ToString();
            var parList = visitor.GetParameters();
            result.SQL = AddWhiteSpace(sql);
            result.Parameters = parList.ToList();
            return result;
        }

        private string AddWhiteSpace(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
                return str + " ";
            return string.Empty;
        }
    }
}