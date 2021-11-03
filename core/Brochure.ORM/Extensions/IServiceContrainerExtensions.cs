using System.Linq.Expressions;
using Brochure.ORM.Database;
using Brochure.ORM.Querys;
using Brochure.ORM.Visitors;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.ORM.Extensions
{
    public static class IServiceContrainerExtensions
    {
        public static IServiceCollection AddVisit(this IServiceCollection services)
        {
            services.AddScoped<ExpressionVisitor, GroupVisitor>();
            services.AddScoped<ExpressionVisitor, HavingVisitor>();
            services.AddScoped<ExpressionVisitor, JoinVisitor>();
            services.AddScoped<ExpressionVisitor, OrderVisitor>();
            services.AddScoped<ExpressionVisitor, SelectVisitor>();
            services.AddScoped<ExpressionVisitor, WhereVisitor>();
            services.AddScoped<IVisitProvider, VisitProvider>();
            services.AddScoped<IQueryBuilder, QueryBuilder>();
            services.AddScoped<IConnectFactory, ConnectFactory>();
            services.AddScoped<ITransactionFactory, TransactionFactory>();
            return services;
        }
    }
}