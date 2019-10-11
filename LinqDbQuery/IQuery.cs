using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace LinqDbQuery
{
    public interface IQuery
    {
        string GetSql ();
        List<IDbDataParameter> GetDbDataParameters ();
    }

    public interface IQuery<T1> : IQuery
    {
        IQuery<T> Select<T> (Expression<Func<T1, T>> fun);
        IQuery<T1, T2> Join<T2> (Expression<Func<T1, T2, bool>> fun);
        IQuery<IGrouping<T2, T1>> Groupby<T2> (Expression<Func<T1, T2>> fun);
        IQuery<T1> OrderBy (Expression<Func<T1, object>> fun);
        IQuery<T1> OrderbyDesc (Expression<Func<T1, object>> fun);
        IQuery<T1> WhereAnd (Expression<Func<T1, bool>> fun);
        IQuery<T1> WhereOr (Expression<Func<T1, bool>> fun);
        IQuery<T1> Distinct ();

        int Insert (T1 obj);

        int InsertMany (IEnumerable<T1> objs);

        T1 Update (object obj, Expression<Func<T1, bool>> Func);

        int Delete (Expression<Func<T1, bool>> fun);

    }

    public interface IQuery<T1, T2> : IQuery
    {
        IQuery<T> Select<T> (Expression<Func<T1, T2, T>> fun);
        IQuery<T1, T2, T3> Join<T3> (Expression<Func<T1, T2, T3, bool>> fun);
        IQuery<T1, T2> WhereAnd (Expression<Func<T1, T2, bool>> fun);
        IQuery<T1, T2> WhereOr (Expression<Func<T1, T2, bool>> fun);
        IQuery<T1, T2> OrderBy (Expression<Func<T1, T2, object>> fun);
        IQuery<T1, T2> OrderByDesc (Expression<Func<T1, T2, object>> fun);
    }

    public interface IQuery<T1, T2, T3>
    {
        IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T>> fun);
        IQuery<T1, T2, T3, T4> Join<T4> (Expression<Func<T1, T2, T3, T4, bool>> fun);
        IQuery<T1, T2, T3> WhereAnd (Expression<Func<T1, T2, T3, bool>> fun);
        IQuery<T1, T2, T3> WhereOr (Expression<Func<T1, T2, T3, bool>> fun);
        IQuery<T1, T2, T3> OrderBy (Expression<Func<T1, T2, T3, object>> fun);
        IQuery<T1, T2, T3> OrderByDesc (Expression<Func<T1, T2, T3, object>> fun);
    }

    public interface IQuery<T1, T2, T3, T4> : IQuery
    {
        IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T4, T>> fun);
        IQuery<T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T1, T2, T3, T4, T5, bool>> fun);
        IQuery<T1, T2, T3, T4> WhereAnd (Expression<Func<T1, T2, T3, T4, bool>> fun);
        IQuery<T1, T2, T3, T4> WhereOr (Expression<Func<T1, T2, T3, T4, bool>> fun);
        IQuery<T1, T2, T3, T4> OrderBy (Expression<Func<T1, T2, T3, T4, object>> fun);
        IQuery<T1, T2, T3, T4> OrderByDesc (Expression<Func<T1, T2, T3, T4, object>> fun);
    }

    public interface IQuery<T1, T2, T3, T4, T5> : IQuery
    {
        IQuery<T> Select<T> (Expression<Func<T1, T2, T3, T4, T5, T>> fun);
        IQuery<T1, T2, T3, T4, T5> WhereAnd (Expression<Func<T1, T2, T3, T4, T5, bool>> fun);
        IQuery<T1, T2, T3, T4, T5> WhereOr (Expression<Func<T1, T2, T3, T4, T5, bool>> fun);
        IQuery<T1, T2, T3, T4, T5> OrderBy (Expression<Func<T1, T2, T3, T4, T5, object>> fun);
        IQuery<T1, T2, T3, T4, T5> OrderByDesc (Expression<Func<T1, T2, T3, T4, T5, object>> fun);
    }
}