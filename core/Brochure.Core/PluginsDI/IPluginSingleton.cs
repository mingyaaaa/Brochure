using Autofac;
using Autofac.Core.Lifetime;
using Brochure.Abstract;

namespace Brochure.Core.PluginsDI
{
    public interface IPluginSingleton<T> where T : class
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        WeakReference<T> Value { get; }
    }

    internal class PluginSingleton<T> : IPluginSingleton<T> where T : class
    {
        private ILifetimeScope scope;
        private ILifetimeScope _pluginScope;

        public PluginSingleton(PluginServiceTypeCache serviceTypeCache)
        {
            var type = typeof(T);
            _pluginScope = serviceTypeCache.GetPluginScope(type);
            scope = _pluginScope?.BeginLifetimeScope();
            if (_pluginScope != null)
                _pluginScope.CurrentScopeEnding += Scope_CurrentScopeEnding;
        }

        private void Scope_CurrentScopeEnding(object sender, LifetimeScopeEndingEventArgs e)
        {
            scope = null;
            if (_pluginScope != null)
                _pluginScope.CurrentScopeEnding -= Scope_CurrentScopeEnding;
        }

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