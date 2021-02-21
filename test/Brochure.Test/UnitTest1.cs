using System.Diagnostics;
using System.Linq;
using Brochure.Utils;
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

        [TestMethod]
        public void TestExpress ()
        {
            Stopwatch a = new Stopwatch ();
            a.Start ();
            for (int i = 0; i < 100000; i++)
            {
                var obj = new A ();
                obj.Ap = "aa";
            }
            Trace.TraceInformation (a.ElapsedMilliseconds.ToString ());
            a.Stop ();

            a.Restart ();
            var ap = typeof (A).GetProperty ("Ap");
            for (int i = 0; i < 100000; i++)
            {
                var obj = new A ();
                ap.SetValue (obj, "aa");
            }
            Trace.TraceInformation (a.ElapsedMilliseconds.ToString ());
            a.Stop ();
            a.Restart ();
            var fun = ReflectorUtil.Instance.GetSetPropertyValueFun<A> (typeof (string), "Ap");

            for (int i = 0; i < 100000; i++)
            {
                var obj = new A ();
                fun.Invoke (obj, "aa");
            }
            Trace.TraceInformation (a.ElapsedMilliseconds.ToString ());
            a.Stop ();
        }

        [TestMethod]
        public void TestExpress2 ()
        {
            Stopwatch a = new Stopwatch ();
            a.Start ();
            for (int i = 0; i < 100000; i++)
            {
                var obj = new A ();
                var apa = obj.Ap;
            }
            Trace.TraceInformation (a.ElapsedMilliseconds.ToString ());
            a.Stop ();

            a.Restart ();
            var ap = typeof (A).GetProperty ("Ap");
            for (int i = 0; i < 100000; i++)
            {
                var obj = new A ();
                ap.GetValue (obj);
            }
            Trace.TraceInformation (a.ElapsedMilliseconds.ToString ());
            a.Stop ();

            a.Restart ();
            var fun = ReflectorUtil.Instance.GetPropertyValueFun<A, string> ("Ap");

            for (int i = 0; i < 100000; i++)
            {
                var obj = new A () { Ap = "aa" };
                var aaa = fun.Invoke (obj);
            }
            Trace.TraceInformation (a.ElapsedMilliseconds.ToString ());
            a.Stop ();
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
            public string Ap { get; set; }
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