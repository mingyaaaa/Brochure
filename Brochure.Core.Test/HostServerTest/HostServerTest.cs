using Brochure.Core.Server;
using HostServer.Server;
using Xunit;

namespace Brochure.Core.Test.HostServerTest
{
    public class HostServerTest
    {
        private string host = "127.0.0.1";
        private int port = 8000;
        public class HostService : IHostService.Iface
        {

            public HostConfig GetAddress(string servicekey)
            {
                return new HostConfig("aa");
            }

            public void RegistAddress(string servicekey, string host, int restPort, int rpcPort)
            {


            }
        }
        [Fact]
        public void TestServer()
        {
            var rpcServer = new RpcService(port);
            var server = new HostService();
            //foreach (var item in processorsServerDic.Keys)
            //{
            rpcServer.RegisterPrcServer(HostServer.ServiceKey.Key, new IHostService.Processor(server));
            rpcServer.Start();

            // }



        }
        [Fact]
        public void TestClient()
        {
            host = "10.0.0.5";
            var client = new RpcClient<IHostService.Client>(host, port, HostServer.ServiceKey.Key).Client;
            try
            {
                var a = client.GetAddress(HostServer.ServiceKey.Key);
            }
            catch (System.Exception e)
            {
            }

        }
    }
}
