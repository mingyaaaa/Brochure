using System.IO;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Server.Main
{
    internal static class IServiceCollectionExtensions
    {
        internal static IServiceCollection AddPlugins (this IServiceCollection service, IMvcBuilder mvcBuilder)
        {
            //处理插件
            service.AddSingleton<IPluginManagers, PluginManagers> ();
            var serviceProvider = service.BuildServiceProvider ();
            var pluginManager = serviceProvider.GetService<IPluginManagers> ();
            var pluginBathPath = PluginUtils.GetBasePluginsPath ();
            var allPlugin = Directory.GetFiles (pluginBathPath, "plugin.config");
            foreach (var pluginPath in allPlugin)
            {
                var plugin = pluginManager.ResolvePlugins (pluginPath);
                pluginManager.Regist (plugin);
                var assembly = plugin.GetAssembly ();
                mvcBuilder.AddApplicationPart (assembly);
            }
            return service;
        }
    }
}