using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Brochure.Core.Extenstions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The plugins service provider.
    /// </summary>
    internal class PluginsServiceProvider : IPluginServiceProvider, IServiceScopeFactory, IServiceResolver
    {
        private int pluginCount = -1;
        private readonly IServiceCollection _services;
        private readonly IPluginManagers pluginManagers;
        private readonly PluginSerivceTypeCache pluginServiceTypeCache;
        private IServiceProvider rootProvider;
        private AspectConfiguration aspectConfiguration;
        private readonly ConcurrentDictionary<Type, ServiceDescriptor> singleCache = new ConcurrentDictionary<Type, ServiceDescriptor>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginsServiceProvider"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public PluginsServiceProvider(IServiceCollection services, IPluginManagers pluginManagers, PluginSerivceTypeCache pluginServiceTypeCache)
        {
            this._services = services;
            this.pluginManagers = pluginManagers;
            this.pluginServiceTypeCache = pluginServiceTypeCache;
            aspectConfiguration = new AspectConfiguration();
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        { }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>An object.</returns>
        public object GetService(Type serviceType)
        {
            var plugins = this.pluginManagers.GetPlugins();
            if (plugins.Count != pluginCount)
            {
                pluginCount = plugins.Count;
                pluginServiceTypeCache.InitCache();
                rootProvider = PopuPluginService();
            }
            var plugin = pluginServiceTypeCache.GetTypeOfPlugin(serviceType);
            if (plugin == null)
            {
                var obj = rootProvider.GetService(serviceType);
                ReplaceSingleImp(serviceType, obj);
                return obj;
            }

            var process = new ResolveProcess(plugin, pluginServiceTypeCache);
            lock (GlobLock.LockObj)
            {
                aspectConfiguration.ResolveCall = process.ResolveType;
                var obj = rootProvider.GetService(serviceType);
                ReplaceSingleImp(serviceType, obj);
                aspectConfiguration.ResolveCall = null;
                return obj;
            }
        }

        private void ReplaceSingleImp(Type type, object obj)
        {
            if (singleCache.TryGetValue(type, out var des) && des.ImplementationInstance == null)
            {
                _services.Replace(ServiceDescriptor.Singleton(type, obj));
            }
        }

        /// <summary>
        /// Popus the plugin service.
        /// </summary>
        /// <returns>An IServiceProvider.</returns>
        private IServiceProvider PopuPluginService()
        {
            singleCache.Clear();
            var service = new ServiceCollection();
            var plugins = this.pluginManagers.GetPlugins().OfType<Plugins>().ToList();
            foreach (var item in plugins)
            {
                MergerCollection(service, item.Context.Services);
            }
            MergerCollection(service, _services);
            return BuildServiceResolver(service);
        }

        /// <summary>
        /// Creates the scope.
        /// </summary>
        /// <returns>An IServiceScope.</returns>
        public IServiceScope CreateScope()
        {
            return new PluginServiceScope(this, pluginServiceTypeCache, aspectConfiguration);
        }

        /// <summary>
        /// Builds the service resolver.
        /// </summary>
        /// <returns>An IServiceProvider.</returns>
        private IServiceProvider BuildServiceResolver(IServiceCollection services)
        {
            return services.BuildServiceContextProvider(aspectConfiguration, t =>
              {
                  var serviceDefinition = t.FirstOrDefault(t => t.ServiceType == typeof(IServiceScopeFactory));
                  t.Remove(serviceDefinition);
                  t.AddInstance<IServiceScopeFactory>(this);
                  t.AddInstance<IPluginServiceProvider>(this);
              });
        }

        /// <summary>
        /// Resolves the.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>An object.</returns>
        public object Resolve(Type serviceType)
        {
            return GetService(serviceType);
        }

        private void MergerCollection(IServiceCollection services, IServiceCollection serviceDescriptors)
        {
            foreach (var item in serviceDescriptors)
            {
                if (item.Lifetime == ServiceLifetime.Singleton && item.ImplementationType != null)
                {
                    singleCache.TryAdd(item.ServiceType, item);
                }
                services.Add(item);
            }
        }
    }
}