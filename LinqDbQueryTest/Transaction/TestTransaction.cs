using System;
using System.Data;
using AspectCore.DynamicProxy;
using Brochure.ORM;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Brochure.ORMTest.Transaction
{

    [TestClass]
    public class TestTransaction
    {

        public TestTransaction () { }

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
                .Returns (new ORM.Database.Transaction (dbTransactionMock.Object))
                .Returns (new InnerTransaction (dbOptionMock.Object))
                .Returns (new InnerTransaction (dbOptionMock.Object));

            var tt = new TransactionAttribute
            {
                IsDisable = false
            };
            tt.Invoke (aspectContextMock.Object, mockDelegateMock.Object);
            transactionManagerMock.Verify (t => t.AddTransaction (It.IsAny<ORM.Database.Transaction> ()));
            transactionManagerMock.Verify (t => t.RemoveTransaction (It.IsAny<ITransaction> ()));
        }

        [TestMethod]
        public void TestTransactionCommit ()
        {
            var transactionManagerMock = new Mock<ITransactionManager> ();
            var aspectContextMock = new Mock<AspectContext> ();
            var mockDelegateMock = new Mock<AspectDelegate> ();
            var dbProviderMock = new Mock<IDbProvider> ();
            var factoryMock = new Mock<ITransactionFactory> ();
            var dbOptionMock = new Mock<DbOption> (dbProviderMock.Object, transactionManagerMock.Object);
            var transactionMock = new Mock<ITransaction> ();
            var dbTransactionMock = new Mock<IDbTransaction> ();
            var manager = new TransactionManager ();
            var services = Mock.Of<IServiceProvider> (sp =>
                sp.GetService (typeof (ITransactionManager)) == manager &&
                sp.GetService (typeof (DbOption)) == dbOptionMock.Object &&
                sp.GetService (typeof (ITransactionFactory)) == factoryMock.Object
            );
            factoryMock.SetupSequence (t => t.GetTransaction ())
                .Returns (transactionMock.Object)
                .Returns (transactionMock.Object)
                .Returns (transactionMock.Object);
            aspectContextMock.Setup (t => t.ServiceProvider).Returns (services);
            var tt = new TransactionAttribute
            {
                IsDisable = false
            };
            tt.Invoke (aspectContextMock.Object, mockDelegateMock.Object);
            transactionMock.Verify (t => t.Commit ());
            var tt1 = new TransactionAttribute
            {
                IsDisable = false
            };
            tt1.Invoke (aspectContextMock.Object, mockDelegateMock.Object);
            transactionMock.Verify (t => t.Commit (), Times.AtLeast (2));
        }

        [TestMethod]
        public void TestTransactionRollback ()
        {
            var transactionManagerMock = new Mock<ITransactionManager> ();
            var aspectContextMock = new Mock<AspectContext> ();
            var mockDelegateMock = new Mock<AspectDelegate> ();
            var dbProviderMock = new Mock<IDbProvider> ();
            var factoryMock = new Mock<ITransactionFactory> ();
            var dbOptionMock = new Mock<DbOption> (dbProviderMock.Object, transactionManagerMock.Object);
            var transactionMock = new Mock<ITransaction> ();
            var dbTransactionMock = new Mock<IDbTransaction> ();
            var manager = new TransactionManager ();
            var services = Mock.Of<IServiceProvider> (sp =>
                sp.GetService (typeof (ITransactionManager)) == manager &&
                sp.GetService (typeof (DbOption)) == dbOptionMock.Object &&
                sp.GetService (typeof (ITransactionFactory)) == factoryMock.Object
            );
            factoryMock.SetupSequence (t => t.GetTransaction ())
                .Returns (transactionMock.Object)
                .Returns (transactionMock.Object)
                .Returns (transactionMock.Object);
            aspectContextMock.Setup (t => t.ServiceProvider).Returns (services);
            var tt = new TransactionAttribute
            {
                IsDisable = false
            };
            tt.Invoke (aspectContextMock.Object, mockDelegateMock.Object);
            var tt1 = new TransactionAttribute
            {
                IsDisable = false
            };
            Assert.ThrowsException<Exception> (() =>
            {
                tt1.Invoke (aspectContextMock.Object, t => { throw new Exception (); });
            });
            transactionMock.Verify (t => t.Rollback ());
        }
    }

}