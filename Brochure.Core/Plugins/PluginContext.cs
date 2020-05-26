using System;
using System.Collections.Generic;
using System.Linq;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    /// <summary>
    /// 插件上下文 用于存储主程序服务以及 自身服务
    /// </summary>
    public class PluginContext : ServiceCollection, IPluginContext
    {
        public PluginContext (IEnumerable<IPluginContextDescript> list)
        {
            this.collections = new List<IPluginContextDescript> (list);
        }
        public IList<IPluginContextDescript> collections;
        public void Add (IPluginContextDescript item)
        {
            collections.Add (item);
        }

        public bool Contains (IPluginContextDescript item)
        {
            return collections.Contains (item);
        }

        public void CopyTo (IPluginContextDescript[] array, int arrayIndex)
        {
            collections.CopyTo (array, arrayIndex);
        }

        public T GetPluginContext<T> ()
        {
            return collections.OfType<T> ().FirstOrDefault ();
        }

        public bool Remove (IPluginContextDescript item)
        {
            return collections.Remove (item);
        }

        IEnumerator<IPluginContextDescript> IEnumerable<IPluginContextDescript>.GetEnumerator ()
        {
            return collections.GetEnumerator ();
        }
    }

    public class PluginServiceCollectionContext : ServiceCollection, IPluginContextDescript
    {
        public PluginServiceCollectionContext (IServiceProvider services)
        {
            MainService = services;
        }
        public IServiceProvider MainService { get; }
    }
}