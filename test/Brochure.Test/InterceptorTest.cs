using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using Brochure.Abstract;
using Brochure.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Brochure.Test
{
    /// <summary>
    /// The interceptor test.
    /// </summary>
    [TestClass]
    public class InterceptorTest : BaseTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptorTest"/> class.
        /// </summary>
        public InterceptorTest()
        {
            base.InitBaseService();
            Service.AddSingleton<IPluginManagers, PluginManagers>();
            Service.AddSingleton<IPluginLoader, PluginLoader>();
            Service.AddSingleton<IPluginLoadAction, DefaultLoadAction>();
            Service.AddSingleton<IPluginUnLoadAction, DefaultUnLoadAction>();
        }

        /// <summary>
        /// Gobles the interceptor test.
        /// </summary>
        [TestMethod]
        public void GobleInterceptorTest()
        {
            var objectFactoryMock = GetMockService<IObjectFactory>();
            objectFactoryMock.Setup(t => t.Create<ConcurrentDictionary<Guid, IPlugins>>()).Returns(new ConcurrentDictionary<Guid, IPlugins>());
            var count = new Mock<ICount>();
            count.Setup(t => t.Count());
            Service.AddSingleton<ITestA, ImpTestA>();
            Service.AddSingleton<ICount>(count.Object);
            Service.AddSingleton<TestInterceptor>();
            var provider = Service.BuildServiceProvider();
            var test = provider.GetService<ITestA>();
            test.Test();
            count.Verify(t => t.Count(), Times.AtLeast(1));
        }

        /// <summary>
        /// Tests the interceptor context singleton test.
        /// </summary>
        [TestMethod]
        public void TestInterceptorContextSingletonTest()
        {
            Service.AddSingleton<ITestA, ImpTestA>();
            Service.AddSingleton<ICount, ImpCount>();
            //  Service.AddBrochureInterceptor(t => t.Interceptors.AddTyped<TestAttInterceptorAttribute>());
            var provider = Service.BuildServiceProvider();
            var test = provider.GetService<ITestA>();
            var count = provider.GetService<ICount>();
            test.Test();//此处Test执行一次，拦截器执行一次 共两次 由于是单例则值不变
            Assert.AreEqual(2, count.GetCount());
            test.Test();
            Assert.AreEqual(4, count.GetCount());
        }

        /// <summary>
        /// Tests the interceptor test.
        /// </summary>
        [TestMethod]
        public void TestInterceptorTest()
        {
            var service = new ServiceCollection();
            service.AddSingleton<IPluginManagers>(new PluginManagers());
            service.AddSingleton<ITestA, ImpTestA>();
            service.AddSingleton<ICount, ImpCount>();
            //  Service.AddBrochureInterceptor(t => t.Interceptors.AddTyped<TestAttInterceptorAttribute>());
            var provider = service.BuildServiceProvider();
            var test = provider.GetService<ITestA>();
            var count = provider.GetService<ICount>();
            test.Test();//此处Test执行一次，拦截器执行一次 共两次 由于是单例则值不变
            Assert.AreEqual(2, count.GetCount());
            test.Test();
            Assert.AreEqual(4, count.GetCount());
        }

        /// <summary>
        /// Tests the interceptor context singleton and scope test.
        /// </summary>
        [TestMethod]
        public void TestInterceptorContextSingletonAndScopeTest()
        {
            var mock = new Mock<ICount>();
            Service.AddSingleton<ITestA, ImpTestA>();
            Service.AddScoped<ICount>(t =>
            {
                mock.Object.Count();
                return mock.Object;
            });
            //  Service.AddBrochureInterceptor(t => t.Interceptors.AddTyped<TestAttInterceptorAttribute>());
            var provider = Service.BuildServiceProvider();
            var test = provider.GetService<ITestA>();//此处的拦截器
            test.Test();
            mock.Verify(t => t.Count(), Times.Exactly(3));
        }

        /// <summary>
        /// Tests the interceptor context scope and singleton test.
        /// </summary>
        [TestMethod]
        public void TestInterceptorContextScopeAndSingletonTest()
        {
            var mock = new Mock<ICount>();
            Service.AddScoped<ITestA, ImpTestA>();
            Service.AddSingleton<ICount>(mock.Object);
            var provider = Service.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                var test = scope.ServiceProvider.GetService<ITestA>();
                test.Test();
                mock.Verify(t => t.Count(), Times.Exactly(2));
                test = scope.ServiceProvider.GetService<ITestA>();
                test.Test();
                mock.Verify(t => t.Count(), Times.Exactly(4));
            }
            using (var scope = provider.CreateScope())
            {
                var test = scope.ServiceProvider.GetService<ITestA>();
                test.Test();
                mock.Verify(t => t.Count(), Times.Exactly(6));
            }
        }

        /// <summary>
        /// Tests the interceptor context scoped and scoped test.
        /// </summary>
        [TestMethod]
        public void TestInterceptorContextScopedAndScopedTest()
        {
            var mock = new Mock<ICount>();
            Service.AddScoped<ITestA, ImpTestA>();
            Service.AddScoped<ICount>(t =>
            {
                mock = new Mock<ICount>();
                mock.Object.Count();
                return mock.Object;
            });
            var provider = Service.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                var test = scope.ServiceProvider.GetService<ITestA>();
                test.Test();
                mock.Verify(t => t.Count(), Times.Exactly(3));
                test = scope.ServiceProvider.GetService<ITestA>();
                test.Test();
                mock.Verify(t => t.Count(), Times.Exactly(5));
            }
            using (var scope = provider.CreateScope())
            {
                var test = scope.ServiceProvider.GetService<ITestA>();
                test.Test();
                mock.Verify(t => t.Count(), Times.Exactly(3));
                test = scope.ServiceProvider.GetService<ITestA>();
                test.Test();
                mock.Verify(t => t.Count(), Times.Exactly(5));
            }
        }
    }

    /// <summary>
    /// The test a.
    /// </summary>
    public interface ITestA
    {
        /// <summary>
        /// Tests the.
        /// </summary>
        void Test();
    }

    /// <summary>
    /// The imp test a.
    /// </summary>
    public class ImpTestA : ITestA
    {
        private readonly ICount _iCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImpTestA"/> class.
        /// </summary>
        /// <param name="ICount">The i count.</param>
        public ImpTestA(ICount ICount)
        {
            _iCount = ICount;
        }

        /// <summary>
        /// Tests the.
        /// </summary>
        [TestAttInterceptor]
        public void Test()
        { _iCount.Count(); }
    }

    /// <summary>
    /// The test b.
    /// </summary>
    public interface ITestB
    {
        /// <summary>
        /// Tests the.
        /// </summary>
        void Test();
    }

    /// <summary>
    /// The imp test b.
    /// </summary>
    public class ImpTestB : ITestB
    {
        /// <summary>
        /// Tests the.
        /// </summary>
        [TestAttInterceptor]
        public void Test()
        { }
    }

    /// <summary>
    /// The test interceptor.
    /// </summary>
    public class TestInterceptor : AbstractInterceptor
    {
        private readonly ICount count;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestInterceptor"/> class.
        /// </summary>
        /// <param name="count">The count.</param>
        public TestInterceptor(ICount count)
        {
            this.count = count;
        }

        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="next">The next.</param>
        /// <returns>A Task.</returns>
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            count.Count();
            next(context);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// The test att interceptor attribute.
    /// </summary>
    public class TestAttInterceptorAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// Invokes the.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="next">The next.</param>
        /// <returns>A Task.</returns>
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var count = context.ServiceProvider.GetService<ICount>();
            count.Count();
            next(context);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// The count.
    /// </summary>
    public interface ICount
    {
        /// <summary>
        /// Counts the.
        /// </summary>
        [NonAspect]
        void Count();

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns>An int.</returns>
        int GetCount();
    }

    /// <summary>
    /// The imp count.
    /// </summary>
    public class ImpCount : ICount
    {
        private int count = 0;

        /// <summary>
        /// Counts the.
        /// </summary>
        public void Count()
        {
            count++;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns>An int.</returns>
        public int GetCount()
        {
            return count;
        }
    }
}