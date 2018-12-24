using Brochure.Core.Server;
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
