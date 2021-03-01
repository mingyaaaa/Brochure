using System;
using Microsoft.AspNetCore.Http;

namespace Brochure.Core.Server
{
    /// <summary>
    /// The request delegate proxy.
    /// </summary>
    public class RequestDelegateProxy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestDelegateProxy"/> class.
        /// </summary>
        /// <param name="middleName">The middle name.</param>
        /// <param name="id">The id.</param>
        /// <param name="order">The order.</param>
        /// <param name="factory">The factory.</param>
        public RequestDelegateProxy(string middleName, Guid id, int order, Func<RequestDelegate, RequestDelegate> factory)
        {
            this.MiddleName = middleName;
            this.PluginId = id;
            this.Order = order;
            this.MiddleFactory = factory;
        }
        /// <summary>
        /// 中间件名称
        /// </summary>
        /// <value></value>
        public string MiddleName { get; }
        /// <summary>
        /// 插件名称
        /// </summary>
        /// <value></value>
        public Guid PluginId { get; }
        /// <summary>
        /// 中间件执行顺序
        /// </summary>
        /// <value></value>
        public int Order { get; set; }

        /// <summary>
        /// 中间件方法
        /// </summary>
        /// <value></value>
        public Func<RequestDelegate, RequestDelegate> MiddleFactory { get; }
    }
}