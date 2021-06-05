using AutoFixture;
using Brochure.ORM.Database;
using LinqDbQueryTest.Datas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Org.BouncyCastle.Bcpg.Sig;
using System.Data;
using System.Threading.Tasks;

namespace LinqDbQueryTest.Querys
{
    [TestClass]
    public class DbTableTest : BaseTest
    {

        [TestMethod]
        public async Task TestIsExistTableName()
        {
            var connectFactory = Fixture.Freeze<Mock<IConnectFactory>>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            mockCommand.Setup(t => t.ExecuteScalar()).Returns(0);
            mockConnection.Setup(t => t.CreateCommand()).Returns(mockCommand.Object);
            connectFactory.Setup(t => t.CreateConnection()).Returns(mockConnection.Object);
            var tableContext = Fixture.Create<DbTable>();
            var s = await tableContext.IsExistTableAsync<A>();
            Assert.IsFalse(s);
        }

        private class A
        {

        }
    }
}
