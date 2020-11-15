using System;
using Brochure.Abstract;
using Brochure.ORM;
namespace Brochure.ORM
{
    public abstract class EntityBase
    {
        /// <summary>
        /// 主建
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; } = Guid.NewGuid ().ToString ();

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public long CreateTime { get; set; }

        [Ingore]
        //序列号
        public long SequenceId { get; set; }

    }

    public abstract class EntityBase<T> : EntityBase where T : ModelBase
    {
        private readonly IConverPolicy _policy;

        protected EntityBase ()
        {
            _policy = new DefaultConverPolicy ();
        }

        protected EntityBase (IConverPolicy policy)
        {
            this._policy = policy;
        }
        public T GetModel ()
        {
            return _policy.ConverTo<EntityBase, T> (this);
        }

        public T GetModel (IConverPolicy policy)
        {
            return policy.ConverTo<EntityBase, T> (this);
        }
    }

    public interface IEntiryCreator
    {
        Guid Creator { get; set; }
    }
    public interface IEntityUpdator
    {
        Guid Updator { get; set; }
    }
    public interface IEntityUpdateTime
    {
        DateTime UpdateTime { get; set; }
    }
}