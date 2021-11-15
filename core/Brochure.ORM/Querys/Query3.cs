using System;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    /// <summary>
    /// The query.
    /// </summary>
    public class Query<T1, T2, T3> : Query, IQuery<T1, T2, T3>, IWhereQuery<T1, T2, T3>
    {
        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3, T4> Join<T4>(Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            return Join(null, fun);
        }

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3, T4> Join<T4>(IQuery<T4> query, Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            if (query == null)
                this.JoinExpression.Add((new TableNameSubQueryType(typeof(T4)), fun));
            else
                JoinExpression.Add((new QuerySubQueryType(query), fun));
            var a = new Query<T1, T2, T3, T4>();
            base.CopyProperty(a);
            return a;
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, object>> fun)
        {
            OrderExpression = fun;
            return this;
        }

        /// <summary>
        /// Orders the by desc.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3> OrderByDesc(Expression<Func<T1, T2, T3, object>> fun)
        {
            OrderDescExpression = fun;
            return this;
        }

        /// <summary>
        /// Selects the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T> Select<T>(Expression<Func<T1, T2, T3, T>> fun)
        {
            this.SelectExpression = fun;
            var a = new Query<T>();
            base.CopyProperty(a);
            return a;
        }

        /// <summary>
        /// Wheres the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IWhereQuery<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> fun)
        {
            WhereExpression = fun;
            return this;
        }

        /// <summary>
        /// Wheres the and.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IWhereQuery<T1, T2, T3> WhereAnd(Expression<Func<T1, T2, T3, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.And, fun));
            return this;
        }

        /// <summary>
        /// Wheres the or.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IWhereQuery<T1, T2, T3> WhereOr(Expression<Func<T1, T2, T3, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.Or, fun));
            return this;
        }
    }
}