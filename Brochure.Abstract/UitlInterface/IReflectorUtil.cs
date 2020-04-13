using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Brochure.Utils
{
    public interface IReflectorUtil
    {
        /// <summary>
        /// 根据接口创建创建指定的对象
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="type">创建的类型</param>
        /// <returns></returns>
        IEnumerable<object> GetObjectOfBase (Assembly assembly, Type type);
        /// <summary>
        /// 根据接口创建创建指定的对象
        /// </summary>
        /// <typeparam name="T">创建的类型</typeparam>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        IEnumerable<T> GetObjectOfBase<T> (Assembly assembly);
        /// <summary>
        /// 根据接口获取指定类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypeOfBase (Assembly assembly, Type type);

        /// <summary>
        /// 根据接口获取指定类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypeOfBase<T> (Assembly assembly);

        /// <summary>
        /// 获取直接继承父类
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<object> GetObjectOfAbsoluteBase (Assembly assembly, Type type);
        /// <summary>
        /// 获取直接继承父类
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<T> GetObjectOfAbsoluteBase<T> (Assembly assembly);

        /// <summary>
        /// 获取直接继承父类类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypeOfAbsoluteBase (Assembly assembly, Type type);
        /// <summary>
        /// 获取直接继承父类类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypeOfAbsoluteBase<T> (Assembly assembly);

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parms"></param>
        /// <returns></returns>
        T CreateInstance<T> (params object[] parms) where T : class;

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parms"></param>
        /// <returns></returns>
        object CreateInstance (Type type, params object[] parms);
    }
}