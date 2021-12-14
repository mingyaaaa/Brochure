using System;
using System.Collections.Concurrent;
using System.Linq;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Brochure.Core.Extenstions;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The plugins service provider.
    /// </summary>
    public class PluginsServiceProvider : IPluginServiceProvider, IServiceScopeFactory, IServiceResolver
    {
        private int pluginCount = -1;
        private readonly IServiceCollection _services;
        private readonly IPluginManagers pluginManagers;
        private IServiceProvider rootProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginsServiceProvider"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public PluginsServiceProvider(IServiceCollection services, IPluginManagers pluginManagers)
        {
            this._services = services;
            this.pluginManagers = pluginManagers;
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
                rootProvider = PopuPluginService();
            }
            return rootProvider.GetService(serviceType);
        }

        /// <summary>
        /// Popus the plugin.
        /// </summary>
        /// <returns>An IServiceProvider.</returns>
        //private IServiceProvider PopuPlugin()
        //{
        //    var plugins = this.pluginManagers.GetPlugins().OfType<Plugins>().ToList();
        //    IServiceCollection t_service = new ServiceCollection();
        //    Action<ServiceDescriptor, IServiceProvider> action = (item, provider) =>
        //    {
        //        if (item.Lifetime == ServiceLifetime.Singleton && !item.ServiceType.IsGenericType)
        //        {
        //            try
        //            {
        //                var a = provider.GetService(item.ServiceType);
        //                if (a != null)
        //                    t_service.AddSingleton(item.ServiceType, a);
        //            }
        //            catch (Exception e)
        //            {
        //            }
        //        }
        //        else
        //        {
        //            t_service.Add(item);
        //        }
        //    };
        //    var t_provider = originalProvider ?? BuildServiceResolver(_services);
        //    foreach (var item in _services)
        //        action(item, t_provider);
        //    foreach (var item in plugins)
        //    {
        //        var pluginsServiceCollection = item.Context.Services;
        //        t_provider = BuildServiceResolver(MergerCollection(_services, pluginsServiceCollection));
        //        foreach (var serviceDescriptor in pluginsServiceCollection)
        //        {
        //            action(serviceDescriptor, t_provider);
        //        }
        //    }
        //    return BuildServiceResolver(t_service);
        //}

        /// <summary>
        /// Popus the plugin service.
        /// </summary>
        /// <returns>An IServiceProvider.</returns>
        private IServiceProvider PopuPluginService()
        {
            var plugins = this.pluginManagers.GetPlugins().OfType<Plugins>().ToList();
            foreach (var item in plugins)
            {
                MergerCollection(item.Context.Services);
            }
            return BuildServiceResolver(_services);
        }

        /// <summary>
        /// Creates the scope.
        /// </summary>
        /// <returns>An IServiceScope.</returns>
        public IServiceScope CreateScope()
        {
            return new PluginServiceScope(this);
        }

        /// <summary>
        /// Builds the service resolver.
        /// </summary>
        /// <returns>An IServiceProvider.</returns>
        private IServiceProvider BuildServiceResolver(IServiceCollection services)
        {
            return services.BuildServiceContextProvider(t =>
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

        private void MergerCollection(IServiceCollection serviceDescriptors)
        {
            var provider = serviceDescriptors.BuildServiceContextProvider();
            foreach (var item in serviceDescriptors)
            {
                _services.Add(item);
            }
        }
    }
}