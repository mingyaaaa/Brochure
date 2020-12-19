using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Models;
using Brochure.Core.Module;
using Brochure.SysInterface;
using Brochure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using Moq.Protected;

namespace Brochure.Test
{
    public class TestPlugins : Plugins
    {
        public TestPlugins (IServiceProvider services) : base (services) { }
    }

    [TestClass]
    public class PluginManagerTest : BaseTest
    {
        public PluginManagerTest ()
        {
            InitBaseService ();
        }

        [TestMethod]
        public async Task TestResolvePlugins ()
        {
            var autoMock = new AutoMocker ();
            var serviceProviderMock = new Mock<IServiceProvider> ();
            var dirMock = new Mock<ISysDirectory> ();
            var pluginLoaderMock = new Mock<IPluginLoader> ();
            var modulerMock = new Mock<IModuleLoader> ();
            var objectFactory = new Mock<IObjectFactory> ();
            var pluginContextMock = new Mock<IPluginContext> ();
            var key = Guid.NewGuid ();
            var pluginMock = new Mock<IPlugins> ();
            pluginLoaderMock.Setup (t => t.LoadPlugin (It.IsAny<IServiceProvider> (), It.IsAny<string> ()))
                .Returns (ValueTask.FromResult<IPlugins> (pluginMock.Object));
            pluginMock.Setup (t => t.Context).Returns (pluginContextMock.Object);

            objectFactory.Setup (t => t.Create<ConcurrentDictionary<Guid, IPlugins>> ()).
            Returns (new ConcurrentDictionary<Guid, IPlugins> ());

            autoMock.Use<ISysDirectory> (dirMock.Object);
            autoMock.Use<IModuleLoader> (modulerMock);
            autoMock.Use<ISysDirectory> (dirMock.Object);
            autoMock.Use<IObjectFactory> (objectFactory.Object);
            autoMock.Use<IPluginLoader> (pluginLoaderMock.Object);
            dirMock.Setup (t => t.GetFiles (It.IsAny<string> (), It.IsAny<string> (), SearchOption.AllDirectories))
                .Returns (new string[] { "PA" });
            var msg = string.Empty;
            pluginMock.Setup (t => t.StartingAsync (out msg)).Returns (Task.FromResult (true));

            var manager = autoMock.CreateInstance<PluginManagers> ();
            await manager.ResolverPlugins (serviceProviderMock.Object);
            modulerMock.Verify (t => t.LoadModule (It.IsAny<IServiceProvider> (), It.IsAny<PluginServiceCollectionContext> (), It.IsAny<Assembly> ()));
            pluginMock.Verify (t => t.StartingAsync (out msg));
            // var dirMock = GetMockService<ISysDirectory> ();
            // var jsonUtilMock = GetMockService<IJsonUtil> ();
            // var objectFactoryMock = GetMockService<IObjectFactory> ();
            // var resolver = new Mock<IAssemblyDependencyResolverProxy> ();
            // var loadContextMock = new Mock<PluginsLoadContext> (serviceProvider, resolver.Object);
            // var reflectorUtilMock = GetMockService<IReflectorUtil> ();
            // var configurationRootMock = new Mock<IConfigurationRoot> ();
            // var logFactory = GetMockService<ILoggerFactory> ();
            // var pluginManagerMock = GetMockService<IPluginManagers> ();
            // logFactory.Setup (t => t.CreateLogger (It.IsAny<string> ())).Returns (Log.Object);
            // pluginManagerMock.Setup (t => t.GetBasePluginsPath ()).Returns (path);
            // dirMock.Setup (t => t.GetFiles (path, It.IsAny<string> (), SearchOption.AllDirectories))
            //     .Returns (new string[] { "path1", "path2" });
            // jsonUtilMock.Setup (t => t.Get<PluginConfig> (It.IsAny<string> ())).Returns (new PluginConfig () { AssemblyName = typeof (TestPlugins).Assembly.FullName });
            // objectFactoryMock.Setup (t => t.Create<PluginsLoadContext> (serviceProvider, resolver.Object))
            //     .Returns (loadContextMock.Object);
            // objectFactoryMock.Setup (t => t.Create (typeof (TestPlugins), serviceProvider))
            //     .Returns (new TestPlugins (serviceProvider));
            // objectFactoryMock.Setup (t => t.Create<IAssemblyDependencyResolverProxy, AssemblyDependencyResolverProxy> (It.IsAny<string> ()))
            //     .Returns (resolver.Object);
            // loadContextMock.Protected ().Setup<Assembly> ("Load", typeof (TestPlugins).Assembly.GetName ()).Returns (typeof (TestPlugins).Assembly);
            // reflectorUtilMock.Setup (t => t.GetTypeOfAbsoluteBase (It.IsAny<Assembly> (), It.IsAny<Type> ())).Returns (new List<Type> { typeof (TestPlugins) });

            // pluginManagerMock.Object.ResolverPlugins (Service);
            // pluginManagerMock.Verify (t => t.Regist (It.IsAny<Plugins> ()), Times.AtMost (2));
        }
    }

    public class PA : Plugins
    {
        public PA (IServiceProvider service) : base (service) { }
    }
}