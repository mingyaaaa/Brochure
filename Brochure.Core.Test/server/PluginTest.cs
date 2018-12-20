using Brochure.Core.Server;
using AspectCore.Injector;
using Brochure.Core.Server.Implements;
using Xunit;

namespace Brochure.Core.Test.server
{
    public class PluginTest
    {
        [Fact]
        public void TestStart()
        {
            var boot = new ServerBootstrap();
            boot.Start();
        }
    }
}
