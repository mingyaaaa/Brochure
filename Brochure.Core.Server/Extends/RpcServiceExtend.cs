using EventServer.Server;

namespace Brochure.Core.Server
{
    public static class RpcServiceExtend
    {
        public static void RegiestPublisheEventService(this RpcService rpcService, string eventServiceName, SubscribeEventManager eventManager)
        {
            var publish = new PublicshEventService(eventManager);
            rpcService.RegisterRpcServer(eventServiceName, new IPublishEventService.AsyncProcessor(publish));
        }
    }
}