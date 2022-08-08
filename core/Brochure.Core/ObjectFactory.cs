using Brochure.Abstract;
using Brochure.Abstract.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Brochure.Core
{
    /// <summary>
    /// The object factory.
    /// </summary>
    public partial class ObjectFactory : IObjectFactory
    {
        /// <summary>
        /// Creates the.
        /// </summary>
        /// <returns>A T.</returns>
        public T Create<T>() where T : new()
        {
            return new T();
        }

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="objs">The objs.</param>
        /// <returns>A T.</returns>
        public T Create<T>(params object[] objs) where T : class
        {
            return (T)Create(typeof(T), objs);
        }

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="objs">The objs.</param>
        /// <returns>An object.</returns>
        public object Create(Type type, params object[] objs)
        {
            return Activator.CreateInstance(type, objs);
        }

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="objs">The objs.</param>
        /// <returns>A T.</returns>
        public T CreateByIoc<T>(IServiceProvider service, params object[] objs)
        {
            return (T)CreateByIoc(service, typeof(T), objs);
        }

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="type">The type.</param>
        /// <param name="objs">The objs.</param>
        /// <returns>An object.</returns>
        public object CreateByIoc(IServiceProvider serviceProvider, Type type, params object[] objs)
        {
            return ActivatorUtilities.CreateInstance(serviceProvider, type, objs);
        }
    }
}