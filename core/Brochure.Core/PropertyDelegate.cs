using System;
using System.Collections.Concurrent;
using System.Linq;
using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.Utils;

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
        /// Initializes a new instance of the <see cref="PropertyDelegate"/> class.
        /// </summary>
        public PropertyDelegate()
        {
            t1Type = typeof(T1);
            t2Type = typeof(T2);
        }

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
        /// Initializes a new instance of the <see cref="RecordPropertyDelegate"/> class.
        /// </summary>
        public GetValuePropertyDelegate()
        {
            t1Type = typeof(T1);
        }

        /// <summary>
        /// Convers the to.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>A T1.</returns>
        public T1 ConverTo(IGetValue read)
        {
            var properties = t1Type.GetProperties();
            var t1 = new T1();
            foreach (var item in properties)
            {
                var value = read.GetValue(item.Name);
                if (value == null)
                {
                    continue;
                }
                PropertySetDelegateCache.TrySet(item, t1, value);
            }
            return t1;
        }
    }

    /// <summary>
    /// The object to record delegate.
    /// </summary>
    public class ObjectToRecordDelegate<T1> where T1 : class
    {
        private readonly Type t1Type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectToRecordDelegate"/> class.
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