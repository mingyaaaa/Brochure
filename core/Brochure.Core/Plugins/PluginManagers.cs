using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Microsoft.Extensions.Logging;
namespace Brochure.Core
{
    /// <summary>
    /// The plugin managers.
    /// </summary>
    public class PluginManagers : IPluginManagers
    {
        private readonly ConcurrentDictionary<Guid, IPlugins> pluginDic;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManagers"/> class.
        /// </summary>
        public PluginManagers()
        {
            pluginDic = new ConcurrentDictionary<Guid, IPlugins>();
        }

        /// <summary>
        /// Regists the.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        public void Regist(IPlugins plugin)
        {
            pluginDic.TryAdd(plugin.Key, plugin);
        }

        /// <summary>
        /// Removes the.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <returns>A Task.</returns>
        public async Task Remove(IPlugins plugin)
        {
            pluginDic.TryRemove(plugin.Key, out var _);
        }

        /// <summary>
        /// Gets the plugin.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>An IPlugins.</returns>
        public IPlugins GetPlugin(Guid key)
        {
            return pluginDic[key];
        }

        /// <summary>
        /// Gets the plugins.
        /// </summary>
        /// <returns>A list of IPlugins.</returns>
        public List<IPlugins> GetPlugins()
        {
            return pluginDic.Values.ToList();
        }

        /// <summary>
        /// Gets the base plugins path.
        /// </summary>
        /// <returns>A string.</returns>
        public string GetBasePluginsPath()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(basePath, "Plugins");
        }

        /// <summary>
        /// Gets the plugin version.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A long.</returns>
        public long GetPluginVersion(Guid key)
        {
            //获取版本信息
            return 0;
        }

        /// <summary>
        /// Are the exist plugins.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A bool.</returns>
        public bool IsExistPlugins(Guid id)
        {
            return pluginDic.ContainsKey(id);
        }

        /// <summary>
        /// Removes the.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A Task.</returns>
        public async Task Remove(Guid key)
        {
            pluginDic.TryRemove(key, out var _);
        }
    }
}