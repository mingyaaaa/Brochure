using AspectCore.Injector;
using Brochure.Core.Server;
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
        private IEventManager _a;

        public C(IEventManager a)
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
            var server = new ServerManager();
            {
                server.AddScoped<IB, B>();
                var provider = server.BuildProvider();
                var b1 = provider.GetService<IB>();
                using (var tprovide = provider.CreateScope())
                {
                    var b2 = tprovide.GetService<IB>();
                    Assert.NotSame(b1, b2);
                }
                var b3 = provider.GetService<IB>();
                Assert.Same(b1, b3);
            }
            {
                var server1 = new ServerManager();
                server1.AddSingleton<IEventManager>(new EventManager(HostServer.ServiceKey.EventServiceKey));
                server1.AddSingleton<C>();
                var provider = server1.BuildProvider();
                var a = provider.GetService<C>();
            }
        }
    }
}