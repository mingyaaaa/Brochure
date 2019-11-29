using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Extends;

namespace CenterService.Core.SchedulerRule.SehedulerPart
{
    public class MinuteSchedulerPart : ISchedulerPart
    {

        public MinuteSchedulerPart(ISchedulerRule schedulerRule)
        {
            this.Order = 2;
            this.Value = schedulerRule.HandleRule();
            this.Rule = schedulerRule;
        }
        public MinuteSchedulerPart(int minute) : this(new ConstSchedulerRule(minute))
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
