using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Server.Main.Abstract.Interfaces;
using Brochure.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Brochure.Core;
namespace Brochure.Server.Main.Extensions
{
    internal static class PluginOptionExtensions
    {

        internal static bool AddPlugin(this IPluginOption pluginOption, IServiceCollection service, IMvcBuilder mvcBuilder)
        {
            var item = pluginOption.Plugin;
            var loggerFactory = service.GetServiceInstance<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("AddPlugins");
            //处理插件          
            var task = Task.Run(async () =>
            {
                var result = true;
                try
                {
                    var r = await item.StartingAsync(out string errorMsg);
                    if (r)
                    {
                        mvcBuilder.AddApplicationPart(item.Assembly);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"{item.Name}插件加载失败");
                    await item.ExitAsync();
                    result = false;
                }
                return result;
            });
            return task.ConfigureAwait(false).GetAwaiter().GetResult();
        }
        internal static void UseConfigure(this IPluginOption pluginOption, IApplicationBuilder applicationBuilder)
        {
            var assembly = pluginOption.Plugin.Assembly;
            var reflectorUtil = applicationBuilder.ApplicationServices.GetService<IReflectorUtil>();
            var configures = reflectorUtil.GetObjectByInterface<IStarupConfigure>(assembly);
            foreach (var item in configures)
            {
                item.Configure(applicationBuilder);
            }
        }
    }
}
