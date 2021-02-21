using System;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Core.Extenstions;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    public class PluginServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
    {
        public IServiceCollection CreateBuilder (IServiceCollection services)
        {
            return services;
        }

        public IServiceProvider CreateServiceProvider (IServiceCollection containerBuilder)
        {
            return containerBuilder.BuildPluginServiceProvider ();
        }
    }
}