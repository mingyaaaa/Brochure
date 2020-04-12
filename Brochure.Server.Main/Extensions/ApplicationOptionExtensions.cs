using System;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Server.Main.Extensions
{
    public static class ApplicationOptionExtensions
    {
        public static async Task AddPluginController (this ApplicationOption applicationOption)
        {
            var mvcBuilder = applicationOption.Services.AddControllers ();
            var manager = applicationOption.Services.GetServiceInstance<IPluginManagers> ();
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
        }
    }
}