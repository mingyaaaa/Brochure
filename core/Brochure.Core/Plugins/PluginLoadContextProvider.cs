using Brochure.Abstract;
using Brochure.Abstract.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core
{
    public interface IPluginLoadContextProvider
    {
        IPluginsLoadContext CreateLoadContext(string pluginConfigPath);
    }

    internal class PluginLoadContextProvider : IPluginLoadContextProvider
    {
        public PluginLoadContextProvider()
        {
        }

        public IPluginsLoadContext CreateLoadContext(string pluginPath)
        {
            var assemblyDependencyResolverProxy = new AssemblyDependencyResolverProxy(pluginPath);
            var loadContex = new PluginsLoadContext(assemblyDependencyResolverProxy);
            return new PluginLoadContextProxy(loadContex);
        }
    }
}