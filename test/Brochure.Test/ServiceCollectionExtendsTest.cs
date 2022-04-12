using Brochure.Core;
using Brochure.Core.Server;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Brochure.Test
{
    /// <summary>
    /// The service collection extends test.
    /// </summary>
    [TestClass]
    public class ServiceCollectionExtendsTest
    {
        private IServiceCollection services;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCollectionExtendsTest"/> class.
        /// </summary>
        public ServiceCollectionExtendsTest()
        {
            services = new ServiceCollection();
            services.AddLogging();
        }

        /// <summary>
        /// Tests the add brochure core.
        /// </summary>
        [TestMethod]
        public void TestAddBrochureCore()
        {
            services.AddBrochureCore();
        }

        /// <summary>
        /// Tests the add brochure core server.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestAddBrochureCoreServer()
        {
            services.AddControllers();//此处需要加入这个 否则  会出错误
            await services.AddBrochureServer();
        }

        /// <summary>
        /// Tests the add mysql db.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public void TestAddMysqlDb()
        {
            services.AddDbCore(t => t.AddMySql());
        }
    }
}