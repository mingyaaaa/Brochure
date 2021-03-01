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
    public class PropertyDelegate<T1, T2> where T1 : class where T2 : class
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
            var t2 = ReflectorUtil.Instance.CreateInstance<T2>();
            foreach (var item in properties)
            {
                var propertyType = item.PropertyType;
                object value = null;
                var key = propertyType.FullName + item.Name;
                if (!t2Properties.Any(t => t.Name == item.Name))
                {
                    continue;
                }
                if (PropertyGetDelegateCache<T1>.TryGetGetAction(key, out var func))
                {
                    value = func?.Invoke(model);
                }
                else
                {
                    var t_func = ReflectorUtil.Instance.GetPropertyValueFun<T1>(item.Name);
                    value = t_func.Invoke(model);
                    PropertyGetDelegateCache<T1>.AddGetAction(key, t_func);
                }
                if (PropertySetDelegateCache<T2>.TryGetSetAction(key, out var action))
                {
                    action?.Invoke(t2, value);
                }
                else
                {
                    var t_action = ReflectorUtil.Instance.GetSetPropertyValueFun<T2>(propertyType, item.Name);
                    t_action.Invoke(t2, value);
                    PropertySetDelegateCache<T2>.AddSetAction(key, t_action);
                }
            }
            return t2;
        }
    }

    /// <summary>
    /// The record property delegate.
    /// </summary>
    public class GetValuePropertyDelegate<T1> where T1 : class
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
            var t1 = ReflectorUtil.Instance.CreateInstance<T1>();
            foreach (var item in properties)
            {
                var propertyType = item.PropertyType;
                var key = propertyType.FullName + item.Name;
                object value;
                value = read.GetValue(item.Name);
                if (value == null)
                {
                    continue;
                }
                if (PropertySetDelegateCache<T1>.TryGetSetAction(key, out var action))
                {
                    action?.Invoke(t1, value);
                }
                else
                {
                    var t_action = ReflectorUtil.Instance.GetSetPropertyValueFun<T1>(propertyType, item.Name);
                    t_action.Invoke(t1, value);
                    PropertySetDelegateCache<T1>.AddSetAction(key, t_action);
                }

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
                var propertyType = item.PropertyType;
                object value;
                var key = propertyType.FullName + item.Name;


                if (PropertyGetDelegateCache<T1>.TryGetGetAction(key, out var fun))
                {
                    value = fun?.Invoke(obj);
                }
                else
                {
                    var t_func = ReflectorUtil.Instance.GetPropertyValueFun<T1>(item.Name);
                    value = t_func.Invoke(obj);
                    PropertyGetDelegateCache<T1>.AddGetAction(key, t_func);
                }
                record[item.Name] = value;
            }
            return record;
        }
    }
}