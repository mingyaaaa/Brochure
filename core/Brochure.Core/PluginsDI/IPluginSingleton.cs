﻿using AspectCore.DependencyInjection;
using Brochure.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Brochure.Core.PluginsDI
{
    /// <summary>
    /// The plugin singleton.
    /// </summary>
    public interface ISingletonService<T> where T : class
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        WeakReference<T> Value { get; }
    }

    /// <summary>
    /// The plugin singleton.
    /// </summary>
    internal class SingletonService<T> : ISingletonService<T> where T : class
    {
        private readonly IServiceProvider _serviceProvider;

        public SingletonService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public WeakReference<T> Value
        {
            get
            {
                var obj = _serviceProvider.GetService<T>();
                return new WeakReference<T>(obj);
            }
        }
    }
}