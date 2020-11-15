using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Brochure.Utils
{
    public class ReflectorUtil : IReflectorUtil
    {
        public static IReflectorUtil Instance => new ReflectorUtil ();

        public IEnumerable<object> GetObjectOfBase (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var listobject = new List<object> ();
            foreach (var item in types)
            {
                if (type.IsAssignableFrom (item))
                    listobject.Add (assembly.CreateInstance (item.FullName));
            }
            return listobject;
        }

        public IEnumerable<T> GetObjectOfBase<T> (Assembly assembly)
        {
            var type = typeof (T);
            var objs = GetObjectOfBase (assembly, type);
            return objs.OfType<T> ();
        }

        public IEnumerable<Type> GetTypeOfBase (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var list = new List<Type> ();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (type.IsAssignableFrom (item))
                    list.Add (item);
            }
            return list;
        }

        public IEnumerable<Type> GetTypeOfBase<T> (Assembly assembly)
        {
            var type = typeof (T);
            return GetTypeOfBase (assembly, type);
        }

        public T CreateInstance<T> (params object[] parms) where T : class
        {
            var type = typeof (T);
            return (T) CreateInstance (type, parms);
        }

        public object CreateInstance (Type type, params object[] parms)
        {
            var typeinfo = type.GetTypeInfo ();
            var paramsTypes = new List<Type> ();
            foreach (var o in parms)
            {
                paramsTypes.Add (o.GetType ());
            }
            var constructor = typeinfo.GetConstructor (paramsTypes.ToArray ());
            return constructor?.Invoke (parms);
        }

        public IEnumerable<object> GetObjectOfAbsoluteBase (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var listobject = new List<object> ();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (item.BaseType == type)
                    listobject.Add (assembly.CreateInstance (item.FullName));
            }
            return listobject;
        }

        public IEnumerable<T> GetObjectOfAbsoluteBase<T> (Assembly assembly)
        {
            var type = typeof (T);
            var listobject = GetObjectOfAbsoluteBase (assembly, type);
            return listobject.OfType<T> ();
        }

        public IEnumerable<Type> GetTypeOfAbsoluteBase (Assembly assembly, Type type)
        {
            var types = assembly.GetTypes ();
            var listobject = new List<Type> ();
            foreach (var item in types)
            {
                if (item.IsAbstract || item.IsInterface)
                    continue;
                if (item.BaseType == type)
                    listobject.Add (item);
            }
            return listobject;
        }

        public IEnumerable<Type> GetTypeOfAbsoluteBase<T> (Assembly assembly)
        {
            var type = typeof (T);
            return GetTypeOfAbsoluteBase (assembly, type);
        }

        public Action<T1, T2> GetSetPropertyValueFun<T1, T2> (string propertyName)
        {
            return GetSetPropertyValueFun<T1> (typeof (T2), propertyName) as Action<T1, T2>;
        }
        public Func<T1, T2> GetPropertyValueFun<T1, T2> (string propertyName)
        {
            // return GetPropertyValueFun<T1> (propertyName) as Func<T1, T2>;
            var classType = typeof (T1);
            var propertyInfo = classType.GetProperty (propertyName);
            var instance = Expression.Parameter (classType, "t");
            var levelProperty = Expression.Property (instance, propertyInfo);
            var lambdaExpression = Expression.Lambda<Func<T1, T2>> (levelProperty, instance);
            return lambdaExpression.Compile ();
        }

        public Func<T1, object> GetPropertyValueFun<T1> (string propertyName)
        {
            // return GetPropertyValueFun (typeof (T1), propertyName) as Func<T1, object>;
            var classType = typeof (T1);
            var propertyInfo = classType.GetProperty (propertyName);
            var instance = Expression.Parameter (classType, "t");
            var valueProperty = Expression.Property (instance, propertyInfo);
            var typeAsExpress = Expression.TypeAs (valueProperty, typeof (object));
            var lambdaExpression = Expression.Lambda<Func<T1, object>> (typeAsExpress, instance);
            return lambdaExpression.Compile ();
        }

        public Action<T1, object> GetSetPropertyValueFun<T1> (Type valueClass, string propertyName)
        {
            var classType = typeof (T1);
            var propertyInfo = classType.GetProperty (propertyName);
            var instance = Expression.Parameter (classType, "c");
            var valueProperty = Expression.Property (instance, propertyInfo);
            var valueExpress = Expression.Parameter (typeof (object), "v");
            var typeAsExpress = Expression.Convert (valueExpress, valueClass);
            var addAssignExpression = Expression.Assign (valueProperty, typeAsExpress);
            var lambdaExpression = Expression.Lambda<Action<T1, object>> (addAssignExpression, instance, valueExpress);
            return lambdaExpression.Compile ();
        }
    }
}