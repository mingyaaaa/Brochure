using System;
using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using Brochure.Abstract.PluginDI;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The plugin service scope.
    /// </summary>
    internal class PluginServiceScope : IServiceScope, IServiceProvider
    {
        private readonly IServiceResolver serviceProvider;
        private readonly PluginSerivceTypeCache cache;
        private readonly AspectConfiguration aspectConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginServiceScope"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="aspectConfiguration">The aspect configuration.</param>
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

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        {
            serviceProvider.Dispose();
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>An object.</returns>
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