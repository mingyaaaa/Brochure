using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.Utils;

namespace Brochure.Core
{
    /// <summary>
    /// The default conver policy.
    /// </summary>
    public class DefaultConverPolicy : IConverPolicy
    {
        /// <summary>
        /// Convers the to.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A T2.</returns>
        public T2 ConverTo<T1, T2>(T1 model)
        where T1 : class
        where T2 : class, new()
        {
            var policy = new PropertyDelegate<T1, T2>();
            return policy.ConverTo(model);
        }
    }

    /// <summary>
    /// The get value conver policy.
    /// </summary>
    public class GetValueConverPolicy : IConverPolicy
    {
        /// <summary>
        /// Convers the to.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A T2.</returns>
        public T2 ConverTo<T1, T2>(T1 model)
        where T1 : class
        where T2 : class, new()
        {
            if (!(model is IGetValue))
                return null;
            var policy = new GetValuePropertyDelegate<T2>();
            return policy.ConverTo(model as IGetValue);
        }
    }

    /// <summary>
    /// The object to record conver policy.
    /// </summary>
    public class ObjectToRecordConverPolicy<T1> : IConverPolicy<T1, Record> where T1 : class
    {
        public Record ConverTo(T1 model)
        {
            var policy = new ObjectToRecordDelegate<T1>();
            return (Record)policy.ConverTo(model);
        }
    }
}