using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core
{
    internal class PluginServiceTypeCache
    {
        private ConcurrentDictionary<string, ILifetimeScope> pluginScope;
        private ConcurrentDictionary<Type, ILifetimeScope> serviceScope;

        public PluginServiceTypeCache()
        {
            pluginScope = new ConcurrentDictionary<string, ILifetimeScope>();
            serviceScope = new ConcurrentDictionary<Type, ILifetimeScope>();
        }

        public void AddPluginServiceType(string guid, ILifetimeScope scope, IServiceCollection services)
        {
            pluginScope.TryAdd(guid, scope);
            foreach (var item in services)
            {
                serviceScope.TryAdd(item.ServiceType, scope);
            }
        }

        public void RemovePluginServiceType(string guid)
        {
            if (pluginScope.TryRemove(guid, out var item))
            {
                var pluginScopes = serviceScope.Where(t => t.Value == item).ToList();
                foreach (var pluginScope in pluginScopes)
                {
                    serviceScope.TryRemove(pluginScope.Key, out _);
                }
            }
        }

        public ILifetimeScope GetPluginScope(Type type)
        {
            serviceScope.TryGetValue(type, out var scope);
            return scope;
        }

        public ILifetimeScope GetPluginScope(string guid)
        {
            pluginScope.TryGetValue(guid, out var scope);
            return scope;
        }
    }
}