using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Extenstions;
using Brochure.SysInterface;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    public class PluginLoader : IPluginLoader
    {
        private readonly ConcurrentDictionary<Guid, IPluginsLoadContext> pluginContextDic;

        private readonly ISysDirectory directory;
        private readonly IJsonUtil jsonUtil;
        private readonly ILogger<PluginManagers> log;
        private readonly IEnumerable<IPluginLoadAction> loadActions;
        private readonly IReflectorUtil reflectorUtil;
        private readonly IObjectFactory objectFactory;
        private readonly IEnumerable<IPluginUnLoadAction> unLoadActions;

        public PluginLoader(ISysDirectory directory,
            IJsonUtil jsonUtil,
            IEnumerable<IPluginLoadAction> loadActions,
            IEnumerable<IPluginUnLoadAction> unLoadActions,
            ILogger<PluginManagers> log,
            IReflectorUtil reflectorUtil,
            IObjectFactory objectFactory)
        {
            this.directory = directory;
            this.jsonUtil = jsonUtil;
            this.log = log;
            this.loadActions = loadActions;
            pluginContextDic = objectFactory.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>();
            this.reflectorUtil = reflectorUtil;
            this.objectFactory = objectFactory;
            this.unLoadActions = unLoadActions;
        }
        public ValueTask<IPlugins> LoadPlugin(IServiceProvider provider, string pluginConfigPath)
        {
            var pluginConfig = jsonUtil.Get<PluginConfig>(pluginConfigPath);
            var pluginDir = Path.GetDirectoryName(pluginConfigPath);
            var pluginPath = Path.Combine(pluginDir, pluginConfig.AssemblyName);
            var assemblyDependencyResolverProxy = objectFactory.Create<IAssemblyDependencyResolverProxy, AssemblyDependencyResolverProxy>(pluginPath);
            var locadContext = objectFactory.Create<IPluginsLoadContext, PluginsLoadContext>(provider, assemblyDependencyResolverProxy);
            var assemably = locadContext.LoadAssembly(new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
            var allPluginTypes = reflectorUtil.GetTypeOfAbsoluteBase(assemably, typeof(Plugins)).ToList();
            if (allPluginTypes.Count == 0)
                throw new Exception("请实现基于Plugins的插件类");
            if (allPluginTypes.Count == 2)
                throw new Exception("存在多个Plugins实现类");
            var pluginType = allPluginTypes[0];
            var plugin = (Plugins)objectFactory.Create(pluginType, provider);
            SetPluginValues(pluginConfig, assemably, ref plugin);
            pluginContextDic.TryAdd(pluginConfig.Key, locadContext);
            return ValueTask.FromResult((IPlugins)plugin);
        }

        public ValueTask<IPlugins> LoadPlugin(IServiceCollection service, string pluginConfigPath)
        {
            var serviceProvider = service.BuildPluginServiceProvider();
            return LoadPlugin(serviceProvider, pluginConfigPath);
        }

        public ValueTask UnLoad(Guid key)
        {
            if (pluginContextDic.TryRemove(key, out var context))
            {
                foreach (var item in unLoadActions)
                {
                    item?.Invoke(key);
                };
                context.UnLoad();
            }
            return ValueTask.CompletedTask;
        }

        private void SetPluginValues(PluginConfig config, Assembly assembly, ref Plugins plugin)
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

    }
}