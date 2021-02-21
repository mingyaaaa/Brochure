using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Brochure.Core
{
    public class PluginModule : IModule
    {
        public Task ConfigModule(IServiceCollection services)
        {

            return Task.CompletedTask;
        }

        public Task Initialization(IServiceProvider provider)
        {
            var pluginManager = provider.GetService<IPluginManagers>();
            return pluginManager.ResolverPlugins(provider);
        }
    }
}