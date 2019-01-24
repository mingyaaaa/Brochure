using HostServer.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public static class IApplicationBuilderExtend
    {
        public static async Task UseRpcServer(this IApplicationBuilder app, int port, Func<RpcService, Task> func)
        {
            var rpcServer = new RpcService(port);
            var provider = app.ApplicationServices.CreateScope().ServiceProvider;
            await func(rpcServer);
            var lifeTime = provider.GetService<IApplicationLifetime>();
            lifeTime.ApplicationStarted.Register(async () =>
            {
                await rpcServer.StartAsync();
            });
            lifeTime.ApplicationStopped.Register(() =>
            {
                rpcServer.Stop();
            });
        }

        public static async Task ConfigHostService(this IApplicationBuilder app, Func<IHostService.Client, Task> func)
        {
            var hostClient = new RpcClient<IHostService.Client>(HostServer.ServiceKey.HostServiceKey);
            await func(hostClient.Client);
        }
    }
}