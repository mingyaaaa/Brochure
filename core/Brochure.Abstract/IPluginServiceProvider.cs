using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Abstract
{
    /// <summary>
    /// The plugin service provider.
    /// </summary>
    public interface IPluginServiceProvider : IServiceProvider, IAsyncDisposable, IDisposable
    {
        public bool IsDisposed { get; }

        /// <summary>
        /// Creates the scope.
        /// </summary>
        /// <param name="action"></param>
        /// <returns>An IPluginServiceProvider.</returns>
        IPluginServiceProvider CreateScope(IServiceCollection action);
    }

    public class DefaultPluginServiceProvider : IPluginServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private IList<IServiceScope> _scopes;

        public DefaultPluginServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _scopes = new List<IServiceScope>();
        }

        public bool IsDisposed { get; private set; }

        public IPluginServiceProvider CreateScope(IServiceCollection action)
        {
            var scope = _serviceProvider.CreateScope();
            _scopes.Add(scope);
            return new DefaultPluginServiceProvider(scope.ServiceProvider);
        }

        public void Dispose()
        {
            IsDisposed = true;
            foreach (var item in _scopes)
            {
                item.Dispose();
            }
        }

        public ValueTask DisposeAsync()
        {
            IsDisposed = true;
            Dispose();
            return ValueTask.CompletedTask;
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }

    public static class PluginServiceProviderExtensions
    {
        /// <summary>
        /// Creates the scope.
        /// </summary>
        /// <param name="pluginServiceProvider"></param>
        /// <param name="action"></param>
        /// <returns>An IPluginServiceProvider.</returns>
        public static IPluginServiceProvider CreateScope(this IPluginServiceProvider pluginServiceProvider, Action<IServiceCollection> action = null)
        {
            var a = new ServiceCollection();
            action?.Invoke(a);
            return pluginServiceProvider.CreateScope(a);
        }
    }
}