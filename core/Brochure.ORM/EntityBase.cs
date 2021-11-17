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

    public interface IEntiryCreator
    {
        string Creator { get; set; }
    }

    public interface IEntityUpdator
    {
        string Updator { get; set; }
    }

    public interface IEntityUpdateTime
    {
        long UpdateTime { get; set; }
    }
}