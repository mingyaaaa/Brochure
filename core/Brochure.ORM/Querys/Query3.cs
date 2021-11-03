using System;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    public class Query<T1, T2, T3> : Query, IQuery<T1, T2, T3>
    {
        public IQuery<T1, T2, T3, T4> Join<T4>(Expression<Func<T1, T2, T3, T4, bool>> fun)
        {
            this.JoinExpression.Add((typeof(T4), fun));
            var a = new Query<T1, T2, T3, T4>();
            base.CopyProperty(a);
            return a;
        }

        public IQuery<T1, T2, T3> OrderBy(Expression<Func<T1, T2, T3, object>> fun)
        {
            OrderExpression = fun;
            return this;
        }

        public IQuery<T1, T2, T3> OrderBy(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2, T3> OrderByDesc(Expression<Func<T1, T2, T3, object>> fun)
        {
            OrderDescExpression = fun;
            return this;
        }

        public IQuery<T1, T2, T3> OrderByDesc(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T> Select<T>(Expression<Func<T1, T2, T3, T>> fun)
        {
            this.SelectExpression = fun;
            var a = new Query<T>();
            base.CopyProperty(a);
            return a;
        }

        public IQuery<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> fun)
        {
            WhereExpression = fun;
            return this;
        }

        public IQuery<T1, T2, T3> WhereAnd(Expression<Func<T1, T2, T3, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.And, fun));
            return this;
        }

        public IQuery<T1, T2, T3> WhereAnd(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2, T3> WhereOr(Expression<Func<T1, T2, T3, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.Or, fun));
            return this;
        }

        public IQuery<T1, T2, T3> WhereOr(string whereSql)
        {
            throw new NotImplementedException();
        }
    }
}