using System;
using System.Linq;
using System.Linq.Expressions;
using AspectCore.Injector;
using LinqDbQuery.Visitors;
using Microsoft.Extensions.Logging;

namespace LinqDbQuery
{
    public class Query : IQuery
    {
        protected string selectSql;
        protected string whereSql;

        protected string groupSql;
        protected string joinSql;
        protected string orderSql;

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
        protected IQuery<T1> Copy<T1> ()
        {
            var query = new Query<T1> ();
            query.selectSql = this.selectSql;
            query.whereSql = this.whereSql;
            query.groupSql = this.groupSql;
            query.joinSql = this.joinSql;
            query.orderSql = this.orderSql;
            return query;
        }
    }
    public class Query<T1> : Query, IQuery<T1>
    {
        private readonly QueryOption option;
        private readonly string tableName;
        public Query () : this (new QueryOption ()) { }
        public Query (QueryOption option)
        {
            this.option = option;
            this.tableName = ReflectedUtli.GetTableName (typeof (T1));
            var log = DI.Ins.ServiceProvider.Resolve<ILogger<Query>> ();
        }

        public string Sql { get; protected set; }

        public int Count (Expression<Func<T1, bool>> func)
        {
            var whereVisit = new WhereVisitor (this.option.DbProvider);
            whereVisit.Visit (func);
            whereSql = whereVisit.GetSql ().ToString ();
            selectSql = $"select count(1) from {tableName}";
            var sql = GetSql ();
            var result = -1;
            using (var conn = this.option.GetDbConnection ())
            {
                var command = conn.CreateCommand ();
                command.CommandText = sql;
                command.CommandTimeout = this.option.Timeout;
                if (option.DbProvider.IsUseParamers)
                {
                    var paramlist = whereVisit.GetParameters ().ToArray ();
                    foreach (var item in paramlist)
                        command.Parameters.Add (item);
                }
                result = command.ExecuteNonQuery ();
            }
            return result;
        }

        public IQuery<T1> Distinct ()
        {
            if (string.IsNullOrWhiteSpace (selectSql))
            {
                selectSql = $"select distinct * from {tableName}";
            }
            else
            {
                selectSql = "select distinct" + selectSql.Trim ().Substring (5);
            }
            return this;
        }

        public IQuery<System.Linq.IGrouping<T1, T2>> Groupby<T2> (Expression<Func<T1, T2>> fun)
        {
            var groupVisit = new GroupVisitor (this.option.DbProvider);
            groupVisit.Visit (fun);
            groupSql = groupVisit.GetSql ().ToString ();
            return this.Copy<IGrouping<T1, T2>> ();
        }

        public IQuery<T1, T2> Join<T2> (Expression<Func<T1, T2, bool>> fun)
        {
            throw new NotImplementedException ();
        }

        public IQuery<T1> Orderby (Expression<Func<T1, object>> fun)
        {
            throw new NotImplementedException ();
        }

        public IQuery<T> Select<T> (Expression<Func<T1, T>> fun)
        {
            throw new NotImplementedException ();
        }

        public IQuery<T1> Where (Expression<Func<T1, bool>> fun)
        {
            throw new NotImplementedException ();
        }
    }
}