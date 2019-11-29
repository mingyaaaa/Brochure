using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CenterService.Core.SchedulerRule
{
    public interface ISchedulerPart
    {
        ISchedulerRule Rule { get; }
        /// <summary>
        /// Gets the order.
        /// </summary>
        int Order { get; }
        /// <summary>
        /// Gets the value.
        /// </summary>
        string Value { get; }

        bool VerifyRule(out string error);
    }
}
