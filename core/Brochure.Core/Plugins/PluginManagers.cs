﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Extenstions;
using Brochure.Core.Models;
using Brochure.Extensions;
using Brochure.SysInterface;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace Brochure.Core
{
    public class PluginManagers : IPluginManagers
    {
        private readonly ConcurrentDictionary<Guid, IPlugins> pluginDic;
        private readonly ISysDirectory directory;
        private readonly IModuleLoader moduleLoader;
        private readonly ILogger<PluginManagers> log;
        private readonly IEnumerable<IPluginLoadAction> loadActions;
        private readonly IPluginLoader pluginLoader;
        private readonly IEnumerable<IPluginUnLoadAction> unLoadActions;

        public PluginManagers(
            ISysDirectory directory,
            IModuleLoader moduleLoader,
            IPluginLoader pluginLoader,
            IObjectFactory objectFactory,
            ILogger<PluginManagers> log,
            IEnumerable<IPluginLoadAction> loadActions,
            IEnumerable<IPluginUnLoadAction> unLoadActions)
        {
            pluginDic = objectFactory.Create<ConcurrentDictionary<Guid, IPlugins>>();
            this.directory = directory;
            this.moduleLoader = moduleLoader;
            this.pluginLoader = pluginLoader;
            this.log = log;
            this.loadActions = loadActions;
            this.unLoadActions = unLoadActions;
        }

        public void Regist(IPlugins plugin)
        {
            pluginDic.TryAdd(plugin.Key, plugin);
        }

        public async Task Remove(IPlugins plugin)
        {
            var r = await pluginLoader.UnLoad(plugin.Key);
            if (!r)
                return;
            NotifyUnload(plugin.Key);
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
        public async Task ResolverPlugins(IServiceProvider provider)
        {
            var pluginBathPath = GetBasePluginsPath();
            var allPluginPath = directory.GetFiles(pluginBathPath, "plugin.config", SearchOption.AllDirectories).ToList();
            foreach (var pluginConfigPath in allPluginPath)
            {
                Guid pluginKey = Guid.Empty;
                try
                {
                    var p = await pluginLoader.LoadPlugin(provider, pluginConfigPath);
                    if (p != null && p is IPlugins plugin)
                    {
                        pluginKey = p.Key;
                        var pluginServiceCollectionContext = plugin.Context.GetPluginContext<PluginServiceCollectionContext>();
                        moduleLoader.LoadModule(provider, pluginServiceCollectionContext, plugin.Assembly);
                        if (await StartPlugin(plugin))
                        {
                            pluginDic.TryAdd(plugin.Key, plugin);
                            NotifyLoad(plugin.Key);
                            continue;
                        }
                    }
                    throw new Exception($"{p.Name}插件加载失败");
                }
                catch (Exception e)
                {
                    log.LogError(e, e.Message);
                    pluginDic.TryRemove(pluginKey, out var _);
                    if (pluginKey != Guid.Empty)
                        await pluginLoader.UnLoad(pluginKey);
                }
            }
        }

        private async Task<bool> StartPlugin(IPlugins plugin)
        {
            //处理插件          
            var result = false;
            try
            {
                result = await plugin.StartingAsync(out string errorMsg);
            }
            catch (Exception e)
            {
                Log.Error($"{plugin.Name}插件加载失败", e);
                await plugin.ExitAsync();
                result = false;
            }
            return result;
        }

        public bool IsExistPlugins(Guid id)
        {
            return pluginDic.ContainsKey(id);
        }

        private void NotifyUnload(Guid key)
        {
            foreach (var item in unLoadActions)
            {
                item?.Invoke(key);
            };
        }

        private void NotifyLoad(Guid key)
        {
            foreach (var item in loadActions)
                item.Invoke(key);
        }
    }
}