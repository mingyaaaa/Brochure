using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Brochure.Core.Server
{
    public class PluginManagers : IPluginManagers
    {
        private IDictionary<Guid, IPlugins> pluginDic;
        private IMvcCoreBuilder _mvcBuilder;
        public PluginManagers(IMvcCoreBuilder mvcBuilder)
        {
            pluginDic = new Dictionary<Guid, IPlugins>();
            _mvcBuilder = mvcBuilder;
        }

        public void Regist(IPlugins plugin)
        {
            var assembly = plugin.GetType().Assembly;
            _mvcBuilder.AddApplicationPart(assembly);
            pluginDic.Add(plugin.Key, plugin);
        }

        public void Remove(IPlugins plugin)
        {
            _mvcBuilder.PartManager.ApplicationParts.Remove(t => t.Name == plugin.AssemblyName);
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
            if (!Directory.Exists(pluginPath))
                Directory.CreateDirectory(pluginPath);
            return pluginPath;
        }
    }
}
