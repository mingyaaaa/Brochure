using EventServer.Server;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public interface IEventManager
    {
        Task RegistEventAsync(string eventName, string serviceKey, Action<object> action);

        Task RemoveEventAsync(string eventName, string serviceKey);

        Task InvokeAsync(string eventName, string serviceKey, IRecord obj);
    }

    public class EventManager : IEventManager
    {
        private IDictionary<string, Action<object>> _eventCollection;
        private string _publishServiceKey;
        private CancellationTokenSource _cancellationTokenSource;

        public EventManager(string publishServiceKey)
        {
            _eventCollection = new Dictionary<string, Action<Object>>();
            _publishServiceKey = publishServiceKey;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task RegistEventAsync(string eventName, string serviceKey, Action<object> action)
        {
            var rpc = new RpcClient<ISubscribeEventService.Client>(serviceKey, EventServer.ServiceKey.Key);
            if (await rpc.Client.RegistEventTypeAsync(eventName, _publishServiceKey, _cancellationTokenSource.Token))
            {
                var key = eventName + serviceKey;
                _eventCollection.Add(key, action);
            }
        }

        public async Task RemoveEventAsync(string eventName, string serviceKey)
        {
            var rpc = new RpcClient<ISubscribeEventService.Client>(serviceKey, EventServer.ServiceKey.Key);
            if (await rpc.Client.RemoveEventTypeAsync(eventName, _publishServiceKey, _cancellationTokenSource.Token))
            {
                var key = eventName + serviceKey;
                _eventCollection.Remove(key);
            }
        }

        public async Task InvokeAsync(string eventName, string serviceKey, IRecord obj)
        {
            await Task.Delay(0);
            var action = _eventCollection[eventName + serviceKey];
            action(obj);
        }
    }
}
