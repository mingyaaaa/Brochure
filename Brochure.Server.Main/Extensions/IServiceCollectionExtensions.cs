using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Server.Main.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Server.Main
{
    internal static class IServiceCollectionExtensions
    {
        public static async Task AddPluginController (this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc ();
            var manager = services.GetServiceInstance<IPluginManagers> ();
            var application = services.GetServiceInstance<IBApplication> () as BApplication;
            application.ApplicationPartManager = mvcBuilder.PartManager;
            var pluginList = manager.GetPlugins ();
            foreach (var item in pluginList)
            {
                try
                {
                    await item.StartAsync ();
                    mvcBuilder.AddApplicationPart (item.Assembly);
                }
                catch (Exception e)
                {
                    Log.Error ($"{item.Name}加载失败", e);
                }
            }
            mvcBuilder.SetCompatibilityVersion (CompatibilityVersion.Version_3_0);
        }
    }
}