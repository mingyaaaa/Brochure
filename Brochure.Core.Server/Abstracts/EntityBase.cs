using System;
using Brochure.Core;
using Brochure.Core.Atrributes;

namespace Brochure.Core.Server.Abstracts
{
    public abstract class EntityBase
    {
        /// <summary>
        /// 主建
        /// </summary>
        /// <returns></returns>
        public Guid Id { get; set; } = Guid.NewGuid ();
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int SequenceId { get; set; }
    }
}