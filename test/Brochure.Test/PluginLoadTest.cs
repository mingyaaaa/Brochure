using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core;
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
            var reflectorUtilMock = new Mock<IReflectorUtil>();
            autoMock.Use<IJsonUtil>(jsonUtilMock.Object);
            autoMock.Use<IObjectFactory>(objFactoryMock.Object);
            autoMock.Use<IReflectorUtil>(reflectorUtilMock.Object);
            reflectorUtilMock.Setup(t => t.GetTypeOfAbsoluteBase(It.IsAny<Assembly>(), typeof(Plugins))).Returns(new List<Type>());
            var loader = autoMock.CreateInstance<PluginLoader>();
            await Assert.ThrowsExceptionAsync<Exception>(() => loader.LoadPlugin(provider.Object, pluginConfigPath).AsTask());

            objFactoryMock.Setup(t => t.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>()).Returns(new ConcurrentDictionary<Guid, IPluginsLoadContext>());
            reflectorUtilMock.Setup(t => t.GetTypeOfAbsoluteBase(It.IsAny<Assembly>(), typeof(Plugins))).Returns(new List<Type>() { typeof(PA) });
            objFactoryMock.Setup(t => t.Create(It.IsAny<Type>(), provider.Object)).Returns(new PA(provider.Object));
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