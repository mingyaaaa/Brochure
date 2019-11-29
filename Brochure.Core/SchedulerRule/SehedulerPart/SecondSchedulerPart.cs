using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Extends;

namespace CenterService.Core.SchedulerRule.SehedulerPart
{
    public class SecondSchedulerPart : ISchedulerPart
    {


        public SecondSchedulerPart(ISchedulerRule schedulerRule)
        {
            this.Order = 1;
            this.Value = schedulerRule.HandleRule();
            this.Rule = schedulerRule;
        }
        public SecondSchedulerPart(int seconds) : this(new ConstSchedulerRule(seconds)) { }
        public int Order { get; }

        public string Value { get; }
        public ISchedulerRule Rule { get; }

        public bool VerifyRule(out string error)
        {
            return Rule.Verify(this, out error);
        }
    }
}
