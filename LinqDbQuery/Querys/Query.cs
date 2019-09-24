using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using LinqDbQuery.Querys;
using LinqDbQuery.Visitors;
using Microsoft.Extensions.Logging;

namespace LinqDbQuery
{
    public abstract class Query : IQuery
    {

        protected QueryOption option { get; private set; }

        public Query () : this (new QueryOption ())
        {

        }
        public Query (QueryOption option)
        {
            this.option = option;
        }
        protected string selectSql;
        protected string whereSql;

        protected List<string> tableNames;

        protected string groupSql;
        protected string joinSql;
        protected string orderSql;
        protected List<IDbDataParameter> parameters;
        protected string GetSql ()
        {
            return $"{AddWhiteSpace(selectSql)}{AddWhiteSpace(joinSql)}{AddWhiteSpace(groupSql)}{AddWhiteSpace( whereSql)}{AddWhiteSpace(orderSql)}".Trim ();
        }
        private string AddWhiteSpace (string str)
        {
            if (!string.IsNullOrWhiteSpace (str))
                return str + " ";
            return string.Empty;
        }

        public int Count ()
        {
            selectSql = $"select count(1) from {string.Join(',',tableNames)}";
            var sql = GetSql ();
            var result = -1;
            using (var conn = this.option.GetDbConnection ())
            {
                var command = conn.CreateCommand ();
                command.CommandText = sql;
                command.CommandTimeout = this.option.Timeout;
                if (option.DbProvider.IsUseParamers)
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
            var joinVisitor = new JoinVisitor (type, this.option.DbProvider);
            joinVisitor.Visit (expression);
            if (string.IsNullOrWhiteSpace (joinSql))
                joinSql = $"{joinVisitor.GetSql()}";
            else
                joinSql = $"{joinSql} {joinVisitor.GetSql()}";
            return this.Copy<T> ();
        }

        protected T Orderby<T> (Expression fun) where T : Query
        {
            var orderVisitor = new OrderVisitor (this.option.DbProvider);
            orderVisitor.Visit (fun);
            orderSql = orderVisitor.GetSql ()?.ToString () ?? string.Empty;
            return this as T;
        }

        public T Select<T> (Expression fun) where T : Query
        {
            var selectVisitor = new SelectVisitor (this.option.DbProvider);
            selectVisitor.Visit (fun);
            selectSql = selectVisitor.GetSql ()?.ToString () ?? string.Empty;
            return this.Copy<T> ();
        }
        public T WhereAnd<T> (Expression fun) where T : Query
        {
            var whereVisitor = new WhereVisitor (this.option.DbProvider);
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
            var whereVisitor = new WhereVisitor (this.option.DbProvider);
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
            var groupVisit = new GroupVisitor (this.option.DbProvider);
            groupVisit.Visit (fun);
            groupSql = groupVisit.GetSql ().ToString ();
            return this.Copy<T> ();
        }

        protected T Copy<T> () where T : Query
        {
            var query = Activator.CreateInstance<T> ();
            query.selectSql = this.selectSql;
            query.whereSql = this.whereSql;
            query.groupSql = this.groupSql;
            query.joinSql = this.joinSql;
            query.orderSql = this.orderSql;
            query.parameters = this.parameters;
            query.tableNames = this.tableNames;
            query.option = this.option;
            return query;
        }
    }

}