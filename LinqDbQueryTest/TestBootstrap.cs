using System;
using AspectCore.DependencyInjection;
using Brochure.LinqDbQuery.MySql;
using Brochure.ORM;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;

namespace Brochure.ORMTest
{
    public static class TestBootstrap
    {
        public class MySqlDbContext : DbContext
        {
            public MySqlDbContext (DbDatabase dbDatabase, DbTable dbTable, DbColumns dbColumns, DbIndex dbIndex, DbData dbData, DbOption dbOption, IDbProvider dbProvider):
                base (dbDatabase, dbTable, dbColumns, dbIndex, dbData, dbOption, dbProvider) { }
        }

        public class TestContext
        {
            private DbContext dbContext;

            public TestContext (DbDatabase dbDatabase,
                DbTable dbTable,
                DbColumns dbColumns,
                DbIndex dbIndex,
                DbData dbData,
                DbOption dbOption,
                IDbProvider dbProvider)
            {
                dbContext = new MySqlDbContext (dbDatabase, dbTable, dbColumns, dbIndex, dbData, dbOption, dbProvider);
            }

            [Transaction]
            public void TestCreate ()
            {
                var students = new Students ()
                {
                    Id = Guid.NewGuid ().ToString (),
                    ClassCount = 1,
                };

                dbContext.Insert (students);
            }

            [Transaction]
            public void TestDelete ()
            {
                var students = new Students ()
                {
                    Id = Guid.NewGuid ().ToString (),
                    ClassCount = 1,
                };

                dbContext.Delete<Students> (t => t.Id == "1");
            }
        }
        public static IServiceContext InitContrainer ()
        {
            var container = new ServiceContext ();
            container.AddType<DbDatabase, MySqlDbDatabase> (Lifetime.Scoped);
            container.AddType<DbData, MySqlDbData> (Lifetime.Scoped);
            container.AddType<DbTable, MySqlDbTable> (Lifetime.Scoped);
            container.AddType<DbIndex, MySqlDbIndex> (Lifetime.Scoped);
            container.AddType<DbColumns, MySqlDbColumns> (Lifetime.Scoped);
            container.AddType<IDbProvider, MySqlDbProvider> (Lifetime.Scoped);
            container.AddType<DbOption, MySqlOption> (Lifetime.Scoped);
            container.AddType<DbSql, MySqlDbSql> (Lifetime.Scoped);
            container.AddType<TransactionManager, MySqlTransactionManager> (Lifetime.Scoped);
            container.AddType<DbContext, MySqlDbContext> ();
            return container;
        }
    }
}