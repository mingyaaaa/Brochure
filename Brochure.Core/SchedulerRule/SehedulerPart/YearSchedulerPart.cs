using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Extends;

namespace CenterService.Core.SchedulerRule.SehedulerPart
{
    public class YearSchedulerPart : ISchedulerPart
    {

        public YearSchedulerPart(ISchedulerRule schedulerRule)
        {
            this.Order = 7;
            Value = schedulerRule.HandleRule();
            this.Rule = schedulerRule;
        }
        public YearSchedulerPart(int year) : this(new ConstSchedulerRule(year))
        {
        }
        public int Order { get; }

        public string Value { get; }

        public ISchedulerRule Rule { get; }

        public bool VerifyRule(out string error)
        {
            return Rule.Verify(this, out error);
        }
    }
}
