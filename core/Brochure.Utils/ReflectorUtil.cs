using Brochure.Abstract.Extensions;
using Brochure.Abstract.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Brochure.Utils
{
    /// <summary>
    /// The reflector util.
    /// </summary>
    public class ReflectorUtil : IReflectorUtil
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static IReflectorUtil Instance => new ReflectorUtil();

        /// <summary>
        /// Gets the object of base.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="type">The type.</param>
        /// <returns>A list of object.</returns>
        public IEnumerable<object> GetObjectOfBase(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var listobject = new List<object>();
            foreach (var item in types)
            {
                if (type.IsAssignableFrom(item))
                    listobject.Add(assembly.CreateInstance(item.FullName));
            }
            return listobject;
        }

        /// <summary>
        /// Gets the object of base.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>A list of TS.</returns>
        public IEnumerable<T> GetObjectOfBase<T>(Assembly assembly)
        {
            var type = typeof(T);
            var objs = GetObjectOfBase(assembly, type);
            return objs.OfType<T>();
        }

        /// <summary>
        /// Gets the type of base.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="type">The type.</param>
        /// <returns>A list of Types.</returns>
        public IEnumerable<Type> GetTypeOfBase(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var list = new List<Type>();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (type.IsAssignableFrom(item))
                    list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// Gets the type of base.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>A list of Types.</returns>
        public IEnumerable<Type> GetTypeOfBase<T>(Assembly assembly)
        {
            var type = typeof(T);
            return GetTypeOfBase(assembly, type);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="parms">The parms.</param>
        /// <returns>A T.</returns>
        public T CreateInstance<T>(params object[] parms) where T : class
        {
            var type = typeof(T);
            return (T)CreateInstance(type, parms);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parms">The parms.</param>
        /// <returns>An object.</returns>
        public object CreateInstance(Type type, params object[] parms)
        {
            var typeinfo = type.GetTypeInfo();
            var paramsTypes = new List<Type>();
            foreach (var o in parms)
            {
                paramsTypes.Add(o.GetType());
            }
            var constructor = typeinfo.GetConstructor(paramsTypes.ToArray());
            return constructor?.Invoke(parms);
        }

        /// <summary>
        /// Gets the object of absolute base.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="type">The type.</param>
        /// <returns>A list of object.</returns>
        public IEnumerable<object> GetObjectOfAbsoluteBase(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var listobject = new List<object>();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (item.BaseType == type)
                    listobject.Add(assembly.CreateInstance(item.FullName));
            }
            return listobject;
        }

        /// <summary>
        /// Gets the object of absolute base.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>A list of TS.</returns>
        public IEnumerable<T> GetObjectOfAbsoluteBase<T>(Assembly assembly)
        {
            var type = typeof(T);
            var listobject = GetObjectOfAbsoluteBase(assembly, type);
            return listobject.OfType<T>();
        }

        /// <summary>
        /// Gets the type of absolute base.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="type">The type.</param>
        /// <returns>A list of Types.</returns>
        public IEnumerable<Type> GetTypeOfAbsoluteBase(Assembly assembly, Type type)
        {
            var types = assembly.GetTypes();
            var listobject = new List<Type>();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (item.BaseType == type)
                    listobject.Add(item);
            }
            return listobject;
        }

        /// <summary>
        /// Gets the type of absolute base.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>A list of Types.</returns>
        public IEnumerable<Type> GetTypeOfAbsoluteBase<T>(Assembly assembly)
        {
            var type = typeof(T);
            return GetTypeOfAbsoluteBase(assembly, type);
        }

        /// <summary>
        /// Gets the set property value fun.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>An Action.</returns>
        public Action<T1, T2> GetSetPropertyValueFun<T1, T2>(string propertyName)
        {
            return GetSetPropertyValueFun<T1>(typeof(T2), propertyName) as Action<T1, T2>;
        }

        /// <summary>
        /// Gets the property value fun.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A Func.</returns>
        public Func<T1, T2> GetPropertyValueFun<T1, T2>(string propertyName)
        {
            // return GetPropertyValueFun<T1> (propertyName) as Func<T1, T2>;
            var classType = typeof(T1);
            var propertyInfo = classType.GetProperty(propertyName);
            var instance = Expression.Parameter(classType, "t");
            var levelProperty = Expression.Property(instance, propertyInfo);
            var lambdaExpression = Expression.Lambda<Func<T1, T2>>(levelProperty, instance);
            return lambdaExpression.Compile();
        }

        /// <summary>
        /// Gets the property value fun.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A Func.</returns>
        public Func<T1, object> GetPropertyValueFun<T1>(string propertyName)
        {
            var classType = typeof(T1);
            var propertyInfo = classType.GetProperty(propertyName);
            var instance = Expression.Parameter(classType, "t");
            var valueProperty = Expression.Property(instance, propertyInfo);
            var typeAsExpress = Expression.TypeAs(valueProperty, typeof(object));
            var lambdaExpression = Expression.Lambda<Func<T1, object>>(typeAsExpress, instance);
            return lambdaExpression.Compile();
        }

        /// <summary>
        /// Gets the set property value fun.
        /// </summary>
        /// <param name="valueClass">The value class.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>An Action.</returns>
        public Action<T1, object> GetSetPropertyValueFun<T1>(Type valueClass, string propertyName)
        {
            var classType = typeof(T1);
            var propertyInfo = classType.GetProperty(propertyName);
            var constTypeExpress = Expression.Constant(propertyInfo.PropertyType);
            var instance = Expression.Parameter(classType, "c");
            var valueProperty = Expression.Property(instance, propertyInfo);
            var valueExpress = Expression.Parameter(typeof(object), "v");
            var menthod = Expression.Call(typeof(ObjectExtend).GetMethod("As", new Type[] { typeof(object), typeof(Type) }), valueExpress, constTypeExpress);
            var typeAsExpress = Expression.Convert(menthod, propertyInfo.PropertyType);
            var addAssignExpression = Expression.Assign(valueProperty, typeAsExpress);
            var lambdaExpression = Expression.Lambda<Action<T1, object>>(addAssignExpression, instance, valueExpress);
            return lambdaExpression.Compile();
        }

        /// <summary>
        /// 获取属性值方法
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public Func<object, object> GetPropertyValueFun(Type classType, string propertyName)
        {
            var propertyInfo = classType.GetProperty(propertyName);
            var parameter = Expression.Parameter(typeof(object), "t");
            var converExpress = Expression.Convert(parameter, classType);
            var valueProperty = Expression.Property(converExpress, propertyInfo);
            var typeAsExpress = Expression.TypeAs(valueProperty, typeof(object));
            var lambdaExpression = Expression.Lambda<Func<object, object>>(typeAsExpress, parameter);
            return lambdaExpression.Compile();
        }
    }
}