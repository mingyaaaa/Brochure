using System;
using Brochure.Abstract;
using Brochure.Core;

namespace Brochure.Core
{
    public partial class ObjectFactory : IObjectFactory
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

        public T1 Create<T1, T2> () where T2 : T1, new ()
        {
            return Create<T2> ();
        }

        public T1 Create<T1, T2> (params object[] objs) where T2 : T1
        {
            return Create<T2> (objs);
        }

    }

    public partial class ObjectFactory
    {
        private readonly IConverPolicy policy;
        public ObjectFactory ()
        {
            policy = new DefaultConverPolicy ();
        }
        public ObjectFactory (IConverPolicy policy)
        {
            this.policy = policy;
        }
        public T2 Create<T1, T2> (T1 model) where T1 : class where T2 : class
        {
            return policy.ConverTo<T1, T2> (model);
        }

        public T2 Create<T1, T2> (T1 model, IConverPolicy converPolicy)
        where T1 : class
        where T2 : class
        {
            return converPolicy.ConverTo<T1, T2> (model);
        }

        public T1 Create<T1> (IRecord record) where T1 : class
        {
            var policy = new RecordConverPolicy ();
            return policy.ConverTo<IRecord, T1> (record);
        }
        public IRecord Create<T1> (T1 obj) where T1 : class
        {
            var policy = new ObjectToRecordConverPolicy ();
            return policy.ConverTo<T1, IRecord> (obj);
        }
    }
}