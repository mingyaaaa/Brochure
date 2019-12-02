namespace Brochure.Abstract
{
    public interface IObjectFactory
    {
        T Create<T> () where T : new ();
    }

    public class ObjectFactory : IObjectFactory
    {
        public T Create<T> () where T : new ()
        {
            return new T ();
        }
    }
}