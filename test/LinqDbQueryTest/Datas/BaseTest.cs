using System;
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoMoq;
using Brochure.LinqDbQuery.MySql;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.Querys;
using Brochure.ORM.Visitors;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;

namespace LinqDbQueryTest.Datas
{
    public class BaseTest
    {
        public AutoMocker MockService;
        protected IServiceCollection Services;
        protected IServiceProvider Provider;

        public Fixture Fixture { get; }

        public BaseTest()
        {
            MockService = new AutoMocker();
            Services = new ServiceCollection();
            Services.AddScoped<IDbProvider, MySqlDbProvider>();
            Services.AddScoped<IVisitProvider, VisitProvider>();
            Services.AddTransient<ExpressionVisitor, WhereVisitor>();
            Services.AddTransient<ExpressionVisitor, GroupVisitor>();
            Services.AddTransient<ExpressionVisitor, OrderVisitor>();
            Services.AddTransient<ExpressionVisitor, SelectVisitor>();
            Services.AddTransient<ExpressionVisitor, JoinVisitor>();
            Services.AddTransient<ExpressionVisitor, HavingVisitor>();
            Services.AddScoped<DbSql, MySqlDbSql>();
            Services.AddScoped<IQueryBuilder, DefaultQueryBuilder>();
            Services.AddScoped<IConnectFactory, ConnectFactory>();
            Services.AddSingleton<DbOption, MySqlOption>();
            Provider = Services.BuildServiceProvider();

            Fixture = new Fixture();
            Fixture.Customize(new AutoMoqCustomization());
        }
    }
}