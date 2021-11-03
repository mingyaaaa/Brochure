using System;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    public class Query<T1, T2, T3, T4, T5> : Query, IQuery<T1, T2, T3, T4, T5>
    {
        public IQuery<T1, T2, T3, T4, T5> Select(Expression<Func<T1, T2, T3, T4, T5, object>> expression)
        {
            this.SelectExpression = expression;
            return this;
        }

        public IQuery<T1, T2, T3, T4, T5> OrderBy(Expression<Func<T1, T2, T3, T4, T5, object>> fun)
        {
            OrderExpression = fun;
            return this;
        }

        public IQuery<T1, T2, T3, T4, T5> OrderBy(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2, T3, T4, T5> OrderByDesc(Expression<Func<T1, T2, T3, T4, T5, object>> fun)
        {
            OrderDescExpression = fun;
            return this;
        }

        public IQuery<T1, T2, T3, T4, T5> OrderByDesc(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T> Select<T>(Expression<Func<T1, T2, T3, T4, T5, T>> fun)
        {
            this.SelectExpression = fun;
            var a = new Query<T>();
            base.CopyProperty(a);
            return a;
        }

        public IQuery<T1, T2, T3, T4, T5> WhereAnd(Expression<Func<T1, T2, T3, T4, T5, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.And, fun));
            return this;
        }

        public IQuery<T1, T2, T3, T4, T5> WhereAnd(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2, T3, T4, T5> WhereOr(Expression<Func<T1, T2, T3, T4, T5, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.Or, fun));
            return this;
        }

        public IQuery<T1, T2, T3, T4, T5> WhereOr(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2, T3, T4, T5> Where(Expression<Func<T1, T2, T3, T4, T5, bool>> fun)
        {
            WhereExpression = fun;
            return this;
        }
    }
}