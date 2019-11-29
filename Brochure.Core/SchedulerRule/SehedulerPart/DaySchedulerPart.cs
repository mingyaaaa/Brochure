using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Extends;

namespace CenterService.Core.SchedulerRule.SehedulerPart
{
    public class DaySchedulerPart : ISchedulerPart
    {

        public DaySchedulerPart(ISchedulerRule schedulerRule)
        {
            this.Order = 4;
            this.Value = schedulerRule.HandleRule();
            this.Rule = schedulerRule;
        }
        public DaySchedulerPart(int days) : this(new ConstSchedulerRule(days)) { }
        public int Order { get; }

        public string Value { get; }
        public ISchedulerRule Rule { get; }

        public bool VerifyRule(out string error)
        {
            return Rule.Verify(this, out error);
        }
    }
}
