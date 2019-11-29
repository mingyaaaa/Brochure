using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Extends;

namespace CenterService.Core.SchedulerRule.SehedulerPart
{
    public class HourSchedulerPart : ISchedulerPart
    {

        public HourSchedulerPart(ISchedulerRule schedulerRule)
        {
            this.Order = 3;
            this.Value = schedulerRule.HandleRule();
            this.Rule = schedulerRule;
        }
        public HourSchedulerPart(int hour) : this(new ConstSchedulerRule(hour)) { }
        public int Order { get; }

        public string Value { get; }
        public ISchedulerRule Rule { get; }

        public bool VerifyRule(out string error)
        {
            return Rule.Verify(this, out error);
        }
    }
}
