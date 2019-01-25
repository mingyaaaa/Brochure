using EventServer.Server;

namespace Brochure.Core.Server
{
    public static class RpcServiceExtend
    {
        public static void RegiestPublisheEventService(this RpcService rpcService, SubscribeEventManager subscribeManager)
        {
            var publish = new PublicshEventService(subscribeManager);
            rpcService.RegisterRpcServer(EventServer.ServiceKey.PublishEventKey, new IPublishEventService.AsyncProcessor(publish));
        }

        public static void RegistSubscribeEventService(this RpcService rpcService, PublishEventManager publishManager)
        {
            var subscribe = new SubscribeEventService(publishManager);
            rpcService.RegisterRpcServer(EventServer.ServiceKey.SubscribeEventKey, new ISubscribeEventService.AsyncProcessor(subscribe));
        }
    }
}