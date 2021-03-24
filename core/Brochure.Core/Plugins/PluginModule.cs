using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    /// <summary>
    /// The plugin module.
    /// </summary>
    public class PluginModule : IModule
    {
        /// <summary>
        /// Configs the module.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>A Task.</returns>
        public Task ConfigModule(IServiceCollection services)
        {

            return Task.CompletedTask;
        }

        /// <summary>
        /// Initializations the.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>A Task.</returns>
        public Task Initialization(IServiceProvider provider)
        {
            var pluginLoader = provider.GetService<IPluginLoader>();
            return pluginLoader.LoadPlugin(provider).AsTask();
        }
    }
}