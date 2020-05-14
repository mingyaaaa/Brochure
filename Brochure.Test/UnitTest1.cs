using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1 ()
        {
            var service = new ServiceCollection ();
            service.AddSingleton<IA, A> ();
            var provider = service.BuildServiceProvider ();
            service.Replace (ServiceDescriptor.Singleton<IA, A1> ());

            var scope = new MyScopePrviderFacory (service);
            var a = scope.CreateScope ().ServiceProvider.GetService<IA> ();
            a.h ();
        }

        public interface IA
        {
            void h ();
        }
        public interface IB
        {
            void k ();
        }
        public class A : IA
        {
            public void h ()
            {
                Trace.TraceInformation ("A");
            }
        }
        public class A1 : IA
        {
            public void h ()
            {
                Trace.TraceInformation ("A1");
            }
        }
        public class B : IB
        {
            public void k ()
            {
                Trace.TraceInformation ("B");
            }
        }

        public class MyScopePrviderFacory : IServiceScopeFactory
        {
            private readonly IServiceCollection services;

            public MyScopePrviderFacory (IServiceCollection services)
            {
                this.services = services;
            }
            public IServiceScope CreateScope ()
            {
                return services.BuildServiceProvider ().CreateScope ();
            }
            public void MergerService (IServiceCollection services)
            {
                this.services.Concat (services);
            }
        }
    }
}