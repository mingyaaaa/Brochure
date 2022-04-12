using AutoFixture;
using AutoFixture.AutoMoq;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core;
using Brochure.Core.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Brochure.Test
{
    /// <summary>
    /// The plugin application builder test.
    /// </summary>
    [TestClass]
    public class PluginApplicationBuilderTest : BaseTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginApplicationBuilderTest"/> class.
        /// </summary>
        public PluginApplicationBuilderTest()
        {
        }

        /// <summary>
        /// Tests the use routing.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestUseRouting()
        {
            var listener = new DiagnosticListener("Microsoft.AspNetCore");
            Service.TryAddSingleton<DiagnosticListener>(listener);
            Service.AddOptions();
            Service.AddRouting();
            await Service.AddBrochureServer();
            var provider = Service.BuildServiceProvider();
            var manager = provider.GetService<IPluginManagers>();
            var middleManager = provider.GetService<IMiddleManager>();
            var builderFactory = new ApplicationBuilderFactory(provider);
            var applicationBuilder = builderFactory.CreateBuilder(new FeatureCollection());
            var pluginbuilderFactory = new PluginApplicationBuilderFactory(provider);
            var builder = pluginbuilderFactory.CreateBuilder(new FeatureCollection());
            builder.IntertMiddle("main-routing", Guid.Empty, 10, () => builder.UseRouting());
            var count = middleManager.GetMiddlesList().Count;
            Assert.AreEqual(1, count);
        }

        /// <summary>
        /// Tests the use config.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestUseConfig()
        {
            var listener = new DiagnosticListener("Microsoft.AspNetCore");
            Service.TryAddSingleton<DiagnosticListener>(listener);
            Service.AddOptions();
            Service.AddRouting();
            await Service.AddBrochureServer();
            var provider = Service.BuildServiceProvider();
            var manager = provider.GetService<IPluginManagers>();

            var middleManager = provider.GetService<IMiddleManager>();
            var builderFactory = new ApplicationBuilderFactory(provider);
            var applicationBuilder = builderFactory.CreateBuilder(new FeatureCollection());
            var pluginbuilderFactory = new PluginApplicationBuilderFactory(provider);
            var builder = pluginbuilderFactory.CreateBuilder(new FeatureCollection());
            builder.ConfigPlugin();
        }

        /// <summary>
        /// Tests the config plugin.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public void TestConfigPlugin()
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

        /// <summary>
        /// The test plugins.
        /// </summary>
        public class TestPlugins : Plugins
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestPlugins"/> class.
            /// </summary>
            /// <param name="service">The service.</param>
            public TestPlugins(IServiceProvider service) : base(new PluginContext())
            {
            }
        }
    }
}