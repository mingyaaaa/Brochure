using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core;
using Brochure.Core.PluginsDI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Brochure.Test
{
    /// <summary>
    /// The plugin load test.
    /// </summary>
    [TestClass]
    public class PluginLoadTest : BaseTest
    {
        private Mock<IObjectFactory> objFactoryMock;
        private Mock<IPluginsLoadContext> loadContextMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadTest"/> class.
        /// </summary>
        public PluginLoadTest()
        {
            InitBaseService();
            objFactoryMock = new Mock<IObjectFactory>();
            loadContextMock = new Mock<IPluginsLoadContext>();
        }

        /// <summary>
        /// Tests the load1.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestLoad1()
        {
            var mockContrainer = new Mock<IPluginServiceProvider>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var pluginManager = Fixture.Freeze<Mock<IPluginManagers>>();
            var dir = Fixture.Freeze<Mock<ISysDirectory>>();
            var basePath = "basePath";
            pluginManager.Setup(t => t.GetBasePluginsPath()).Returns("basePath");

            var load = Fixture.Create<PluginLoader>();
            await load.LoadPlugin();

            pluginManager.Verify(t => t.GetBasePluginsPath());
            dir.Verify(t => t.GetFiles(basePath, "plugin.config", SearchOption.AllDirectories));
        }

        [TestMethod("测试没有实现Plugins插件")]
        public void TestLoadPlugin()
        {
            var mockContrainer = new Mock<IPluginServiceProvider>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var dir = Fixture.Freeze<Mock<ISysDirectory>>();
            var jsonUtil = Fixture.Freeze<Mock<IJsonUtil>>();
            var basePath = "/a/basePath";
            var pluginConfig = Fixture.Create<PluginConfig>();
            dir.Setup(t => t.GetFiles(It.IsAny<string>(), "plugin.config", SearchOption.AllDirectories)).Returns(new string[] { basePath });
            jsonUtil.Setup(t => t.Get<PluginConfig>(basePath)).Returns(pluginConfig);
            var load = Fixture.Create<PluginLoader>();

            Assert.ThrowsExceptionAsync<Exception>(() => load.LoadPlugin().AsTask());
        }

        [TestMethod("测试多个Plugins实现")]
        public void MyTestMethod()
        {
            var mockContrainer = new Mock<IPluginServiceProvider>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var dir = Fixture.Freeze<Mock<ISysDirectory>>();
            var jsonUtil = Fixture.Freeze<Mock<IJsonUtil>>();
            var reflectorUtil = Fixture.Freeze<Mock<IReflectorUtil>>();
            var basePath = "/a/basePath";
            var pluginConfig = Fixture.Create<PluginConfig>();
            dir.Setup(t => t.GetFiles(It.IsAny<string>(), "plugin.config", SearchOption.AllDirectories)).Returns(new string[] { basePath });
            jsonUtil.Setup(t => t.Get<PluginConfig>(basePath)).Returns(pluginConfig);
            reflectorUtil.Setup(t => t.GetTypeOfAbsoluteBase(It.IsAny<Assembly>(), typeof(Plugins))).Returns(new Type[] { typeof(PA), typeof(PB) });
            var load = Fixture.Create<PluginLoader>();

            Assert.ThrowsExceptionAsync<Exception>(() => load.LoadPlugin().AsTask());
        }

        [TestMethod("测试正常插件实现")]
        public async Task TestLoadPlugin3()
        {
            var mockContrainer = new Mock<IPluginServiceProvider>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var jsonUtil = Fixture.Freeze<Mock<IJsonUtil>>();
            var reflectorUtil = Fixture.Freeze<Mock<IReflectorUtil>>();

            var basePath = "/a/basePath";
            var pluginConfig = Fixture.Create<PluginConfig>();
            jsonUtil.Setup(t => t.Get<PluginConfig>(basePath)).Returns(pluginConfig);
            reflectorUtil.Setup(t => t.GetTypeOfAbsoluteBase(It.IsAny<Assembly>(), typeof(Plugins))).Returns(new Type[] { typeof(PA) });

            var load = Fixture.Create<PluginLoader>();

            var a = await load.LoadPlugin(basePath);
            Assert.IsNotNull(a);
        }

        /// <summary>
        /// Tests the un load.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestUnLoad()
        {
            var autoMock = new AutoMocker();
            var key = Guid.NewGuid();
            autoMock.Use<IObjectFactory>(objFactoryMock.Object);
            objFactoryMock.Setup(t => t.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>())
                .Returns(new ConcurrentDictionary<Guid, IPluginsLoadContext>());
            var load = autoMock.CreateInstance<PluginLoader>();
            await load.UnLoad(key);
            loadContextMock.Verify(t => t.UnLoad(), Times.Never);

            objFactoryMock.Setup(t => t.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>())
                .Returns(new ConcurrentDictionary<Guid, IPluginsLoadContext>()
                {
                    [key] = loadContextMock.Object
                });
            load = autoMock.CreateInstance<PluginLoader>();
            await load.UnLoad(key);
            loadContextMock.Verify(t => t.UnLoad());
        }

        /// <summary>
        /// The p a.
        /// </summary>
        protected class PA : Plugins
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PA"/> class.
            /// </summary>
            /// <param name="service">The service.</param>
            public PA() : base()
            {
            }
        }

        /// <summary>
        /// The p b.
        /// </summary>
        protected class PB : Plugins
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PB"/> class.
            /// </summary>
            /// <param name="service">The service.</param>
            public PB() : base()
            {
            }
        }
    }
}