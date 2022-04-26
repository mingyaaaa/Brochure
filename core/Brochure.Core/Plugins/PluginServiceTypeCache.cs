using Autofac;
using Brochure.Abstract;
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
    public class PluginServiceTypeCache
    {
        private ConcurrentDictionary<string, IPluginServiceProvider> pluginScope;
        private ConcurrentDictionary<Type, IPluginServiceProvider> serviceScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginServiceTypeCache"/> class.
        /// </summary>
        public PluginServiceTypeCache()
        {
            pluginScope = new ConcurrentDictionary<string, IPluginServiceProvider>();
            serviceScope = new ConcurrentDictionary<Type, IPluginServiceProvider>();
        }

        /// <summary>
        /// Adds the plugin service type.
        /// </summary>
        /// <param name="guid">The guid.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="services">The services.</param>
        public virtual void AddPluginServiceType(string guid, IPluginServiceProvider scope, IServiceCollection services)
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
        public virtual void RemovePluginServiceType(string guid)
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
        public virtual IPluginServiceProvider GetPluginScope(Type type)
        {
            serviceScope.TryGetValue(type, out var scope);
            return scope;
        }

        /// <summary>
        /// Gets the plugin scope.
        /// </summary>
        /// <param name="guid">The guid.</param>
        /// <returns>An ILifetimeScope.</returns>
        public virtual IPluginServiceProvider GetPluginScope(string guid)
        {
            pluginScope.TryGetValue(guid, out var scope);
            return scope;
        }
    }
}