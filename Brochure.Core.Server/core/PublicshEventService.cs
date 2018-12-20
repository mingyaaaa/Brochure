using EventServer.Server;

namespace Brochure.Core.Server
{
    public class PublicshEventService : IPublishEventService.Iface
    {
        private IEventManager _eventManager;
        public PublicshEventService(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }
        public void Invoke(string eventName, string serviceKey, string jsonParams)
        {
            var param = JsonUtil.ReadJson(jsonParams);
            _eventManager.Invoke(eventName, serviceKey, param);
        }
    }
}
