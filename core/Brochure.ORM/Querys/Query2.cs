using System;
using System.Linq.Expressions;

namespace Brochure.ORM.Querys
{
    public class Query<T1, T2> : Query, IQuery<T1, T2>
    {
        public IQuery<T1, T2, T3> Join<T3>(Expression<Func<T1, T2, T3, bool>> fun)
        {
            this.JoinExpression.Add((typeof(T3), fun));
            var a = new Query<T1, T2, T3>();
            base.CopyProperty(a);
            return a;
        }

        public IQuery<T1, T2> OrderBy(Expression<Func<T1, T2, object>> fun)
        {
            OrderExpression = fun;
            return this;
        }

        public IQuery<T1, T2> OrderBy(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2> OrderByDesc(Expression<Func<T1, T2, object>> fun)
        {
            OrderDescExpression = fun;
            return this;
        }

        public IQuery<T1, T2> OrderByDesc(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T> Select<T>(Expression<Func<T1, T2, T>> fun)
        {
            var a = new Query<T>();
            this.SelectExpression = fun;
            base.CopyProperty(a);
            return a;
        }

        public IQuery<T1, T2> Where(Expression<Func<T1, T2, bool>> fun)
        {
            WhereExpression = fun;
            return this;
        }

        public IQuery<T1, T2> WhereAnd(Expression<Func<T1, T2, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.And, fun));
            return this;
        }

        public IQuery<T1, T2> WhereAnd(string whereSql)
        {
            throw new NotImplementedException();
        }

        public IQuery<T1, T2> WhereOr(Expression<Func<T1, T2, bool>> fun)
        {
            WhereListExpression.Add((DbOperationName.Or, fun));
            return this;
        }

        public IQuery<T1, T2> WhereOr(string whereSql)
        {
            throw new NotImplementedException();
        }
    }
}