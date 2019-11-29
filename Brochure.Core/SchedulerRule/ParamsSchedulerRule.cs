using CenterService.Core.SchedulerRule.SehedulerPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CenterService.Core.SchedulerRule
{
    public class ParamsSchedulerRule : ISchedulerRule
    {
        private int[] parms;
        public ParamsSchedulerRule(params int[] parms)
        {
            this.parms = parms;
            Symbol = ",";
        }
        public string Symbol { get; }

        public string HandleRule()
        {
            return string.Join(Symbol, parms);
        }
        public bool Verify(ISchedulerPart schedulerPart, out string error)
        {
            error = string.Empty;
            if (schedulerPart is YearSchedulerPart)
            {
            }
            else if (schedulerPart is MonthSchedulerPart) { }
            else if (schedulerPart is WeekSchedulerPart) { }
            else if (schedulerPart is DaySchedulerPart) { }
            else if (schedulerPart is HourSchedulerPart) { }
            else if (schedulerPart is MinuteSchedulerPart) { }
            else if (schedulerPart is SecondSchedulerPart) { }
            return true;
        }
    }
}
