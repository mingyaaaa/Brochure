using System;
namespace Brochure.Abstract
{
    public interface IObjectFactory
    {
        T Create<T> () where T : new ();

        T Create<T> (params object[] objs);

        object Create (Type type, params object[] objs);
    }

    public class ObjectFactory : IObjectFactory
    {
        public T Create<T> () where T : new ()
        {
            return new T ();
        }

        public T Create<T> (params object[] objs)
        {
            return (T) Create (typeof (T), objs);
        }

        public object Create (Type type, params object[] objs)
        {
            return Activator.CreateInstance (type, objs);
        }
    }
}