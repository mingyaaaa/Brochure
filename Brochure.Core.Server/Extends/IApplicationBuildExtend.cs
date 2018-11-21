using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Thrift;

namespace Brochure.Core.Server
{
    public static class IApplicationBuilderExtend
    {
        public static void UseRpcServer(this IApplicationBuilder app, int port, IDictionary<string, TProcessor> processorsServerDic)
        {
            var rpcServer = new RpcService(port);
            foreach (var item in processorsServerDic.Keys)
            {
                rpcServer.RegisterPrcServer(item, processorsServerDic[item]);

            }
            var provider = app.ApplicationServices.CreateScope().ServiceProvider;
            var lifeTime = provider.GetService<IApplicationLifetime>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                rpcServer.Start();
            });
            lifeTime.ApplicationStopped.Register(() =>
            {
                rpcServer.Stop();
            });
        }
    }
}
