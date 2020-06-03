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
        public async Task Initialization (IServiceCollection services)
        {
            var pluginManagers = new PluginManagers ();
            services.TryAddSingleton<IPluginManagers> (pluginManagers);
            //加载插件
            await pluginManagers.ResolverPlugins (services);
        }
    }
}