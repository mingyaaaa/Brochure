﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    /// <summary>
    /// The query.
    /// </summary>
    public class Query<T1> : Query, IQuery<T1>
    {
        /// <summary>
        /// Selects the.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T> Select<T>(Expression<Func<T1, T>> expression = null)
        {
            var a = new Query<T>();
            this.SelectExpression = expression;
            base.CopyProperty(a);
            return a;
        }

        /// <summary>
        /// Joins the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1, T2> Join<T2>(Expression<Func<T1, T2, bool>> fun)
        {
            this.JoinExpression.Add((typeof(T2), fun));
            var a = new Query<T1, T2>();
            base.CopyProperty(a);
            return a;
        }

        /// <summary>
        /// Groupbies the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<IGrouping<T2, T1>> Groupby<T2>(Expression<Func<T1, T2>> fun)
        {
            this.GroupExpress = fun;
            var a = new Query<IGrouping<T2, T1>>();
            base.CopyProperty(a);
            return a;
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1> OrderBy(Expression<Func<T1, object>> fun)
        {
            OrderExpression = fun;
            return this;
        }

        /// <summary>
        /// Orders the by desc.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1> OrderByDesc(Expression<Func<T1, object>> fun)
        {
            OrderDescExpression = fun;
            return this;
        }

        /// <summary>
        /// Wheres the and.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1> WhereAnd(Expression<Func<T1, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.And, fun));
            return this;
        }

        /// <summary>
        /// Wheres the or.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1> WhereOr(Expression<Func<T1, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.Or, fun));
            return this;
        }

        /// <summary>
        /// Distincts the.
        /// </summary>
        /// <returns>An IQuery.</returns>
        public IQuery<T1> Distinct()
        {
            IsDistinct = true;
            return this;
        }

        /// <summary>
        /// Wheres the.
        /// </summary>
        /// <param name="fun">The fun.</param>
        /// <returns>An IQuery.</returns>
        public IQuery<T1> Where(Expression<Func<T1, bool>> fun)
        {
            WhereExpression = fun;
            return this;
        }

        public IQuery<T1> Take(int count)
        {
            TakeCount = count;
            return this;
        }

        public IQuery<T1> Skip(int count)
        {
            SkipCount = count;
            return this;
        }
    }
}