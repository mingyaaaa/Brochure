using System;
using System.Linq.Expressions;

namespace LinqDbQuery.Querys
{
    public class Query<T1, T2, T3> : Query, IQuery<T1, T2, T3>
    {
        public IQuery<T1, T2, T3, T4> Join<T4> (Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            return base.Join<Query<T1, T2, T3, T4>> (typeof (T4), fun);
        }

        public IQuery<T1, T2, T3> OrderBy (Expression<Func<T1, T2, T3, object>> fun)
        {
            return base.Orderby<Query<T1, T2, T3>> (fun);
        }

        public IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T>> fun)
        {
            return base.Select<Query<T>> (fun);
        }

        public IQuery<T1, T2, T3> Where (Expression<Func<T1, T2, T3, bool>> fun)
        {
            return base.Where<Query<T1, T2, T3>> (fun);
        }
    }
}