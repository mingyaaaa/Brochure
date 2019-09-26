using System;
using System.Linq.Expressions;

namespace LinqDbQuery.Querys
{
    public class Query<T1, T2, T3, T4, T5> : Query, IQuery<T1, T2, T3, T4, T5>
    {
        public Query () { }

        public Query (DbQueryOption option) : base (option)
        {

        }

        public IQuery<T1, T2, T3, T4, T5> OrderBy (Expression<Func<T1, T2, T3, T4, T5, object>> fun)
        {
            return base.Orderby<Query<T1, T2, T3, T4, T5>> (fun);
        }

        public IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T4, T5, T>> fun)
        {
            return base.Select<Query<T>> (fun);
        }

        public IQuery<T1, T2, T3, T4, T5> WhereAnd (Expression<Func<T1, T2, T3, T4, T5, bool>> fun)
        {
            return base.WhereAnd<Query<T1, T2, T3, T4, T5>> (fun);
        }
        public IQuery<T1, T2, T3, T4, T5> WhereOr (Expression<Func<T1, T2, T3, T4, T5, bool>> fun)
        {
            return base.WhereOr<Query<T1, T2, T3, T4, T5>> (fun);
        }
    }
}