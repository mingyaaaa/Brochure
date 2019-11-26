using System;
using AspectCore.Injector;
using Brochure.LinqDbQuery.MySql;
using LinqDbQuery;
using LinqDbQuery.Database;
using LinqDbQuery.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LinqDbQueryTest
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

        [LinqDbQuery.Atrributes.Transaction]
        public void TestCreate ()
        {
            var students = new Students ()
            {
                Id = Guid.NewGuid ().ToString (),
                ClassCount = 1,
            };

            dbContext.Insert (students);
        }
    }

    [TestClass]
    public class TestTransaction
    {
        private DbContext dbContext;

        private Mock<DbData> dbDataMock;

        private Mock<DbTable> dbTableMock;

        private Mock<DbColumns> dbColumnMock;

        private Mock<DbIndex> dbIndexMock;

        private Mock<DbDatabase> dbDatabaseMock;

        private Mock<IDbProvider> dbProviderMock;

        private ServiceContainer container;
        private IServiceProvider serviceProvider;

        public TestTransaction ()
        {
            container = TestBootstrap.InitContrainer ();
            serviceProvider = container.Build ();
        }

        [TestMethod]
        public void TestTransactionCreate ()
        {
            dbContext = serviceProvider.GetRequiredService<DbContext> ();
            Assert.IsNotNull (dbContext);
        }
    }

}