using System.Data;
using System.Data.Common;
using LinqDbQuery;
using LinqDbQuery.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LinqDbQueryTest.Transaction
{
    [TestClass]
    public class TestTransactionFactory
    {
        private readonly Mock<ITransactionManager> managerMock;
        private readonly Mock<DbOption> dbOptionMock;
        private readonly Mock<IDbConnection> dbConnectionMock;

        public TestTransactionFactory ()
        {
            managerMock = new Mock<ITransactionManager> ();
            dbOptionMock = new Mock<DbOption> (new Mock<IDbProvider> ().Object, managerMock.Object);
            dbConnectionMock = new Mock<IDbConnection> ();
            dbOptionMock.Setup (t => t.GetDbConnection ()).Returns (dbConnectionMock.Object);
        }

        [TestMethod]
        public void TestCreateTransaction ()
        {
            managerMock.Setup (t => t.IsEmpty).Returns (true);
            var factory = new TransactionFactory (managerMock.Object, dbOptionMock.Object);
            var r = factory.GetTransaction ();
            Assert.IsInstanceOfType (r, typeof (LinqDbQuery.Database.Transaction));
        }

        [TestMethod]
        public void TestCreateInnerTransaction ()
        {
            managerMock.Setup (t => t.IsEmpty).Returns (false);
            var factory = new TransactionFactory (managerMock.Object, dbOptionMock.Object);
            var r = factory.GetTransaction ();
            Assert.IsInstanceOfType (r, typeof (LinqDbQuery.Database.InnerTransaction));

        }
    }
}