using System;
using System.Threading.Tasks;
using Brochure.Core;
using Brochure.Core.Server;
using Brochure.ORM.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    [TestClass]
    public class ServiceCollectionExtendsTest
    {
        IServiceCollection services;
        public ServiceCollectionExtendsTest()
        {
            services = new ServiceCollection();
            services.AddLogging();
        }

        [TestMethod]
        public void TestAddBrochureCore()
        {
            services.AddBrochureCore();
        }

        [TestMethod]
        public async Task TestAddBrochureCoreServer()
        {
            services.AddControllers();//此处需要加入这个 否则  会出错误
            await services.AddBrochureServer();
        }

        [TestMethod]
        public async Task TestAddMysqlDb()
        {
            services.AddMySql();
        }
    }
}
