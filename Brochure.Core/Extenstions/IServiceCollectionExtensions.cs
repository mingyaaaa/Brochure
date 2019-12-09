using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.Core.Models;
using Brochure.System;
using Brochure.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Core
{
    public static class IServiceCollectionExtensions
    {

        public static List<IPlugins> ResolvePlugins (this IServiceCollection serviceDescriptors,
            IPluginUtil pluginUtil,
            ISysDirectory directory,
            IJsonUtil jsonUtil,
            IObjectFactory objectFactory,
            IReflectorUtil reflectorUtil)
        {
            var pluginBathPath = pluginUtil.GetBasePluginsPath ();
            var allPluginPath = directory.GetFiles (pluginBathPath, "plugin.config", SearchOption.AllDirectories).ToList ();
            var plugins = new List<IPlugins> ();
            foreach (var pluginPath in allPluginPath)
            {
                var pluginConfig = jsonUtil.Get<PluginConfig> (pluginPath);
                var locadContext = objectFactory.Create<PluginsLoadContext> (serviceDescriptors);
                //此处需要保证插件的文件夹的名称与 程序集的名称保持一致
                var assemably = locadContext.LoadAssembly (Path.Combine (pluginBathPath, Path.GetFileNameWithoutExtension (pluginConfig.AssemblyName), pluginConfig.AssemblyName));
                var allPluginTypes = reflectorUtil.GetTypeByClass (assemably, typeof (Plugins));
                if (allPluginTypes.Count == 0)
                    throw new Exception ("请实现基于Plugins的插件类");
                if (allPluginTypes.Count == 2)
                    throw new Exception ("存在多个Plugins实现类");
                var pluginType = allPluginTypes[0];
                var plugin = (Plugins) objectFactory.Create (pluginType, locadContext);
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