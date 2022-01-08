using System;

namespace Brochure.Abstract
{
    /// <summary>
    /// The object factory.
    /// </summary>
    public interface IObjectFactory
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Create<T>() where T : new();

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">对象参数</param>
        /// <returns></returns>
        T Create<T>(params object[] objs) where T : class;

        /// <summary>
        /// 创建对象 并返回其基类对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        T1 Create<T1, T2>() where T2 : T1, new();

        /// <summary>
        /// 创建对象 并返回其基类对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        T1 Create<T1, T2>(params object[] objs) where T2 : class, T1;

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        object Create(Type type, params object[] objs);

        /// <summary>
        /// 将T1类型转化为T2类型 属性名称相同 并且能赋值的可以转换
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        T2 Create<T1, T2>(T1 model) where T1 : class where T2 : class, new();

        /// <summary>
        /// 将T1类型转化为T2类型 属性名称相同 并且能赋值的可以转换
        /// </summary>
        /// <param name="model"></param>
        /// <param name="converPolicy"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        T2 Create<T1, T2>(T1 model, IConverPolicy converPolicy) where T1 : class where T2 : class, new();

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="converPolicy">The conver policy.</param>
        /// <returns>A T2.</returns>
        T2 Create<T1, T2>(T1 model, IConverPolicy<T1, T2> converPolicy) where T1 : class where T2 : class, new();

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>An IRecord.</returns>
        IRecord Create<T1>(T1 obj) where T1 : class;

        /// <summary>
        /// Creates the.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>A T.</returns>
        T Create<T>(IGetValue reader) where T : class, new();
    }
}