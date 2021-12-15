using Brochure.Abstract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.PluginsDI
{
    internal class PluginSerivceTypeCache
    {
        private readonly IPluginManagers pluginManagers;
        private readonly ConcurrentDictionary<Type, Plugins> cache;

        public PluginSerivceTypeCache(IPluginManagers pluginManagers)
        {
            this.pluginManagers = pluginManagers;
            cache = new ConcurrentDictionary<Type, Plugins>();
        }

        internal void InitCache()
        {
            var plugins = this.pluginManagers.GetPlugins().OfType<Plugins>();
            foreach (var item in plugins)
            {
                foreach (var ss in item.Context.Services)
                {
                    cache.TryAdd(ss.ServiceType, item);
                }
            }
        }

        public Plugins GetTypeOfPlugin(Type type)
        {
            cache.TryGetValue(type, out var plugin);
            return plugin;
        }
    }
}