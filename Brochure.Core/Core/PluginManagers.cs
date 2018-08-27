using Brochure.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Brochure.Core.Core
{
    public class PluginManagers
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
    }
}
