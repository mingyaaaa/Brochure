using AspectCore.Injector;
using Brochure.Core.Server;
using Brochure.DI.AspectCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Brochure.Core.Test.server
{
    public interface IA
    {
        string AA(string str);
    }

    public class A : IA
    {
        public A(string a)
        {
        }

        public string AA(string str)
        {
            return str;
        }
    }

    public interface IB
    {
        string BB(string str);
    }

    public class B : IB
    {
        public string BB(string str)
        {
            return str;
        }
    }

    public interface IC
    {
        void CC();
    }

    public class IIC
    {
        public interface IC
        {
            void CC();
        }
    }

    public class IIC2
    {
        public interface IC
        {
            void CC();
        }
    }

    public class C : IIC.IC
    {
        private SubscribeEventManager _a;

        public C(SubscribeEventManager a)
        {
            _a = a;
        }

        public void CC()
        {
        }
    }

    public class IocTest
    {
        [Fact]
        public void IocScopeTest()
        {
            var serviceCollection = new ServiceCollection();
            var server = new AspectCoreDI(new ServiceCollection());
            {
                server.AddScoped<IB, B>();
                var provider = server.BuildServiceProvider();
                var b1 = provider.GetService<IB>();
                using (var tprovide = provider.CreateScope())
                {
                    var b2 = tprovide.ServiceProvider.GetService<IB>();
                    Assert.NotSame(b1, b2);
                }
                var b3 = provider.GetService<IB>();
                Assert.Same(b1, b3);
            }
            {
                var server1 = new AspectCoreDI(serviceCollection);
                server1.AddSingleton(new SubscribeEventManager());
                server1.AddSingleton<C>();
                var provider = server1.BuildServiceProvider();
                var a = provider.GetService<C>();
            }
        }
    }
}
