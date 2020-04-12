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
            pluginManager.ResolverPlugins (services, t => Task.FromResult (AddPlugin (t)));
            return Task.CompletedTask;
        }

        private bool AddPlugin (IPluginOption pluginOption)
        {
            var item = pluginOption.Plugin;
            //处理插件          
            var task = Task.Run (async () =>
            {
                var result = true;
                try
                {
                    result = await item.StartingAsync (out string errorMsg);
                }
                catch (Exception e)
                {
                    Log.Error ($"{item.Name}插件加载失败", e);
                    await item.ExitAsync ();
                    result = false;
                }
                return result;
            });
            return task.ConfigureAwait (false).GetAwaiter ().GetResult ();
        }
    }
}