using System;
using AspectCore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{

    public class PluginServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceResolver serviceProvider;

        public PluginServiceScopeFactory (IServiceResolver serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public IServiceScope CreateScope ()
        {
            return new PluginServiceScope (serviceProvider.CreateScope ());
        }
    }

}