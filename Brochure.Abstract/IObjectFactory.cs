using System;
namespace Brochure.Abstract
{
    public interface IObjectFactory
    {
        T Create<T> () where T : new ();

        T Creater<T> (params object[] objs);
    }

    public class ObjectFactory : IObjectFactory
    {
        public T Create<T> () where T : new ()
        {
            return new T ();
        }

        public T Creater<T> (params object[] objs)
        {
            return (T) Activator.CreateInstance (typeof (T), objs);
        }
    }
}