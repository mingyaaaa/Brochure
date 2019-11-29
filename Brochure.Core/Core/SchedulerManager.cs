using CenterService.Core.SchedulerRule;
using CenterService.Core.SchedulerRule.SehedulerPart;
using CenterService.Utils;
using Data.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using Utilities.Extends;
using Utilities.Utils;

namespace CenterService.Core
{

    /// <summary>
    /// 定时器的时间设置 
    /// 格式  * * * * * *  秒 分 时 日 月 周
    ///  整体规则排列如下，且日和周必须有一个位数是 ？
    /// ?： 代表示模糊的意思，必须存在，且只能在日或周中的一个存在
    /// *： 最小单位轮询，在分钟的字段域里，表示每分钟；在小时的字段域里，表示每小时
    /// /： 表示递增： 如0/5在秒的字段域里，表示第0、5、15、20.... 秒 可以省略0，即 /5
    /// -： 表示范围, 如1-10在秒字段域里，表示1s、2s、3s到10s都执行
    /// ,： 表示并且， 如1,10,20在秒字段域里，表示1s，10s，20s都执行
    /// #： 只能存在周这一个域，表示第几周的星期几，如果超出范围，则忽略不记，如2#4，表示第四周的星期二
    /// L： 表示last的意思： 天： 10L 表示本月的倒数第十天执行， 5L 表示本月的最后一个周四执行（暂不实现）
    /// </summary>
    public class SchedulerTime
    {
        private Dictionary<Type, ISchedulerPart> schedulerParts;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerTime"/> class.
        /// </summary>
        /// <param name="parmSchedulerParts">The parm scheduler parts.</param>
        public SchedulerTime(params ISchedulerPart[] parmSchedulerParts)
        {
            schedulerParts = new Dictionary<Type, ISchedulerPart>();
            InitSchedulerPart();
            var dayPartType = typeof(DaySchedulerPart);
            var weekPartType = typeof(WeekSchedulerPart);
            //如果传入参数包含星期 则直接将日期赋值为? 如果都包含 下面的逻辑判断将会用日期覆盖星期
            if (parmSchedulerParts.Any(t => t is WeekSchedulerPart))
            {
                schedulerParts[dayPartType] = new DaySchedulerPart(new AnySchedulerRule());
            }
            //校验单个规则
            foreach (var item in parmSchedulerParts)
            {
                if (!item.VerifyRule(out var error))
                    throw new ArgumentException(error);
                var type = item.GetType();
                if (schedulerParts.ContainsKey(type))
                    schedulerParts[type] = item;
            }


            //联合规则校验
            //日期和星期必须存在一个？ 默认日期覆盖星期

            if (schedulerParts.ContainsKey(dayPartType) && schedulerParts.ContainsKey(weekPartType))
            {
                var dayPart = schedulerParts[dayPartType];
                var weekPart = schedulerParts[weekPartType];
                //星期和日期两个都不是？ 默认日期覆盖星期
                if (!(dayPart.Rule is AnySchedulerRule) && !(weekPart.Rule is AnySchedulerRule))
                {
                    schedulerParts[weekPartType] = new WeekSchedulerPart(new AnySchedulerRule());
                }//星期和日期两个都是？ 默认日期为？星期为*
                if (dayPart.Rule is AnySchedulerRule && weekPart.Rule is AnySchedulerRule)
                {
                    schedulerParts[weekPartType] = new WeekSchedulerPart(new AllSchedulerRule());
                }
            }
            //年份 月份 日期能够转化为日期格式
            var yearPartType = typeof(YearSchedulerPart);
            var monthPathType = typeof(MonthSchedulerPart);
            if (schedulerParts.ContainsKey(dayPartType) && schedulerParts.ContainsKey(weekPartType) && schedulerParts.ContainsKey(monthPathType))
            {
                var dayPart = schedulerParts[dayPartType];
                var yearPart = schedulerParts[yearPartType];
                var monthPart = schedulerParts[monthPathType];
                if (dayPart.Rule is ConstSchedulerRule && yearPart.Rule is ConstSchedulerRule && monthPart.Rule is ConstSchedulerRule)
                {
                    var day = dayPart.Rule.Symbol.As<int>();
                    var year = dayPart.Rule.Symbol.As<int>();
                    var month = dayPart.Rule.Symbol.As<int>();
                    if (!DateTime.TryParse($"{year}-{month}-{day}", out var _))
                        throw new ArgumentException("年月日参数日期不存在");
                }
            }
        }

        /// <summary>
        /// Inits the scheduler part.
        /// </summary>
        private void InitSchedulerPart()
        {
            schedulerParts.Add(typeof(YearSchedulerPart), new YearSchedulerPart(new AllSchedulerRule()));
            schedulerParts.Add(typeof(WeekSchedulerPart), new WeekSchedulerPart(new AnySchedulerRule()));
            schedulerParts.Add(typeof(MonthSchedulerPart), new MonthSchedulerPart(new AllSchedulerRule()));
            schedulerParts.Add(typeof(DaySchedulerPart), new DaySchedulerPart(new AllSchedulerRule()));
            schedulerParts.Add(typeof(HourSchedulerPart), new HourSchedulerPart(new AllSchedulerRule()));
            schedulerParts.Add(typeof(MinuteSchedulerPart), new MinuteSchedulerPart(new AllSchedulerRule()));
            schedulerParts.Add(typeof(SecondSchedulerPart), new SecondSchedulerPart(new AllSchedulerRule()));
        }


        public override string ToString()
        {
            return string.Join(" ", schedulerParts.Values.OrderBy(t => t.Order).Select(t => t.Value));
        }
    }

    /// <summary>
    /// 定时任务管理  一个定时器计划  管理多个触发器和多个工作  单例  多个会导致相同Job问题的问题
    /// </summary>
    public class SchedulerManager
    {
        private IScheduler scheduler = null;
        private readonly ILogger<SchedulerManager> logger;
        private List<JobInfo> schedulerInfos;
        private static object lockObj = new object();
        private string SCHEDULERSFILE = "LocalData\\schedulers.json";
        private string fileRecordPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerManager"/> class.
        /// </summary>
        /// <param name="schedulerFactory">The scheduler factory.</param>
        /// <param name="logger">The logger.</param>
        public SchedulerManager(ILogger<SchedulerManager> logger)
        {
            this.logger = logger;
            schedulerInfos = new List<JobInfo>();
            fileRecordPath = Path.Combine(CommonUtils.GetDataConfigPath(), SCHEDULERSFILE);
        }

        /// <summary>
        ///按指定时间定时
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="jobKey">The job key.</param>
        /// <param name="second">The second.</param>
        /// <returns>A Task.</returns>
        public Task AddSchedulerJob(Type type, string jobKey, SchedulerTime schedulerTime, Document parms = null)
        {
            return AddSchedulerJob(type, jobKey, schedulerTime.ToString(), parms);
        }

        /// <summary>
        /// 按时间间隔 定时 只限于秒钟
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jobKey"></param>
        /// <param name="seconds"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async Task AddSchedulerJob(Type type, string jobKey, int seconds, Document parms = null)
        {
            await InitSchedulerAsync();
            if (!type.GetInterfaces().Any(t => t.FullName == typeof(IJob).FullName))
            {
                logger.LogWarning($"类型错误：{type.FullName}没有继承IJob类型");
                return;
            }
            var job = await scheduler.GetJobDetail(new JobKey(jobKey));
            if (job != null)
            {
                throw new ArgumentException("该任务已存在");
            }
            try
            {
                var trigger = TriggerBuilder.Create().WithSimpleSchedule(t => t.WithIntervalInSeconds(seconds).RepeatForever()).Build();
                var jobBuilder = JobBuilder.Create(type).WithIdentity(new JobKey(jobKey));
                if (parms != null)
                {
                    jobBuilder = jobBuilder.UsingJobData(new JobDataMap((IDictionary<string, object>)parms));
                }
                job = jobBuilder.Build();
                await scheduler.ScheduleJob(job, trigger);
                AddData(type, jobKey, JobType.Interval, seconds.ToString(), parms, scheduler);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        private async Task AddSchedulerJob(Type type, string jobKey, string schedulerTime, Document parms = null)
        {
            await InitSchedulerAsync();
            if (!type.GetInterfaces().Any(t => t.FullName == typeof(IJob).FullName))
            {
                logger.LogWarning($"类型错误：{type.FullName}没有继承IJob类型");
                return;
            }
            var job = await scheduler.GetJobDetail(new JobKey(jobKey));
            if (job != null)
                return;
            try
            {

                var trigger = TriggerBuilder.Create().WithCronSchedule(schedulerTime, t =>
                {
                    t.WithMisfireHandlingInstructionDoNothing();
                }).Build();
                var jobBuilder = JobBuilder.Create(type).WithIdentity(new JobKey(jobKey));
                if (parms != null)
                {
                    jobBuilder = jobBuilder.UsingJobData(new JobDataMap((IDictionary<string, object>)parms));
                }
                job = jobBuilder.Build();
                await scheduler.ScheduleJob(job, trigger);
                AddData(type, jobKey, JobType.DateTime, schedulerTime, parms, scheduler);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }


        ///// <summary>
        ///// 指定周期添加Job 周期单位 秒
        ///// </summary>
        ///// <param name="type">The type.</param>
        ///// <param name="jobKey">The job key.</param>
        ///// <param name="second">The second.</param>
        ///// <returns>A Task.</returns>
        //public async Task AddIntervalScheduler(Type type, string jobKey, Action<SimpleScheduleBuilder> action, Document parms = null)
        //{
        //    await InitSchedulerAsync();
        //    if (!type.GetInterfaces().Any(t => t.FullName == typeof(IJob).FullName))
        //    {
        //        logger.LogWarning($"类型错误：{type.FullName}没有继承IJob类型");
        //        return;
        //    }
        //    var job = await scheduler.GetJobDetail(new JobKey(jobKey));
        //    if (job != null)
        //        return;
        //    try
        //    {
        //        var trigger = TriggerBuilder.Create().WithSimpleSchedule(action).Build();
        //        var jobBuilder = JobBuilder.Create(type).WithIdentity(new JobKey(jobKey));
        //        if (parms != null)
        //        {
        //            jobBuilder = jobBuilder.UsingJobData(new JobDataMap((IDictionary<string, object>)parms));
        //        }
        //        job = jobBuilder.Build();
        //        await scheduler.ScheduleJob(job, trigger);
        //        AddData(type, jobKey, schedulerTime, parms, scheduler);
        //    }
        //    catch (Exception e)
        //    {
        //        logger.LogError(e, e.Message);
        //    }
        //}


        private void AddData(Type type, string jobKey, JobType jobType, string schedulerTime, Document parms, IScheduler scheduler)
        {
            lock (lockObj)
            {
                schedulerInfos.Add(new JobInfo()
                {
                    JobName = jobKey,
                    SchedulerTime = schedulerTime,
                    JobTypeFullName = type.AssemblyQualifiedName,
                    JobType = (int)jobType,
                    Params = parms
                });
                SaveJobRecord();
            }
        }

        /// <summary>
        /// 删除指定的定时器
        /// </summary>
        /// <param name="schedulerName">The scheduler name.</param>
        /// <returns>A Task.</returns>
        public async Task RemoveJob(string jobKey)
        {
            await scheduler.DeleteJob(new JobKey(jobKey));
        }

        /// <summary>
        /// 删除指定的定时器
        /// </summary>
        /// <param name="schedulerName">The scheduler name.</param>
        /// <returns>A Task.</returns>
        public async Task StopJob(string jobKey)
        {
            await scheduler.PauseJob(new JobKey(jobKey));
        }

        /// <summary>
        /// 指定定时任务启动
        /// </summary>
        /// <param name="jobKey">The scheduler name.</param>
        /// <returns>A Task.</returns>
        public async Task StartJob(string jobKey)
        {
            await scheduler.ResumeJob(new JobKey(jobKey));
        }

        /// <summary>
        /// 保存一下定时任务记录
        /// </summary>
        public void SaveJobRecord()
        {
            //创建目录
            FileUtils.CreateDir(fileRecordPath);
            //写入数据
            FileUtils.WriteAllText(fileRecordPath, schedulerInfos);
        }

        /// <summary>
        /// 载入定时任务
        /// </summary>
        public async Task LoadJobRecord()
        {
            try
            {
                if (!File.Exists(fileRecordPath))
                    return;
                var str = File.ReadAllText(fileRecordPath);
                var list = JsonConvert.DeserializeObject<List<JobInfo>>(str);
                foreach (var item in list)
                {
                    await LoadJob(item);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }

        }

        /// <summary>
        /// Loads the scheduler.
        /// </summary>
        /// <param name="jobInfo">The schduler info.</param>
        /// <returns>A Task.</returns>
        private async Task LoadJob(JobInfo jobInfo)
        {
            if (jobInfo == null)
                throw new ArgumentNullException(nameof(jobInfo));
            var type = Type.GetType(jobInfo.JobTypeFullName);
            var param = jobInfo.Params.As<Document>();
            if (jobInfo.JobType == (int)JobType.Interval)
                await AddSchedulerJob(type, jobInfo.JobName, jobInfo.SchedulerTime.As<int>(), param);
            else if (jobInfo.JobType == (int)JobType.DateTime)
                await AddSchedulerJob(type, jobInfo.JobName, jobInfo.SchedulerTime, param);
        }

        /// <summary>
        /// 初始化定时器的信息
        /// </summary>
        /// <returns>A Task.</returns>
        private async Task<IScheduler> InitSchedulerAsync()
        {
            if (scheduler != null)
                return scheduler;
            scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();
            return scheduler;
        }
    }

    /// <summary>
    /// The job info.
    /// </summary>
    public class JobInfo
    {
        /// <summary>
        /// Gets or sets the job name.
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// Gets or sets the job type full name.
        /// </summary>
        public string JobTypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the scheduler time.
        /// </summary>
        public string SchedulerTime { get; set; }

        /// <summary>
        /// Gets or sets the job type.
        /// </summary>
        public int JobType { get; set; }

        /// <summary>
        /// Gets or sets the params.
        /// </summary>
        public object Params { get; set; }

    }

    public enum JobType
    {
        Interval = 1,
        DateTime = 2
    }
}
