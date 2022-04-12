using Brochure.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Brochure.Core.Server
{
    /// <summary>
    /// The middle manager.
    /// </summary>
    public class MiddleManager : IMiddleManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MiddleManager"/> class.
        /// </summary>
        public MiddleManager()
        {
            middleCollection = new List<RequestDelegateProxy>();
        }

        private readonly List<RequestDelegateProxy> middleCollection;

        /// <summary>
        /// Gets or sets the middle action.
        /// </summary>
        public Action<Func<RequestDelegate, RequestDelegate>> MiddleAction { get; set; }

        /// <summary>
        /// Adds the middle.
        /// </summary>
        /// <param name="middleName">The middle name.</param>
        /// <param name="pluginId">The plugin id.</param>
        /// <param name="middle">The middle.</param>
        public void AddMiddle(string middleName, Guid pluginId, Func<RequestDelegate, RequestDelegate> middle)
        {
            var count = middleCollection.Count + 1; //顺序从1开始
            AddAndRefreshOrder(middleName, pluginId, count, middle);
        }

        /// <summary>
        /// Interts the middle.
        /// </summary>
        /// <param name="middleName">The middle name.</param>
        /// <param name="pluginId">The id.</param>
        /// <param name="index">The index.</param>
        /// <param name="middle">The middle.</param>
        public void IntertMiddle(string middleName, Guid pluginId, int index, Func<RequestDelegate, RequestDelegate> middle)
        {
            AddAndRefreshOrder(middleName, pluginId, index, middle);
        }

        /// <summary>
        /// Gets the middles list.
        /// </summary>
        /// <returns>A list of RequestDelegateProxies.</returns>
        public IReadOnlyList<RequestDelegateProxy> GetMiddlesList()
        {
            return middleCollection;
        }

        /// <summary>
        /// Removes the plugin middle.
        /// </summary>
        /// <param name="guid">The guid.</param>
        public void RemovePluginMiddle(Guid guid)
        {
            middleCollection.RemoveAll(t => t.PluginId == guid);
        }

        /// <summary>
        /// Resets the.
        /// </summary>
        public void Reset()
        {
            middleCollection.Clear();
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        public void AddRange(IEnumerable<RequestDelegateProxy> proxy)
        {
            middleCollection.AddRange(proxy);
        }

        /// <summary>
        /// Adds the and refresh order.
        /// </summary>
        /// <param name="middleName">The middle name.</param>
        /// <param name="id">The id.</param>
        /// <param name="index">The index.</param>
        /// <param name="middleFun">The middle fun.</param>
        private void AddAndRefreshOrder(string middleName, Guid id, int index, Func<RequestDelegate, RequestDelegate> middleFun)
        {
            if (string.IsNullOrWhiteSpace(middleName))
                throw new Exception("中间件名称为null");
            if (middleCollection.Any(t => t.MiddleName == middleName))
                return;
            var orderMiddle = middleCollection.Find(t => t.Order == index);
            if (orderMiddle != null)
            {
                AddMiddleIndex(ref orderMiddle, int.MaxValue);
            }
            middleCollection.Add(new RequestDelegateProxy(middleName, id, index, middleFun));
        }

        /// <summary>
        /// Adds the middle index.
        /// </summary>
        /// <param name="orderMiddle">The order middle.</param>
        /// <param name="maxOrder">The max order.</param>
        private void AddMiddleIndex(ref RequestDelegateProxy orderMiddle, int maxOrder)
        {
            var order = orderMiddle.Order;
            if (orderMiddle.Order == maxOrder)
                order--;
            else
                order++;
            var nextMiddle = middleCollection.Find(t => t.Order == order);
            if (nextMiddle == null)
            {
                orderMiddle.Order = order;
                return;
            }
            AddMiddleIndex(ref nextMiddle, order);
        }
    }

    /// <summary>
    /// The plugin middle un load action.
    /// </summary>
    public class PluginMiddleUnLoadAction : IPluginUnLoadAction
    {
        private readonly IMiddleManager middleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginMiddleUnLoadAction"/> class.
        /// </summary>
        /// <param name="middleManager">The middle manager.</param>
        public PluginMiddleUnLoadAction(IMiddleManager middleManager)
        {
            this.middleManager = middleManager;
        }

        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Invoke(Guid key)
        {
            middleManager.RemovePluginMiddle(key);
        }
    }
}