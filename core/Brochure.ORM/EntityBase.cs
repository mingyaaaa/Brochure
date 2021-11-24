using Brochure.ORM.Atrributes;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LinqDbQueryTest")]

namespace Brochure.ORM
{
    public abstract class EntityBase
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public long CreateTime { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        /// <value></value>
        [Sequence]
        public long SequenceId { get; set; }
    }

    /// <summary>
    /// The entity key.
    /// </summary>
    public interface IEntityKey<T> where T : class, IComparable<T>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        T Id { get; set; }
    }

    /// <summary>
    /// The entiry creator.
    /// </summary>
    public interface IEntiryCreator
    {
        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        string Creator { get; set; }
    }

    /// <summary>
    /// The entity updator.
    /// </summary>
    public interface IEntityUpdator
    {
        /// <summary>
        /// Gets or sets the updator.
        /// </summary>
        string Updator { get; set; }
    }

    /// <summary>
    /// The entity update time.
    /// </summary>
    public interface IEntityUpdateTime
    {
        /// <summary>
        /// Gets or sets the update time.
        /// </summary>
        long UpdateTime { get; set; }
    }
}