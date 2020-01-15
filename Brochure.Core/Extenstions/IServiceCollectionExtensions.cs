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
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection LoadPlugins(this IServiceCollection serviceDescriptors,
            IPluginUtil pluginUtil,
            ISysDirectory directory,
            IJsonUtil jsonUtil,
            IObjectFactory objectFactory,
            IReflectorUtil reflectorUtil,
            ILoggerFactory loggerFactory,
            IPluginManagers pluginManagers)
        {
            var log = loggerFactory.CreateLogger("ResolvePlugins");
            var pluginBathPath = pluginUtil.GetBasePluginsPath();
            var allPluginPath = directory.GetFiles(pluginBathPath, "plugin.config", SearchOption.AllDirectories).ToList();
            foreach (var pluginConfigPath in allPluginPath)
            {
                try
                {
                    var pluginConfig = jsonUtil.Get<PluginConfig>(pluginConfigPath);
                    var pluginPath = Path.Combine(pluginBathPath, Path.GetFileNameWithoutExtension(pluginConfig.AssemblyName), pluginConfig.AssemblyName);
                    var locadContext = objectFactory.Create<PluginsLoadContext>(serviceDescriptors, pluginPath);
                    //此处需要保证插件的文件夹的名称与 程序集的名称保持一致
                    var assemably = locadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
                    var allPluginTypes = reflectorUtil.GetTypeByClass(assemably, typeof(Plugins)).ToList();
                    if (allPluginTypes.Count == 0)
                        throw new Exception("请实现基于Plugins的插件类");
                    if (allPluginTypes.Count == 2)
                        throw new Exception("存在多个Plugins实现类");
                    var pluginType = allPluginTypes[0];
                    var plugin = (Plugins)objectFactory.Create(pluginType, locadContext, serviceDescriptors);
                    SetPluginValues(pluginConfig, assemably, ref plugin);
                    pluginManagers.Regist(plugin);
                }
                catch (Exception e)
                {
                    log.LogError(e, e.Message);
                }

            }
            return serviceDescriptors;
        }

        private static void SetPluginValues(PluginConfig config, Assembly assembly, ref Plugins plugin)
        {
            if (config == null)
                throw new ArgumentException(nameof(PluginConfig));
            plugin.Assembly = assembly;
            plugin.AssemblyName = config.AssemblyName;
            plugin.Author = config.Author;
            plugin.DependencesKey = config.DependencesKey;
            plugin.Key = config.Key;
            plugin.Name = config.Name;
            plugin.Version = config.Version;
            plugin.Order = config.Order;
        }

        public static void ResolverPlugins(this IServiceCollection serviceDescriptors, IPluginManagers pluginManagers, Action<PluginOption> action)
        {
            var allPlugin = pluginManagers.GetPlugins();
            foreach (var item in allPlugin)
            {
                action?.Invoke(new PluginOption(item));
            }
        }
    }
}