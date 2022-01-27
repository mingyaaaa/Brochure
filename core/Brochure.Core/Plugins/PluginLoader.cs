using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Brochure.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    /// <summary>
    /// The plugin loader.
    /// </summary>
    public class PluginLoader : IPluginLoader, IDisposable
    {
        private static ConcurrentDictionary<Guid, IPluginsLoadContext> pluginContextDic;

        private readonly ISysDirectory directory;
        private readonly IPluginLoadContextProvider pluginLoadContextProvider;
        private readonly IJsonUtil jsonUtil;
        private readonly ILogger<PluginLoader> log;
        private readonly IReflectorUtil reflectorUtil;
        private readonly IObjectFactory objectFactory;
        private readonly IPluginManagers _pluginManagers;
        private readonly IModuleLoader _moduleLoader;
        private readonly IEnumerable<IPluginLoadAction> _loadActions;
        private readonly IEnumerable<IPluginUnLoadAction> _unLoadActions;
        private readonly ApplicationOption _applicationOption;
        private readonly IHostEnvironment _hostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoader"/> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="jsonUtil">The json util.</param>
        /// <param name="pluginLoadContextProvider"></param>
        /// <param name="log">The log.</param>
        /// <param name="reflectorUtil">The reflector util.</param>
        /// <param name="objectFactory">The object factory.</param>
        /// <param name="pluginManagers"></param>
        /// <param name="moduleLoader"></param>
        /// <param name="loadActions"></param>
        /// <param name="unLoadActions"></param>
        /// <param name="applicationOption"></param>
        /// <param name="hostEnvironment"></param>
        public PluginLoader(ISysDirectory directory,
            IJsonUtil jsonUtil,
            IPluginLoadContextProvider pluginLoadContextProvider,
            ILogger<PluginLoader> log,
            IReflectorUtil reflectorUtil,
            IObjectFactory objectFactory,
            IPluginManagers pluginManagers,
            IModuleLoader moduleLoader,
            IEnumerable<IPluginLoadAction> loadActions,
            IEnumerable<IPluginUnLoadAction> unLoadActions,
            ApplicationOption applicationOption,
            IHostEnvironment hostEnvironment)
        {
            this.directory = directory;
            this.jsonUtil = jsonUtil;
            this.pluginLoadContextProvider = pluginLoadContextProvider;
            this.log = log;
            pluginContextDic = objectFactory.Create<ConcurrentDictionary<Guid, IPluginsLoadContext>>();
            this.reflectorUtil = reflectorUtil;
            this.objectFactory = objectFactory;
            _pluginManagers = pluginManagers;
            _moduleLoader = moduleLoader;
            _loadActions = loadActions;
            _unLoadActions = unLoadActions;
            _applicationOption = applicationOption;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Loads the plugin.
        /// </summary>
        /// <param name="pluginConfigPath">The plugin config path.</param>
        /// <returns>A ValueTask.</returns>
        public async ValueTask<IPlugins> LoadPlugin(string pluginConfigPath)
        {
            IPluginsLoadContext locadContext = null;
            try
            {
                var pluginConfig = jsonUtil.Get<PluginConfig>(pluginConfigPath);
                var pluginDir = Path.GetDirectoryName(pluginConfigPath);
                var pluginPath = Path.Combine(pluginDir, pluginConfig.AssemblyName);
                locadContext = pluginLoadContextProvider.CreateLoadContext(pluginPath);
                var assemably = locadContext.LoadAssembly(new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
                var allPluginTypes = reflectorUtil.GetTypeOfAbsoluteBase(assemably, typeof(Plugins)).ToList();
                if (allPluginTypes.Count == 0)
                    throw new Exception($"{pluginConfig.AssemblyName}请实现基于Plugins的插件类");
                if (allPluginTypes.Count == 2)
                    throw new Exception($"{ pluginConfig.AssemblyName}存在多个Plugins实现类");
                var pluginType = allPluginTypes[0];
                var plugin = (Plugins)objectFactory.Create(pluginType);
                SetPluginValues(pluginDir, pluginConfig, assemably, ref plugin);
                pluginContextDic.TryAdd(pluginConfig.Key, locadContext);
                return plugin;
            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);
                locadContext?.UnLoad();
                throw;
            }
        }

        /// <summary>
        /// Loads the plugin.
        /// </summary>
        /// <returns>A ValueTask.</returns>
        public async ValueTask LoadPlugin()
        {
            await ResolverPlugins();
        }

        /// <summary>
        /// 加载插件
        /// </summary>
        private async Task ResolverPlugins()
        {
            var pluginBathPath = _pluginManagers.GetBasePluginsPath();
            var allPluginPath = directory.GetFiles(pluginBathPath, "plugin.config", SearchOption.AllDirectories).ToList();
            var listPlugins = new List<IPlugins>();
            //加载程序集
            foreach (var pluginConfigPath in allPluginPath)
            {
                try
                {
                    var p = await LoadPlugin(pluginConfigPath);
                    p.ConfigureService(p.Context.Services);
                    if (p != null)
                    {
                        listPlugins.Add(p);
                    }
                }
                catch (Exception e)
                {
                    log.LogError(e, e.Message);
                }
            }
            //执行插件初始化
            foreach (var plugin in listPlugins)
            {
                try
                {
                    _moduleLoader.LoadModule(plugin.Context.Services, plugin.Assembly);
                    if (await StartPlugin(plugin))
                    {
                        _pluginManagers.Regist(plugin);
                        NotifyLoad(plugin.Key);
                    }
                }
                catch (Exception e)
                {
                    log.LogError(e, e.Message);
                    if (plugin.Key != Guid.Empty)
                        await UnLoad(plugin.Key);
                }
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
            }
            catch (Exception e)
            {
                Log.Error($"{plugin.Name}插件加载失败", e);
                await plugin.ExitAsync();
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
            if (pluginContextDic.TryRemove(key, out var context))
            {
                try
                {
                    context.UnLoad();
                    NotifyUnload(key);
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
        /// <param name="pluginDir"></param>
        /// <param name="config">The config.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="plugin">The plugin.</param>
        private void SetPluginValues(string pluginDir, PluginConfig config, Assembly assembly, ref Plugins plugin)
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
            plugin.PluginOption = GetPluginConfigSection(config, pluginDir, plugin);
        }

        /// <summary>
        /// Gets the plugin config section.
        /// </summary>
        /// <param name="pluginConfig">The plugin config.</param>
        /// <param name="pluginDir"></param>
        /// <param name="plugins"></param>
        /// <returns>An IConfiguration.</returns>
        private PluginOption GetPluginConfigSection(PluginConfig pluginConfig, string pluginDir, Plugins plugins)
        {
            var dllName = Path.GetFileNameWithoutExtension(pluginConfig.AssemblyName);
            IConfiguration configurtion = _applicationOption.Configuration?.GetSection(dllName);
            var builder = new ConfigurationBuilder();
            if (configurtion == null)
            {
                configurtion = _applicationOption.Configuration?.GetSection(pluginConfig.Key.ToString());
                if (configurtion != null)
                    builder.AddConfiguration(configurtion);
            }
            //查询插件中的配置文件 默认使用插件中的配置文件覆盖 主程序中的配置文件

            var pluginSettingEnvFile = GetPluginSettingFile();
            var pluginSettingFile = "pluginSetting.json";
            var pluginSettingPath = Path.Combine(pluginDir, pluginSettingFile);
            var pluginSettingEnvPath = Path.Combine(pluginDir, pluginSettingEnvFile);
            if (File.Exists(pluginSettingEnvPath))
            {
                builder.AddJsonFile(pluginSettingEnvPath);
            }
            if (File.Exists(pluginSettingPath))
            {
                builder.AddJsonFile(pluginSettingPath);
            }
            var configuration = builder.Build();
            return new PluginOption(plugins, configuration);
        }

        /// <summary>
        /// Gets the plugin setting file.
        /// </summary>
        /// <returns>A string.</returns>
        private string GetPluginSettingFile()
        {
            return $"plugin{_hostEnvironment.EnvironmentName}.json";
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        {
        }
    }
}