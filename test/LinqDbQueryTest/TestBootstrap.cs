using AspectCore.DependencyInjection;
using Brochure.Abstract;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.MySql;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.ORMTest
{
    /// <summary>
    /// The test bootstrap.
    /// </summary>
    public static class TestBootstrap
    {
        /// <summary>
        /// The my sql db context.
        /// </summary>
        public class MySqlDbContext : DbContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MySqlDbContext"/> class.
            /// </summary>
            /// <param name="objectFactory">The object factory.</param>
            /// <param name="connectFactory">The connect factory.</param>
            /// <param name="transactionManager">The transaction manager.</param>
            /// <param name="sqlBuilder">The sql builder.</param>
            /// <param name="serviceScope">The service scope.</param>
            public MySqlDbContext(IObjectFactory objectFactory, IConnectFactory connectFactory, ITransactionManager transactionManager, ISqlBuilder sqlBuilder, IServiceScopeFactory serviceScope) : base(objectFactory, connectFactory, transactionManager, sqlBuilder, serviceScope)
            {
            }
        }

        /// <summary>
        /// The test context.
        /// </summary>
        public class TestContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestContext"/> class.
            /// </summary>
            /// <param name="connectFactory">The connect factory.</param>
            /// <param name="transactionManager">The transaction manager.</param>
            /// <param name="objectFactory">The object factory.</param>
            /// <param name="sqlBuilder">The sql builder.</param>
            /// <param name="serviceScope">The service scope.</param>
            public TestContext(IConnectFactory connectFactory, ITransactionManager transactionManager, IObjectFactory objectFactory, ISqlBuilder sqlBuilder, IServiceScopeFactory serviceScope)
            {
                var dbContext = new MySqlDbContext(objectFactory, connectFactory, transactionManager, sqlBuilder, serviceScope);
            }
        }

        /// <summary>
        /// Inits the contrainer.
        /// </summary>
        /// <returns>An IServiceContext.</returns>
        public static IServiceContext InitContrainer()
        {
            var container = new ServiceContext();
            container.AddType<DbDatabase, MySqlDbDatabase>(Lifetime.Scoped);
            container.AddType<DbData, MySqlDbData>(Lifetime.Scoped);
            container.AddType<DbTable, MySqlDbTable>(Lifetime.Scoped);
            container.AddType<DbIndex, MySqlDbIndex>(Lifetime.Scoped);
            container.AddType<DbColumns, MySqlDbColumns>(Lifetime.Scoped);
            container.AddType<IDbProvider, MySqlDbProvider>(Lifetime.Scoped);
            container.AddType<DbOption, MySqlOption>(Lifetime.Scoped);
            container.AddType<ISqlBuilder, MySqlSqlBuilder>(Lifetime.Scoped);
            container.AddType<TransactionManager, MySqlTransactionManager>(Lifetime.Scoped);
            container.AddType<DbContext, MySqlDbContext>();
            return container;
        }
    }
}