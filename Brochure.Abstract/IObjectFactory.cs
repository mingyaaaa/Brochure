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
        T Create<T>() where T : new();

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">对象参数</param>
        /// <returns></returns>
        T Create<T>(params object[] objs);

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
        T1 Create<T1, T2>(params object[] objs) where T2 : T1;

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        object Create(Type type, params object[] objs);
    }

    public class ObjectFactory : IObjectFactory
    {
        public T Create<T>() where T : new()
        {
            return new T();
        }

        public T Create<T>(params object[] objs)
        {
            return (T)Create(typeof(T), objs);
        }

        public object Create(Type type, params object[] objs)
        {
            return Activator.CreateInstance(type, objs);
        }

        public T1 Create<T1, T2>() where T2 : T1, new()
        {
            return Create<T2>();
        }

        public T1 Create<T1, T2>(params object[] objs) where T2 : T1
        {
            return Create<T2>(objs);
        }
    }
}