using Brochure.Abstract;
using Brochure.Abstract.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Brochure.Core
{
    /// <summary>
    /// The plugin configuration load.
    /// </summary>
    internal class PluginConfigurationLoad : IPluginConfigurationLoad
    {
        private readonly ApplicationOption _applicationOption;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IFile _file;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfigurationLoad"/> class.
        /// </summary>
        /// <param name="applicationOption">The application option.</param>
        /// <param name="hostEnvironment">The host environment.</param>
        /// <param name="file">The file.</param>
        public PluginConfigurationLoad(ApplicationOption applicationOption, IHostEnvironment hostEnvironment, IFile file)
        {
            _applicationOption = applicationOption;
            _hostEnvironment = hostEnvironment;
            _file = file;
        }

        /// <summary>
        /// Gets the plugin configuration.
        /// </summary>
        /// <param name="plugins">The plugins.</param>
        /// <returns>An IConfiguration.</returns>
        public IConfiguration GetPluginConfiguration(IPlugins plugins)
        {
            var dllName = plugins.AssemblyName;
            IConfiguration configurtion = _applicationOption.Configuration?.GetSection(dllName);
            var builder = new ConfigurationBuilder();
            builder.AddConfiguration(_applicationOption.Configuration);
            if (configurtion != null)
            {
                builder.AddConfiguration(configurtion);
            }
            //查询插件中的配置文件 默认使用插件中的配置文件覆盖 主程序中的配置文件
            var pluginDir = plugins.PluginDirectory;
            var pluginSettingEnvFile = GetPluginSettingFile();
            var pluginSettingFile = "pluginSetting.json";
            var pluginSettingPath = Path.Combine(pluginDir, pluginSettingFile);
            var pluginSettingEnvPath = Path.Combine(pluginDir, pluginSettingEnvFile);
            if (_file.Exists(pluginSettingEnvPath))
            {
                builder.AddJsonFile(pluginSettingEnvPath);
            }
            if (_file.Exists(pluginSettingPath))
            {
                builder.AddJsonFile(pluginSettingPath);
            }
            var configuration = builder.Build();
            return configuration;
        }

        /// <summary>
        /// Gets the plugin setting file.
        /// </summary>
        /// <returns>A string.</returns>
        private string GetPluginSettingFile()
        {
            return $"plugin{_hostEnvironment.EnvironmentName}.json";
        }
    }
}