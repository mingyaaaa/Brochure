using System;
using System.Linq.Expressions;
using Brochure.ORM.Visitors;

namespace Brochure.ORM.Querys
{
    public class Query<T1, T2, T3> : Query, IQuery<T1, T2, T3>
    {

        public Query (IDbProvider dbProvider, DbOption option, IVisitProvider visitProvider) : base (dbProvider, option, visitProvider) { }

        public IQuery<T1, T2, T3, T4> Join<T4> (Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            return base.Join<Query<T1, T2, T3, T4>> (typeof (T4), fun);
        }

        public IQuery<T1, T2, T3> OrderBy (Expression<Func<T1, T2, T3, object>> fun)
        {
            return base.OrderBy<Query<T1, T2, T3>> (fun);
        }

        public IQuery<T1, T2, T3> OrderBy (string orderSql)
        {
            return base.OrderBy<Query<T1, T2, T3>> (orderSql);
        }

        public IQuery<T1, T2, T3> OrderByDesc (Expression<Func<T1, T2, T3, object>> fun)
        {
            return base.OrderByDesc<Query<T1, T2, T3>> (fun);
        }

        public IQuery<T1, T2, T3> OrderByDesc (string orderSql)
        {
            return base.OrderByDesc<Query<T1, T2, T3>> (orderSql);
        }

        public IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T>> fun)
        {
            return base.Select<Query<T>> (fun);
        }

        public IQuery<T1, T2, T3> WhereAnd (Expression<Func<T1, T2, T3, bool>> fun)
        {
            return base.WhereAnd<Query<T1, T2, T3>> (fun);
        }

        public IQuery<T1, T2, T3> WhereAnd (string whereSql)
        {
            return base.WhereAnd<Query<T1, T2, T3>> (whereSql);
        }

        public IQuery<T1, T2, T3> WhereOr (Expression<Func<T1, T2, T3, bool>> fun)
        {
            return base.WhereOr<Query<T1, T2, T3>> (fun);
        }

        public IQuery<T1, T2, T3> WhereOr (string whereSql)
        {
            return base.WhereOr<Query<T1, T2, T3>> (whereSql);
        }
    }
}