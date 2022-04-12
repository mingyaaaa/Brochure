using Brochure.ORM;
using Brochure.ORM.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Brochure.ORMTest.Transaction
{
    /// <summary>
    /// The test transaction factory.
    /// </summary>
    [TestClass]
    public class TestTransactionFactory
    {
        private readonly Mock<ITransactionManager> managerMock;
        private readonly Mock<DbOption> dbOptionMock;
        private readonly Mock<IConnectFactory> connectFactoryMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTransactionFactory"/> class.
        /// </summary>
        public TestTransactionFactory()
        {
            managerMock = new Mock<ITransactionManager>();
            dbOptionMock = new Mock<DbOption>();
            connectFactoryMock = new Mock<IConnectFactory>();
        }

        /// <summary>
        /// Tests the create transaction.
        /// </summary>
        [TestMethod]
        public void TestCreateTransaction()
        {
            managerMock.Setup(t => t.IsEmpty).Returns(true);
            var factory = new TransactionFactory(managerMock.Object, connectFactoryMock.Object);
            var r = factory.GetTransaction();
            Assert.IsInstanceOfType(r, typeof(ORM.Database.Transaction));
        }

        /// <summary>
        /// Tests the create inner transaction.
        /// </summary>
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