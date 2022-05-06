using Autofac;
using Autofac.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core.PluginsDI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;

namespace Brochure.Core
{
    /// <summary>
    /// The plugin loader.
    /// </summary>
    public class PluginLoader : IPluginLoader, IDisposable
    {
        private static ConcurrentDictionary<Guid, IPluginsLoadContext> _pluginContextDic;

        private readonly ISysDirectory _directory;
        private readonly IPluginLoadContextProvider _pluginLoadContextProvider;
        private readonly IJsonUtil _jsonUtil;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPluginServiceProvider _pluginServiceProvider;
        private readonly ILogger<PluginLoader> _log;
        private readonly IReflectorUtil _reflectorUtil;
        private readonly IObjectFactory _objectFactory;
        private readonly IMvcBuilder _mvcBuilder;
        private readonly PluginServiceTypeCache _pluginServiceTypeCache;
        private readonly IPluginManagers _pluginManagers;
        private readonly IEnumerable<IPluginLoadAction> _loadActions;
        private readonly IEnumerable<IPluginUnLoadAction> _unLoadActions;
        private readonly IPluginConfigurationLoad _pluginConfigurationLoad;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoader"/> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="jsonUtil">The json util.</param>
        /// <param name="serviceProvider"></param>
        /// <param name="pluginServiceProvider"></param>
        /// <param name="pluginLoadContextProvider"></param>
        /// <param name="log">The log.</param>
        /// <param name="reflectorUtil">The reflector util.</param>
        /// <param name="objectFactory">The object factory.</param>
        /// <param name="mvcBuilder"></param>
        /// <param name="pluginServiceTypeCache"></param>
        /// <param name="pluginManagers"></param>
        /// <param name="loadActions"></param>
        /// <param name="unLoadActions"></param>
        /// <param name="pluginConfigurationLoad"></param>
        public PluginLoader(ISysDirectory directory,
            IJsonUtil jsonUtil,
            IServiceProvider serviceProvider,
            IPluginServiceProvider pluginServiceProvider,
            IPluginLoadContextProvider pluginLoadContextProvider,
            ILogger<PluginLoader> log,
            IReflectorUtil reflectorUtil,
            IObjectFactory objectFactory,
            IMvcBuilder mvcBuilder,
            PluginServiceTypeCache pluginServiceTypeCache,
            IPluginManagers pluginManagers,
            IEnumerable<IPluginLoadAction> loadActions,
            IEnumerable<IPluginUnLoadAction> unLoadActions,
            IPluginConfigurationLoad pluginConfigurationLoad)
        {
            _directory = directory;
            _jsonUtil = jsonUtil;
            _serviceProvider = serviceProvider;
            _pluginServiceProvider = pluginServiceProvider;
            _pluginLoadContextProvider = pluginLoadContextProvider;
            _log = log;
            _pluginContextDic = objectFactory.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>();
            _reflectorUtil = reflectorUtil;
            _objectFactory = objectFactory;
            _mvcBuilder = mvcBuilder;
            _pluginServiceTypeCache = pluginServiceTypeCache;
            _pluginManagers = pluginManagers;
            _loadActions = loadActions;
            _unLoadActions = unLoadActions;
            _pluginConfigurationLoad = pluginConfigurationLoad;
        }

        /// <summary>
        /// Loads the plugin.
        /// </summary>
        /// <param name="pluginConfigPath">The plugin config path.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IPlugins> LoadPlugin(string pluginConfigPath)
        {
            var p = await ResolverPlugins(pluginConfigPath);
            var service = new ServiceCollection();
            p.ConfigureService(service);
            var pluginScope = _pluginServiceProvider.CreateScope(service);
            _pluginServiceTypeCache.AddPluginServiceType(p.Key.ToString(), pluginScope, service);

            //启动插件
            if (await StartPlugin(p))
                return p;
            return null;
        }

        /// <summary>
        /// Loads the plugin.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        public async ValueTask LoadPlugin()
        {
            var pluginBathPath = _pluginManagers.GetBasePluginsPath();
            var allPluginPath = _directory.GetFiles(pluginBathPath, "plugin.config", SearchOption.AllDirectories).ToList();
            foreach (var item in allPluginPath)
            {
                var p = await LoadPlugin(item);
                _mvcBuilder.AddApplicationPart(p.Assembly);
            }
        }

        /// <summary>
        /// Resolvers the plugins.
        /// </summary>
        /// <param name="pluginConfigPath">The plugin config path.</param>
        /// <returns>A ValueTask.</returns>
        private ValueTask<IPlugins> ResolverPlugins(string pluginConfigPath)
        {
            IPluginsLoadContext locadContext = null;
            try
            {
                var pluginConfig = _jsonUtil.Get<PluginConfig>(pluginConfigPath);
                var pluginDir = Path.GetDirectoryName(pluginConfigPath);
                var pluginPath = Path.Combine(pluginDir, pluginConfig.AssemblyName);
                locadContext = _pluginLoadContextProvider.CreateLoadContext(pluginPath);
                var assemably = locadContext.LoadAssembly(new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
                var allPluginTypes = _reflectorUtil.GetTypeOfAbsoluteBase(assemably, typeof(Plugins)).ToList();
                if (allPluginTypes.Count == 0)
                    throw new Exception($"{pluginConfig.AssemblyName}请实现基于Plugins的插件类");
                if (allPluginTypes.Count == 2)
                    throw new Exception($"{ pluginConfig.AssemblyName}存在多个Plugins实现类");
                var pluginType = allPluginTypes[0];
                var plugin = _objectFactory.CreateByIoc<Plugins>(_serviceProvider);
                SetPluginValues(pluginDir, pluginConfig, assemably, plugin);
                _pluginContextDic.TryAdd(pluginConfig.Key, locadContext);
                return new ValueTask<IPlugins>(plugin);
            }
            catch (Exception e)
            {
                _log.LogError(e, e.Message);
                locadContext?.UnLoad();
                throw;
            }
        }

        /// <summary>
        /// Notifies the unload.
        /// </summary>
        /// <param name="key">The key.</param>
        private void NotifyUnload(Guid key)
        {
            foreach (var item in _unLoadActions)
            {
                item?.Invoke(key);
            };
        }

        /// <summary>
        /// Notifies the load.
        /// </summary>
        /// <param name="key">The key.</param>
        private void NotifyLoad(Guid key)
        {
            foreach (var item in _loadActions)
                item.Invoke(key);
        }

        /// <summary>
        /// Starts the plugin.
        /// </summary>
        /// <param name="plugin">The plugin.</param>
        /// <returns>A Task.</returns>
        private async Task<bool> StartPlugin(IPlugins plugin)
        {
            //处理插件
            var result = false;
            try
            {
                result = await plugin.StartingAsync();
                if (!result)
                    return result;
                _pluginManagers.Regist(plugin);
                NotifyLoad(plugin.Key);
                result = true;
            }
            catch (Exception e)
            {
                Log.Error($"{plugin.Name}插件加载失败", e);
                await plugin.ExitAsync();
                await UnLoad(plugin.Key);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Uns the load.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A ValueTask.</returns>
        public ValueTask<bool> UnLoad(Guid key)
        {
            if (_pluginContextDic.TryRemove(key, out var context))
            {
                try
                {
                    _pluginManagers.Remove(key);
                    context.UnLoad();
                    NotifyUnload(key);
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, ex.Message);
                    _pluginContextDic.TryAdd(key, context);
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
        /// <param name="pluginDir"></param>
        /// <param name="config">The config.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="plugin">The plugin.</param>
        private void SetPluginValues(string pluginDir, PluginConfig config, Assembly assembly, Plugins plugin)
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
            plugin.PluginDirectory = pluginDir;
            plugin.Configuration = GetPluginConfigSection(plugin);
        }

        /// <summary>
        /// Gets the plugin config section.
        /// </summary>
        /// <param name="plugins"></param>
        /// <returns>An IConfiguration.</returns>
        private IConfiguration GetPluginConfigSection(Plugins plugins)
        {
            return _pluginConfigurationLoad.GetPluginConfiguration(plugins);
        }

        /// <summary>
        /// Uns the load all.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<bool> UnLoadAll()
        {
            var allPlugin = _pluginManagers.GetPlugins();
            var r = true;
            foreach (var item in allPlugin)
            {
                r = r && await UnLoad(item.Key);
            }
            return r;
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        {
        }
    }
}