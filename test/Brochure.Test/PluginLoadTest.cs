using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core;
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
            var mockServiceProvider = new Mock<IServiceProvider>();
            var pluginManager = Fixture.Freeze<Mock<IPluginManagers>>();
            var dir = Fixture.Freeze<Mock<ISysDirectory>>();
            var basePath = "basePath";
            pluginManager.Setup(t => t.GetBasePluginsPath()).Returns("basePath");

            var load = Fixture.Create<PluginLoader>();
            await load.LoadPlugin(Service);

            pluginManager.Verify(t => t.GetBasePluginsPath());
            dir.Verify(t => t.GetFiles(basePath, "plugin.config", SearchOption.AllDirectories));
        }

        /// <summary>
        /// Tests the load2.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestLoad2()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            var dir = Fixture.Freeze<Mock<ISysDirectory>>();
            var jsonUtil = Fixture.Freeze<Mock<IJsonUtil>>();
            var basePath = "/a/basePath";
            var pluginConfig = Fixture.Create<PluginConfig>();
            dir.Setup(t => t.GetFiles(It.IsAny<string>(), "plugin.config", SearchOption.AllDirectories)).Returns(new string[] { basePath });
            jsonUtil.Setup(t => t.Get<PluginConfig>(basePath)).Returns(pluginConfig);
            var load = Fixture.Create<PluginLoader>();

            await load.LoadPlugin(Service);

            jsonUtil.Verify(t => t.Get<PluginConfig>(basePath));
        }

        /// <summary>
        /// Tests the load3.
        /// </summary>
        /// <param name="typeCount">The type count.</param>
        /// <returns>A Task.</returns>
        [TestMethod]
        [DataRow(0)]
        [DataRow(2)]
        public async Task TestLoad3(int typeCount)
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            var dir = Fixture.Freeze<Mock<ISysDirectory>>();
            var jsonUtil = Fixture.Freeze<Mock<IJsonUtil>>();
            var reflector = Fixture.Freeze<Mock<IReflectorUtil>>();
            var basePath = "/a/basePath";
            dir.Setup(t => t.GetFiles(It.IsAny<string>(), "plugin.config", SearchOption.AllDirectories)).Returns(new string[] { basePath });

            var plugins = Fixture.CreateMany<Type>(typeCount);
            reflector.Setup(t => t.GetTypeOfAbsoluteBase(It.IsAny<Assembly>(), typeof(Plugins))).Returns(plugins);
            var load = Fixture.Create<PluginLoader>();

            await load.LoadPlugin(Service);
            //    moduleLoader.Verify(t => t.LoadModule(It.IsAny<ServiceCollection>(), It.IsAny<Assembly>()), Times.Never);
        }

        /// <summary>
        /// Tests the load4.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestLoad4()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            var dir = Fixture.Freeze<Mock<ISysDirectory>>();
            var jsonUtil = Fixture.Freeze<Mock<IJsonUtil>>();
            var reflector = Fixture.Freeze<Mock<IReflectorUtil>>();
            var objectFactory = Fixture.Freeze<Mock<IObjectFactory>>();
            var loadProvider = Fixture.Freeze<Mock<IPluginLoadContextProvider>>();
            var basePath = "/a/basePath";
            dir.Setup(t => t.GetFiles(It.IsAny<string>(), "plugin.config", SearchOption.AllDirectories)).Returns(new string[] { basePath });

            var pluginsType = Fixture.Create<Type>();
            var plugin = Fixture.Create<Plugins>();
            var pluginConfig = Fixture.Create<PluginConfig>();
            reflector.Setup(t => t.GetTypeOfAbsoluteBase(It.IsAny<Assembly>(), typeof(Plugins))).Returns(new Type[] { pluginsType });
            objectFactory.Setup(t => t.Create(pluginsType)).Returns(plugin);
            jsonUtil.Setup(t => t.Get<PluginConfig>(basePath)).Returns(pluginConfig);
            loadProvider.Setup(t => t.CreateLoadContext(It.IsAny<string>())).Returns(loadContextMock.Object);
            var load = Fixture.Create<PluginLoader>();

            await load.LoadPlugin(Service);
            //  moduleLoader.Verify(t => t.LoadModule(It.IsAny<ServiceCollection>(), It.IsAny<Assembly>()), Times.Exactly(1));
        }

        /// <summary>
        /// Tests the resolve plugins.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestResolvePlugins()
        {
            var autoMock = new AutoMocker();
            Fixture.Customizations.Add(new TypeRelay(typeof(Plugins), typeof(TestPlugin)));
            var allPluginPath = Fixture.CreateMany<string>(1).ToArray();
            var pluginConfig = Fixture.Create<PluginConfig>();
            var assemably = Fixture.Create<Assembly>();
            loadContextMock.Setup(t => t.LoadAssembly(It.IsAny<AssemblyName>())).Returns(assemably);
            var serviceProvider = Fixture.Freeze<Mock<IServiceProvider>>();
            Fixture.Customize(new AutoMoqCustomization());
            serviceProvider.Setup(t => t.GetService(typeof(IPluginContext))).Returns(new Mock<IPluginContext>().Object);
            var plugin = Fixture.Create<Plugins>();
            autoMock.GetMock<IObjectFactory>().Setup(t => t.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>()).Returns(new ConcurrentDictionary<Guid, IPluginsLoadContext>());
            var ins = autoMock.CreateInstance<PluginLoader>();
            var loaderActionMock = autoMock.GetMock<IPluginLoadAction>();
            var directoryMokc = autoMock.GetMock<ISysDirectory>();
            directoryMokc.Setup(t => t.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
                .Returns(allPluginPath);
            var configurationMock = new Mock<IConfiguration>();
            autoMock.Use(new ApplicationOption(null, configurationMock.Object));
            autoMock.GetMock<IJsonUtil>().Setup(t => t.Get<PluginConfig>(It.IsAny<string>())).Returns(pluginConfig);
            autoMock.GetMock<IReflectorUtil>().Setup(t => t.GetTypeOfAbsoluteBase(assemably, typeof(Plugins))).Returns(Fixture.CreateMany<Type>(1));
            autoMock.GetMock<IObjectFactory>().Setup(t => t.Create(It.IsAny<Type>())).Returns(plugin);
            autoMock.GetMock<IPluginLoadContextProvider>().Setup(t => t.CreateLoadContext(It.IsAny<string>())).Returns(loadContextMock.Object);

            await ins.LoadPlugin(Service);
            var manager = autoMock.CreateInstance<PluginLoader>();
            await manager.LoadPlugin(Service);
            configurationMock.Verify(t => t.GetSection(Path.GetFileNameWithoutExtension(plugin.AssemblyName)));
            //        autoMock.GetMock<IModuleLoader>().Verify(t => t.LoadModule(It.IsAny<IServiceCollection>(), It.IsAny<Assembly>()));
            var str = string.Empty;
        }

        /// <summary>
        /// Tests the plugins config.
        /// </summary>
        /// <returns>A Task.</returns>
        [TestMethod]
        public async Task TestPluginsConfig()
        {
            var autoMock = new AutoMocker();
            Fixture.Customizations.Add(new TypeRelay(typeof(Plugins), typeof(TestPlugin)));
            var allPluginPath = Fixture.CreateMany<string>(1).ToArray();
            var pluginConfig = Fixture.Create<PluginConfig>();
            var assemably = Fixture.Create<Assembly>();
            loadContextMock.Setup(t => t.LoadAssembly(It.IsAny<AssemblyName>())).Returns(assemably);
            var serviceProvider = Fixture.Freeze<Mock<IServiceProvider>>();
            Fixture.Customize(new AutoMoqCustomization());
            serviceProvider.Setup(t => t.GetService(typeof(IPluginContext))).Returns(new Mock<IPluginContext>().Object);
            var plugin = Fixture.Create<Plugins>();
            autoMock.GetMock<IObjectFactory>().Setup(t => t.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>()).Returns(new ConcurrentDictionary<Guid, IPluginsLoadContext>());
            var ins = autoMock.CreateInstance<PluginLoader>();
            var loaderActionMock = autoMock.GetMock<IPluginLoadAction>();
            var directoryMokc = autoMock.GetMock<ISysDirectory>();

            directoryMokc.Setup(t => t.GetFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
                .Returns(allPluginPath);
            var configurationMock = new Mock<IConfiguration>();
            autoMock.Use(new ApplicationOption(null, configurationMock.Object));
            autoMock.GetMock<IJsonUtil>().Setup(t => t.Get<PluginConfig>(It.IsAny<string>())).Returns(pluginConfig);
            autoMock.GetMock<IReflectorUtil>().Setup(t => t.GetTypeOfAbsoluteBase(assemably, typeof(Plugins))).Returns(Fixture.CreateMany<Type>(1));
            autoMock.GetMock<IObjectFactory>().Setup(t => t.Create(It.IsAny<Type>())).Returns(plugin);
            autoMock.GetMock<IPluginLoadContextProvider>().Setup(t => t.CreateLoadContext(It.IsAny<string>())).Returns(loadContextMock.Object);

            await ins.LoadPlugin(new ServiceCollection());
            var manager = autoMock.CreateInstance<PluginLoader>();
            await manager.LoadPlugin(new ServiceCollection());

            //     autoMock.GetMock<IModuleLoader>().Verify(t => t.LoadModule(It.IsAny<IServiceCollection>(), It.IsAny<Assembly>()));
            var str = string.Empty;
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
            public PA(IServiceProvider service) : base()
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
            public PB(IServiceProvider service) : base()
            {
            }
        }
    }
}