using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CenterService.Core.SchedulerRule
{
    /// <summary>
    /// "0/5"规则
    /// </summary>
    public class AddSechulerRule : ISchedulerRule
    {
        private int start;
        private int interval;

        public AddSechulerRule(int start, int interval)
        {
            Symbol = "/";
            this.start = start;
            this.interval = interval;
        }
        public string Symbol { get; }

        public string HandleRule()
        {
            return $"{start}{Symbol}{interval}";
        }

        public bool Verify(ISchedulerPart schedulerPart, out string error)
        {
            error = string.Empty;
            return true;
        }
    }
}
