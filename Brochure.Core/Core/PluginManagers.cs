using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Brochure.Abstract;
using Brochure.Core.Models;
using Brochure.System;
using Brochure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    public class PluginManagers : IPluginManagers
    {
        private readonly ConcurrentDictionary<Guid, IPlugins> pluginDic;

        public PluginManagers ()
        {
            pluginDic = new ConcurrentDictionary<Guid, IPlugins> ();
        }

        public void Regist (IPlugins plugin)
        {
            pluginDic.TryAdd (plugin.Key, plugin);
        }

        public void Remove (IPlugins plugin)
        {
            pluginDic.TryRemove (plugin.Key, out var _);
        }

        public IPlugins GetPlugin (Guid key)
        {
            return pluginDic[key];
        }

        public List<IPlugins> GetPlugins ()
        {
            return pluginDic.Values.ToList ();
        }

    }
}