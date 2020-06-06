using System;

namespace Brochure.Core.Server
{
    public class RequestDelegateProxy
    {
        public RequestDelegateProxy (Guid id, int order, Func<object> factory)
        {
            this.PluginId = id;
            this.Order = order;
            this.MiddleFactory = factory;
        }

        public Guid PluginId { get; set; }

        public int Order { get; set; }

        public Func<object> MiddleFactory;
    }
}