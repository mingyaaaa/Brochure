using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Extenstions;
using Brochure.Core.Server;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Brochure.Abstract.PluginDI;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Brochure.Test
{
    [TestClass]
    public class PluginApplicationBuilderTest : BaseTest
    {
        public PluginApplicationBuilderTest()
        {

        }

        [TestMethod]
        public async Task TestUseRouting()
        {
            var listener = new DiagnosticListener("Microsoft.AspNetCore");
            Service.TryAddSingleton<DiagnosticListener>(listener);
            Service.AddOptions();
            Service.AddRouting();
            await Service.AddBrochureServer();
            var provider = Service.BuildPluginServiceProvider();
            var manager = provider.GetService<IPluginManagers>();
            var middleManager = provider.GetService<IMiddleManager>();
            var builderFactory = new ApplicationBuilderFactory(provider);
            var applicationBuilder = builderFactory.CreateBuilder(new FeatureCollection());
            var pluginbuilderFactory = new PluginApplicationBuilderFactory(provider, manager);
            var builder = pluginbuilderFactory.CreateBuilder(new FeatureCollection());
            builder.IntertMiddle("main-routing", Guid.Empty, 10, () => builder.UseRouting());
            var count = middleManager.GetMiddlesList();
            Assert.AreEqual(1, count);
        }
    }
}
