using Brochure.Core.Models;
using EventServer.Server;
using System.Collections.Generic;

namespace Brochure.Core.Server
{
    public class PublishEventManager
    {
        internal Dictionary<string, HashSet<string>> EventSourceKeyDic;

        public PublishEventManager()
        {
            EventSourceKeyDic = new Dictionary<string, HashSet<string>>();
        }

        public void Invoke(string eventName, object obj)
        {
            if (!EventSourceKeyDic.ContainsKey(eventName))
                return;
            var service = EventSourceKeyDic[eventName];
            foreach (var item in service)
            {
                var publish = new RpcClient<IPublishEventService.Client>(item);
                try
                {
                    publish.Client.InvokeAsync(eventName, Config.AppKey, JsonUtil.ConverToJsonString(obj), CancelTokenSource.Default.Token);
                }
                catch (System.Exception e)
                {
                    throw;
                }
            }
        }
    }
}