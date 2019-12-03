using System;
using System.Data;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Brochure.LinqDbQuery.MySql;
using LinqDbQuery;
using LinqDbQuery.Atrributes;
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

        public TestTransaction ()
        { }

        [TestMethod]
        public void TestTransactionDisableTrue ()
        {
            var transactionManagerMock = new Mock<ITransactionManager> ();
            var aspectContextMock = new Mock<AspectContext> ();
            var mockDelegateMock = new Mock<AspectDelegate> ();
            var dbOptionMock = new Mock<DbOption> ();
            var tt = new TransactionAttribute
            {
                IsDisable = true
            };
            tt.Invoke (aspectContextMock.Object, mockDelegateMock.Object);
            mockDelegateMock.Verify (t => t.Invoke (aspectContextMock.Object));
        }

        [TestMethod]
        public void TestTransactionDisableFalse ()
        {
            var transactionManagerMock = new Mock<ITransactionManager> ();
            var aspectContextMock = new Mock<AspectContext> ();
            var mockDelegateMock = new Mock<AspectDelegate> ();
            var dbProviderMock = new Mock<IDbProvider> ();
            var factoryMock = new Mock<ITransactionFactory> ();
            var dbOptionMock = new Mock<DbOption> (dbProviderMock.Object, transactionManagerMock.Object);
            var transactionMock = new Mock<ITransaction> ();
            var dbTransactionMock = new Mock<IDbTransaction> ();
            var services = Mock.Of<IServiceProvider> (sp =>

                sp.GetService (typeof (ITransactionManager)) == transactionManagerMock.Object &&
                sp.GetService (typeof (DbOption)) == dbOptionMock.Object &&
                sp.GetService (typeof (ITransactionFactory)) == factoryMock.Object
            );

            aspectContextMock.Setup (t => t.ServiceProvider).Returns (services);
            factoryMock.SetupSequence (t => t.GetTransaction ())
                .Returns (new LinqDbQuery.Database.Transaction (dbTransactionMock.Object))
                .Returns (new InnerTransaction (dbOptionMock.Object))
                .Returns (new InnerTransaction (dbOptionMock.Object));

            var tt = new TransactionAttribute
            {
                IsDisable = false
            };
            tt.Invoke (aspectContextMock.Object, mockDelegateMock.Object);
            transactionManagerMock.Verify (t => t.AddTransaction (It.IsAny<LinqDbQuery.Database.Transaction> ()));
            transactionManagerMock.Verify (t => t.RemoveTransaction (It.IsAny<ITransaction> ()));

            var tt2 = new TransactionAttribute
            {
                IsDisable = false
            };
            tt2.Invoke (aspectContextMock.Object, mockDelegateMock.Object);
            transactionManagerMock.Verify (t => t.AddTransaction (It.IsAny<InnerTransaction> ()));
            transactionManagerMock.Verify (t => t.RemoveTransaction (It.IsAny<ITransaction> ()));

        }
    }

}