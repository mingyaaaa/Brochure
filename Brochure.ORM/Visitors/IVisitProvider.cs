using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
namespace Brochure.ORM.Visitors
{
    public interface IVisitProvider
    {
        T Builder<T> () where T : ExpressionVisitor;
        T BuilderNew<T> () where T : ExpressionVisitor, ICloneable;
    }

    public class VisitProvider : IVisitProvider
    {
        private readonly IEnumerable<ExpressionVisitor> expressionVisitors;

        public VisitProvider (IEnumerable<ExpressionVisitor> expressionVisitors)
        {
            this.expressionVisitors = expressionVisitors;
        }
        public T Builder<T> () where T : ExpressionVisitor
        {
            var type = typeof (T);
            var exp = expressionVisitors.FirstOrDefault (t => t.GetType ().FullName == type.FullName);
            if (exp == null)
                throw new System.Exception ($"{type.FullName}服务没有注入");
            return (T) exp;
        }

        public T BuilderNew<T> () where T : ExpressionVisitor, ICloneable
        {
            var type = typeof (T);
            var exp = expressionVisitors.FirstOrDefault (t => t.GetType ().FullName == type.FullName);
            if (exp == null)
                throw new System.Exception ($"{type.FullName}服务没有注入");
            if (!typeof (ICloneable).IsAssignableFrom (type))
                throw new System.Exception ($"{type.FullName}必须实现ICloneable接口");
            return (T) (exp as ICloneable)?.Clone ();
        }
    }
}