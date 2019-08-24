using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqDbQuery
{
    public interface IQuery<T1>
    {
        IQuery<T1, T2> Join<T2>(Expression<Func<T1, T2, bool>> fun);
        IQuery<IGrouping<T1, object>> Groupby(Expression<Func<T1, object>> fun);
        IQuery<T1> Orderby(Expression<Func<T1, object>> fun);
        IQuery<T1> Where(Expression<Func<T1, bool>> fun);
    }
    public interface IQuery<T1, T2>
    {
        IQuery<T1, T2, T3> Join<T3>(Expression<Func<T1, T2, T3, bool>> fun);
        IQuery<T1, T2> Where(Expression<Func<T1, T2, bool>> fun);
    }
    public interface IQuery<T1, T2, T3>
    {
        IQuery<T1, T2, T3, T4> Join<T4>(Expression<Func<T1, T2, T3, T4, bool>> fun);
        IQuery<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> fun);
    }
    public interface IQuery<T1, T2, T3, T4>
    {
        IQuery<T1, T2, T3, T4, T5> Join<T5>(Expression<Func<T1, T2, T3, T4, T5, bool>> fun);
        IQuery<T1, T2, T3, T4> Where(Expression<Func<T1, T2, T3, T4, bool>> fun);
    }
    public interface IQuery<T1, T2, T3, T4, T5>
    {
        IQuery<T1, T2, T3, T4, T5> Where(Expression<Func<T1, T2, T3, T4, T5, bool>> fun);
    }
    public static class SelectQueryExtends
    {
        public static IQuery<T> Select<T1, T>(this IQuery<T1> query, Expression<Func<T1, T>> fun)
        {
            return null;
        }
        public static IQuery<T> Select<T1, T2, T>(this IQuery<T1, T2, T> query, Expression<Func<T1, T2, T>> fun)
        {
            return null;
        }
        public static IQuery<T> Select<T1, T2, T3, T>(this IQuery<T1, T2, T3> query,
            Expression<Func<T1, T2, T3, T>> fun)
        {
            return null;
        }
        public static IQuery<T> Select<T1, T2, T3, T4, T>(this IQuery<T1, T2, T3, T4> query,
    Expression<Func<T1, T2, T3, T4, T>> fun)
        {
            return null;
        }

    }

}
