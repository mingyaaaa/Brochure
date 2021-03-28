using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brochure.Abstract;
using Brochure.Abstract.PluginDI;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core
{
    /// <summary>
    /// 插件上下文 用于存储主程序服务以及 自身服务
    /// </summary>
    public class PluginContext : IPluginContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginContext"/> class.
        /// </summary>
        public PluginContext()
        {
            Services = new ServiceCollection();
        }

        public PluginContext(IServiceCollection services)
        {
            Services = services;
        }
        /// <summary>
        /// Gets the services.
        /// </summary>
        public IServiceCollection Services { get; }
    }
    //public class PluginContext : IPluginContext
    //{
    //    public PluginContext()
    //    {
    //        this.collections = new List<IPluginContextDescript>();
    //    }
    //    public PluginContext(IEnumerable<IPluginContextDescript> list)
    //    {
    //        this.collections = new List<IPluginContextDescript>(list);
    //    }
    //    public IList<IPluginContextDescript> collections;

    //    public int Count => collections.Count;

    //    public bool IsReadOnly => collections.IsReadOnly;

    //    public IPluginContextDescript this[int index]
    //    {
    //        get =>
    //            collections[index];
    //        set =>
    //            collections[index] = value;
    //    }

    //    public void Add(IPluginContextDescript item)
    //    {
    //        collections.Add(item);
    //    }

    //    public bool Contains(IPluginContextDescript item)
    //    {
    //        return collections.Contains(item);
    //    }

    //    public void CopyTo(IPluginContextDescript[] array, int arrayIndex)
    //    {
    //        collections.CopyTo(array, arrayIndex);
    //    }

    //    public T GetPluginContext<T>()
    //    {
    //        return collections.OfType<T>().FirstOrDefault();
    //    }

    //    public bool Remove(IPluginContextDescript item)
    //    {
    //        return collections.Remove(item);
    //    }

    //    IEnumerator<IPluginContextDescript> IEnumerable<IPluginContextDescript>.GetEnumerator()
    //    {
    //        return collections.GetEnumerator();
    //    }

    //    public IEnumerator GetEnumerator()
    //    {
    //        return collections.GetEnumerator();
    //    }

    //    public int IndexOf(IPluginContextDescript item)
    //    {
    //        return collections.IndexOf(item);
    //    }

    //    public void Insert(int index, IPluginContextDescript item)
    //    {
    //        collections.Insert(index, item);
    //    }

    //    public void RemoveAt(int index)
    //    {
    //        collections.RemoveAt(index);
    //    }

    //    public void Clear()
    //    {
    //        collections.Clear();
    //    }
    //}

    //public class PluginServiceCollectionContext : ServiceCollection
    //{
    //    private List<IPluginContextDescript> collections;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="PluginServiceCollectionContext"/> class.
    //    /// </summary>
    //    /// <param name="services">The services.</param>
    //    public PluginServiceCollectionContext()
    //    {
    //        this.collections = new List<IPluginContextDescript>();
    //    }
    //}
}