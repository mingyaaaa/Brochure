using EventServer.Server;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public class SubscribeEventService : ISubscribeEventService.IAsync
    {
        private PublishEventManager _publishManager;

        public SubscribeEventService(PublishEventManager publish)
        {
            _publishManager = publish;
        }

        public async Task<bool> RegistEventTypeAsync(string eventName, string eventSourceKey, CancellationToken cancellationToken)
        {
            await Task.Delay(1);
            if (!HostServer.EventType.ContainEvent(eventName))
                return false;
            if (_publishManager.EventSourceKeyDic.ContainsKey(eventName))
                _publishManager.EventSourceKeyDic[eventName].Add(eventSourceKey);
            else
                _publishManager.EventSourceKeyDic[eventName] = new HashSet<string>() { eventSourceKey };
            return true;
        }

        public async Task<bool> RemoveEventTypeAsync(string eventName, string eventSourceKey, CancellationToken cancellationToken)
        {
            await Task.Delay(1);
            if (_publishManager.EventSourceKeyDic.ContainsKey(eventName))
            {
                _publishManager.EventSourceKeyDic[eventName].Remove(eventSourceKey);
            }
            return true;
        }
    }
}