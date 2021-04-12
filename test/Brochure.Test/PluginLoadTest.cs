using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

namespace Brochure.Test
{
    [TestClass]
    public class PluginLoadTest : BaseTest
    {
        private Mock<IObjectFactory> objFactoryMock;
        private Mock<IPluginsLoadContext> loadContextMock;

        public PluginLoadTest()
        {
            InitBaseService();
            objFactoryMock = new Mock<IObjectFactory>();
            loadContextMock = new Mock<IPluginsLoadContext>();

        }

        [TestMethod]
        public async Task TestLoad()
        {
            var pluginConfigPath = "p1/plugin.config";
            var autoMock = new AutoMocker();
            var guid = Guid.NewGuid();
            var name = "PA";
            var provider = new Mock<IServiceProvider>();
            var jsonUtilMock = new Mock<IJsonUtil>();
            jsonUtilMock.Setup(t => t.Get<PluginConfig>(pluginConfigPath)).Returns(new PluginConfig() { Key = guid, Name = name, AssemblyName = "test.dll" });
            var loadContextMock = new Mock<IPluginsLoadContext>();
            objFactoryMock.Setup(t => t.Create<IPluginsLoadContext, PluginsLoadContext>(provider.Object, It.IsAny<IAssemblyDependencyResolverProxy>()))
                .Returns(loadContextMock.Object);
            var configurationMock = new Mock<IConfiguration>();
            autoMock.Use(new ApplicationOption(null, configurationMock.Object));
            var reflectorUtilMock = new Mock<IReflectorUtil>();
            autoMock.Use<IJsonUtil>(jsonUtilMock.Object);
            autoMock.Use<IObjectFactory>(objFactoryMock.Object);
            autoMock.Use<IReflectorUtil>(reflectorUtilMock.Object);
            reflectorUtilMock.Setup(t => t.GetTypeOfAbsoluteBase(It.IsAny<Assembly>(), typeof(Plugins))).Returns(new List<Type>());
            var loader = autoMock.CreateInstance<PluginLoader>();
            await Assert.ThrowsExceptionAsync<Exception>(() => loader.LoadPlugin(provider.Object, pluginConfigPath).AsTask());

            objFactoryMock.Setup(t => t.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>()).Returns(new ConcurrentDictionary<Guid, IPluginsLoadContext>());
            reflectorUtilMock.Setup(t => t.GetTypeOfAbsoluteBase(It.IsAny<Assembly>(), typeof(Plugins))).Returns(new List<Type>() { typeof(PA) });
            objFactoryMock.Setup(t => t.Create(It.IsAny<Type>())).Returns(new PA(provider.Object));
            loader = autoMock.CreateInstance<PluginLoader>();
            var plugin = await loader.LoadPlugin(provider.Object, pluginConfigPath);
            Assert.AreEqual(guid, plugin.Key);
            Assert.AreEqual(name, plugin.Name);
            objFactoryMock.Verify(t => t.Create<IAssemblyDependencyResolverProxy, AssemblyDependencyResolverProxy>(Path.Combine("p1", "test.dll")));
            reflectorUtilMock.Setup(t => t.GetTypeOfAbsoluteBase(It.IsAny<Assembly>(), typeof(Plugins))).Returns(new List<Type>() { typeof(PA), typeof(PB) });
            loader = autoMock.CreateInstance<PluginLoader>();
            await Assert.ThrowsExceptionAsync<Exception>(() => loader.LoadPlugin(provider.Object, pluginConfigPath).AsTask());

            loadContextMock.Verify(t => t.LoadAssembly(It.IsAny<AssemblyName>()));
        }

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
            autoMock.GetMock<IObjectFactory>().Setup(t => t.Create<IPluginsLoadContext, PluginsLoadContext>(It.IsAny<object>(), It.IsAny<object>())).Returns(loadContextMock.Object);

            await ins.LoadPlugin(serviceProvider.Object);
            var manager = autoMock.CreateInstance<PluginLoader>();
            await manager.LoadPlugin(serviceProvider.Object);
            configurationMock.Verify(t => t.GetSection(Path.GetFileNameWithoutExtension(plugin.AssemblyName)));
            autoMock.GetMock<IModuleLoader>().Verify(t => t.LoadModule(It.IsAny<IServiceProvider>(), It.IsAny<IServiceCollection>(), It.IsAny<Assembly>()));
            var str = string.Empty;
        }

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
            autoMock.GetMock<IObjectFactory>().Setup(t => t.Create<IPluginsLoadContext, PluginsLoadContext>(It.IsAny<object>(), It.IsAny<object>())).Returns(loadContextMock.Object);

            await ins.LoadPlugin(serviceProvider.Object);
            var manager = autoMock.CreateInstance<PluginLoader>();
            await manager.LoadPlugin(serviceProvider.Object);

            autoMock.GetMock<IModuleLoader>().Verify(t => t.LoadModule(It.IsAny<IServiceProvider>(), It.IsAny<IServiceCollection>(), It.IsAny<Assembly>()));
            var str = string.Empty;
        }


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

        protected class PA : Plugins
        {
            public PA(IServiceProvider service) : base() { }
        }
        protected class PB : Plugins
        {
            public PB(IServiceProvider service) : base() { }
        }
    }
}