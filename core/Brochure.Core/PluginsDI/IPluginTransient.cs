using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Core.PluginsDI
{
    public interface IPluginTransient<T> : IDisposable where T : class
    {
        WeakReference<T> Value { get; }
    }

    internal class PluginTransient<T> : IPluginTransient<T> where T : class
    {
        private ILifetimeScope _lifetimeScope;
        private ILifetimeScope _pluginScope;

        public PluginTransient(PluginServiceTypeCache serviceTypeCache)
        {
            var type = typeof(T);
            _pluginScope = serviceTypeCache.GetPluginScope(type);
            _lifetimeScope = _pluginScope?.BeginLifetimeScope();
            if (_pluginScope != null)
                _pluginScope.CurrentScopeEnding += Scope_CurrentScopeEnding;
        }

        private void Scope_CurrentScopeEnding(object sender, Autofac.Core.Lifetime.LifetimeScopeEndingEventArgs e)
        {
            _lifetimeScope = null;
            if (_pluginScope != null)
                _pluginScope.CurrentScopeEnding -= Scope_CurrentScopeEnding;
        }

        public WeakReference<T> Value
        {
            get
            {
                var value = _lifetimeScope?.Resolve<T>();
                return new WeakReference<T>(value);
            }
        }

        public void Dispose()
        {
            _lifetimeScope?.Dispose();
        }
    }
}