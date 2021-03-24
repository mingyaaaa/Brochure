using System;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Core.Extenstions;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The plugin service provider factory.
    /// </summary>
    public class PluginServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
    {
        /// <summary>
        /// Creates the builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>An IServiceCollection.</returns>
        public IServiceCollection CreateBuilder(IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// Creates the service provider.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>An IServiceProvider.</returns>
        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
        {
            return containerBuilder.BuildPluginServiceProvider();
        }
    }
}