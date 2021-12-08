﻿using AspectCore.Extensions.DependencyInjection;
using AutoFixture;
using AutoFixture.AutoMoq;
using Brochure.Core.Server;
using Brochure.ORM;
using Brochure.ORM.Database;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using LinqDbQueryTest.Datas;
using Microsoft.Extensions.DependencyInjection;
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