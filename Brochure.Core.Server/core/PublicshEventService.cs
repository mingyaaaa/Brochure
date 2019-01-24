using EventServer.Server;
using System.Threading;
using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public class PublicshEventService : IPublishEventService.IAsync
    {
        private IEventManager _eventManager;

        public PublicshEventService(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public async Task InvokeAsync(string eventName, string eventSourceKey, string jsonParams, CancellationToken cancellationToken)
        {
            var param = JsonUtil.ConverToJson<object>(jsonParams);
            await _eventManager.InvokeAsync(eventName, eventSourceKey, param);
        }
    }
}