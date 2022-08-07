using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The plugin scope.
    /// </summary>
    public interface IScopeService<T> : IDisposable where T : class
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        WeakReference<T> Value { get; }
    }

    /// <summary>
    /// The plugin scope.
    /// </summary>
    internal class ScopeService<T> : IScopeService<T> where T : class
    {
        private readonly IServiceProvider _serviceProvider;
        private IServiceScope _scope;

        public ScopeService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _scope = _serviceProvider.CreateScope();
        }

        public WeakReference<T> Value
        {
            get
            {
                return new WeakReference<T>(_scope.ServiceProvider.GetService<T>());
            }
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}