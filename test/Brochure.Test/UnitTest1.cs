using System.Diagnostics;
using System.Linq;
using Brochure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Brochure.Test
{
    /// <summary>
    /// The unit test1.
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Tests the method1.
        /// </summary>
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

        /// <summary>
        /// Mies the test method.
        /// </summary>
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

        /// <summary>
        /// Test1S the.
        /// </summary>
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
        /// <summary>
        /// Test3S the.
        /// </summary>
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
        /// <summary>
        /// Tests the express.
        /// </summary>
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

        /// <summary>
        /// Tests the express2.
        /// </summary>
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
        /// <summary>
        /// The a.
        /// </summary>
        public interface IA
        {
            /// <summary>
            /// Gets or sets the ap.
            /// </summary>
            string Ap { get; set; }
            /// <summary>
            /// hs the.
            /// </summary>
            void h();
        }
        /// <summary>
        /// The b.
        /// </summary>
        public interface IB
        {
            /// <summary>
            /// ks the.
            /// </summary>
            void k();
        }
        /// <summary>
        /// The a.
        /// </summary>
        public class A : IA
        {
            /// <summary>
            /// Gets or sets the ap.
            /// </summary>
            public string Ap { get; set; } = "1";
            /// <summary>
            /// hs the.
            /// </summary>
            public void h()
            {
                Trace.TraceInformation("A");
            }

        }
        /// <summary>
        /// The a1.
        /// </summary>
        public class A1 : IA
        {
            /// <summary>
            /// Gets or sets the ap.
            /// </summary>
            public string Ap { get; set; }

            /// <summary>
            /// hs the.
            /// </summary>
            public void h()
            {
                Trace.TraceInformation("A1");
            }
        }
        /// <summary>
        /// The b.
        /// </summary>
        public class B : IB
        {
            /// <summary>
            /// ks the.
            /// </summary>
            public void k()
            {
                Trace.TraceInformation("B");
            }
        }

        /// <summary>
        /// The my scope prvider facory.
        /// </summary>
        public class MyScopePrviderFacory : IServiceScopeFactory
        {
            private readonly IServiceCollection services;

            /// <summary>
            /// Initializes a new instance of the <see cref="MyScopePrviderFacory"/> class.
            /// </summary>
            /// <param name="services">The services.</param>
            public MyScopePrviderFacory(IServiceCollection services)
            {
                this.services = services;
            }
            /// <summary>
            /// Creates the scope.
            /// </summary>
            /// <returns>An IServiceScope.</returns>
            public IServiceScope CreateScope()
            {
                return services.BuildServiceProvider().CreateScope();
            }
            /// <summary>
            /// Mergers the service.
            /// </summary>
            /// <param name="services">The services.</param>
            public void MergerService(IServiceCollection services)
            {
                this.services.Concat(services);
            }
        }
    }
}