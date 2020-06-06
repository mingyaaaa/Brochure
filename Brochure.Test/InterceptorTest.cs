using System.Threading.Tasks;
using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;
using Brochure.Abstract;
using Brochure.Core;
using Brochure.Core.Extenstions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Brochure.Test
{
    [TestClass]
    public class InterceptorTest
    {
        private IServiceCollection InitServiveCollection ()
        {
            var service = new ServiceCollection ();
            service.AddSingleton<IPluginContextDescript, PluginServiceCollectionContext> ();
            var managers = new PluginManagers ();
            service.AddSingleton<IPluginManagers> (managers);
            return service;
        }

        [TestMethod]
        public void GobleInterceptorTest ()
        {
            var count = new Mock<ICount> ();
            count.Setup (t => t.Count ());
            var service = InitServiveCollection ();
            service.AddSingleton<ITestA, ImpTestA> ();
            service.AddSingleton<ICount> (count.Object);
            service.AddSingleton<TestInterceptor> ();
            service.AddBrochureInterceptor (t => t.Interceptors.AddServiced<TestInterceptor> ());
            var provider = service.BuildPluginServiceProvider ();
            var test = provider.GetService<ITestA> ();
            test.Test ();
            count.Verify (t => t.Count (), Times.Once ());
        }

    }
    public interface ITestA
    {
        void Test ();
    }

    public class ImpTestA : ITestA
    {
        public void Test () { }
    }
    public class TestInterceptor : AbstractInterceptor
    {

        private readonly ICount count;

        public TestInterceptor (ICount count)
        {
            this.count = count;
        }
        public override Task Invoke (AspectContext context, AspectDelegate next)
        {
            count.Count ();
            return Task.CompletedTask;
        }
    }
    public interface ICount
    {
        [NonAspect]
        void Count ();
    }
}