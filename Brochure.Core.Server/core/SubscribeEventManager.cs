using Brochure.Core.Models;
using EventServer.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brochure.Core.Server
{
    public class SubscribeEventManager
    {
        internal IDictionary<string, Action<object>> SubscribeEventCollection;

        public SubscribeEventManager()
        {
            SubscribeEventCollection = new Dictionary<string, Action<Object>>();
        }

        public async Task RegistEventAsync(string eventName, string serviceAppKey, Action<object> action)
        {
            var rpc = new RpcClient<ISubscribeEventService.Client>(serviceAppKey, EventServer.ServiceKey.SubscribeEventKey);
            if (await rpc.Client.RegistEventTypeAsync(eventName, Config.AppKey, CancelTokenSource.Default.Token))
            {
                var key = eventName + serviceAppKey;
                SubscribeEventCollection.Add(key, action);
            }
        }

        public async Task RemoveEventAsync(string eventName, string serviceAppKey)
        {
            var rpc = new RpcClient<ISubscribeEventService.Client>(serviceAppKey, EventServer.ServiceKey.SubscribeEventKey);
            if (await rpc.Client.RemoveEventTypeAsync(eventName, Config.AppKey, CancelTokenSource.Default.Token))
            {
                var key = eventName + serviceAppKey;
                SubscribeEventCollection.Remove(key);
            }
        }
    }
}