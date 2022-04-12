using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    /// <summary>
    /// The plugin module.
    /// </summary>
    public class PluginModule : IModule
    {
        private readonly IPluginLoader _pluginLoader;

        public PluginModule(IPluginLoader pluginLoader)
        {
            _pluginLoader = pluginLoader;
        }

        /// <summary>
        /// Configs the module.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>A Task.</returns>
        public async Task ConfigModule(IServiceCollection services)
        {
            await _pluginLoader.LoadPlugin(services);
        }

        /// <summary>
        /// Initializations the.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>A Task.</returns>
        public Task Initialization(IServiceProvider provider)
        {
            return Task.CompletedTask;
        }
    }
}