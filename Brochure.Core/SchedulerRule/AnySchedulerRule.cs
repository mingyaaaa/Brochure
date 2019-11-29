using CenterService.Core.SchedulerRule.SehedulerPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CenterService.Core.SchedulerRule
{
    public class AnySchedulerRule : ISchedulerRule
    {
        public AnySchedulerRule()
        {
            Symbol = "?";
        }
        public string Symbol { get; }

        public string HandleRule()
        {
            return Symbol;
        }
        public bool Verify(ISchedulerPart schedulerPart, out string error)
        {
            error = string.Empty;
            return true;
        }
    }
}
