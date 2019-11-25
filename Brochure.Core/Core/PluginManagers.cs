using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Brochure.Abstract;
using Brochure.Core.Models;
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

        public static List<IPlugins> ResolvePlugins (string pluginBathPath, IServiceCollection serviceDescriptors)
        {
            var allPluginPath = Directory.GetFiles (pluginBathPath, "plugin.config", SearchOption.AllDirectories).ToList ();
            var configBuilder = new ConfigurationBuilder ();
            var plugins = new List<IPlugins> ();
            foreach (var pluginPath in allPluginPath)
            {
                var pluginConfig = configBuilder.AddJsonFile (pluginPath).Build ().Get<PluginConfig> ();
                var locadContext = new PluginsLoadContext ();
                //此处需要保证插件的文件夹的名称与 程序集的名称保持一致
                var assemably = locadContext.LoadFromAssemblyPath (Path.Combine (pluginBathPath, Path.GetFileNameWithoutExtension (pluginConfig.AssemblyName), pluginConfig.AssemblyName));
                var allPluginTypes = ReflectorUtil.GetTypeByClass (assemably, typeof (Plugins));
                if (allPluginTypes.Count == 0)
                    throw new Exception ("请实现基于Plugins的插件类");
                if (allPluginTypes.Count == 2)
                    throw new Exception ("存在多个Plugins实现类");
                var pluginType = allPluginTypes[0];
                var plugin = (Plugins) Activator.CreateInstance (pluginType, locadContext, serviceDescriptors);
                SetPluginValues (pluginConfig, assemably, ref plugin);
                plugins.Add (plugin);
            }
            return plugins;
        }

        private static void SetPluginValues (PluginConfig config, Assembly assembly, ref Plugins plugin)
        {
            if (config == null)
                throw new ArgumentException (nameof (PluginConfig));
            plugin.Assembly = assembly;
            plugin.AssemblyName = config.AssemblyName;
            plugin.Author = config.Author;
            plugin.DependencesKey = config.DependencesKey;
            plugin.Key = config.Key;
            plugin.Name = config.Name;
            plugin.Version = config.Version;
        }
    }
}