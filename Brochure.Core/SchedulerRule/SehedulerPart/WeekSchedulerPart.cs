using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Extends;

namespace CenterService.Core.SchedulerRule.SehedulerPart
{
    public class WeekSchedulerPart : ISchedulerPart
    {

        public WeekSchedulerPart(ISchedulerRule schedulerRule)
        {
            this.Order = 6;
            this.Value = schedulerRule.HandleRule();
            this.Rule = schedulerRule;
        }
        public WeekSchedulerPart(int week) : this(new ConstSchedulerRule(week))
        {
        }
        public int Order { get; }

        public string Value { get; }

        public ISchedulerRule Rule { get; }
        /// <summary>
        /// 只针对常量进行校验
        /// </summary>
        /// <param name="schedulerRule"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool VerifyRule(out string error)
        {
            return Rule.Verify(this, out error);
        }
    }
}
