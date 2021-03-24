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
        public void TestMethod1()
        {
            var service = new ServiceCollection();
            service.AddSingleton<IA, A>();
            var provider = service.BuildServiceProvider();
            service.Replace(ServiceDescriptor.Singleton<IA, A1>());

            var scope = new MyScopePrviderFacory(service);
            var a = scope.CreateScope().ServiceProvider.GetService<IA>();
            a.h();
        }

        [TestMethod("测试工厂方法会重建数据")]
        public void MyTestMethod()
        {
            var service = new ServiceCollection();
            service.AddSingleton<IA>(t => new A() { Ap = "1" });
            var provider = service.BuildServiceProvider();
            var a = provider.GetService<IA>();
            Assert.AreEqual("1", a.Ap);
            a.Ap = "2";
            var aa = provider.GetService<IA>();
            Assert.AreEqual("2", aa.Ap);

            var provider1 = service.BuildServiceProvider();
            var aaa = provider1.GetService<IA>();
            Assert.AreEqual("1", aaa.Ap);
        }

        [TestMethod("测试类型注入会重建数据")]
        public void Test1()
        {
            var service = new ServiceCollection();
            service.AddSingleton<IA, A>();
            var provider = service.BuildServiceProvider();
            var a = provider.GetService<IA>();
            Assert.AreEqual("1", a.Ap);
            a.Ap = "2";
            var aa = provider.GetService<IA>();
            Assert.AreEqual("2", aa.Ap);

            var provider1 = service.BuildServiceProvider();
            var aaa = provider1.GetService<IA>();
            Assert.AreEqual("1", aaa.Ap);
        }
        [TestMethod("测试实例注入不会重建数据")]
        public void Test3()
        {
            var service = new ServiceCollection();
            service.AddSingleton<IA>(new A());
            var provider = service.BuildServiceProvider();
            var a = provider.GetService<IA>();
            Assert.AreEqual("1", a.Ap);
            a.Ap = "2";
            var aa = provider.GetService<IA>();
            Assert.AreEqual("2", aa.Ap);

            var provider1 = service.BuildServiceProvider();
            var aaa = provider1.GetService<IA>();
            Assert.AreEqual("2", aaa.Ap);
        }
        [TestMethod]
        public void TestExpress()
        {
            Stopwatch a = new Stopwatch();
            a.Start();
            for (int i = 0; i < 100000; i++)
            {
                var obj = new A();
                obj.Ap = "aa";
            }
            Trace.TraceInformation(a.ElapsedMilliseconds.ToString());
            a.Stop();

            a.Restart();
            var ap = typeof(A).GetProperty("Ap");
            for (int i = 0; i < 100000; i++)
            {
                var obj = new A();
                ap.SetValue(obj, "aa");
            }
            Trace.TraceInformation(a.ElapsedMilliseconds.ToString());
            a.Stop();
            a.Restart();
            var fun = ReflectorUtil.Instance.GetSetPropertyValueFun<A>(typeof(string), "Ap");

            for (int i = 0; i < 100000; i++)
            {
                var obj = new A();
                fun.Invoke(obj, "aa");
            }
            Trace.TraceInformation(a.ElapsedMilliseconds.ToString());
            a.Stop();
        }

        [TestMethod]
        public void TestExpress2()
        {
            Stopwatch a = new Stopwatch();
            a.Start();
            for (int i = 0; i < 100000; i++)
            {
                var obj = new A();
                var apa = obj.Ap;
            }
            Trace.TraceInformation(a.ElapsedMilliseconds.ToString());
            a.Stop();

            a.Restart();
            var ap = typeof(A).GetProperty("Ap");
            for (int i = 0; i < 100000; i++)
            {
                var obj = new A();
                ap.GetValue(obj);
            }
            Trace.TraceInformation(a.ElapsedMilliseconds.ToString());
            a.Stop();

            a.Restart();
            var fun = ReflectorUtil.Instance.GetPropertyValueFun<A, string>("Ap");

            for (int i = 0; i < 100000; i++)
            {
                var obj = new A() { Ap = "aa" };
                var aaa = fun.Invoke(obj);
            }
            Trace.TraceInformation(a.ElapsedMilliseconds.ToString());
            a.Stop();
        }
        public interface IA
        {
            string Ap { get; set; }
            void h();
        }
        public interface IB
        {
            void k();
        }
        public class A : IA
        {
            public string Ap { get; set; } = "1";
            public void h()
            {
                Trace.TraceInformation("A");
            }

        }
        public class A1 : IA
        {
            public string Ap { get; set; }

            public void h()
            {
                Trace.TraceInformation("A1");
            }
        }
        public class B : IB
        {
            public void k()
            {
                Trace.TraceInformation("B");
            }
        }

        public class MyScopePrviderFacory : IServiceScopeFactory
        {
            private readonly IServiceCollection services;

            public MyScopePrviderFacory(IServiceCollection services)
            {
                this.services = services;
            }
            public IServiceScope CreateScope()
            {
                return services.BuildServiceProvider().CreateScope();
            }
            public void MergerService(IServiceCollection services)
            {
                this.services.Concat(services);
            }
        }
    }
}