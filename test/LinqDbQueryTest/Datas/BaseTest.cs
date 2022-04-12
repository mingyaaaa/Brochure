using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Brochure.Abstract;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.MySql;
using Brochure.ORM.Visitors;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqDbQueryTest.Datas
{
    /// <summary>
    /// The base test.
    /// </summary>
    public class BaseTest
    {
        public AutoMocker MockService;
        protected IServiceCollection Services;
        protected IServiceProvider Provider;

        /// <summary>
        /// Gets the fixture.
        /// </summary>
        public Fixture Fixture { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTest"/> class.
        /// </summary>
        public BaseTest()
        {
            MockService = new AutoMocker();
            Services = new ServiceCollection();
            Services.AddScoped<IDbProvider, MySqlDbProvider>();
            Services.AddScoped<IVisitProvider, VisitProvider>();
            Services.AddScoped<ISqlBuilder, SqlBuilder>();
            Services.AddTransient<ExpressionVisitor, WhereVisitor>();
            Services.AddTransient<ExpressionVisitor, GroupVisitor>();
            Services.AddTransient<ExpressionVisitor, OrderVisitor>();
            Services.AddTransient<ExpressionVisitor, SelectVisitor>();
            Services.AddTransient<ExpressionVisitor, JoinVisitor>();
            Services.AddTransient<ExpressionVisitor, HavingVisitor>();
            Services.AddScoped<ISqlBuilder, MySqlSqlBuilder>();
            Services.AddScoped<IConnectFactory, ConnectFactory>();
            Services.AddSingleton<DbOption, MySqlOption>();
            Services.AddSingleton<TypeMap, MySqlTypeMap>();
            Provider = Services.BuildServiceProvider();

            Fixture = new Fixture();
            Fixture.Customize(new AutoMoqCustomization() { ConfigureMembers = true });
            Fixture.Customize(new ConstructorCustomization(typeof(DbContext),
                new MyMethodSelector(typeof(IObjectFactory), typeof(IConnectFactory),
                typeof(ITransactionManager), typeof(ISqlBuilder), typeof(IServiceScope)
                )));
            Fixture.Customize(new ConstructorCustomization(typeof(MySqlDbTable),
    new MyMethodSelector(typeof(DbContext))));
        }
    }

    /// <summary>
    /// The my method selector.
    /// </summary>
    public class MyMethodSelector : IMethodQuery
    {
        private readonly Type[] _types;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyMethodSelector"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public MyMethodSelector(params Type[] type)
        {
            _types = type;
        }

        /// <summary>
        /// Selects the methods.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A list of IMethods.</returns>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }

            var constructors = type
                .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(a => a.GetParameters().Select(t => t.ParameterType).SequenceEqual(_types))
                .Select(a => new ConstructorMethod(a));

            return constructors;
        }
    }
}