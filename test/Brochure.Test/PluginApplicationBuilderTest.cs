using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Extenstions;
using Brochure.Core.Server;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Brochure.Abstract.PluginDI;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq.AutoMock;
using Moq;
using Brochure.Abstract.Utils;
using System.Collections.Generic;
using Brochure.Core;
using System.Reflection;
using AutoFixture;
using AutoFixture.AutoMoq;

namespace Brochure.Test
{
    [TestClass]
    public class PluginApplicationBuilderTest : BaseTest
    {
        public PluginApplicationBuilderTest()
        {

        }

        [TestMethod]
        public async Task TestUseRouting()
        {
            var listener = new DiagnosticListener("Microsoft.AspNetCore");
            Service.TryAddSingleton<DiagnosticListener>(listener);
            Service.AddOptions();
            Service.AddRouting();
            await Service.AddBrochureServer();
            var provider = Service.BuildPluginServiceProvider();
            var manager = provider.GetService<IPluginManagers>();
            var middleManager = provider.GetService<IMiddleManager>();
            var builderFactory = new ApplicationBuilderFactory(provider);
            var applicationBuilder = builderFactory.CreateBuilder(new FeatureCollection());
            var pluginbuilderFactory = new PluginApplicationBuilderFactory(provider as IPluginServiceProvider, manager);
            var builder = pluginbuilderFactory.CreateBuilder(new FeatureCollection());
            builder.IntertMiddle("main-routing", Guid.Empty, 10, () => builder.UseRouting());
            var count = middleManager.GetMiddlesList().Count;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task TestUseConfig()
        {
            var listener = new DiagnosticListener("Microsoft.AspNetCore");
            Service.TryAddSingleton<DiagnosticListener>(listener);
            Service.AddOptions();
            Service.AddRouting();
            await Service.AddBrochureServer();
            var provider = Service.BuildPluginServiceProvider();
            var manager = provider.GetService<IPluginManagers>();

            var middleManager = provider.GetService<IMiddleManager>();
            var builderFactory = new ApplicationBuilderFactory(provider);
            var applicationBuilder = builderFactory.CreateBuilder(new FeatureCollection());
            var pluginbuilderFactory = new PluginApplicationBuilderFactory(provider as IPluginServiceProvider, manager);
            var builder = pluginbuilderFactory.CreateBuilder(new FeatureCollection());
            builder.ConfigPlugin();
        }

        /// <summary>
        /// Tests the config plugin.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestConfigPlugin()
        {
            var autoMock = new AutoMocker();
            var fix = new Fixture();
            fix.Customize(new AutoMoqCustomization());
            var serviceProviderMock = new Mock<IServiceProvider>();
            var pluginManagerMock = new Mock<IPluginManagers>();
            var startConfigMock = new Mock<IStarupConfigure>();
            var middleManager = new Mock<IMiddleManager>();
            fix.Freeze<Mock<IServiceProvider>>().Setup(t => t.GetService(typeof(IPluginContext))).Returns(new Mock<IPluginContext>().Object);
            var plugin = fix.Create<TestPlugin>();
            var reflectUtilMock = new Mock<IReflectorUtil>();
            serviceProviderMock.Setup(t => t.GetService(typeof(IPluginManagers))).Returns(pluginManagerMock.Object);
            serviceProviderMock.Setup(t => t.GetService(typeof(IReflectorUtil))).Returns(reflectUtilMock.Object);
            serviceProviderMock.Setup(t => t.GetService(typeof(IMiddleManager))).Returns(middleManager.Object);

            pluginManagerMock.Setup(t => t.GetPlugins()).Returns(new List<IPlugins>() { plugin });
            reflectUtilMock.Setup(t => t.GetObjectOfBase<IStarupConfigure>(It.IsAny<Assembly>())).Returns(new List<IStarupConfigure>() { startConfigMock.Object });
            var appBuilderMock = autoMock.GetMock<IApplicationBuilder>();
            appBuilderMock.Setup(t => t.ApplicationServices).Returns(serviceProviderMock.Object);
            var ins = autoMock.CreateInstance<PluginApplicationBuilder>();
            ins.ConfigPlugin();
            reflectUtilMock.Verify(t => t.GetObjectOfBase<IStarupConfigure>(plugin.Assembly));
            startConfigMock.Verify(t => t.Configure(plugin.Key, ins));

        }
        public class TestPlugins : Plugins
        {
            public TestPlugins(IServiceProvider service) : base(new PluginContext())
            {
            }
        }
    }
}
