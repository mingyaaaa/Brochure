using Brochure.Core.Server.Implements;
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
    public class IocTest
    {
        [Fact]
        public void IocScopeTest()
        {
            var server = new ServerManager();
            {
                var provider = server.BuildProvider();
                server.AddScoped<IA, A>();

                IA a1, a2;
                {
                    a1 = provider.GetService<IA>();
                    a2 = provider.GetService<IA>();
                }
                Assert.Same(a1, a2);
            }
            {
                var provider = server.BuildProvider();
                server.AddTransient<IB, B>();
                var b1 = provider.GetService<IB>();
                var b2 = provider.GetService<IB>();
                Assert.NotSame(b1, b2);
            }
        }
    }
}
