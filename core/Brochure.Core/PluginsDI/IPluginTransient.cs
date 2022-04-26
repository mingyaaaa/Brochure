using Autofac;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;
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
        private IPluginServiceProvider _lifetimeScope;
        private IPluginServiceProvider _pluginScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginTransient"/> class.
        /// </summary>
        /// <param name="serviceTypeCache">The service type cache.</param>
        public PluginTransient(PluginServiceTypeCache serviceTypeCache)
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