using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;

namespace Brochure.Test
{
    public class TestPlugins : Plugins
    {
        public TestPlugins(IServiceProvider services) : base(services) { }
    }

    [TestClass]
    public class PluginManagerTest : BaseTest
    {
        public PluginManagerTest()
        {
            InitBaseService();
        }

        [TestMethod]
        public async Task TestResolvePlugins()
        {
            var autoMock = new AutoMocker();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var dirMock = new Mock<ISysDirectory>();
            var pluginLoaderMock = new Mock<IPluginLoader>();
            var modulerMock = new Mock<IModuleLoader>();
            var objectFactory = new Mock<IObjectFactory>();
            var pluginContextMock = new Mock<IPluginContext>();
            var key = Guid.NewGuid();
            var pluginMock = new Mock<IPlugins>();
            pluginLoaderMock.Setup(t => t.LoadPlugin(It.IsAny<IServiceProvider>(), It.IsAny<string>()))
                .Returns(ValueTask.FromResult<IPlugins>(pluginMock.Object));
            pluginMock.Setup(t => t.Context).Returns(pluginContextMock.Object);

            objectFactory.Setup(t => t.Create<ConcurrentDictionary<Guid, IPlugins>>()).
            Returns(new ConcurrentDictionary<Guid, IPlugins>());

            autoMock.Use<ISysDirectory>(dirMock.Object);
            autoMock.Use<IModuleLoader>(modulerMock);
            autoMock.Use<ISysDirectory>(dirMock.Object);
            autoMock.Use<IObjectFactory>(objectFactory.Object);
            autoMock.Use<IPluginLoader>(pluginLoaderMock.Object);
            dirMock.Setup(t => t.GetFiles(It.IsAny<string>(), It.IsAny<string>(), SearchOption.AllDirectories))
                .Returns(new string[] { "PA" });
            var msg = string.Empty;
            pluginMock.Setup(t => t.StartingAsync(out msg)).Returns(Task.FromResult(true));

            var manager = autoMock.CreateInstance<PluginLoader>();
            await manager.LoadPlugin(serviceProviderMock.Object);
            modulerMock.Verify(t => t.LoadModule(It.IsAny<IServiceProvider>(), It.IsAny<PluginServiceCollectionContext>(), It.IsAny<Assembly>()));
            pluginMock.Verify(t => t.StartingAsync(out msg));
        }

        [TestMethod]
        public async Task TestLoadAction()
        {
            string str = string.Empty;
            var autoMock = new AutoMocker();
            var pluginContextMock = new Mock<IPluginContext>();
            var objectFactory = new Mock<IObjectFactory>();
            var dirMock = new Mock<ISysDirectory>();
            var pluginMock = new Mock<IPlugins>();
            var pluginLoaderMock = new Mock<IPluginLoader>();
            var loaderActionMock = new Mock<IPluginLoadAction>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            pluginMock.Setup(t => t.StartingAsync(out str)).Returns(Task.FromResult(true));
            pluginMock.Setup(t => t.Context).Returns(pluginContextMock.Object);
            objectFactory.Setup(t => t.Create<ConcurrentDictionary<Guid, IPlugins>>()).
          Returns(new ConcurrentDictionary<Guid, IPlugins>());
            dirMock.Setup(t => t.GetFiles(It.IsAny<string>(), It.IsAny<string>(), SearchOption.AllDirectories))
              .Returns(new string[] { "PA" });
            pluginLoaderMock.Setup(t => t.LoadPlugin(It.IsAny<IServiceProvider>(), It.IsAny<string>()))
    .Returns(ValueTask.FromResult<IPlugins>(pluginMock.Object));

            autoMock.Use(dirMock.Object);
            autoMock.Use(pluginLoaderMock.Object);
            autoMock.Use(loaderActionMock.Object);

            autoMock.Use<IObjectFactory>(objectFactory.Object);
            var ins = autoMock.CreateInstance<PluginLoader>();
            await ins.LoadPlugin(serviceProviderMock.Object);

            loaderActionMock.Verify(t => t.Invoke(It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public async Task TestUnLoadAction()
        {
            var autoMock = new AutoMocker();
            var objectFactory = new Mock<IObjectFactory>();
            var guid = Guid.NewGuid();
            var pluginMock = new Mock<IPlugins>();
            var pluginLoaderMock = new Mock<IPluginLoader>();

            objectFactory.Setup(t => t.Create<ConcurrentDictionary<Guid, IPlugins>>()).
                     Returns(new ConcurrentDictionary<Guid, IPlugins>() { [guid] = pluginMock.Object });
            pluginMock.Setup(t => t.Key).Returns(guid);
            pluginLoaderMock.Setup(t => t.UnLoad(guid)).Returns(new ValueTask<bool>(true));
            var unLoaderActionMock = new Mock<IPluginUnLoadAction>();
            autoMock.Use(unLoaderActionMock.Object);
            autoMock.Use(pluginLoaderMock.Object);
            autoMock.Use(objectFactory.Object);
            var ins = autoMock.CreateInstance<PluginManagers>();
            await ins.Remove(pluginMock.Object);
            unLoaderActionMock.Verify(t => t.Invoke(It.IsAny<Guid>()));
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
        public PA(IServiceProvider service) : base(service) { }
    }
}