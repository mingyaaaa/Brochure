using Autofac;
using Brochure.Abstract;

namespace Brochure.Core
{
    /// <summary>
    /// The plugin app builder.
    /// </summary>
    public class PluginAppBuilder : IPluginAppBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginAppBuilder"/> class.
        /// </summary>
        /// <param name="appService">The app service.</param>
        /// <param name="pluginService">The plugin service.</param>
        public PluginAppBuilder(IServiceProvider appService, ILifetimeScope pluginService)
        {
            AppService = appService;
            PluginService = pluginService;
        }

        /// <summary>
        /// Gets the app service.
        /// </summary>
        public IServiceProvider AppService { get; }

        /// <summary>
        /// Gets the plugin service.
        /// </summary>
        public ILifetimeScope PluginService { get; }
    }
}