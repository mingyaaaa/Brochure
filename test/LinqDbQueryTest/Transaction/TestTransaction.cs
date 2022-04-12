using AspectCore.DynamicProxy;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.ORM;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Brochure.ORMTest.Transaction
{
    /// <summary>
    /// The test transaction.
    /// </summary>
    [TestClass]
    public class TestTransaction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestTransaction"/> class.
        /// </summary>
        public TestTransaction()
        {
        }

        /// <summary>
        /// Tests the transaction disable true.
        /// </summary>
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

        /// <summary>
        /// Tests the transaction disable false.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestTransactionDisableFalse()
        {
            var transactionManagerMock = new Mock<ITransactionManager>();
            var aspectContextMock = new Mock<AspectContext>();
            var mockDelegateMock = new Mock<AspectDelegate>();
            var dbProviderMock = new Mock<IDbProvider>();
            var factoryMock = new Mock<ITransactionFactory>();
            var dbOptionMock = new Mock<DbOption>();
            var transactionMock = new Mock<ITransaction>();
            var connectFactoryMock = new Mock<IConnectFactory>();
            var IdbConnectMock = new Mock<DbConnection>();
            var IdbTransactionMock = new Mock<DbTransaction>();
            var services = Mock.Of<IServiceProvider>(sp =>

               sp.GetService(typeof(ITransactionManager)) == transactionManagerMock.Object &&
               sp.GetService(typeof(DbOption)) == dbOptionMock.Object &&
               sp.GetService(typeof(ITransactionFactory)) == factoryMock.Object
            );
            connectFactoryMock.Setup(t => t.CreateAndOpenConnectionAsync()).ReturnsAsync(IdbConnectMock.Object);

            aspectContextMock.Setup(t => t.ServiceProvider).Returns(services);
            factoryMock.SetupSequence(t => t.GetTransaction())
                .Returns(new ORM.Database.Transaction(connectFactoryMock.Object))
                .Returns(new InnerTransaction())
                .Returns(new InnerTransaction());

            var tt = new TransactionAttribute
            {
                IsDisable = false
            };
            await tt.Invoke(aspectContextMock.Object, mockDelegateMock.Object);
            transactionManagerMock.Verify(t => t.AddTransaction(It.IsAny<ORM.Database.Transaction>()));
            transactionManagerMock.Verify(t => t.RemoveTransaction(It.IsAny<ITransaction>()));
        }

        /// <summary>
        /// Tests the transaction commit.
        /// </summary>
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
            var dbTransactionMock = new Mock<DbTransaction>();
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
            transactionMock.Verify(t => t.CommitAsync());
            var tt1 = new TransactionAttribute
            {
                IsDisable = false
            };
            tt1.Invoke(aspectContextMock.Object, mockDelegateMock.Object);
            transactionMock.Verify(t => t.CommitAsync(), Times.AtLeast(2));
        }

        /// <summary>
        /// Tests the transaction rollback.
        /// </summary>
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
            transactionMock.Verify(t => t.RollbackAsync());
        }

        /// <summary>
        /// Tests the transation excute.
        /// </summary>
        [TestMethod]
        public void TestTransationExcute()
        {
            var connectFactoryMock = new Mock<IConnectFactory>();
            var collection = new ServiceCollection();
            collection.AddDbCore(t => t.AddMySql());
            collection.AddScoped<IConnectFactory>(t => connectFactoryMock.Object);
            collection.AddSingleton<IPluginManagers>(new PluginManagers());
            collection.AddScoped<ISqlExcute, SqlExcute>();
            collection.AddScoped<ISqlExcute2, SqlExcute2>();
            var provider = collection.BuildServiceProvider();
            var ins = provider.GetRequiredService<ISqlExcute>();
            ins.InserData();
            ins.DeleteData();
            connectFactoryMock.Verify(t => t.CreateConnection(), Times.Exactly(2));
        }

        /// <summary>
        /// Tests the transation singleton excute.
        /// </summary>
        [TestMethod]
        public void TestTransationSingletonExcute()
        {
            var connectFactoryMock = new Mock<IConnectFactory>();
            var dbTransMock = new Mock<DbTransaction>();
            var collection = new ServiceCollection();
            collection.AddDbCore(t => t.AddMySql());
            collection.AddScoped<IConnectFactory>(t => connectFactoryMock.Object);
            collection.AddSingleton<IPluginManagers>(new PluginManagers());
            collection.AddSingleton<ISqlExcute, SqlExcute>();
            collection.AddSingleton<ISqlExcute2, SqlExcute2>();
            var provider = collection.BuildServiceProvider();
            var ins = provider.GetRequiredService<ISqlExcute>();
            ins.InserData();
            ins.DeleteData();
            connectFactoryMock.Verify(t => t.CreateConnection(), Times.Exactly(2));
        }

        #region TestData

        /// <summary>
        /// The sql excute.
        /// </summary>
        public interface ISqlExcute
        {
            /// <summary>
            /// Insers the data.
            /// </summary>
            void InserData();

            /// <summary>
            /// Deletes the data.
            /// </summary>
            void DeleteData();
        }

        /// <summary>
        /// The sql excute.
        /// </summary>
        public class SqlExcute : ISqlExcute
        {
            private readonly IConnectFactory _connectFactory;
            private readonly ISqlExcute2 _sqlExcute2;

            /// <summary>
            /// Initializes a new instance of the <see cref="SqlExcute"/> class.
            /// </summary>
            /// <param name="connectFactory">The connect factory.</param>
            /// <param name="sqlExcute2">The sql excute2.</param>
            public SqlExcute(IConnectFactory connectFactory, ISqlExcute2 sqlExcute2)
            {
                _connectFactory = connectFactory;
                _sqlExcute2 = sqlExcute2;
            }

            /// <summary>
            /// Insers the data.
            /// </summary>
            [Transaction]
            public void InserData()
            {
                var connect = _connectFactory.CreateConnection();
                _sqlExcute2.InserData();
                _sqlExcute2.DeleteData();
                this.DeleteData();
            }

            /// <summary>
            /// Deletes the data.
            /// </summary>
            [Transaction]
            public void DeleteData()
            {
            }
        }

        /// <summary>
        /// The sql excute2.
        /// </summary>
        public interface ISqlExcute2
        {
            /// <summary>
            /// Insers the data.
            /// </summary>
            void InserData();

            /// <summary>
            /// Deletes the data.
            /// </summary>
            void DeleteData();
        }

        /// <summary>
        /// The sql excute2.
        /// </summary>
        public class SqlExcute2 : ISqlExcute2
        {
            private readonly IConnectFactory _connectFactory;

            /// <summary>
            /// Initializes a new instance of the <see cref="SqlExcute2"/> class.
            /// </summary>
            /// <param name="connectFactory">The connect factory.</param>
            public SqlExcute2(IConnectFactory connectFactory)
            {
                _connectFactory = connectFactory;
            }

            /// <summary>
            /// Insers the data.
            /// </summary>
            [Transaction]
            public void InserData()
            {
                var connect = _connectFactory.CreateConnection();
                this.DeleteData();
            }

            /// <summary>
            /// Deletes the data.
            /// </summary>
            [Transaction]
            public void DeleteData()
            {
            }
        }

        /// <summary>
        /// The test att interceptor attribute.
        /// </summary>
        public class TestAttInterceptorAttribute : AbstractInterceptorAttribute
        {
            /// <summary>
            /// Invokes the.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="next">The next.</param>
            /// <returns>A Task.</returns>
            public override Task Invoke(AspectContext context, AspectDelegate next)
            {
                next(context);
                return Task.CompletedTask;
            }
        }

        #endregion TestData
    }
}