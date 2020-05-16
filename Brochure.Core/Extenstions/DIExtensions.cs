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
            var provider = services.BuildPlugnScopeProvider ();
            var managers = provider.GetService<IPluginManagers> ();
            return new PluginsServiceProvider (managers, provider);
        }

        public static IServiceResolver BuildPlugnScopeProvider (this IServiceCollection services)
        {
            var provider = services.BuildServiceContextProvider (t =>
            {
                var serviceDefinition = t.FirstOrDefault (t => t.ServiceType == typeof (IServiceScopeFactory));
                t.Remove (serviceDefinition);
                t.AddType<IServiceScopeFactory, PluginServiceScopeFactory> (Lifetime.Scoped);
                t.AddType<IPluginServiceProvider, PluginsServiceProvider> (Lifetime.Scoped);
            });
            return provider as IServiceResolver;
        }

    }
}