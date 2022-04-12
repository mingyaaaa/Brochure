using Brochure.Abstract;
using Brochure.Abstract.Models;
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
    }
}