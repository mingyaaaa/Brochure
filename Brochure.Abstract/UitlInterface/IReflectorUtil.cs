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
        IEnumerable<object> GetObjectByInterface(Assembly assembly, Type type);
        /// <summary>
        /// 根据接口创建创建指定的对象
        /// </summary>
        /// <typeparam name="T">创建的类型</typeparam>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        IEnumerable<T> GetObjectByInterface<T>(Assembly assembly);
        /// <summary>
        /// 根据接口获取指定类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypeByInterface(Assembly assembly, Type type);

        /// <summary>
        /// 根据基类创建指定父类的对象
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<object> GetObjectByClass(Assembly assembly, Type type);

        /// <summary>
        /// 根据基类创建指定父类的对象
        /// </summary>
        /// <typeparam name="T">指定基类</typeparam>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        IEnumerable<T> GetObjectByClass<T>(Assembly assembly);
        /// <summary>
        /// 根据基类创建指定父类的对象类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<Type> GetTypeByClass(Assembly assembly, Type type);

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parms"></param>
        /// <returns></returns>
        T CreateInstance<T>(params object[] parms) where T : class;

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parms"></param>
        /// <returns></returns>
        object CreateInstance(Type type, params object[] parms);
    }
}