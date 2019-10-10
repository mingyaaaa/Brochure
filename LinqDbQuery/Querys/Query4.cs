using System;
using System.Linq.Expressions;

namespace LinqDbQuery.Querys
{
    public class Query<T1, T2, T3, T4> : Query, IQuery<T1, T2, T3, T4>
    {
        public Query () { }

        public Query (DbQueryOption option) : base (option)
        {

        }

        public IQuery<T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T1, T2, T3, T4, T5, bool>> fun)
        {
            return base.Join<Query<T1, T2, T3, T4, T5>> (typeof (T5), fun);
        }

        public IQuery<T1, T2, T3, T4> OrderBy (Expression<Func<T1, T2, T3, T4, object>> fun)
        {
            return base.OrderBy<Query<T1, T2, T3, T4>> (fun);
        }

        public IQuery<T1, T2, T3, T4> OrderByDesc (Expression<Func<T1, T2, T3, T4, object>> fun)
        {
            return base.OrderByDesc<Query<T1, T2, T3, T4>> (fun);
        }

        public IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T4, T>> fun)
        {
            return base.Select<Query<T>> (fun);
        }

        public IQuery<T1, T2, T3, T4> WhereAnd (Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            return base.WhereAnd<Query<T1, T2, T3, T4>> (fun);
        }
        public IQuery<T1, T2, T3, T4> WhereOr (Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            return base.WhereOr<Query<T1, T2, T3, T4>> (fun);
        }
    }
}