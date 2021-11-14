using System;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    /// <summary>
    /// The query.
    /// </summary>
    public class Query<T1, T2, T3, T4> : Query, IQuery<T1, T2, T3, T4>
    {
        /// <summary>
        /// Selects the.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3, T4> Select(Expression<Func<T1, T2, T3, T4, object>> expression)
        {
            this.SelectExpression = expression;
            return this;
        }

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3, T4, T5> Join<T5>(IQuery<T5> query, Expression<Func<T1, T2, T3, T4, T5, bool>> fun)
        {
            if (query == null)
                this.JoinExpression.Add((new TableNameSubQueryType(typeof(T5)), fun));
            else
                JoinExpression.Add((new QuerySubQueryType(query), fun));
            var a = new Query<T1, T2, T3, T4, T5>();
            base.CopyProperty(a);
            return a;
        }

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3, T4, T5> Join<T5>(Expression<Func<T1, T2, T3, T4, T5, bool>> fun)
        {
            return Join(null, fun);
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3, T4> OrderBy(Expression<Func<T1, T2, T3, T4, object>> fun)
        {
            OrderExpression = fun;
            return this;
        }

        /// <summary>
        /// Orders the by desc.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3, T4> OrderByDesc(Expression<Func<T1, T2, T3, T4, object>> fun)
        {
            OrderDescExpression = fun;
            return this;
        }

        /// <summary>
        /// Selects the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T> Select<T>(Expression<Func<T1, T2, T3, T4, T>> fun)
        {
            this.SelectExpression = fun;
            var a = new Query<T>();
            base.CopyProperty(a);
            return a;
        }

        /// <summary>
        /// Wheres the and.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3, T4> WhereAnd(Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.And, fun));
            return this;
        }

        /// <summary>
        /// Wheres the or.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3, T4> WhereOr(Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.Or, fun));
            return this;
        }

        /// <summary>
        /// Wheres the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3, T4> Where(Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            WhereExpression = fun;
            return this;
        }
    }
}