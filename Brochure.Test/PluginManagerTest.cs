using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Models;
using Brochure.System;
using Brochure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace Brochure.Test
{
    public class TestPlugins : Plugins
    {
        public TestPlugins (AssemblyLoadContext assemblyContext, IServiceCollection services) : base (assemblyContext, services) { }
    }

    [TestClass]
    public class PluginManagerTest : BaseTest
    {
        public PluginManagerTest ()
        {
            InitBaseService ();
        }

        [TestMethod]
        public void TestResolvePlugins ()
        {
            var path = "aaa";
            var dirMock = GetMockService<ISysDirectory> ();
            var jsonUtilMock = GetMockService<IJsonUtil> ();
            var objectFactoryMock = GetMockService<IObjectFactory> ();
            var resolver = new Mock<IAssemblyDependencyResolverProxy> ();
            var loadContextMock = new Mock<PluginsLoadContext> (Service, resolver.Object);
            var reflectorUtilMock = GetMockService<IReflectorUtil> ();
            var configurationRootMock = new Mock<IConfigurationRoot> ();
            var logFactory = GetMockService<ILoggerFactory> ();
            var pluginManagerMock = GetMockService<IPluginManagers> ();
            logFactory.Setup (t => t.CreateLogger (It.IsAny<string> ())).Returns (Log.Object);
            pluginManagerMock.Setup (t => t.GetBasePluginsPath ()).Returns (path);
            dirMock.Setup (t => t.GetFiles (path, It.IsAny<string> (), SearchOption.AllDirectories))
                .Returns (new string[] { "path1", "path2" });
            jsonUtilMock.Setup (t => t.Get<PluginConfig> (It.IsAny<string> ())).Returns (new PluginConfig () { AssemblyName = typeof (TestPlugins).Assembly.FullName });
            objectFactoryMock.Setup (t => t.Create<PluginsLoadContext> (Service, resolver.Object))
                .Returns (loadContextMock.Object);
            objectFactoryMock.Setup (t => t.Create (typeof (TestPlugins), loadContextMock.Object, Service))
                .Returns (new TestPlugins (loadContextMock.Object, Service));
            objectFactoryMock.Setup (t => t.Create<IAssemblyDependencyResolverProxy, AssemblyDependencyResolverProxy> (It.IsAny<string> ()))
                .Returns (resolver.Object);
            loadContextMock.Protected ().Setup<Assembly> ("Load", typeof (TestPlugins).Assembly.GetName ()).Returns (typeof (TestPlugins).Assembly);
            reflectorUtilMock.Setup (t => t.GetTypeOfAbsoluteBase (It.IsAny<Assembly> (), It.IsAny<Type> ())).Returns (new List<Type> { typeof (TestPlugins) });

            pluginManagerMock.Object.ResolverPlugins (Service, null);
            pluginManagerMock.Verify (t => t.Regist (It.IsAny<Plugins> ()), Times.AtMost (2));
        }
    }
}