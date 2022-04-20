using Autofac;
using Autofac.Extensions.DependencyInjection;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brochure.Core
{
    internal class PluginLoadService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public PluginLoadService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var pluginLoader = _serviceProvider.GetService<IPluginLoader>();
            var autofacServiceProvider = _serviceProvider as AutofacServiceProvider;
            var container = autofacServiceProvider?.LifetimeScope as IContainer;
            if (container == null)
                throw new InvalidOperationException("需要autofac容器支持");
            pluginLoader.LoadPlugin(container);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}