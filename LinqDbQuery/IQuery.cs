using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqDbQuery
{

    public interface IQuery<T1>
    {
        IQuery<T> Select<T> (Expression<Func<T1, T>> fun);
        IQuery<T1, T2> Join<T2> (Expression<Func<T1, T2, bool>> fun);
        IQuery<IGrouping<T1, T2>> Groupby<T2> (Expression<Func<T1, T2>> fun);
        IQuery<T1> Orderby (Expression<Func<T1, object>> fun);
        IQuery<T1> Where (Expression<Func<T1, bool>> fun);
        int Count (Expression<Func<T1, bool>> func);
        IQuery<T1> Distinct ();
    }
    public interface IQuery<T1, T2>
    {
        IQuery<T> Select<T> (Expression<Func<T1, T2, T>> fun);
        IQuery<T1, T2, T3> Join<T3> (Expression<Func<T1, T2, T3, bool>> fun);
        IQuery<T1, T2> Where (Expression<Func<T1, T2, bool>> fun);
        IQuery<T1, T2> OrderBy (Expression<Func<T1, T2, object>> fun);

    }
    public interface IQuery<T1, T2, T3>
    {
        IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T>> fun);
        IQuery<T1, T2, T3, T4> Join<T4> (Expression<Func<T1, T2, T3, T4, bool>> fun);
        IQuery<T1, T2, T3> Where (Expression<Func<T1, T2, T3, bool>> fun);
        IQuery<T1, T2, T3> OrderBy (Expression<Func<T1, T2, T3, object>> fun);
    }
    public interface IQuery<T1, T2, T3, T4>
    {
        IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T4, T>> fun);
        IQuery<T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T1, T2, T3, T4, T5, bool>> fun);
        IQuery<T1, T2, T3, T4> Where (Expression<Func<T1, T2, T3, T4, bool>> fun);
        IQuery<T1, T2, T3, T4> OrderBy (Expression<Func<T1, T2, T3, T4, object>> fun);
    }
    public interface IQuery<T1, T2, T3, T4, T5>
    {
        IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T4, T5, T>> fun);
        IQuery<T1, T2, T3, T4, T5> Where (Expression<Func<T1, T2, T3, T4, T5, bool>> fun);
        IQuery<T1, T2, T3, T4, T5> OrderBy (Expression<Func<T1, T2, T3, T4, T5, object>> fun);
    }

}