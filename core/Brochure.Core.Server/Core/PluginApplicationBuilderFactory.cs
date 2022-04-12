using Brochure.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http.Features;
using System;

namespace Brochure.Core.Server
{
    /// <summary>
    /// The plugin application builder factory.
    /// </summary>
    public class PluginApplicationBuilderFactory : IApplicationBuilderFactory
    {
        private readonly IServiceProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginApplicationBuilderFactory"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>

        public PluginApplicationBuilderFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Creates the builder.
        /// </summary>
        /// <param name="serverFeatures">The server features.</param>
        /// <returns>An IApplicationBuilder.</returns>
        public IApplicationBuilder CreateBuilder(IFeatureCollection serverFeatures)
        {
            var builder = new PluginApplicationBuilder(new ApplicationBuilder(provider, serverFeatures));
            return builder;
        }
    }
}