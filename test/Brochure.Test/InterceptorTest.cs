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
            Service.AddSingleton<IPluginContextDescript, PluginServiceCollectionContext>();
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

    }
    public interface ITestA
    {
        void Test();
    }

    public class ImpTestA : ITestA
    {
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
    public interface ICount
    {
        [NonAspect]
        void Count();
    }
}