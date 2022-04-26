using Autofac;
using Autofac.Extensions.DependencyInjection;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The autofac plugin service provider.
    /// </summary>
    public class AutofacPluginServiceProvider : IPluginServiceProvider
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IList<IPluginServiceProvider> _sunScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacPluginServiceProvider"/> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        public AutofacPluginServiceProvider(ILifetimeScope lifetimeScope) : this()
        {
            _lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacPluginServiceProvider"/> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        public AutofacPluginServiceProvider(IServiceProvider lifetimeScope) : this()
        {
            if (lifetimeScope is AutofacServiceProvider autofacServiceProvider)
                _lifetimeScope = autofacServiceProvider.LifetimeScope;
            throw new Exception("需要使用Autofac容器");
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="AutofacPluginServiceProvider"/> class from being created.
        /// </summary>
        private AutofacPluginServiceProvider()
        {
            _sunScope = new List<IPluginServiceProvider>();
        }

        public bool IsDisposed { get; private set; }

        /// <inheritdoc/>

        public IPluginServiceProvider CreateScope(IServiceCollection services)
        {
            services = services ?? new ServiceCollection();
            var scope = _lifetimeScope.BeginLifetimeScope(t =>
            {
                t.Populate(services);
            });
            var provider = new AutofacPluginServiceProvider(scope);
            _sunScope.Add(provider);
            return provider;
        }

        /// <inheritdoc/>

        public void Dispose()
        {
            _lifetimeScope?.Dispose();
            foreach (var item in _sunScope)
                item.Dispose();
            IsDisposed = true;
        }

        /// <inheritdoc/>

        public async ValueTask DisposeAsync()
        {
            IsDisposed = true;
            foreach (var item in _sunScope)
                await item.DisposeAsync();
            await _lifetimeScope.DisposeAsync();
        }

        /// <inheritdoc/>

        public object GetService(Type serviceType)
        {
            return _lifetimeScope.Resolve(serviceType);
        }
    }
}