using System;
using System.Linq;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Brochure.Core.PluginsDI;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Extenstions
{
    public static class DIExtensions
    {
        public static IServiceProvider BuildPluginServiceProvider (this IServiceCollection services)
        {
            var managers = services.GetServiceInstance<IPluginManagers> ();
            return new PluginsServiceProvider (managers, services);
        }

        public static IServiceResolver BuildPlugnScopeProvider (this IServiceCollection services, PluginsServiceProvider serviceProvider)
        {
            var provider = services.BuildServiceContextProvider (t =>
            {
                var serviceDefinition = t.FirstOrDefault (t => t.ServiceType == typeof (IServiceScopeFactory));
                t.Remove (serviceDefinition);
                t.AddInstance<IServiceScopeFactory> (serviceProvider);
                t.AddInstance<IPluginServiceProvider> (serviceProvider);
            });
            return provider as IServiceResolver;
        }

    }
}