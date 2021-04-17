using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Brochure.Core.Module;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Brochure.Test
{
    [TestClass]
    public class InterceptorTest : BaseTest
    {
        public InterceptorTest()
        {
            base.InitBaseService();
            Service.AddSingleton<IPluginManagers, PluginManagers>();
            Service.AddSingleton<IModuleLoader, ModuleLoader>();
            Service.AddSingleton<IPluginLoader, PluginLoader>();
            Service.AddSingleton<IPluginLoadAction, DefaultLoadAction>();
            Service.AddSingleton<IPluginUnLoadAction, DefaultUnLoadAction>();
        }

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
            Service.AddBrochureInterceptor(t => t.Interceptors.AddServiced<TestInterceptor>());
            var provider = Service.BuildPluginServiceProvider();
            var test = provider.GetService<ITestA>();
            test.Test();
            count.Verify(t => t.Count(), Times.AtLeast(1));
        }

        [TestMethod]
        public void TestInterceptorContextSingletonTest()
        {
            Service.AddSingleton<ITestA, ImpTestA>();
            Service.AddSingleton<ICount, ImpCount>();
            //  Service.AddBrochureInterceptor(t => t.Interceptors.AddTyped<TestAttInterceptorAttribute>());
            var provider = Service.BuildPluginServiceProvider();
            var test = provider.GetService<ITestA>();
            var count = provider.GetService<ICount>();
            test.Test();//此处Test执行一次，拦截器执行一次 共两次 由于是单例则值不变 
            Assert.AreEqual(2, count.GetCount());
            test.Test();
            Assert.AreEqual(4, count.GetCount());
        }


        [TestMethod]
        public void TestInterceptorTest()
        {
            var service = new ServiceCollection();
            service.AddSingleton<IPluginManagers>(new PluginManagers());
            service.AddSingleton<ITestA, ImpTestA>();
            service.AddSingleton<ICount, ImpCount>();
            //  Service.AddBrochureInterceptor(t => t.Interceptors.AddTyped<TestAttInterceptorAttribute>());
            var provider = service.BuildPluginServiceProvider();
            var test = provider.GetService<ITestA>();
            var count = provider.GetService<ICount>();
            test.Test();//此处Test执行一次，拦截器执行一次 共两次 由于是单例则值不变 
            Assert.AreEqual(2, count.GetCount());
            test.Test();
            Assert.AreEqual(4, count.GetCount());
        }


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
            var provider = Service.BuildPluginServiceProvider();
            var test = provider.GetService<ITestA>();//此处的拦截器
            test.Test();
            mock.Verify(t => t.Count(), Times.Exactly(3));
        }

        [TestMethod]
        public void TestInterceptorContextScopeAndSingletonTest()
        {
            var mock = new Mock<ICount>();
            Service.AddScoped<ITestA, ImpTestA>();
            Service.AddSingleton<ICount>(mock.Object);
            var provider = Service.BuildPluginServiceProvider();
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
            var provider = Service.BuildPluginServiceProvider();
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
    public interface ITestA
    {

        void Test();
    }

    public class ImpTestA : ITestA
    {
        private readonly ICount _iCount;

        public ImpTestA(ICount ICount)
        {
            _iCount = ICount;
        }
        [TestAttInterceptor]
        public void Test() { _iCount.Count(); }
    }

    public interface ITestB
    {

        void Test();
    }

    public class ImpTestB : ITestB
    {
        [TestAttInterceptor]
        public void Test() { }
    }

    public class TestInterceptor : AbstractInterceptor
    {

        private readonly ICount count;

        public TestInterceptor(ICount count)
        {
            this.count = count;
        }
        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            count.Count();
            next(context);
            return Task.CompletedTask;
        }
    }

    public class TestAttInterceptorAttribute : AbstractInterceptorAttribute
    {

        public override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var count = context.ServiceProvider.GetService<ICount>();
            count.Count();
            next(context);
            return Task.CompletedTask;
        }
    }
    public interface ICount
    {
        [NonAspect]
        void Count();

        int GetCount();
    }

    public class ImpCount : ICount
    {
        private int count = 0;
        public void Count()
        {
            count++;
        }

        public int GetCount()
        {
            return count;
        }
    }
}