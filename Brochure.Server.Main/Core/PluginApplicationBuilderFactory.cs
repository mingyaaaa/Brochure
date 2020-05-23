using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Server.Main.Core
{
    public class PluginApplicationBuilderFactory : IApplicationBuilderFactory
    {
        private readonly IServiceProvider provider;

        public PluginApplicationBuilderFactory (IServiceProvider provider)
        {
            this.provider = provider;
        }
        public IApplicationBuilder CreateBuilder (IFeatureCollection serverFeatures)
        {
            var builder = new PluginApplicationBuilder (new ApplicationBuilder (provider, serverFeatures));
            return builder;
        }
    }
}