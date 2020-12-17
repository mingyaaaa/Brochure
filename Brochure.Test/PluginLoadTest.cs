using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    [TestClass]
    public class PluginLoadTest : BaseTest
    {
        public PluginLoadTest ()
        {
            InitBaseService ();
        }

        [TestMethod]
        public async Task TestLoad ()
        {
            var provider = Service.BuildServiceProvider ();
            var load = provider.GetService<IPluginLoader> ();
            var pluginConfigPath = "/p1/plugin.config";
            await load.LoadPlugin (provider, pluginConfigPath);
        }

        [TestMethod]
        public async Task TestUnLoad ()
        {
            var provider = Service.BuildServiceProvider ();
            var load = provider.GetService<IPluginLoader> ();
            var key = Guid.NewGuid ();
            var pluginConfigPath = "/p1/plugin.config";
            await load.UnLoad (key);
        }
    }
}