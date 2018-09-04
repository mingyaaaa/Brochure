using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Brochure.Core
{
    public class PluginManagers : IPluginManagers
    {
        private IDictionary<Guid, IPlugins> pluginDic;

        public PluginManagers()
        {
            pluginDic = new Dictionary<Guid, IPlugins>();
        }

        public void Regist(IPlugins plugin)
        {
            pluginDic.Add(plugin.Key, plugin);
        }

        public void Remove(Guid key)
        {
            pluginDic.Remove(key);
        }

        public IPlugins GetPlugin(Guid key)
        {
            return pluginDic[key];
        }

        public List<IPlugins> GetPlugins()
        {
            return pluginDic.Values.ToList();
        }

        public static string GetPluginPath()
        {
            var bathPath = AppDomain.CurrentDomain.BaseDirectory;
            var pluginPath = Path.Combine(bathPath, "Plugin");
            return pluginPath;
        }
    }
}
