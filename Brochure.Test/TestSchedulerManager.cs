using CenterService.Core;
using CenterService.Core.SchedulerRule;
using CenterService.Core.SchedulerRule.SehedulerPart;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Quartz;
using Quartz.Impl;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CenterServiceTest
{
    public class TestJob : IJob
    {
        public static int i = 0;
        public Task Execute(IJobExecutionContext context)
        {
            i++;
            Trace.TraceInformation(DateTime.Now.ToString());
            return Task.CompletedTask;
        }
    }
    public class TestJob1 : IJob
    {
        public static int i = 0;
        public Task Execute(IJobExecutionContext context)
        {
            i++;
            Trace.TraceInformation(DateTime.Now.ToString());
            return Task.CompletedTask;
        }
    }
    [TestClass]
    public class TestSchedulerManager
    {

        Mock<ILogger<SchedulerManager>> loggerMock;
        SchedulerManager manager;
        public TestSchedulerManager()
        {
            loggerMock = new Mock<ILogger<SchedulerManager>>();
            manager = new SchedulerManager(loggerMock.Object);
        }
        /// <summary>
        /// 测试创建对象
        /// </summary>
        [TestMethod]
        public async Task TestCreateTestSchudler()
        {
            var manager = new SchedulerManager(loggerMock.Object);
            Assert.IsNotNull(manager);
        }

        /// <summary>
        /// 指定时间间隔
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestIntervalScheduler()
        {
            await manager.AddSchedulerJob(typeof(TestJob1), "test1", 2);
            await Task.Delay(3000);
            Assert.AreEqual(2, TestJob1.i);
            await Task.Delay(2000);
            Assert.AreEqual(3, TestJob1.i);
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => manager.AddSchedulerJob(typeof(TestJob1), "test1", 2));
        }

        /// <summary>
        /// 规则是否正确
        /// </summary>
        [TestMethod]
        public void TestSchedulerPart()
        {
            var yearRule = new ConstSchedulerRule(2014);
            var month = new ConstSchedulerRule(2);
            var testTimer = new SchedulerTime();
            var str = testTimer.ToString();
            Assert.AreEqual("* * * * * ? *", str);
            testTimer = new SchedulerTime(new YearSchedulerPart(yearRule));
            Assert.AreEqual("* * * * * ? 2014", testTimer.ToString());
            testTimer = new SchedulerTime(new YearSchedulerPart(yearRule), new MonthSchedulerPart(month));
            Assert.AreEqual("* * * * 2 ? 2014", testTimer.ToString());
            testTimer = new SchedulerTime(
                new YearSchedulerPart(2019),
                new MonthSchedulerPart(2),
                new WeekSchedulerPart(2),
                new DaySchedulerPart(1),
                new HourSchedulerPart(22),
                new MinuteSchedulerPart(1),
                new SecondSchedulerPart(1)
                );
            Assert.AreEqual("1 1 22 1 2 ? 2019", testTimer.ToString());

            Assert.ThrowsException<ArgumentException>(() =>
            {
                testTimer = new SchedulerTime(new MonthSchedulerPart(13));
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                testTimer = new SchedulerTime(new YearSchedulerPart(-1));
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                testTimer = new SchedulerTime(new DaySchedulerPart(33));
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                testTimer = new SchedulerTime(new HourSchedulerPart(26));
            });

            Assert.ThrowsException<ArgumentException>(() =>
            {
                testTimer = new SchedulerTime(new MinuteSchedulerPart(61));
            });
            Assert.ThrowsException<ArgumentException>(() =>
            {
                testTimer = new SchedulerTime(new SecondSchedulerPart(61));
            });
        }

        [TestMethod]
        public void TestSchedulerRule()
        {
            //默认
            var testTimer = new SchedulerTime();
            var str = testTimer.ToString();
            Assert.AreEqual("* * * * * ? *", str);

            //常量
            var yearRule = new ConstSchedulerRule(2014);
            testTimer = new SchedulerTime(new YearSchedulerPart(yearRule));
            Assert.AreEqual("* * * * * ? 2014", testTimer.ToString());

            //增量
            var addrule = new AddSechulerRule(1, 2);
            var month = new MonthSchedulerPart(addrule);
            testTimer = new SchedulerTime(month);
            Assert.AreEqual("* * * * 1/2 ? *", testTimer.ToString());

            //?字符
            var anyrule = new AnySchedulerRule();
            var day = new DaySchedulerPart(anyrule);
            testTimer = new SchedulerTime(day);
            Assert.AreEqual("* * * ? * * *", testTimer.ToString());

            //参数 指定值
            var parmsRule = new ParamsSchedulerRule(1, 4, 5, 6);
            day = new DaySchedulerPart(parmsRule);
            testTimer = new SchedulerTime(day);
            Assert.AreEqual("* * * 1,4,5,6 * ? *", testTimer.ToString());

            //参数 范围
            var rangeRule = new RangeSchedulerRule(1, 4);
            day = new DaySchedulerPart(rangeRule);
            testTimer = new SchedulerTime(day);
            Assert.AreEqual("* * * 1-4 * ? *", testTimer.ToString());
        }


        /// <summary>
        /// 指定时间
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestDateTimeScheduler()
        {
            var rule = new ConstSchedulerRule(DateTime.Now.AddSeconds(1).Second);
            var rule1 = new ConstSchedulerRule(DateTime.Now.Year);
            var part = new SecondSchedulerPart(rule);
            var year = new YearSchedulerPart(rule1);
            //  await manager.AddSchedulerJob(typeof(TestJob), "test", new SchedulerTime(part));
            await manager.AddSchedulerJob(typeof(TestJob), "test", new SchedulerTime(part, year));
            //测试定时执行
            await Task.Delay(2000);
            Assert.AreEqual(1, TestJob.i);
        }
    }
}
