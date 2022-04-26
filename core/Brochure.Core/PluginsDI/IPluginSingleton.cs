using Autofac;
using Autofac.Core.Lifetime;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The plugin singleton.
    /// </summary>
    public interface IPluginSingleton<T> where T : class
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        WeakReference<T> Value { get; }
    }

    /// <summary>
    /// The plugin singleton.
    /// </summary>
    internal class PluginSingleton<T> : IPluginSingleton<T> where T : class
    {
        private IPluginServiceProvider scope;
        private IPluginServiceProvider _pluginScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSingleton"/> class.
        /// </summary>
        /// <param name="serviceTypeCache">The service type cache.</param>
        public PluginSingleton(PluginServiceTypeCache serviceTypeCache)
        {
            var type = typeof(T);
            _pluginScope = serviceTypeCache.GetPluginScope(type);
            scope = _pluginScope?.CreateScope();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public WeakReference<T> Value
        {
            get
            {
                if (_pluginScope.IsDisposed)
                    scope = null;
                var value = scope?.GetService<T>();
                return new WeakReference<T>(value);
            }
        }
    }
}