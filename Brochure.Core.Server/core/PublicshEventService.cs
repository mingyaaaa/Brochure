using EventServer.Server;
using System.Threading;
using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public class PublicshEventService : IPublishEventService.IAsync
    {
        private SubscribeEventManager _eventManager;

        public PublicshEventService(SubscribeEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public async Task InvokeAsync(string eventName, string eventSourceKey, string jsonParams, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
             {
                 var param = JsonUtil.ConverToJson<object>(jsonParams);
                 var action = _eventManager.SubscribeEventCollection[eventName + eventSourceKey];
                 action(param);
             });
        }
    }
}