using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AspectCore.Injector;
using LinqDbQuery.Querys;
using LinqDbQuery.Visitors;
using Microsoft.Extensions.Logging;

namespace LinqDbQuery
{
    public abstract class Query : IQuery
    {
        protected DbQueryOption Option { get; private set; }

        protected Query ()
        {
            var provider = DI.Ins.ServiceProvider.ResolveRequired<IDbProvider> ();
            this.Option = provider.CreateOption.Invoke ();
            InitMainTableNames ();
        }

        protected Query (DbQueryOption option)
        {
            this.Option = option;
            InitMainTableNames ();
        }

        protected string selectSql;
        protected string whereSql;

        protected List<string> mainTableNames;

        protected string groupSql;
        protected string joinSql;
        protected string orderSql;
        protected List<IDbDataParameter> parameters;

        private string AddWhiteSpace (string str)
        {
            if (!string.IsNullOrWhiteSpace (str))
                return str + " ";
            return string.Empty;
        }

        public int Count ()
        {
            selectSql = $"select count(1) from {JoinTableNames()}";
            var sql = GetSql ();
            var result = -1;
            using (var conn = this.Option.GetDbConnection ())
            {
                var command = conn.CreateCommand ();
                command.CommandText = sql;
                command.CommandTimeout = this.Option.Timeout;
                if (Option.IsUseParamers)
                {
                    foreach (var item in parameters)
                        command.Parameters.Add (item);
                }
                result = command.ExecuteNonQuery ();
            }
            return result;
        }

        protected T Join<T> (Type type, Expression expression) where T : Query
        {
            var joinVisitor = new JoinVisitor (type, this.Option.DbProvider);
            joinVisitor.Visit (expression);
            string t_joinSql;
            if (string.IsNullOrWhiteSpace (joinSql))
                t_joinSql = $"{joinVisitor.GetSql()}";
            else
                t_joinSql = $"{joinSql} {joinVisitor.GetSql()}";
            var tt = this.Copy<T> ();
            tt.joinSql = t_joinSql;
            return tt;
        }

        protected T OrderBy<T> (Expression fun) where T : Query
        {
            var orderVisitor = new OrderVisitor (this.Option.DbProvider);
            orderVisitor.Visit (fun);
            var t_orderSql = orderVisitor.GetSql ()?.ToString () ?? string.Empty;
            var tt = this as T;
            tt.orderSql = t_orderSql;
            return tt;
        }

        protected T OrderByDesc<T> (Expression fun) where T : Query
        {
            var orderVisitor = new OrderVisitor (this.Option.DbProvider, false);
            orderVisitor.Visit (fun);
            var t_orderSql = orderVisitor.GetSql ()?.ToString () ?? string.Empty;
            var tt = this as T;
            tt.orderSql = t_orderSql;
            return tt;
        }

        public T Select<T> (Expression fun) where T : Query
        {
            var selectVisitor = new SelectVisitor (this.Option.DbProvider);
            selectVisitor.Visit (fun);
            var t_selectSql = selectVisitor.GetSql ()?.ToString () ?? string.Empty;
            //如果有group 则需要将Key替换成对应的分组属性
            if (t_selectSql.Contains ("[<>f__AnonymousType5`2]") && !string.IsNullOrWhiteSpace (groupSql))
            {
                var groupField = groupSql.Replace ("group by", "").Trim ();
                var groupFields = groupField.Split (',');
                foreach (var item in groupFields)
                {
                    var filed = item.Split ('.') [1];
                    Regex rx = new Regex ($@"\[<>f__AnonymousType5`2\].\[{filed.Trim('[',']')}\]");
                    var match = rx.Match (t_selectSql);
                    if (match.Length > 0)
                    {
                        t_selectSql = t_selectSql.Replace (match.Value, item);
                    }
                }
            }
            if (t_selectSql.Contains ("[IGrouping`2].[Key]") && !string.IsNullOrWhiteSpace (groupSql))
            {
                var groupField = groupSql.Replace ("group by", "").Trim ();
                t_selectSql = t_selectSql.Replace ("[IGrouping`2].[Key]", groupField);
            }
            t_selectSql += JoinTableNames ();
            T tt;
            if (this is T t)
                tt = t;
            else
                tt = this.Copy<T> ();
            tt.selectSql = t_selectSql;
            return tt;
        }

        public T WhereAnd<T> (Expression fun) where T : Query
        {
            var whereVisitor = new WhereVisitor (this.Option.DbProvider);
            whereVisitor.Visit (fun);
            var sql = whereVisitor.GetSql ()?.ToString () ?? string.Empty;
            if (string.IsNullOrWhiteSpace (whereSql))
            {
                whereSql = sql;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace (sql))
                    whereSql = $"{whereSql} and ({sql})";
            }
            return this as T;
        }

        public T WhereOr<T> (Expression fun) where T : Query
        {
            var whereVisitor = new WhereVisitor (this.Option.DbProvider);
            whereVisitor.Visit (fun);
            var sql = whereVisitor.GetSql ()?.ToString () ?? string.Empty;
            if (string.IsNullOrWhiteSpace (whereSql))
            {
                whereSql = sql;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace (sql))
                    whereSql = $"{whereSql} or ({sql})";
            }
            return this as T;
        }

        public T Groupby<T> (Expression fun) where T : Query
        {
            var groupVisit = new GroupVisitor (this.Option.DbProvider);
            groupVisit.Visit (fun);
            var t_groupSql = groupVisit.GetSql ().ToString ();
            var tt = this.Copy<T> ();
            tt.groupSql = t_groupSql;
            return tt;
        }

        protected T Copy<T> () where T : Query
        {
            var query = (T) Activator.CreateInstance (typeof (T), Option);
            query.selectSql = this.selectSql;
            query.whereSql = this.whereSql;
            query.groupSql = this.groupSql;
            query.joinSql = this.joinSql;
            query.orderSql = this.orderSql;
            query.parameters = this.parameters;
            query.mainTableNames = this.mainTableNames;
            query.Option = this.Option;
            return query;
        }

        public string GetSql ()
        {
            if (string.IsNullOrWhiteSpace (selectSql))
            {
                selectSql = $"select * from {JoinTableNames()}";
            }
            return $"{AddWhiteSpace(selectSql)}{AddWhiteSpace(joinSql)}{AddWhiteSpace(groupSql)}{AddWhiteSpace(whereSql)}{AddWhiteSpace(orderSql)}".Trim ();
        }

        protected string JoinTableNames ()
        {
            return string.Join (",", mainTableNames.Select (t => $"[{t}]"));
        }

        private void InitMainTableNames ()
        {
            mainTableNames = new List<string> ();
            var thisTypes = this.GetType ();
            var types = thisTypes.GetGenericArguments ();
            foreach (var item in types)
            {
                this.mainTableNames.Add (TableUtlis.GetTableName (item));
            }
        }
    }

}