using EventServer.Server;
using System;
using System.Collections.Generic;

namespace Brochure.Core.Server
{
    public interface IEventManager
    {
        void RegistEvent(string eventName, string serviceKey, Action<object> action);
        void RemoveEvent(string eventName, string serviceKey);
        void Invoke(string eventName, string serviceKey, IRecord obj);

    }
    public class EventManager : IEventManager
    {
        private IDictionary<string, Action<object>> _eventCollection;
        private string _publishServiceKey;
        public EventManager(string publishServiceKey)
        {
            _eventCollection = new Dictionary<string, Action<Object>>();
            _publishServiceKey = publishServiceKey;
        }
        public void RegistEvent(string eventName, string serviceKey, Action<object> action)
        {
            var rpc = new RpcClient<ISubscribeEventService.Client>(serviceKey, EventServer.ServiceKey.Key);
            if (rpc.Client.RegistEventType(eventName, _publishServiceKey))
            {
                var key = eventName + serviceKey;
                _eventCollection.Add(key, action);
            }
        }

        public void RemoveEvent(string eventName, string serviceKey)
        {
            var rpc = new RpcClient<ISubscribeEventService.Client>(serviceKey, EventServer.ServiceKey.Key);
            if (rpc.Client.RemoveEventType(eventName, _publishServiceKey))
            {
                var key = eventName + serviceKey;
                _eventCollection.Remove(key);
            }
        }
        public void Invoke(string eventName, string serviceKey, IRecord obj)
        {
            var action = _eventCollection[eventName + serviceKey];
            action(obj);
        }

    }
}
