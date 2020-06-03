using System;
using System.Collections.Concurrent;
using System.Linq;
using AspectCore.DependencyInjection;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Brochure.Core.Extenstions;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    public class PluginsServiceProvider : IPluginServiceProvider, IServiceScopeFactory, IServiceResolver
    {
        private readonly IPluginManagers managers;
        private IServiceResolver originalProvider;
        private int pluginCount = -1;
        private readonly IServiceCollection services;

        public PluginsServiceProvider (IPluginManagers managers, IServiceCollection services)
        {
            this.managers = managers;
            this.services = services;
        }
        public void Dispose () { }

        public object GetService (Type serviceType)
        {
            var plugins = this.managers.GetPlugins ();
            if (plugins.Count != pluginCount)
            {
                PopuPlugin ();
                pluginCount = plugins.Count;
            }
            var obj = originalProvider.GetService (serviceType);
            return obj;
        }

        public void PopuPlugin ()
        {
            var plugins = this.managers.GetPlugins ().OfType<Plugins> ().ToList ();
            IServiceCollection t_service = new ServiceCollection ();
            Action<ServiceDescriptor, IServiceProvider> action = (item, provider) =>
            {
                if (item.Lifetime == ServiceLifetime.Singleton && !item.ServiceType.IsGenericType)
                {
                    var a = provider.GetService (item.ServiceType);
                    if (a != null)
                        t_service.AddSingleton (item.ServiceType, a);
                }
                else
                {
                    t_service.Add (item);
                }
            };
            var t_provider = originalProvider?? services.BuildPlugnScopeProvider (this);
            foreach (var item in services)
                action (item, t_provider);
            foreach (var item in plugins)
            {

                var pluginsServiceCollection = item.Context.GetPluginContext<PluginServiceCollectionContext> ();
                t_provider = originalProvider?? pluginsServiceCollection.BuildPlugnScopeProvider (this);
                foreach (var serviceDescriptor in pluginsServiceCollection)
                {
                    action (serviceDescriptor, t_provider);
                }
            }
            originalProvider = t_service.BuildPlugnScopeProvider (this);
        }

        public IServiceScope CreateScope ()
        {
            return new PluginServiceScope (this);
        }

        public object Resolve (Type serviceType)
        {
            return GetService (serviceType);
        }
    }
}