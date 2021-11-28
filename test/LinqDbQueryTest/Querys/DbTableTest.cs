using AutoFixture;
using AutoFixture.AutoMoq;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.MySql;
using LinqDbQueryTest.Datas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace LinqDbQueryTest.Querys
{
    [TestClass]
    public class DbTableTest : BaseTest
    {
        [TestMethod]
        public async Task TestIsExistTableName()
        {
            var mockContext = Fixture.Freeze<Mock<DbContext>>();
            mockContext.Setup(t => t.ExecuteScalarAsync(It.IsAny<ISql>())).ReturnsAsync(1);
            var tableContext = Fixture.Create<DbTable>();
            var s = await tableContext.IsExistTableAsync<A>("test");
            mockContext.Verify(t => t.ExecuteScalarAsync(It.IsAny<ISql>()));
        }

        private class A
        {
        }
    }
}