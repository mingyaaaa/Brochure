using System.Data;
using System.Data.Common;
using Brochure.ORM;
using Brochure.ORM.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Brochure.ORMTest.Transaction
{
    [TestClass]
    public class TestTransactionFactory
    {
        private readonly Mock<ITransactionManager> managerMock;
        private readonly Mock<DbOption> dbOptionMock;
        private readonly Mock<IConnectFactory> connectFactoryMock;

        public TestTransactionFactory()
        {
            managerMock = new Mock<ITransactionManager>();
            dbOptionMock = new Mock<DbOption>();
            connectFactoryMock = new Mock<IConnectFactory>();
        }

        [TestMethod]
        public void TestCreateTransaction()
        {
            managerMock.Setup(t => t.IsEmpty).Returns(true);
            var factory = new TransactionFactory(managerMock.Object, connectFactoryMock.Object);
            var r = factory.GetTransaction();
            Assert.IsInstanceOfType(r, typeof(ORM.Database.Transaction));
        }

        [TestMethod]
        public void TestCreateInnerTransaction()
        {
            managerMock.Setup(t => t.IsEmpty).Returns(false);
            var factory = new TransactionFactory(managerMock.Object, connectFactoryMock.Object);
            var r = factory.GetTransaction();
            Assert.IsInstanceOfType(r, typeof(InnerTransaction));
        }
    }
}