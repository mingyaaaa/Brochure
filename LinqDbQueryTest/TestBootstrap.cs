using AspectCore.Injector;
using Brochure.LinqDbQuery.MySql;
using LinqDbQuery;
using LinqDbQuery.Database;

namespace LinqDbQueryTest
{
    public static class TestBootstrap
    {
        public static ServiceContainer InitContrainer ()
        {
            var container = new ServiceContainer ();
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