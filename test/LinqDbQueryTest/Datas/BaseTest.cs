using AutoFixture;
using AutoFixture.AutoMoq;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.MySql;
using Brochure.ORM.Querys;
using Brochure.ORM.Visitors;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;
using System;
using System.Linq.Expressions;

namespace LinqDbQueryTest.Datas
{
    public class BaseTest
    {
        public AutoMocker MockService;
        protected IServiceCollection Services;
        protected IServiceProvider Provider;

        /// <summary>
        /// Gets the fixture.
        /// </summary>
        public Fixture Fixture { get; }

        public BaseTest()
        {
            MockService = new AutoMocker();
            Services = new ServiceCollection();
            Services.AddScoped<IDbProvider, MySqlDbProvider>();
            Services.AddScoped<IVisitProvider, VisitProvider>();
            Services.AddScoped<IQueryBuilder, QueryBuilder>();
            Services.AddTransient<ExpressionVisitor, WhereVisitor>();
            Services.AddTransient<ExpressionVisitor, GroupVisitor>();
            Services.AddTransient<ExpressionVisitor, OrderVisitor>();
            Services.AddTransient<ExpressionVisitor, SelectVisitor>();
            Services.AddTransient<ExpressionVisitor, JoinVisitor>();
            Services.AddTransient<ExpressionVisitor, HavingVisitor>();
            Services.AddScoped<DbSql, MySqlDbSql>();
            Services.AddScoped<IConnectFactory, ConnectFactory>();
            Services.AddSingleton<DbOption, MySqlOption>();
            Provider = Services.BuildServiceProvider();

            Fixture = new Fixture();
            Fixture.Customize(new AutoMoqCustomization());
        }
    }
}