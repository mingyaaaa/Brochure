﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Brochure.Core
{
    public class PluginManagers : IPluginManagers
    {
        private IDictionary<Guid, IPlugins> pluginDic;
        private IMvcCoreBuilder _mvcBuilder;
        public PluginManagers()
        {
            pluginDic = new Dictionary<Guid, IPlugins>();
        }

        public void Regist(IPlugins plugin)
        {
            pluginDic.Add(plugin.Key, plugin);
        }

        public void Remove(IPlugins plugin)
        {
            pluginDic.Remove(plugin.Key);
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
            if (Directory.Exists(pluginPath))
                Directory.CreateDirectory(pluginPath);
            return pluginPath;
        }
    }
}