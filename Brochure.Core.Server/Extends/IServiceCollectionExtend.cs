using Brochure.Core.Core;
using Brochure.Core.Interfaces;
using HostServer.Server;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

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
            //注册启动项
            serverManager.AddTransient<ServerBootstrap>();
            DI.ServerManager = serverManager;
            return serverManager;
        }

        public static IServerManager ConfigHostService(this IServerManager serverManager, Func<IHostService.Client, Task> func)
        {
            var hostClient = new RpcClient<IHostService.Client>(HostServer.ServiceKey.HostServiceKey);
            func(hostClient.Client).ConfigureAwait(false).GetAwaiter().GetResult();
            return serverManager;
        }
    }
}