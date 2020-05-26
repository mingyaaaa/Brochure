using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Server.Main.Abstract.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
namespace Brochure.Server.Main.Core
{
    public class MiddleManager : IMiddleManager
    {
        public MiddleManager ()
        {
            middleCollection = new List<RequestDelegateProxy> ();
        }
        private readonly List<RequestDelegateProxy> middleCollection;

        public void AddMiddle (Guid id, Func<RequestDelegate, RequestDelegate> middle)
        {
            var count = middleCollection.Count;
            middleCollection.Add (new RequestDelegateProxy (id, count, () => middle)); //顺序从1开始
        }

        public void IntertMiddle (Guid id, int index, Func<RequestDelegate, RequestDelegate> middle)
        {
            middleCollection.Add (new RequestDelegateProxy (id, index, () => middle));
        }

        public void AddMiddle (Guid guid, Action action)
        {
            var count = middleCollection.Count;
            middleCollection.Add (new RequestDelegateProxy (guid, count, () =>
            {
                action.Invoke ();
                return null;
            })); //顺序从1开始
        }

        public void IntertMiddle (Guid guid, int index, Action action)
        {
            middleCollection.Add (new RequestDelegateProxy (guid, index, () =>
            {
                action.Invoke ();
                return null;
            })); //顺序从1开始
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
    }

    public class PluginMiddleUnLoadAction : IPluginUnLoadAction
    {
        private readonly IMiddleManager middleManager;

        public PluginMiddleUnLoadAction (IMiddleManager middleManager)
        {
            this.middleManager = middleManager;
        }
        public void Invoke (IPlugins plugins)
        {
            middleManager.RemovePluginMiddle (plugins.Key);
        }
    }
}