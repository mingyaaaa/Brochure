using EventServer.Server;

namespace Brochure.Core.Server.Extends
{
    public static class RpcServiceExtend
    {
        public static void RegiestPublisheEventService(this RpcService rpcService, string eventServiceName, IEventManager eventManager)
        {
            var publish = new PublicshEventService(eventManager);
            rpcService.RegisterRpcServer(eventServiceName, new IPublishEventService.AsyncProcessor(publish));
        }
    }
}