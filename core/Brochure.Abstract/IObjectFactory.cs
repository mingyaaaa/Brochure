using System;
namespace Brochure.Abstract
{
    public interface IObjectFactory
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Create<T> () where T : new ();

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">对象参数</param>
        /// <returns></returns>
        T Create<T> (params object[] objs);

        /// <summary>
        /// 创建对象 并返回其基类对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        T1 Create<T1, T2> () where T2 : T1, new ();

        /// <summary>
        /// 创建对象 并返回其基类对象
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        T1 Create<T1, T2> (params object[] objs) where T2 : T1;

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        object Create (Type type, params object[] objs);

        /// <summary>
        /// 将T1类型转化为T2类型 属性名称相同 并且能赋值的可以转换
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        T2 Create<T1, T2> (T1 model) where T1 : class where T2 : class;

        /// <summary>
        /// 将T1类型转化为T2类型 属性名称相同 并且能赋值的可以转换
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        T2 Create<T1, T2> (T1 model, IConverPolicy converPolicy) where T1 : class where T2 : class;

        /// <summary>
        /// 安装Key值对应转换
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        T1 Create<T1> (IRecord record) where T1 : class;

        IRecord Create<T1> (T1 obj) where T1 : class;
    }
}