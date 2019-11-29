using CenterService.Core.SchedulerRule.SehedulerPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CenterService.Core.SchedulerRule
{
    public class ConstSchedulerRule : ISchedulerRule
    {
        private readonly int value;

        public ConstSchedulerRule(int value)
        {
            Symbol = value.ToString();
            this.value = value;
        }
        public string Symbol { get; }

        public string HandleRule()
        {
            return Symbol;
        }
        public bool Verify(ISchedulerPart schedulerPart, out string error)
        {
            error = string.Empty;
            if (schedulerPart is YearSchedulerPart)
            {
                if (value < 0)
                {
                    error = "年份超出指定范围";
                    return false;
                }
            }
            else if (schedulerPart is MonthSchedulerPart)
            {
                if (value < 0 || value > 12)
                {
                    error = "月份超出指定范围";
                    return false;
                }
            }
            else if (schedulerPart is WeekSchedulerPart)
            {
                if (value < 0 || value > 7)
                {
                    error = "星期超出指定范围";
                    return false;
                }
            }
            else if (schedulerPart is DaySchedulerPart)
            {
                if (value < 0 || value > 31)
                {
                    error = "日期超出指定范围";
                    return false;
                }
            }
            else if (schedulerPart is HourSchedulerPart)
            {
                if (value < 0 || value >= 23)
                {
                    error = "小时超出指定范围";
                    return false;
                }
            }
            else if (schedulerPart is MinuteSchedulerPart)
            {
                if (value < 0 || value >= 60)
                {
                    error = "分钟超出指定范围";
                    return false;
                }
            }
            else if (schedulerPart is SecondSchedulerPart)
            {
                if (value < 0 || value >= 60)
                {
                    error = "秒钟超出指定范围";
                    return false;
                }
            }
            return true;
        }
    }
}
