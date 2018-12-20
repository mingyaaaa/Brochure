using Brochure.Core.Core;
using Brochure.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Server
{
    public static class IServiceCollectionExtend
    {
        public static IServerManager AddBrochureServer(this IServiceCollection services)
        {
            var serverManager = new ServerManager(services);
            //注册Ioc代理
            serverManager.AddSingleton(serverManager);
            //注册上下文
            serverManager.AddSingleton<IContext, Context>();
            //注册插件管理
            serverManager.AddSingleton<IPluginManagers, PluginManagers>();
            serverManager.AddSingleton<EventManager>();
            serverManager.AddSingleton<PublicshEventService>();
            //注册启动项
            serverManager.AddTransient<ServerBootstrap>();
            DI.ServerManager = serverManager;
            return serverManager;
        }
    }
}
