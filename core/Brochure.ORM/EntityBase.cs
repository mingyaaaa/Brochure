using System;
using Brochure.Abstract;
using Brochure.Core;
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

        /// <summary>
        /// 序列号
        /// </summary>
        /// <value></value>
        public long SequenceId { get; set; }

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