using System;

namespace Brochure.Core
{
    public abstract class EntityBase
    {
        /// <summary>
        /// 主建
        /// </summary>
        /// <returns></returns>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        //序列号
        public int SequenceId { get; set; }
    }
}