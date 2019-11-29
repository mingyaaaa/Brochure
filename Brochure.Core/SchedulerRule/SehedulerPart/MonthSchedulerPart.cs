using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Extends;

namespace CenterService.Core.SchedulerRule.SehedulerPart
{
    public class MonthSchedulerPart : ISchedulerPart
    {

        public MonthSchedulerPart(ISchedulerRule schedulerRule)
        {
            this.Order = 5;
            this.Value = schedulerRule.HandleRule();
            this.Rule = schedulerRule;
        }
        public MonthSchedulerPart(int mounth) : this(new ConstSchedulerRule(mounth)) { }
        public int Order { get; }

        public string Value { get; }
        public ISchedulerRule Rule { get; }

        public bool VerifyRule(out string error)
        {
            return Rule.Verify(this, out error);
        }
    }
}
