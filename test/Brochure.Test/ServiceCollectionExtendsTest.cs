using Autofac;
using Autofac.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core;
using Brochure.Core.PluginsDI;
using Brochure.Core.Server;
using Brochure.Core.Server.Core;
using Brochure.ORM.Extensions;
using Brochure.ORM.MySql;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
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
        }

        /// <summary>
        /// Tests the add brochure core.
        /// </summary>
        [TestMethod]
        public async Task TestAddBrochureServer()
        {
            await services.AddBrochureServer();
            var provider = services.BuildServiceProvider();
            var s = provider.GetServices<IHostedService>().FirstOrDefault(t => t is PluginLoadService);
            Assert.IsNotNull(s);
            var a = provider.GetService<IControllerActivator>();
            Assert.IsNotNull(a as PluginScopeControllerActivator);

            Assert.IsNotNull(Log.Logger);

            var m = provider.GetService<IMiddleManager>();
            Assert.IsNotNull(m);
            var b = provider.GetService<IPluginUnLoadAction>();
            Assert.IsNotNull(b);
            var c = provider.GetService<IActionDescriptorChangeProvider>();
            Assert.IsNotNull(c);
            var d = provider.GetService<IApplicationBuilderFactory>();
            Assert.IsNotNull(d as PluginApplicationBuilderFactory);

            var aa = provider.GetService<IPluginManagers>();
            Assert.IsNotNull(aa);
            var bb = provider.GetService<IJsonUtil>();
            Assert.IsNotNull(bb);

            var cc = provider.GetService<IReflectorUtil>();
            Assert.IsNotNull(cc);
            var dd = provider.GetService<IObjectFactory>();
            Assert.IsNotNull(dd);
            var dd2 = provider.GetService<ISysDirectory>();
            Assert.IsNotNull(dd2);
            var dd3 = provider.GetService<IFile>();
            Assert.IsNotNull(dd3);
            var dd4 = provider.GetService<IHostEnvironment>();
            Assert.IsNotNull(dd4);
            var dd5 = provider.GetService<IHostEnvironment>();
            Assert.IsNotNull(dd5);
            var dd6 = provider.GetService<IPluginLoadAction>();
            var dd7 = provider.GetService<IPluginLoader>();
            Assert.IsNotNull(dd7);
            var cc1 = provider.GetService<IPluginLoadContextProvider>();
            Assert.IsNotNull(cc1);
            var cc2 = provider.GetService<IPluginUnLoadAction>();
            Assert.IsNotNull(cc2);
            var cc3 = provider.GetService<IPluginConfigurationLoad>();
            Assert.IsNotNull(cc3);
            var cc4 = provider.GetService<ApplicationOption>();
            Assert.IsNotNull(cc4);
        }

        [TestMethod]
        public async Task TestAutofac()
        {
            await services.AddBrochureServer();
            var builder = new ContainerBuilder();
            builder.Populate(services);
            var provider = builder.Build();
            var cc5 = provider.Resolve<IPluginScope<Student>>();
            Assert.IsNotNull(cc5);
            var cc6 = provider.Resolve<IPluginSingleton<Student>>();
            Assert.IsNotNull(cc6);
            var cc7 = provider.Resolve<IPluginTransient<Student>>();
            Assert.IsNotNull(cc7);
        }

        /// <summary>
        /// Tests the add brochure core server.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod("IMvcBuilder")]
        public async Task TestAddBrochureController1()
        {
            await services.AddPluginController();
            var provider = services.BuildServiceProvider();
            var a = provider.GetService<IMvcCoreBuilder>();
            Assert.IsNotNull(a);
        }

        [TestMethod]
        public void TestAddBrochureCore()
        {
            services.AddBrochureCore();
        }

        /// <summary>
        /// Tests the add mysql db.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public void TestAddMysqlDb()
        {
            //    services.AddDbCore(t => t.AddMySql());
        }
    }

    public class Student
    {
    }
}