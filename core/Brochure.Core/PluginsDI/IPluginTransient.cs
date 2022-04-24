using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The plugin transient.
    /// </summary>
    public interface IPluginTransient<T> : IDisposable where T : class
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        WeakReference<T> Value { get; }
    }

    /// <summary>
    /// The plugin transient.
    /// </summary>
    internal class PluginTransient<T> : IPluginTransient<T> where T : class
    {
        private ILifetimeScope _lifetimeScope;
        private ILifetimeScope _pluginScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginTransient"/> class.
        /// </summary>
        /// <param name="serviceTypeCache">The service type cache.</param>
        public PluginTransient(PluginServiceTypeCache serviceTypeCache)
        {
            var type = typeof(T);
            _pluginScope = serviceTypeCache.GetPluginScope(type);
            _lifetimeScope = _pluginScope?.BeginLifetimeScope();
            if (_pluginScope != null)
                _pluginScope.CurrentScopeEnding += Scope_CurrentScopeEnding;
        }

        /// <summary>
        /// Scope_S the current scope ending.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Scope_CurrentScopeEnding(object sender, Autofac.Core.Lifetime.LifetimeScopeEndingEventArgs e)
        {
            _lifetimeScope = null;
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
                var value = _lifetimeScope?.Resolve<T>();
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