using Brochure.Core.Interfaces;
using Brochure.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.Server
{
    public static class IServiceCollectionExtend
    {
        public static IServiceCollection AddBrochureServer(this IServiceCollection services, IDIServiceManager serverManager = null)
        {
            //注册Ioc代理
            serverManager.AddSingleton(serverManager);
            //注册上下文
            serverManager.AddSingleton<IContext, Context>();
            //注册插件管理
            serverManager.AddSingleton<IPluginManagers, PluginManagers>();
            //注册hostManager
            serverManager.AddTransient<IHostManager, HostManager>();
            serverManager.AddTransient<HostManagerProvider>();
            //注册启动项
            serverManager.AddTransient<ServerBootstrap>();
            DI.ServerManager = serverManager;
            return services;
        }
    }
}
