using Brochure.Core.Models;
using EventServer.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public interface IEventManager
    {
        Task RegistEventAsync(string eventName, string serviceAppKey, Action<object> action);

        Task RemoveEventAsync(string eventName, string serviceAppKey);

        Task InvokeAsync(string eventName, string serviceAppKey, object obj);
    }

    public class EventManager : IEventManager
    {
        private IDictionary<string, Action<object>> _eventCollection;
        private string _eventServiceName;

        public EventManager(string eventServiceName)
        {
            _eventCollection = new Dictionary<string, Action<Object>>();
            _eventServiceName = eventServiceName;
        }

        public async Task RegistEventAsync(string eventName, string serviceAppKey, Action<object> action)
        {
            var rpc = new RpcClient<ISubscribeEventService.Client>(serviceAppKey, _eventServiceName);
            if (await rpc.Client.RegistEventTypeAsync(eventName, Config.AppKey, CancelTokenSource.Default.Token))
            {
                var key = eventName + serviceAppKey;
                _eventCollection.Add(key, action);
            }
        }

        public async Task RemoveEventAsync(string eventName, string serviceAppKey)
        {
            var rpc = new RpcClient<ISubscribeEventService.Client>(serviceAppKey, _eventServiceName);
            if (await rpc.Client.RemoveEventTypeAsync(eventName, Config.AppKey, CancelTokenSource.Default.Token))
            {
                var key = eventName + serviceAppKey;
                _eventCollection.Remove(key);
            }
        }

        public async Task InvokeAsync(string eventName, string serviceAppKey, object obj)
        {
            await Task.Delay(0);
            var action = _eventCollection[eventName + serviceAppKey];
            action(obj);
        }
    }
}