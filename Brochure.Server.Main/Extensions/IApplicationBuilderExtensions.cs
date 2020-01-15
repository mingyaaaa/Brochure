using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Models;
using Brochure.Server.Main.Abstract.Interfaces;
using Brochure.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brochure.Server.Main.Extensions
{
    public static class IApplicationBuilderExtensions
    {

        internal static async Task AddPlugins(this IApplicationBuilder applicationBuilder, Action<IApplicationBuilder, PluginOption> action = null)
        {
            var serviceProvider = applicationBuilder.ApplicationServices;
            var pluginManager = serviceProvider.GetService<IPluginManagers>();
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var plugins = pluginManager.GetPlugins();
            foreach (var item in plugins)
            {
                action?.Invoke(applicationBuilder, new PluginOption(item));
                var task = await Task.Run(async () =>
                 {
                     try
                     {
                         var r = await item.StartingAsync();
                         if (r)
                         {
                             await item.StartAsync();
                         }
                         else
                         {
                             r = await item.ExitingAsync();
                             if (r)
                                 await item.ExitAsync();
                             logger.LogError($"{item.Name}插件加载失败");
                         }
                     }
                     catch (Exception e)
                     {
                         logger.LogError(e, $"{item.Name}插件加载失败");
                         await item.ExitAsync();
                     }
                     return Task.CompletedTask;
                 });
            }
        }
    }
}