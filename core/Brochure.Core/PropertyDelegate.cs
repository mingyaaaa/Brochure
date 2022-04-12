using Brochure.Abstract;
using Brochure.Abstract.Models;
using System;
using System.Linq;

namespace Brochure.Core
{
    /// <summary>
    /// 默认使用的是jsonConver
    /// </summary>
    public class PropertyDelegate<T1, T2> where T1 : class where T2 : class, new()
    {
        private readonly Type t1Type;
        private readonly Type t2Type;

        /// <summary>
        /// Initializes a new instance of the
        /// </summary>
        public PropertyDelegate()
        {
            t1Type = typeof(T1);
            t2Type = typeof(T2);
        }

        /// <summary>
        /// Convers the to.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A T2.</returns>
        public T2 ConverTo(T1 model)
        {
            var properties = t1Type.GetProperties();
            var t2Properties = t2Type.GetProperties();
            var t2 = new T2();
            foreach (var item in properties)
            {
                if (!t2Properties.Any(t => t.Name == item.Name))
                {
                    continue;
                }
                var value = PropertyGetDelegateCache.TryGet(item, model);
                PropertySetDelegateCache.TrySet(item, t2, value);
            }
            return t2;
        }
    }

    /// <summary>
    /// The record property delegate.
    /// </summary>
    public class GetValuePropertyDelegate<T1> where T1 : class, new()
    {
        private readonly Type t1Type;

        /// <summary>
        /// Initializes a new instance of the
        /// </summary>
        public GetValuePropertyDelegate()
        {
            t1Type = typeof(T1);
        }
    }

    /// <summary>
    /// The object to record delegate.
    /// </summary>
    public class ObjectToRecordDelegate<T1> where T1 : class
    {
        private readonly Type t1Type;

        /// <summary>
        /// Initializes a new instance of the
        /// </summary>
        public ObjectToRecordDelegate()
        {
            this.t1Type = typeof(T1);
        }

        /// <summary>
        /// Convers the to.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An IRecord.</returns>
        public IRecord ConverTo(T1 obj)
        {
            var properties = t1Type.GetProperties();
            var record = new Record();
            foreach (var item in properties)
            {
                var value = PropertyGetDelegateCache.TryGet(item, obj);
                record[item.Name] = value;
            }
            return record;
        }
    }
}