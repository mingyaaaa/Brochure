using System;
using System.Collections.Generic;
using System.Linq;
using Brochure.Abstract;
using Microsoft.AspNetCore.Http;
namespace Brochure.Core.Server
{
    public class MiddleManager : IMiddleManager
    {
        public MiddleManager ()
        {
            middleCollection = new List<RequestDelegateProxy> ();
        }
        private readonly List<RequestDelegateProxy> middleCollection;

        public void AddMiddle (string middleName, Guid id, Func<RequestDelegate, RequestDelegate> middle)
        {
            var count = middleCollection.Count + 1; //顺序从1开始
            AddAndRefreshOrder (middleName, id, count, () => middle);
        }

        public void IntertMiddle (string middleName, Guid id, int index, Func<RequestDelegate, RequestDelegate> middle)
        {
            AddAndRefreshOrder (middleName, id, index, () => middle);
        }

        public void AddMiddle (string middleName, Guid guid, Action action)
        {
            var count = middleCollection.Count + 1; //顺序从1开始
            AddAndRefreshOrder (middleName, guid, count, () =>
            {
                action.Invoke ();
                return null;
            });
        }

        public void IntertMiddle (string middleName, Guid guid, int index, Action action)
        {
            AddAndRefreshOrder (middleName, guid, index, () =>
            {
                action.Invoke ();
                return null;
            }); //顺序从1开始
        }

        public IReadOnlyList<RequestDelegateProxy> GetMiddlesList ()
        {
            return middleCollection;
        }

        public void RemovePluginMiddle (Guid guid)
        {
            middleCollection.RemoveAll (t => t.PluginId == guid);
        }

        public void Reset ()
        {
            middleCollection.Clear ();
        }
        public void AddRange (IEnumerable<RequestDelegateProxy> proxy)
        {
            middleCollection.AddRange (proxy);
        }

        private void AddAndRefreshOrder (string middleName, Guid id, int index, Func<object> middleFun)
        {
            if (string.IsNullOrWhiteSpace (middleName))
                throw new Exception ("中间件名称为null");
            if (middleCollection.Any (t => t.MiddleName == middleName))
                throw new Exception ($"{middleName}中间件已存在");
            var orderMiddle = middleCollection.Find (t => t.Order == index);
            if (orderMiddle != null)
            {
                AddMiddleIndex (ref orderMiddle, int.MaxValue);
            }
            middleCollection.Add (new RequestDelegateProxy (middleName, id, index, middleFun));
        }
        private void AddMiddleIndex (ref RequestDelegateProxy orderMiddle, int maxOrder)
        {
            var order = orderMiddle.Order;
            if (orderMiddle.Order == maxOrder)
                order--;
            else
                order++;
            var nextMiddle = middleCollection.Find (t => t.Order == order);
            if (nextMiddle == null)
            {
                orderMiddle.Order = order;
                return;
            }
            AddMiddleIndex (ref nextMiddle, order);
        }
    }

    public class PluginMiddleUnLoadAction : IPluginUnLoadAction
    {
        private readonly IMiddleManager middleManager;

        public PluginMiddleUnLoadAction (IMiddleManager middleManager)
        {
            this.middleManager = middleManager;
        }
        public void Invoke (Guid key)
        {
            middleManager.RemovePluginMiddle (key);
        }
    }
}