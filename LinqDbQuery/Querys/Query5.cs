using System;
using System.Linq.Expressions;

namespace LinqDbQuery.Querys
{
    public class Query<T1, T2, T3, T4, T5> : Query, IQuery<T1, T2, T3, T4, T5>
    {
        public IQuery<T1, T2, T3, T4, T5> OrderBy (Expression<Func<T1, T2, T3, T4, T5, object>> fun)
        {
            return base.Orderby<Query<T1, T2, T3, T4, T5>> (fun);
        }

        public IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T4, T5, T>> fun)
        {
            return base.Select<Query<T>> (fun);
        }

        public IQuery<T1, T2, T3, T4, T5> Where (Expression<Func<T1, T2, T3, T4, T5, bool>> fun)
        {
            return base.Where<Query<T1, T2, T3, T4, T5>> (fun);
        }
    }
}