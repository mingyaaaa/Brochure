using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core.Extenstions;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Brochure.Core.Server
{
    /// <summary>
    /// The i service collection extensions.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the plugin controller.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>A Task.</returns>
        internal static async Task AddPluginController(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();
            services.TryAddSingleton<IMvcBuilder>(mvcBuilder);
            var provider = services.BuildServiceProvider();
            var manager = provider.GetService<IPluginManagers>();
            var pluginList = manager.GetPlugins();
            foreach (var item in pluginList)
            {
                try
                {
                    await item.StartAsync();
                    mvcBuilder.AddApplicationPart(item.Assembly);
                    Log.Info($"{item.Name}加载成功");
                }
                catch (Exception e)
                {
                    Log.Error($"{item.Name}加载失败", e);
                    await item.ExitAsync();
                }
            }
        }

        /// <summary>
        /// Adds the brochure server.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="action">The action.</param>
        /// <returns>A Task.</returns>
        public static async Task AddBrochureServer(this IServiceCollection services, Action<ApplicationOption> action = null)
        {
            services.AddLogging(t => t.AddConsole());
            services.AddBrochureCore(option =>
           {
               option.AddLog();
               option.Services.TryAddSingleton<IMiddleManager>(new MiddleManager());
               option.Services.AddTransient<IPluginUnLoadAction, PluginMiddleUnLoadAction>();
               services.AddSingleton<IActionDescriptorChangeProvider>(PluginActionDescriptorChangeProvider.Instance);
               option.Services.Replace(ServiceDescriptor.Transient(typeof(IApplicationBuilderFactory), typeof(PluginApplicationBuilderFactory)));
               action?.Invoke(option);
           });
            await services.AddPluginController();
        }
    }
}