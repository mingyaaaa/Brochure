using System;
using System.Linq;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Brochure.Core.PluginsDI;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Extenstions
{
    public static class DIExtensions
    {
        /// <summary>
        /// Builds the plugin service provider.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An IServiceProvider.</returns>
        public static IServiceProvider BuildPluginServiceProvider(this IServiceCollection services)
        {
            var servicePorvider = services.BuildServiceProvider();
            var pluginManagers = servicePorvider.GetRequiredService<IPluginManagers>();
            return new PluginsServiceProvider(services, pluginManagers);
        }
    }
}