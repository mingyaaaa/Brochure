using Brochure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The resolve process.
    /// </summary>
    internal class ResolveProcess
    {
        private readonly IPlugins plugins;
        private readonly PluginSerivceTypeCache pluginAssemblyCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResolveProcess"/> class.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <param name="pluginAssemblyCache">The plugin assembly cache.</param>
        public ResolveProcess(IPlugins plugin, PluginSerivceTypeCache pluginAssemblyCache)
        {
            this.plugins = plugin;
            this.pluginAssemblyCache = pluginAssemblyCache;
        }

        /// <summary>
        /// Resolves the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A bool.</returns>
        public bool ResolveType(Type type)
        {
            var t_p = pluginAssemblyCache.GetTypeOfPlugin(type);
            if (t_p == null)
                return true;
            return plugins.AuthKey.Any(t => t == t_p.Key) || plugins.Key == t_p.Key;
        }
    }
}