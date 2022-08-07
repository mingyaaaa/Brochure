using Brochure.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.Server.Core
{
    /// <summary>
    /// The plugin startup filter.
    /// </summary>
    internal class PluginStartupFilter : IStartupFilter
    {
        private readonly IPluginManagers _pluginManagers;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginStartupFilter"/> class.
        /// </summary>
        /// <param name="pluginManagers">The plugin managers.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public PluginStartupFilter(IPluginManagers pluginManagers, IServiceProvider serviceProvider)
        {
            _pluginManagers = pluginManagers;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Configures the.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <returns>An Action.</returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                var plugins = _pluginManagers.GetPlugins().OfType<Plugins>();
                foreach (var item in plugins)
                {
                    item.ConfigApplication(builder);
                }
                next(builder);
            };
        }
    }
}