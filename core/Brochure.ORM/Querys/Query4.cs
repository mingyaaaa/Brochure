using System;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    public class Query<T1, T2, T3, T4> : Query, IQuery<T1, T2, T3, T4>
    {
        public IQuery<T1, T2, T3, T4> Select(Expression<Func<T1, T2, T3, T4, object>> expression)
        {
            this.SelectExpression = expression;
            return this;
        }

        public IQuery<T1, T2, T3, T4, T5> Join<T5>(Expression<Func<T1, T2, T3, T4, T5, bool>> fun)
        {
            this.JoinExpression.Add((typeof(T5), fun));
            var a = new Query<T1, T2, T3, T4, T5>();
            base.CopyProperty(a);
            return a;
        }

        public IQuery<T1, T2, T3, T4> OrderBy(Expression<Func<T1, T2, T3, T4, object>> fun)
        {
            OrderExpression = fun;
            return this;
        }

        public IQuery<T1, T2, T3, T4> OrderBy(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2, T3, T4> OrderByDesc(Expression<Func<T1, T2, T3, T4, object>> fun)
        {
            OrderDescExpression = fun;
            return this;
        }

        public IQuery<T1, T2, T3, T4> OrderByDesc(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T> Select<T>(Expression<Func<T1, T2, T3, T4, T>> fun)
        {

            this.SelectExpression = fun;
            var a = new Query<T>();
            base.CopyProperty(a);
            return a;
        }

        public IQuery<T1, T2, T3, T4> WhereAnd(Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.And, fun));
            return this;
        }

        public IQuery<T1, T2, T3, T4> WhereAnd(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2, T3, T4> WhereOr(Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.Or, fun));
            return this;
        }

        public IQuery<T1, T2, T3, T4> WhereOr(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2, T3, T4> Where(Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            WhereExpression = fun;
            return this;
        }
    }
}