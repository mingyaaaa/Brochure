using System;
using System.Linq;
using System.Linq.Expressions;
using Brochure.ORM.Visitors;
using Microsoft.Extensions.Logging;

namespace Brochure.ORM.Querys
{
    public class Query<T1> : Query, IQuery<T1>
    {

        public Query (IDbProvider dbProvider, DbOption option, IVisitProvider visitProvider) : base (dbProvider, option, visitProvider) { }

        public string Sql { get; protected set; }

        public IQuery<T1> Distinct ()
        {
            if (string.IsNullOrWhiteSpace (selectSql))
            {
                selectSql = $"select distinct * from {JoinTableNames()}";
            }
            else
            {
                selectSql = "select distinct" + selectSql.Trim ().Substring (5);
            }
            return this;
        }

        public IQuery<System.Linq.IGrouping<T2, T1>> Groupby<T2> (Expression<Func<T1, T2>> fun)
        {
            return base.Groupby<Query<IGrouping<T2, T1>>> (fun);
        }

        public IQuery<T1, T2> Join<T2> (Expression<Func<T1, T2, bool>> fun)
        {
            return base.Join<Query<T1, T2>> (typeof (T2), fun);
        }

        public IQuery<T1> OrderBy (Expression<Func<T1, object>> fun)
        {
            return base.OrderBy<Query<T1>> (fun);
        }

        public IQuery<T1> OrderBy (string orderSql)
        {
            return base.OrderBy<Query<T1>> (orderSql);
        }

        public IQuery<T1> OrderbyDesc (Expression<Func<T1, object>> fun)
        {
            return base.OrderByDesc<Query<T1>> (fun);
        }

        public IQuery<T1> OrderbyDesc (string orderSql)
        {
            return base.OrderByDesc<Query<T1>> (orderSql);
        }

        public IQuery<T> Select<T> (Expression<Func<T1, T>> fun)
        {
            return base.Select<Query<T>> (fun);
        }

        public IQuery<T1> WhereAnd (Expression<Func<T1, bool>> fun)
        {
            return base.WhereAnd<Query<T1>> (fun);
        }

        public IQuery<T1> WhereAnd (string whereSql)
        {
            return base.WhereAnd<Query<T1>> (whereSql);
        }

        public IQuery<T1> WhereOr (Expression<Func<T1, bool>> fun)
        {
            return base.WhereOr<Query<T1>> (fun);
        }

        public IQuery<T1> WhereOr (string whereSql)
        {
            return base.WhereOr<Query<T1>> (whereSql);
        }
    }
}