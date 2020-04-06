using System;
namespace Brochure.Abstract
{
    public interface IObjectFactory
    {
        T Create<T>() where T : new();

        T Create<T>(params object[] objs);

        T1 Create<T1, T2>() where T2 : T1, new();

        T1 Create<T1, T2>(params object[] objs) where T2 : T1;

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