using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The plugin transient.
    /// </summary>
    public interface ITransientService<T> : IDisposable where T : class
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        WeakReference<T> Value { get; }
    }

    /// <summary>
    /// The plugin transient.
    /// </summary>
    internal class TransientService<T> : ITransientService<T> where T : class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScope _scope;

        public TransientService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _scope = _serviceProvider.CreateScope();
        }

        public WeakReference<T> Value
        {
            get
            {
                var obj = _scope.ServiceProvider.GetService<T>();
                return new WeakReference<T>(obj);
            }
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}