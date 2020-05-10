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
        public Task Initialization (IServiceCollection services)
        {
            services.TryAddSingleton<IPluginManagers, PluginManagers> ();
            //加载插件
            var pluginManager = services.GetServiceInstance<IPluginManagers> ();
            pluginManager.ResolverPlugins (services);
            return Task.CompletedTask;
        }

    }
}