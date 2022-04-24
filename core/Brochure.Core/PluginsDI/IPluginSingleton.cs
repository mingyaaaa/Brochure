using Autofac;
using Autofac.Core.Lifetime;
using Brochure.Abstract;

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
        private ILifetimeScope scope;
        private ILifetimeScope _pluginScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginSingleton"/> class.
        /// </summary>
        /// <param name="serviceTypeCache">The service type cache.</param>
        public PluginSingleton(PluginServiceTypeCache serviceTypeCache)
        {
            var type = typeof(T);
            _pluginScope = serviceTypeCache.GetPluginScope(type);
            scope = _pluginScope?.BeginLifetimeScope();
            if (_pluginScope != null)
                _pluginScope.CurrentScopeEnding += Scope_CurrentScopeEnding;
        }

        /// <summary>
        /// Scope_S the current scope ending.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Scope_CurrentScopeEnding(object sender, LifetimeScopeEndingEventArgs e)
        {
            scope = null;
            if (_pluginScope != null)
                _pluginScope.CurrentScopeEnding -= Scope_CurrentScopeEnding;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public WeakReference<T> Value
        {
            get
            {
                var value = scope?.Resolve<T>();
                return new WeakReference<T>(value);
            }
        }
    }
}