using System;
using System.Linq;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Brochure.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Server.Main.Core
{
    public class PluginApplicationBuilderFactory : IApplicationBuilderFactory
    {
        private readonly IServiceProvider provider;
        private readonly IPluginManagers pluginManagers;

        public PluginApplicationBuilderFactory (IPluginServiceProvider provider, IPluginManagers pluginManagers)
        {
            this.provider = provider;
            this.pluginManagers = pluginManagers;
        }
        public IApplicationBuilder CreateBuilder (IFeatureCollection serverFeatures)
        {
            var plugins = pluginManagers.GetPlugins ().OfType<Plugins> ();
            foreach (var item in plugins)
            {
                item.Context.Add (new PluginMiddleContext (provider, item.Key));
            }
            var builder = new PluginApplicationBuilder (new ApplicationBuilder (provider, serverFeatures));
            return builder;
        }
    }
}