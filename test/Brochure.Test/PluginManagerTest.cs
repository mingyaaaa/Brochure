using System;
using System.Collections.Concurrent;
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;

namespace Brochure.Test
{
    public class TestPlugin : Plugins
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestPlugin"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public TestPlugin(IServiceProvider service) : base(new PluginContext())
        {
        }
    }
    [TestClass]
    public class PluginManagerTest : BaseTest
    {
        private Mock<IPluginsLoadContext> loadPluginContext;
        private Mock<IServiceProvider> serviceProviderMock;

        public PluginManagerTest()
        {
            InitBaseService();
            InitPluginMock();
        }

        private void InitPluginMock()
        {
            loadPluginContext = new Mock<IPluginsLoadContext>();
            serviceProviderMock = new Mock<IServiceProvider>();
        }



        [TestMethod]
        public async Task TestLoadAction()
        {
            var autoMock = new AutoMocker();
            Fixture.Customizations.Add(new TypeRelay(typeof(Plugins), typeof(TestPlugin)));
            var allPluginPath = Fixture.CreateMany<string>(1).ToArray();
            var pluginConfig = Fixture.Create<PluginConfig>();
            var assemably = Fixture.Create<Assembly>();
            loadPluginContext.Setup(t => t.LoadAssembly(It.IsAny<AssemblyName>())).Returns(assemably);
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

            autoMock.GetMock<IJsonUtil>().Setup(t => t.Get<PluginConfig>(It.IsAny<string>())).Returns(pluginConfig);
            autoMock.GetMock<IReflectorUtil>().Setup(t => t.GetTypeOfAbsoluteBase(assemably, typeof(Plugins))).Returns(Fixture.CreateMany<Type>(1));
            autoMock.GetMock<IObjectFactory>().Setup(t => t.Create(It.IsAny<Type>())).Returns(plugin);
            autoMock.GetMock<IObjectFactory>().Setup(t => t.Create<IPluginsLoadContext, PluginsLoadContext>(It.IsAny<object>(), It.IsAny<object>())).Returns(loadPluginContext.Object);

            await ins.LoadPlugin(serviceProviderMock.Object);

            autoMock.GetMock<IReflectorUtil>().Verify(t => t.GetTypeOfAbsoluteBase(assemably, typeof(Plugins)), Times.Exactly(1));

            loaderActionMock.Verify(t => t.Invoke(It.IsAny<Guid>()), Times.Exactly(1));
        }

        [TestMethod]
        public async Task TestUnLoadAction()
        {
            var autoMock = new AutoMocker();
            var pluginLoadContent = new Mock<IPluginsLoadContext>();
            var guid = Fixture.Create<Guid>();
            var pluginDic = Fixture.Build<ConcurrentDictionary<Guid, IPluginsLoadContext>>().Do(t =>
            {
                t.TryAdd(guid, pluginLoadContent.Object);
            }).Create(); ;
            autoMock.GetMock<IObjectFactory>().Setup(t => t.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>()).Returns(pluginDic);

            var ins = autoMock.CreateInstance<PluginLoader>();
            await ins.UnLoad(guid);
            ;
            autoMock.GetMock<IPluginUnLoadAction>().Verify(t => t.Invoke(guid));
        }

        [TestMethod]
        public async Task TestUnLoadFailAction()
        {
            var autoMock = new AutoMocker();
            var objectFactory = new Mock<IObjectFactory>();
            var guid = Guid.NewGuid();
            var pluginMock = new Mock<IPlugins>();
            var pluginLoaderMock = new Mock<IPluginLoader>();

            objectFactory.Setup(t => t.Create<ConcurrentDictionary<Guid, IPlugins>>()).
                     Returns(new ConcurrentDictionary<Guid, IPlugins>() { [guid] = pluginMock.Object });
            pluginMock.Setup(t => t.Key).Returns(guid);
            pluginLoaderMock.Setup(t => t.UnLoad(guid)).Returns(new ValueTask<bool>(false));
            var unLoaderActionMock = new Mock<IPluginUnLoadAction>();
            autoMock.Use(unLoaderActionMock.Object);
            autoMock.Use(pluginLoaderMock.Object);
            autoMock.Use(objectFactory.Object);
            var ins = autoMock.CreateInstance<PluginManagers>();
            await ins.Remove(pluginMock.Object);
            unLoaderActionMock.Verify(t => t.Invoke(It.IsAny<Guid>()), Times.Never);
        }

    }

    public class PA : Plugins
    {
        public PA(IServiceProvider service) : base(new PluginContext()) { }
    }
}