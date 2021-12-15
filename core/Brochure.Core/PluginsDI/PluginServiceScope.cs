using System;
using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using Brochure.Abstract.PluginDI;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    internal class PluginServiceScope : IServiceScope, IServiceProvider
    {
        private readonly IServiceResolver serviceProvider;
        private readonly PluginSerivceTypeCache cache;
        private readonly AspectConfiguration aspectConfiguration;

        public PluginServiceScope(IServiceResolver serviceProvider, PluginSerivceTypeCache cache, AspectConfiguration aspectConfiguration)
        {
            this.serviceProvider = serviceProvider.CreateScope();
            this.cache = cache;
            this.aspectConfiguration = aspectConfiguration;
        }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        public IServiceProvider ServiceProvider => this;

        public void Dispose()
        {
            serviceProvider.Dispose();
        }

        public object GetService(Type serviceType)
        {
            var plugin = cache.GetTypeOfPlugin(serviceType);
            if (plugin == null)
                return serviceProvider.GetService(serviceType);

            var process = new ResolveProcess(plugin, cache);
            lock (GlobLock.LockObj)
            {
                aspectConfiguration.ResolveCall = process.ResolveType;
                var obj = serviceProvider.GetService(serviceType);
                aspectConfiguration.ResolveCall = null;
                return obj;
            }
        }
    }
}