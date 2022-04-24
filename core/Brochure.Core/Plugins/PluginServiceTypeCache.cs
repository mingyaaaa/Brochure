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
    /// <summary>
    /// The plugin service type cache.
    /// </summary>
    internal class PluginServiceTypeCache
    {
        private ConcurrentDictionary<string, ILifetimeScope> pluginScope;
        private ConcurrentDictionary<Type, ILifetimeScope> serviceScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginServiceTypeCache"/> class.
        /// </summary>
        public PluginServiceTypeCache()
        {
            pluginScope = new ConcurrentDictionary<string, ILifetimeScope>();
            serviceScope = new ConcurrentDictionary<Type, ILifetimeScope>();
        }

        /// <summary>
        /// Adds the plugin service type.
        /// </summary>
        /// <param name="guid">The guid.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="services">The services.</param>
        public void AddPluginServiceType(string guid, ILifetimeScope scope, IServiceCollection services)
        {
            pluginScope.TryAdd(guid, scope);
            foreach (var item in services)
            {
                serviceScope.TryAdd(item.ServiceType, scope);
            }
        }

        /// <summary>
        /// Removes the plugin service type.
        /// </summary>
        /// <param name="guid">The guid.</param>
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

        /// <summary>
        /// Gets the plugin scope.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>An ILifetimeScope.</returns>
        public ILifetimeScope GetPluginScope(Type type)
        {
            serviceScope.TryGetValue(type, out var scope);
            return scope;
        }

        /// <summary>
        /// Gets the plugin scope.
        /// </summary>
        /// <param name="guid">The guid.</param>
        /// <returns>An ILifetimeScope.</returns>
        public ILifetimeScope GetPluginScope(string guid)
        {
            pluginScope.TryGetValue(guid, out var scope);
            return scope;
        }
    }
}