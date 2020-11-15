using System;
using Brochure.Abstract;

namespace Brochure.ORM
{
    public abstract class ModelBase
    {

    }
    public abstract class ModelBase<T> : ModelBase where T : EntityBase
    {
        private readonly IConverPolicy _policy;

        protected ModelBase ()
        {
            _policy = new DefaultConverPolicy ();
        }
        protected ModelBase (IConverPolicy policy)
        {
            this._policy = policy;
        }

        public T GetEntiry ()
        {
            return _policy.ConverTo<ModelBase, T> (this);
        }
        public T GetEntiry (IConverPolicy policy)
        {
            return policy.ConverTo<ModelBase, T> (this);
        }
    }
}