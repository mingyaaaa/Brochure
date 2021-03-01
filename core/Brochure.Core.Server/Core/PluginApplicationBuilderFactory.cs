using System;
using System.Linq;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Brochure.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Core.Server
{
    /// <summary>
    /// The plugin application builder factory.
    /// </summary>
    public class PluginApplicationBuilderFactory : IApplicationBuilderFactory
    {
        private readonly IServiceProvider provider;
        private readonly IPluginManagers pluginManagers;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginApplicationBuilderFactory"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="pluginManagers">The plugin managers.</param>
        public PluginApplicationBuilderFactory(IServiceProvider provider, IPluginManagers pluginManagers)
        {
            this.provider = provider;
            this.pluginManagers = pluginManagers;
        }
        /// <summary>
        /// Creates the builder.
        /// </summary>
        /// <param name="serverFeatures">The server features.</param>
        /// <returns>An IApplicationBuilder.</returns>
        public IApplicationBuilder CreateBuilder(IFeatureCollection serverFeatures)
        {
            var plugins = pluginManagers.GetPlugins().OfType<Plugins>();
            foreach (var item in plugins)
            {
                item.Context.Add(new PluginMiddleContext(provider, item.Key));
            }
            var builder = new PluginApplicationBuilder(new ApplicationBuilder(provider, serverFeatures));
            return builder;
        }
    }
}