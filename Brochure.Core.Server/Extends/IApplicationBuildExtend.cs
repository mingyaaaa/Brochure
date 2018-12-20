using EventServer.Server;
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
            var provider = app.ApplicationServices.CreateScope().ServiceProvider;
            var publishEventService = provider.GetService<PublicshEventService>();
            //默认加入事件服务
            rpcServer.RegisterRpcServer(EventServer.ServiceKey.Key, new IPublishEventService.Processor(publishEventService));
            foreach (var item in processorsServerDic.Keys)
            {
                rpcServer.RegisterRpcServer(item, processorsServerDic[item]);
            }
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
