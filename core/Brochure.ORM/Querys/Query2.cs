using System;
using System.Linq;
using System.Linq.Expressions;
using Brochure.ORM.Visitors;

namespace Brochure.ORM.Querys
{
    public class Query<T1, T2> : Query, IQuery<T1, T2>
    {

        public Query (IDbProvider dbProvider, DbOption option, IVisitProvider visitProvider) : base (dbProvider, option, visitProvider) { }

        public IQuery<T1, T2, T3> Join<T3> (Expression<Func<T1, T2, T3, bool>> fun)
        {
            return base.Join<Query<T1, T2, T3>> (typeof (T3), fun);
        }

        public IQuery<T1, T2> OrderBy (Expression<Func<T1, T2, object>> fun)
        {
            return base.OrderBy<Query<T1, T2>> (fun);
        }

        public IQuery<T1, T2> OrderBy (string orderSql)
        {
            return base.OrderBy<Query<T1, T2>> (orderSql);
        }

        public IQuery<T1, T2> OrderByDesc (Expression<Func<T1, T2, object>> fun)
        {
            return base.OrderByDesc<Query<T1, T2>> (fun);
        }

        public IQuery<T1, T2> OrderByDesc (string orderSql)
        {
            return base.OrderByDesc<Query<T1, T2>> (orderSql);
        }

        public IQuery<T> Select<T> (Expression<Func<T1, T2, T>> fun)
        {
            return base.Select<Query<T>> (fun);
        }

        public IQuery<T1, T2> WhereAnd (Expression<Func<T1, T2, bool>> fun)
        {
            return base.WhereAnd<Query<T1, T2>> (fun);
        }

        public IQuery<T1, T2> WhereAnd (string whereSql)
        {
            throw new NotImplementedException ();
        }

        public IQuery<T1, T2> WhereOr (Expression<Func<T1, T2, bool>> fun)
        {
            return base.WhereOr<Query<T1, T2>> (fun);
        }

        public IQuery<T1, T2> WhereOr (string whereSql)
        {
            throw new NotImplementedException ();
        }
    }
}