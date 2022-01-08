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
    /// <summary>
    /// The plugin serivce type cache.
    /// </summary>
    internal class PluginSerivceTypeCache
    {
        private readonly IPluginManagers pluginManagers;
        private readonly ConcurrentDictionary<Type, Plugins> cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSerivceTypeCache"/> class.
        /// </summary>
        /// <param name="pluginManagers">The plugin managers.</param>
        public PluginSerivceTypeCache(IPluginManagers pluginManagers)
        {
            this.pluginManagers = pluginManagers;
            cache = new ConcurrentDictionary<Type, Plugins>();
        }

        /// <summary>
        /// Inits the cache.
        /// </summary>
        internal void InitCache()
        {
            cache.Clear();
            var plugins = this.pluginManagers.GetPlugins().OfType<Plugins>();
            foreach (var item in plugins)
            {
                foreach (var ss in item.Context.Services)
                {
                    cache.TryAdd(ss.ServiceType, item);
                }
            }
        }

        /// <summary>
        /// Gets the type of plugin.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A Plugins.</returns>
        public Plugins GetTypeOfPlugin(Type type)
        {
            cache.TryGetValue(type, out var plugin);
            return plugin;
        }
    }
}