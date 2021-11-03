using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace Brochure.ORM.Visitors
{
    public interface IVisitProvider
    {
        T Builder<T>() where T : ExpressionVisitor;
    }

    public class VisitProvider : IVisitProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public VisitProvider(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public T Builder<T>() where T : ExpressionVisitor
        {
            var expressionVisitors = _serviceProvider.GetServices<ExpressionVisitor>();
            var type = typeof(T);
            var exp = expressionVisitors.FirstOrDefault(t => t.GetType().FullName == type.FullName);
            if (exp == null)
                throw new System.Exception($"{type.FullName}服务没有注入");
            return (T)exp;
        }
    }
}