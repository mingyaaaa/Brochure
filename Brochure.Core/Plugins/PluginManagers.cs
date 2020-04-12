using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Models;
using Brochure.System;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 

namespace Brochure.Core
{
    public class PluginManagers : IPluginManagers
    {
        private readonly ConcurrentDictionary<Guid, IPlugins> pluginDic;

        public PluginManagers()
        {
            pluginDic = new ConcurrentDictionary<Guid, IPlugins>();
        }

        public void Regist(IPlugins plugin)
        {
            pluginDic.TryAdd(plugin.Key, plugin);
        }

        public void Remove(IPlugins plugin)
        {
            pluginDic.TryRemove(plugin.Key, out var _);
        }

        public IPlugins GetPlugin(Guid key)
        {
            return pluginDic[key];
        }

        public List<IPlugins> GetPlugins()
        {
            return pluginDic.Values.ToList();
        }

        public string GetBasePluginsPath()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var pluginPath = Path.Combine(basePath, "Plugins");
            return pluginPath;
        }

        public long GetPluginVersion(Guid key)
        {
            //获取版本信息
            return 0;
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        public async Task ResolverPlugins(IServiceCollection serviceDescriptors, Func<IPluginOption, Task<bool>> func)
        {
            var directory = serviceDescriptors.GetServiceInstance<ISysDirectory>();
            var jsonUtil = serviceDescriptors.GetServiceInstance<IJsonUtil>();
            var objectFactory = serviceDescriptors.GetServiceInstance<IObjectFactory>();
            var pluginManagers = serviceDescriptors.GetServiceInstance<IPluginManagers>();
            var reflectorUtil = serviceDescriptors.GetServiceInstance<IReflectorUtil>();
            var loggerFactory = serviceDescriptors.GetServiceInstance<ILoggerFactory>();
            var log = loggerFactory.CreateLogger("ResolvePlugins");
            var pluginBathPath = pluginManagers.GetBasePluginsPath();
            var allPluginPath = directory.GetFiles(pluginBathPath, "plugin.config", SearchOption.AllDirectories).ToList();
            if (func == null)
            {
                func = DefaultInvokePlugin;
            }
            foreach (var pluginConfigPath in allPluginPath)
            {
                try
                {
                    var pluginConfig = jsonUtil.Get<PluginConfig>(pluginConfigPath);
                    var pluginPath = Path.Combine(pluginBathPath, Path.GetFileNameWithoutExtension(pluginConfig.AssemblyName), pluginConfig.AssemblyName);
                    var assemblyDependencyResolverProxy = objectFactory.Create<IAssemblyDependencyResolverProxy, AssemblyDependencyResolverProxy>(pluginPath);
                    var locadContext = objectFactory.Create<PluginsLoadContext>(serviceDescriptors, assemblyDependencyResolverProxy);
                    //此处需要保证插件的文件夹的名称与 程序集的名称保持一致
                    var assemably = locadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
                    var allPluginTypes = reflectorUtil.GetTypeByClass(assemably, typeof(Plugins)).ToList();
                    if (allPluginTypes.Count == 0)
                        throw new Exception("请实现基于Plugins的插件类");
                    if (allPluginTypes.Count == 2)
                        throw new Exception("存在多个Plugins实现类");
                    var pluginType = allPluginTypes[0];
                    var plugin = (Plugins)objectFactory.Create(pluginType, locadContext, serviceDescriptors);
                    if (plugin == null)
                    {
                        throw new Exception("Plugins必须包含AssemblyLoadContext，IServiceCollection两个参数的构造函数");
                    }
                    SetPluginValues(pluginConfig, assemably, ref plugin);
                    var result = await func.Invoke(new PluginOption(plugin));
                    if (!result)
                    {

                        UnLoad(plugin);
                        locadContext.Unload();
                        throw new Exception($"{plugin.Name}插件加载失败");
                    }
                    else
                    {
                        pluginManagers.Regist(plugin);
                    }
                }
                catch (Exception e)
                {
                    log.LogError(e, e.Message);
                }
            }
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

        private async Task<bool> DefaultInvokePlugin(IPluginOption pluginOption)
        {
            var result = true;
            try
            {
                var r = await pluginOption.Plugin.StartingAsync(out string errorMsg);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }


        public void UnLoad(IPlugins plugin)
        {
            throw new NotImplementedException();
        }
    }
}