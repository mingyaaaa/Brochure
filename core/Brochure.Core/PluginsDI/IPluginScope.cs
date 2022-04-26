using Autofac;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The plugin scope.
    /// </summary>
    public interface IPluginScope<T> : IDisposable where T : class
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        WeakReference<T> Value { get; }
    }

    /// <summary>
    /// The plugin scope.
    /// </summary>
    internal class PluginScope<T> : IPluginScope<T> where T : class
    {
        private IPluginServiceProvider _pluginScope;
        private IPluginServiceProvider _lifetimeScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginScope"/> class.
        /// </summary>
        /// <param name="serviceTypeCache">The service type cache.</param>
        public PluginScope(PluginServiceTypeCache serviceTypeCache)
        {
            var type = typeof(T);
            _pluginScope = serviceTypeCache.GetPluginScope(type);
            _lifetimeScope = _pluginScope?.CreateScope();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public WeakReference<T> Value
        {
            get
            {
                if (_pluginScope.IsDisposed)
                    _lifetimeScope = null;
                var value = _lifetimeScope?.GetService<T>();
                return new WeakReference<T>(value);
            }
        }

        /// <summary>
        /// Disposes the.
        /// </summary>
        public void Dispose()
        {
            _lifetimeScope?.Dispose();
        }
    }
}