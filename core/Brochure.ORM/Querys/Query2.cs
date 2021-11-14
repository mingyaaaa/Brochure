using System;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    /// <summary>
    /// The query.
    /// </summary>
    public class Query<T1, T2> : Query, IQuery<T1, T2>
    {
        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2, T3> Join<T3>(Expression<Func<T1, T2, T3, bool>> fun)
        {
            return Join(null, fun);
        }

        public IQuery<T1, T2, T3> Join<T3>(IQuery<T3> query, Expression<Func<T1, T2, T3, bool>> fun)
        {
            if (query == null)
                this.JoinExpression.Add((new TableNameSubQueryType(typeof(T3)), fun));
            else
                JoinExpression.Add((new QuerySubQueryType(query), fun));
            var a = new Query<T1, T2, T3>();
            base.CopyProperty(a);
            return a;
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2> OrderBy(Expression<Func<T1, T2, object>> fun)
        {
            OrderExpression = fun;
            return this;
        }

        /// <summary>
        /// Orders the by desc.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2> OrderByDesc(Expression<Func<T1, T2, object>> fun)
        {
            OrderDescExpression = fun;
            return this;
        }

        /// <summary>
        /// Selects the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T> Select<T>(Expression<Func<T1, T2, T>> fun)
        {
            var a = new Query<T>();
            this.SelectExpression = fun;
            base.CopyProperty(a);
            return a;
        }

        /// <summary>
        /// Wheres the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2> Where(Expression<Func<T1, T2, bool>> fun)
        {
            WhereExpression = fun;
            return this;
        }

        /// <summary>
        /// Wheres the and.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2> WhereAnd(Expression<Func<T1, T2, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.And, fun));
            return this;
        }

        /// <summary>
        /// Wheres the or.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2> WhereOr(Expression<Func<T1, T2, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.Or, fun));
            return this;
        }
    }
}