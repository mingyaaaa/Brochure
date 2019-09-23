using System;
using System.Linq;
using System.Linq.Expressions;
using AspectCore.Injector;
using LinqDbQuery.Visitors;
using Microsoft.Extensions.Logging;

namespace LinqDbQuery.Querys
{
    public class Query<T1> : Query, IQuery<T1>
    {
        private readonly string tableName;
        public Query () : this (new QueryOption ()) { }
        public Query (QueryOption option) : base (option)
        {
            this.tableName = ReflectedUtli.GetTableName (typeof (T1));
            var log = DI.Ins.ServiceProvider.Resolve<ILogger<Query>> ();
        }

        public string Sql { get; protected set; }

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
            return base.Groupby<Query<IGrouping<T1, T2>>> (fun);
        }

        public IQuery<T1, T2> Join<T2> (Expression<Func<T1, T2, bool>> fun)
        {
            return base.Join<Query<T1, T2>> (typeof (T2), fun);
        }

        public IQuery<T1> Orderby (Expression<Func<T1, object>> fun)
        {
            return base.Orderby<Query<T1>> (fun);
        }

        public IQuery<T> Select<T> (Expression<Func<T1, T>> fun)
        {
            return base.Select<Query<T>> (fun);
        }

        public IQuery<T1> Where (Expression<Func<T1, bool>> fun)
        {
            return base.Where<Query<T1>> (fun);
        }
    }

}