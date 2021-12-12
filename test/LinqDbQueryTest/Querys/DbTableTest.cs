using AspectCore.Extensions.DependencyInjection;
using AutoFixture;
using AutoFixture.AutoMoq;
using Brochure.Core.PluginsDI;
using Brochure.Core.Server;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using LinqDbQueryTest.Datas;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;

namespace LinqDbQueryTest
{
    [TestClass]
    public class DbTableTest : BaseTest
    {
        [TestMethod]
        public async Task TestIsExistTableName()
        {
            var autoMock = new AutoMocker();
            //var mockContext = Fixture.Freeze<Mock<DbContext>>();
            //    mockContext.Setup(t => t.ExecuteScalarAsync(It.IsAny<ISql>())).ReturnsAsync(1);
            var tableContext = autoMock.CreateInstance<MySqlDbTable>(true);
            var mockContext = autoMock.GetMock<DbContext>();
            mockContext.Setup(t => t.ExecuteScalarAsync(It.IsAny<ISql>())).ReturnsAsync(1L);
            var s = await tableContext.IsExistTableAsync<A>();
            mockContext.Verify(t => t.ExecuteScalarAsync(It.IsAny<ISql>()));
        }

        [TestMethod]
        public async Task TestDbContextContrus()
        {
            var service = new ServiceCollection();
            await service.AddBrochureServer();
            service.AddDbCore(p => p.AddMySql(t =>
            {
            }));
            var provider = service.BuildServiceContextProvider();
            using var socpe = provider.CreateScope();
            var taple = socpe.ServiceProvider.GetService<DbTable>();
        }

        private class A
        {
        }
    }
}