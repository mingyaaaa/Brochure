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
        where T2 : class
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
        public T2 ConverTo<T1, T2>(T1 model)
        where T1 : class
        where T2 : class
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
    public class ObjectToRecordConverPolicy : IConverPolicy
    {
        /// <summary>
        /// Convers the to.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A T2.</returns>
        public T2 ConverTo<T1, T2>(T1 model)
        where T1 : class
        where T2 : class
        {
            var policy = new ObjectToRecordDelegate<T1>();
            return (T2)policy.ConverTo(model);
        }
    }

}