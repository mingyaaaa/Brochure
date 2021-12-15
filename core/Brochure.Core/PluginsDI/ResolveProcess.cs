using Brochure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.PluginsDI
{
    internal class ResolveProcess
    {
        private readonly IPlugins plugins;
        private readonly PluginSerivceTypeCache pluginAssemblyCache;

        public ResolveProcess(IPlugins plugin, PluginSerivceTypeCache pluginAssemblyCache)
        {
            this.plugins = plugin;
            this.pluginAssemblyCache = pluginAssemblyCache;
        }

        public bool ResolveType(Type type)
        {
            var t_p = pluginAssemblyCache.GetTypeOfPlugin(type);
            if (t_p == null)
                return true;
            return plugins.AuthKey.Any(t => t == t_p.Key) || plugins.Key == t_p.Key;
        }
    }
}