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
    /// <summary>
    /// The plugin loader.
    /// </summary>
    public class PluginLoader : IPluginLoader
    {
        private readonly ConcurrentDictionary<Guid, IPluginsLoadContext> pluginContextDic;

        private readonly ISysDirectory directory;
        private readonly IJsonUtil jsonUtil;
        private readonly ILogger<PluginManagers> log;
        private readonly IReflectorUtil reflectorUtil;
        private readonly IObjectFactory objectFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoader"/> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="jsonUtil">The json util.</param>
        /// <param name="log">The log.</param>
        /// <param name="reflectorUtil">The reflector util.</param>
        /// <param name="objectFactory">The object factory.</param>
        public PluginLoader(ISysDirectory directory,
            IJsonUtil jsonUtil,
            ILogger<PluginManagers> log,
            IReflectorUtil reflectorUtil,
            IObjectFactory objectFactory)
        {
            this.directory = directory;
            this.jsonUtil = jsonUtil;
            this.log = log;
            pluginContextDic = objectFactory.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>();
            this.reflectorUtil = reflectorUtil;
            this.objectFactory = objectFactory;
        }
        /// <summary>
        /// Loads the plugin.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="pluginConfigPath">The plugin config path.</param>
        /// <returns>A ValueTask.</returns>
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
                throw new Exception($"{pluginConfig.AssemblyName}请实现基于Plugins的插件类");
            if (allPluginTypes.Count == 2)
                throw new Exception("${ pluginConfig .AssemblyName}存在多个Plugins实现类");
            var pluginType = allPluginTypes[0];
            var plugin = (Plugins)objectFactory.Create(pluginType, provider);
            SetPluginValues(pluginConfig, assemably, ref plugin);
            pluginContextDic.TryAdd(pluginConfig.Key, locadContext);
            return ValueTask.FromResult((IPlugins)plugin);
        }

        /// <summary>
        /// Loads the plugin.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="pluginConfigPath">The plugin config path.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<IPlugins> LoadPlugin(IServiceCollection service, string pluginConfigPath)
        {
            var serviceProvider = service.BuildPluginServiceProvider();
            return LoadPlugin(serviceProvider, pluginConfigPath);
        }

        /// <summary>
        /// Uns the load.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<bool> UnLoad(Guid key)
        {
            if (pluginContextDic.TryRemove(key, out var context))
            {
                try
                {
                    context.UnLoad();
                }
                catch (Exception ex)
                {
                    log.LogError(ex, ex.Message);
                    pluginContextDic.TryAdd(key, context);
                    return new ValueTask<bool>(false);
                }
            }
            else
            {
                return new ValueTask<bool>(false);
            }
            return new ValueTask<bool>(true);
        }

        /// <summary>
        /// Sets the plugin values.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="plugin">The plugin.</param>
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