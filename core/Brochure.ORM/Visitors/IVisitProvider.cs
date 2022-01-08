using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
namespace Brochure.ORM.Visitors
{
    /// <summary>
    /// The visit provider.
    /// </summary>
    public interface IVisitProvider
    {
        /// <summary>
        /// Builders the.
        /// </summary>
        /// <returns>A T.</returns>
        T Builder<T>() where T : ExpressionVisitor;
    }

    /// <summary>
    /// The visit provider.
    /// </summary>
    public class VisitProvider : IVisitProvider
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitProvider"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public VisitProvider(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// Builders the.
        /// </summary>
        /// <returns>A T.</returns>
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