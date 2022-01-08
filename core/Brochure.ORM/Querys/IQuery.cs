using Brochure.ORM.Querys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Brochure.ORM
{
    /// <summary>
    /// The query.
    /// </summary>
    public interface IQuery : ISql
    {
        /// <summary>
        /// Gets the take count.
        /// </summary>
        int TakeCount { get; }

        /// <summary>
        /// Gets the skip count.
        /// </summary>
        int SkipCount { get; }

        /// <summary>
        /// Gets a value indicating whether is distinct.
        /// </summary>
        bool IsDistinct { get; }

        /// <summary>
        /// Gets the main tables.
        /// </summary>
        List<BaseSubQueryType> MainTables { get; }

        /// <summary>
        /// Gets the select expression.
        /// </summary>
        Expression SelectExpression { get; }

        /// <summary>
        /// Gets the where expression.
        /// </summary>
        Expression WhereExpression { get; }

        /// <summary>
        /// Gets the where list expression.
        /// </summary>
        IList<(string, Expression)> WhereListExpression { get; }

        /// <summary>
        /// Gets the order expression.
        /// </summary>
        Expression OrderExpression { get; }

        /// <summary>
        /// Gets the order desc expression.
        /// </summary>
        Expression OrderDescExpression { get; }

        /// <summary>
        /// Gets the join expression.
        /// </summary>
        IList<(BaseSubQueryType, Expression)> JoinExpression { get; }

        /// <summary>
        /// Gets the left join express.
        /// </summary>
        IList<(BaseSubQueryType, Expression)> LeftJoinExpress { get; }

        /// <summary>
        /// Gets the group express.
        /// </summary>
        Expression GroupExpress { get; }

        /// <summary>
        /// 判断是否只有Where的sql语句
        /// </summary>
        /// <returns>A bool.</returns>
        bool IsWhereSql();
    }

    /// <summary>
    /// The query.
    /// </summary>
    public interface IQuery<T1> : IQuery
    {
        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2> Join<T2>(Expression<Func<T1, T2, bool>> fun);

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="subQuery">The sub query.</param>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2> Join<T2>(IQuery<T2> subQuery, Expression<Func<T1, T2, bool>> fun);

        /// <summary>
        /// Groupbies the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<IGrouping<T2, T1>> Groupby<T2>(Expression<Func<T1, T2>> fun);

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1> OrderBy(Expression<Func<T1, object>> fun);

        /// <summary>
        /// Orders the by desc.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1> OrderByDesc(Expression<Func<T1, object>> fun);

        /// <summary>
        /// Wheres the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1> Where(Expression<Func<T1, bool>> fun);

        /// <summary>
        /// Wheres the and.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1> WhereAnd(Expression<Func<T1, bool>> fun);

        /// <summary>
        /// Wheres the or.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1> WhereOr(Expression<Func<T1, bool>> fun);

        /// <summary>
        /// Distincts the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        IQuery<T1> Distinct();

        /// <summary>
        /// Takes the.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1> Take(int count);

        /// <summary>
        /// Skips the.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1> Skip(int count);

        /// <summary>
        /// Selects the.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T> Select<T>(Expression<Func<T1, T>> expression = null);

        /// <summary>
        /// Selects the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        IQuery<T1> Select();
    }

    /// <summary>
    /// The query.
    /// </summary>
    public interface IQuery<T1, T2> : IQuery
    {
        /// <summary>
        /// Selects the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T> Select<T>(Expression<Func<T1, T2, T>> fun);

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3> Join<T3>(Expression<Func<T1, T2, T3, bool>> fun);

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3> Join<T3>(IQuery<T3> query, Expression<Func<T1, T2, T3, bool>> fun);

        /// <summary>
        /// Wheres the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2> Where(Expression<Func<T1, T2, bool>> fun);

        /// <summary>
        /// Wheres the and.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2> WhereAnd(Expression<Func<T1, T2, bool>> fun);

        /// <summary>
        /// Wheres the or.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2> WhereOr(Expression<Func<T1, T2, bool>> fun);

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2> OrderBy(Expression<Func<T1, T2, object>> fun);

        /// <summary>
        /// Orders the by desc.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2> OrderByDesc(Expression<Func<T1, T2, object>> fun);
    }

    /// <summary>
    /// The query.
    /// </summary>
    public interface IQuery<T1, T2, T3>
    {
        /// <summary>
        /// Selects the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T> Select<T>(Expression<Func<T1, T2, T3, T>> fun);

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3, T4> Join<T4>(Expression<Func<T1, T2, T3, T4, bool>> fun);

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3, T4> Join<T4>(IQuery<T4> query, Expression<Func<T1, T2, T3, T4, bool>> fun);

        /// <summary>
        /// Wheres the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> fun);

        /// <summary>
        /// Wheres the and.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2, T3> WhereAnd(Expression<Func<T1, T2, T3, bool>> fun);

        /// <summary>
        /// Wheres the or.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2, T3> WhereOr(Expression<Func<T1, T2, T3, bool>> fun);

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, object>> fun);

        /// <summary>
        /// Orders the by desc.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3> OrderByDesc(Expression<Func<T1, T2, T3, object>> fun);
    }

    /// <summary>
    /// The query.
    /// </summary>
    public interface IQuery<T1, T2, T3, T4> : IQuery
    {
        /// <summary>
        /// Selects the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T> Select<T>(Expression<Func<T1, T2, T3, T4, T>> fun);

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3, T4, T5> Join<T5>(Expression<Func<T1, T2, T3, T4, T5, bool>> fun);

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3, T4, T5> Join<T5>(IQuery<T5> query, Expression<Func<T1, T2, T3, T4, T5, bool>> fun);

        /// <summary>
        /// Wheres the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2, T3, T4> Where(Expression<Func<T1, T2, T3, T4, bool>> fun);

        /// <summary>
        /// Wheres the and.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2, T3, T4> WhereAnd(Expression<Func<T1, T2, T3, T4, bool>> fun);

        /// <summary>
        /// Wheres the or.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2, T3, T4> WhereOr(Expression<Func<T1, T2, T3, T4, bool>> fun);

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3, T4> OrderBy(Expression<Func<T1, T2, T3, T4, object>> fun);

        /// <summary>
        /// Orders the by desc.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3, T4> OrderByDesc(Expression<Func<T1, T2, T3, T4, object>> fun);
    }

    /// <summary>
    /// The query.
    /// </summary>
    public interface IQuery<T1, T2, T3, T4, T5> : IQuery
    {
        /// <summary>
        /// Selects the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T> Select<T>(Expression<Func<T1, T2, T3, T4, T5, T>> fun);

        /// <summary>
        /// Wheres the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2, T3, T4, T5> Where(Expression<Func<T1, T2, T3, T4, T5, bool>> fun);

        /// <summary>
        /// Wheres the and.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2, T3, T4, T5> WhereAnd(Expression<Func<T1, T2, T3, T4, T5, bool>> fun);

        /// <summary>
        /// Wheres the or.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IWhereQuery<T1, T2, T3, T4, T5> WhereOr(Expression<Func<T1, T2, T3, T4, T5, bool>> fun);

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3, T4, T5> OrderBy(Expression<Func<T1, T2, T3, T4, T5, object>> fun);

        /// <summary>
        /// Orders the by desc.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        IQuery<T1, T2, T3, T4, T5> OrderByDesc(Expression<Func<T1, T2, T3, T4, T5, object>> fun);
    }
}