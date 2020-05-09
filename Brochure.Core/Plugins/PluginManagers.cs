using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Models;
using Brochure.SysInterface;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    public class PluginManagers : IPluginManagers
    {
        private readonly ConcurrentDictionary<Guid, IPlugins> pluginDic;
        private readonly ConcurrentDictionary<Guid, PluginsLoadContext> pluginContextDic;
        public PluginManagers ()
        {
            pluginDic = new ConcurrentDictionary<Guid, IPlugins> ();
            pluginContextDic = new ConcurrentDictionary<Guid, PluginsLoadContext> ();
        }

        public void Regist (IPlugins plugin)
        {
            pluginDic.TryAdd (plugin.Key, plugin);
        }

        public Task Remove (IPlugins plugin)
        {
            pluginDic.TryRemove (plugin.Key, out var _);
            return Task.CompletedTask;
        }

        public IPlugins GetPlugin (Guid key)
        {
            return pluginDic[key];
        }

        public List<IPlugins> GetPlugins ()
        {
            return pluginDic.Values.ToList ();
        }

        public string GetBasePluginsPath ()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var pluginPath = Path.Combine (basePath, "Plugins");
            return pluginPath;
        }

        public long GetPluginVersion (Guid key)
        {
            //获取版本信息
            return 0;
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        public async Task ResolverPlugins (IServiceCollection serviceDescriptors, Func<IPluginOption, Task<bool>> func)
        {
            var directory = serviceDescriptors.GetServiceInstance<ISysDirectory> ();
            var loggerFactory = serviceDescriptors.GetServiceInstance<ILoggerFactory> ();
            var moduleLoader = serviceDescriptors.GetServiceInstance<IModuleLoader> ();
            var log = loggerFactory.CreateLogger ("ResolvePlugins");
            var pluginBathPath = GetBasePluginsPath ();
            var allPluginPath = directory.GetFiles (pluginBathPath, "plugin.config", SearchOption.AllDirectories).ToList ();
            if (func == null)
            {
                func = DefaultInvokePlugin;
            }
            foreach (var pluginConfigPath in allPluginPath)
            {
                try
                {
                    if (!(await LoadPlugins (serviceDescriptors, pluginConfigPath) is Plugins plugin))
                    {
                        throw new Exception ("Plugins必须包含AssemblyLoadContext，IServiceCollection两个参数的构造函数");
                    }
                    var result = await func.Invoke (new PluginOption (plugin));
                    if (!result)
                    {
                        await UnLoad (plugin);
                        pluginContextDic.TryRemove (plugin.Key, out var _);
                        throw new Exception ($"{plugin.Name}插件加载失败");
                    }
                    else
                    {
                        moduleLoader.LoadModule (serviceDescriptors, plugin.Assembly);
                        Regist (plugin);
                    }
                }
                catch (Exception e)
                {
                    log.LogError (e, e.Message);
                }
            }
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
            plugin.Order = config.Order;
        }

        private async Task<bool> DefaultInvokePlugin (IPluginOption pluginOption)
        {
            var result = true;
            try
            {
                var r = await pluginOption.Plugin.StartingAsync (out string errorMsg);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public async Task UnLoad (IPlugins plugin)
        {
            if (!(plugin is Plugins pp))
                throw new Exception ("插件卸载失败");
            if (await pp.ExitingAsync (out string _))
            {
                await pp.ExitAsync ();
                if (pluginContextDic.TryGetValue (plugin.Key, out var loadContext))
                {
                    loadContext.Unload ();
                    pluginContextDic.TryRemove (plugin.Key, out var _);
                }

            }
            await Remove (plugin);
        }

        public Task<IPlugins> LoadPlugins (IServiceProvider service, string path)
        {
            var jsonUtil = service.GetService<IJsonUtil> ();
            var objectFactory = service.GetService<IObjectFactory> ();
            var reflectorUtil = service.GetService<IReflectorUtil> ();
            var application = service.GetService<IBApplication> ();
            var pluginConfig = jsonUtil.Get<PluginConfig> (path);
            var pluginBathPath = GetBasePluginsPath ();
            var pluginPath = Path.Combine (pluginBathPath, Path.GetFileNameWithoutExtension (pluginConfig.AssemblyName), pluginConfig.AssemblyName);
            var assemblyDependencyResolverProxy = objectFactory.Create<IAssemblyDependencyResolverProxy, AssemblyDependencyResolverProxy> (pluginPath);
            var locadContext = objectFactory.Create<PluginsLoadContext> (service, assemblyDependencyResolverProxy);
            var assemably = locadContext.LoadFromAssemblyName (new AssemblyName (Path.GetFileNameWithoutExtension (pluginPath)));
            var allPluginTypes = reflectorUtil.GetTypeOfAbsoluteBase (assemably, typeof (Plugins)).ToList ();
            if (allPluginTypes.Count == 0)
                throw new Exception ("请实现基于Plugins的插件类");
            if (allPluginTypes.Count == 2)
                throw new Exception ("存在多个Plugins实现类");
            var pluginType = allPluginTypes[0];
            var plugin = (Plugins) objectFactory.Create (pluginType, application.Services);
            SetPluginValues (pluginConfig, assemably, ref plugin);
            pluginContextDic.TryAdd (plugin.Key, locadContext);
            return Task.FromResult ((IPlugins) plugin);
        }

        public Task<IPlugins> LoadPlugins (IServiceCollection service, string path)
        {
            var serviceProvider = service.BuildServiceProvider ();
            return LoadPlugins (serviceProvider, path);
        }

    }
}