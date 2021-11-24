using System;
using System.Data;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.ORM;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Brochure.ORMTest.Transaction
{
    [TestClass]
    public class TestTransaction
    {
        public TestTransaction()
        {
        }

        [TestMethod]
        public void TestTransactionDisableTrue()
        {
            var transactionManagerMock = new Mock<ITransactionManager>();
            var aspectContextMock = new Mock<AspectContext>();
            var mockDelegateMock = new Mock<AspectDelegate>();
            var dbOptionMock = new Mock<DbOption>();
            var tt = new TransactionAttribute
            {
                IsDisable = true
            };
            tt.Invoke(aspectContextMock.Object, mockDelegateMock.Object);
            mockDelegateMock.Verify(t => t.Invoke(aspectContextMock.Object));
        }

        [TestMethod]
        public void TestTransactionDisableFalse()
        {
            var transactionManagerMock = new Mock<ITransactionManager>();
            var aspectContextMock = new Mock<AspectContext>();
            var mockDelegateMock = new Mock<AspectDelegate>();
            var dbProviderMock = new Mock<IDbProvider>();
            var factoryMock = new Mock<ITransactionFactory>();
            var dbOptionMock = new Mock<DbOption>();
            var transactionMock = new Mock<ITransaction>();
            var connectFactoryMock = new Mock<IConnectFactory>();
            var IdbConnectMock = new Mock<IDbConnection>();
            var IdbTransactionMock = new Mock<IDbTransaction>();
            var services = Mock.Of<IServiceProvider>(sp =>

               sp.GetService(typeof(ITransactionManager)) == transactionManagerMock.Object &&
               sp.GetService(typeof(DbOption)) == dbOptionMock.Object &&
               sp.GetService(typeof(ITransactionFactory)) == factoryMock.Object
            );
            IdbConnectMock.Setup(t => t.BeginTransaction()).Returns(IdbTransactionMock.Object);
            connectFactoryMock.Setup(t => t.CreateAndOpenConnection()).Returns(IdbConnectMock.Object);

            aspectContextMock.Setup(t => t.ServiceProvider).Returns(services);
            factoryMock.SetupSequence(t => t.GetTransaction())
                .Returns(new ORM.Database.Transaction(connectFactoryMock.Object))
                .Returns(new InnerTransaction())
                .Returns(new InnerTransaction());

            var tt = new TransactionAttribute
            {
                IsDisable = false
            };
            tt.Invoke(aspectContextMock.Object, mockDelegateMock.Object);
            transactionManagerMock.Verify(t => t.AddTransaction(It.IsAny<ORM.Database.Transaction>()));
            transactionManagerMock.Verify(t => t.RemoveTransaction(It.IsAny<ITransaction>()));
        }

        [TestMethod]
        public void TestTransactionCommit()
        {
            var transactionManagerMock = new Mock<ITransactionManager>();
            var aspectContextMock = new Mock<AspectContext>();
            var mockDelegateMock = new Mock<AspectDelegate>();
            var dbProviderMock = new Mock<IDbProvider>();
            var factoryMock = new Mock<ITransactionFactory>();
            var dbOptionMock = new Mock<DbOption>();
            var transactionMock = new Mock<ITransaction>();
            var dbTransactionMock = new Mock<IDbTransaction>();
            var manager = new TransactionManager();
            var services = Mock.Of<IServiceProvider>(sp =>
               sp.GetService(typeof(ITransactionManager)) == manager &&
               sp.GetService(typeof(DbOption)) == dbOptionMock.Object &&
               sp.GetService(typeof(ITransactionFactory)) == factoryMock.Object
            );
            factoryMock.SetupSequence(t => t.GetTransaction())
                .Returns(transactionMock.Object)
                .Returns(transactionMock.Object)
                .Returns(transactionMock.Object);
            aspectContextMock.Setup(t => t.ServiceProvider).Returns(services);
            var tt = new TransactionAttribute
            {
                IsDisable = false
            };
            tt.Invoke(aspectContextMock.Object, mockDelegateMock.Object);
            transactionMock.Verify(t => t.Commit());
            var tt1 = new TransactionAttribute
            {
                IsDisable = false
            };
            tt1.Invoke(aspectContextMock.Object, mockDelegateMock.Object);
            transactionMock.Verify(t => t.Commit(), Times.AtLeast(2));
        }

        [TestMethod]
        public void TestTransactionRollback()
        {
            var transactionManagerMock = new Mock<ITransactionManager>();
            var aspectContextMock = new Mock<AspectContext>();
            var mockDelegateMock = new Mock<AspectDelegate>();
            var dbProviderMock = new Mock<IDbProvider>();
            var factoryMock = new Mock<ITransactionFactory>();
            var dbOptionMock = new Mock<DbOption>();
            var transactionMock = new Mock<ITransaction>();
            var dbTransactionMock = new Mock<IDbTransaction>();
            var manager = new TransactionManager();
            var services = Mock.Of<IServiceProvider>(sp =>
               sp.GetService(typeof(ITransactionManager)) == manager &&
               sp.GetService(typeof(DbOption)) == dbOptionMock.Object &&
               sp.GetService(typeof(ITransactionFactory)) == factoryMock.Object
            );
            factoryMock.SetupSequence(t => t.GetTransaction())
                .Returns(transactionMock.Object)
                .Returns(transactionMock.Object)
                .Returns(transactionMock.Object);
            aspectContextMock.Setup(t => t.ServiceProvider).Returns(services);
            var tt = new TransactionAttribute
            {
                IsDisable = false
            };
            tt.Invoke(aspectContextMock.Object, mockDelegateMock.Object);
            var tt1 = new TransactionAttribute
            {
                IsDisable = false
            };
            Assert.ThrowsExceptionAsync<Exception>(() =>
           {
               return tt1.Invoke(aspectContextMock.Object, t => { throw new Exception(); });
           });
            transactionMock.Verify(t => t.Rollback());
        }

        [TestMethod]
        public void TestTransationExcute()
        {
            var connectFactoryMock = new Mock<IConnectFactory>();
            var dbConnectMock = new Mock<IDbConnection>();
            var dbTransMock = new Mock<IDbTransaction>();
            connectFactoryMock.Setup(t => t.CreateAndOpenConnection()).Returns(dbConnectMock.Object);
            connectFactoryMock.Setup(t => t.CreateConnection()).Returns(dbConnectMock.Object);
            dbConnectMock.Setup(t => t.BeginTransaction()).Returns(dbTransMock.Object);

            var collection = new ServiceCollection();
            collection.AddDbCore(t => t.AddMySql());
            collection.AddScoped<IConnectFactory>(t => connectFactoryMock.Object);
            collection.AddSingleton<IPluginManagers>(new PluginManagers());
            collection.AddScoped<ISqlExcute, SqlExcute>();
            collection.AddScoped<ISqlExcute2, SqlExcute2>();
            var provider = collection.BuildPluginServiceProvider();
            var ins = provider.GetRequiredService<ISqlExcute>();
            ins.InserData();
            ins.DeleteData();
            connectFactoryMock.Verify(t => t.CreateAndOpenConnection(), Times.Exactly(2));
            dbConnectMock.Verify(t => t.BeginTransaction(), Times.Exactly(2));
        }

        [TestMethod]
        public void TestTransationSingletonExcute()
        {
            var connectFactoryMock = new Mock<IConnectFactory>();
            var dbConnectMock = new Mock<IDbConnection>();
            var dbTransMock = new Mock<IDbTransaction>();
            connectFactoryMock.Setup(t => t.CreateAndOpenConnection()).Returns(dbConnectMock.Object);
            connectFactoryMock.Setup(t => t.CreateConnection()).Returns(dbConnectMock.Object);
            dbConnectMock.Setup(t => t.BeginTransaction()).Returns(dbTransMock.Object);

            var collection = new ServiceCollection();
            collection.AddDbCore(t => t.AddMySql());
            collection.AddScoped<IConnectFactory>(t => connectFactoryMock.Object);
            collection.AddSingleton<IPluginManagers>(new PluginManagers());
            collection.AddSingleton<ISqlExcute, SqlExcute>();
            collection.AddSingleton<ISqlExcute2, SqlExcute2>();
            var provider = collection.BuildPluginServiceProvider();
            var ins = provider.GetRequiredService<ISqlExcute>();
            ins.InserData();
            ins.DeleteData();
            connectFactoryMock.Verify(t => t.CreateAndOpenConnection(), Times.Exactly(2));
            dbConnectMock.Verify(t => t.BeginTransaction(), Times.Exactly(2));
        }

        #region TestData

        public interface ISqlExcute
        {
            void InserData();

            void DeleteData();
        }

        public class SqlExcute : ISqlExcute
        {
            private readonly IConnectFactory _connectFactory;
            private readonly ISqlExcute2 _sqlExcute2;

            public SqlExcute(IConnectFactory connectFactory, ISqlExcute2 sqlExcute2)
            {
                _connectFactory = connectFactory;
                _sqlExcute2 = sqlExcute2;
            }

            [Transaction]
            public void InserData()
            {
                var connect = _connectFactory.CreateConnection();
                _sqlExcute2.InserData();
                _sqlExcute2.DeleteData();
                this.DeleteData();
            }

            [Transaction]
            public void DeleteData()
            {
            }
        }

        public interface ISqlExcute2
        {
            void InserData();

            void DeleteData();
        }

        public class SqlExcute2 : ISqlExcute2
        {
            private readonly IConnectFactory _connectFactory;

            public SqlExcute2(IConnectFactory connectFactory)
            {
                _connectFactory = connectFactory;
            }

            [Transaction]
            public void InserData()
            {
                var connect = _connectFactory.CreateConnection();
                this.DeleteData();
            }

            [Transaction]
            public void DeleteData()
            {
            }
        }

        public class TestAttInterceptorAttribute : AbstractInterceptorAttribute
        {
            public override Task Invoke(AspectContext context, AspectDelegate next)
            {
                next(context);
                return Task.CompletedTask;
            }
        }

        #endregion TestData
    }
}