using System;
using AspectCore.DependencyInjection;
using Brochure.Abstract;
using Brochure.ORM;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Brochure.ORM.Visitors;

namespace Brochure.ORMTest
{
    public static class TestBootstrap
    {
        public class MySqlDbContext : DbContext
        {
            public MySqlDbContext(IObjectFactory objectFactory, IConnectFactory connectFactory, ITransactionManager transactionManager, ISqlBuilder sqlBuilder) : base(objectFactory, connectFactory, transactionManager, sqlBuilder)
            {
            }
        }

        public class TestContext
        {
            public TestContext(IConnectFactory connectFactory, ITransactionManager transactionManager, IObjectFactory objectFactory, ISqlBuilder sqlBuilder)
            {
                var dbContext = new MySqlDbContext(objectFactory, connectFactory, transactionManager, sqlBuilder);
            }
        }

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