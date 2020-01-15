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
        public TestPlugins (AssemblyLoadContext assemblyContext) : base (assemblyContext, new ServiceCollection ()) { }
    }

    [TestClass]
    public class PluginManagerTest
    {
        public PluginManagerTest ()
        {

        }

        [TestMethod]
        public void TestResolvePlugins ()
        {
            var service = new ServiceCollection ();
            var dirMock = new Mock<ISysDirectory> ();
            var jsonUtilMock = new Mock<IJsonUtil> ();
            var objectFactoryMock = new Mock<IObjectFactory> ();
            var loadContextMock = new Mock<PluginsLoadContext> (service);
            var reflectorUtilMock = new Mock<IReflectorUtil> ();
            var configurationRootMock = new Mock<IConfigurationRoot> ();
            var pluginUtilMock = new Mock<IPluginUtil> ();
            var logFactory = new Mock<ILoggerFactory> ();
            var path = "aaa";
            pluginUtilMock.Setup (t => t.GetBasePluginsPath ()).Returns (path);
            dirMock.Setup (t => t.GetFiles (path, It.IsAny<string> (), SearchOption.AllDirectories))
                .Returns (new string[] { "path1", "path2" });
            jsonUtilMock.Setup (t => t.Get<PluginConfig> (It.IsAny<string> ())).Returns (new PluginConfig () { AssemblyName = typeof (TestPlugins).AssemblyQualifiedName });
            objectFactoryMock.Setup (t => t.Create<PluginsLoadContext> (service))
                .Returns (loadContextMock.Object);
            objectFactoryMock.Setup (t => t.Create (typeof (TestPlugins), loadContextMock.Object))
                .Returns (new TestPlugins (loadContextMock.Object));
            loadContextMock.Protected ().Setup<Assembly> ("Load", typeof (TestPlugins).Assembly.GetName ()).Returns (typeof (TestPlugins).Assembly);
            reflectorUtilMock.Setup (t => t.GetTypeByClass (It.IsAny<Assembly> (), It.IsAny<Type> ())).Returns (new List<Type> { typeof (TestPlugins) });
            var pp = service.ResolvePlugins (pluginUtilMock.Object, dirMock.Object, jsonUtilMock.Object, objectFactoryMock.Object, reflectorUtilMock.Object, logFactory.Object);
            Assert.AreEqual (2, pp.Count);
        }
    }
}