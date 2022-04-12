using Brochure.ORM.Database;
using Brochure.ORM.Visitors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;

namespace Brochure.ORM.Extensions
{
    /// <summary>
    /// The i service contrainer extensions.
    /// </summary>
    public static class IServiceContrainerExtensions
    {
        /// <summary>
        /// Adds the visit.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddVisit(this IServiceCollection services)
        {
            services.AddScoped<ExpressionVisitor, GroupVisitor>();
            services.AddScoped<ExpressionVisitor, HavingVisitor>();
            services.AddScoped<ExpressionVisitor, JoinVisitor>();
            services.AddScoped<ExpressionVisitor, OrderVisitor>();
            services.AddScoped<ExpressionVisitor, SelectVisitor>();
            services.AddScoped<ExpressionVisitor, WhereVisitor>();
            services.AddScoped<IVisitProvider, VisitProvider>();
            services.AddScoped<IConnectFactory, ConnectFactory>();
            services.AddScoped<ITransactionFactory, TransactionFactory>();
            return services;
        }

        /// <summary>
        /// Adds the db core.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="builderAction">The builder action.</param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection AddDbCore(this IServiceCollection services, Action<IDbBuilder> builderAction)
        {
            services.AddVisit();
            var builder = new DbBuilder(services);
            builderAction.Invoke(builder);
            return services;
        }
    }
}