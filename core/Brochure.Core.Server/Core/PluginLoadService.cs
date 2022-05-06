﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Brochure.Abstract;
using Brochure.Core.PluginsDI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brochure.Core
{
    /// <summary>
    /// The plugin load service.
    /// </summary>
    internal class PluginLoadService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public PluginLoadService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Starts the async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var pluginLoader = _serviceProvider.GetService<IPluginLoader>();
            await pluginLoader.LoadPlugin();
        }

        /// <summary>
        /// Stops the async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task.</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var pluginLoader = _serviceProvider.GetService<IPluginLoader>();
            await pluginLoader.UnLoadAll();
        }
    }
}