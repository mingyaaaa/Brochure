using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Brochure.Core.Server
{
    public static class IServiceCollectionExtensions
    {
        internal static async Task AddPluginController(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();
            var manager = services.GetServiceInstance<IPluginManagers>();
            var application = services.GetServiceInstance<IBApplication>() as BApplication;
            application.ApplicationPartManager = mvcBuilder.PartManager;
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
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public static async Task AddBrochureServer(this IServiceCollection services, Action<ApplicationOption> action = null)
        {
            services.AddMvcCore();
            services.AddLogging(t => t.AddConsole());
            services.AddBrochureCore(option =>
           {
               option.Services.AddSingleton<IBApplication>(new BApplication());
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