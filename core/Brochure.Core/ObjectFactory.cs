using System;
using System.Data;
using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.Core;

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
        public T Create<T>(params object[] objs)
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
        /// <returns>A T1.</returns>
        public T1 Create<T1, T2>() where T2 : T1, new()
        {
            return Create<T2>();
        }

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="objs">The objs.</param>
        /// <returns>A T1.</returns>
        public T1 Create<T1, T2>(params object[] objs) where T2 : T1
        {
            return Create<T2>(objs);
        }


    }

    /// <summary>
    /// The object factory.
    /// </summary>
    public partial class ObjectFactory
    {
        private readonly IConverPolicy policy;
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactory"/> class.
        /// </summary>
        public ObjectFactory()
        {
            policy = new DefaultConverPolicy();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFactory"/> class.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public ObjectFactory(IConverPolicy policy)
        {
            this.policy = policy;
        }
        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A T2.</returns>
        public T2 Create<T1, T2>(T1 model) where T1 : class where T2 : class, new()
        {
            var t2 = policy.ConverTo<T1, T2>(model);
            if (model is IConverPolicy<T2>)
            {
                ((IConverPolicy<T2>)model).ConverTo(t2);
            }
            return t2;
        }

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="converPolicy">The conver policy.</param>
        /// <returns>A T2.</returns>
        public T2 Create<T1, T2>(T1 model, IConverPolicy converPolicy)
        where T1 : class
        where T2 : class, new()
        {
            return converPolicy.ConverTo<T1, T2>(model);
        }

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>A T1.</returns>
        public T1 Create<T1>(IRecord record) where T1 : class, new()
        {
            var policy = new GetValueConverPolicy();
            return Create<IRecord, T1>(record, policy);
        }
        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An IRecord.</returns>
        public IRecord Create<T1>(T1 obj) where T1 : class
        {
            var policy = new ObjectToRecordConverPolicy();
            return Create<T1, Record>(obj, policy);
        }

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>A T.</returns>
        public T Create<T>(IGetValue reader) where T : class
        {
            throw new NotImplementedException();
        }

    }
}