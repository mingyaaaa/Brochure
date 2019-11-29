using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CenterService.Core.SchedulerRule
{
    public interface ISchedulerRule
    {
        string Symbol { get; }
        string HandleRule();
        bool Verify(ISchedulerPart schedulerPart, out string error);
    }
}
