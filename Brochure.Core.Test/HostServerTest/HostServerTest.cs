using Brochure.Core.Server;
using EventServer.Server;
using HostServer.Server;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Brochure.Core.Test.HostServerTest
{
    public class HostServerTest
    {
        private IServerManager serverManager;
        public HostServerTest()
        {
            serverManager = new ServerManager();
        }

        private string host = "127.0.0.1";
        private int port = 8000;
        public class HostService : IHostService.Iface, ISubscribeEventService.Iface
        {

            public HostConfig GetAddress(string servicekey)
            {
                return new HostConfig("aa");
            }

            public void RegistAddress(string servicekey, string host, int restPort, int rpcPort)
            {


            }

            public bool RegistEventType(string fullTypeName, string key)
            {
                throw new System.NotImplementedException();
            }

            public bool RemoveEventType(string fullTypeName, string key)
            {
                throw new System.NotImplementedException();
            }
        }
        [Fact]
        public void TestServer()
        {
            serverManager.AddSingleton<IEventManager>(new EventManager("aaa"));
            serverManager.AddSingleton<PublicshEventService>();
            var provider = serverManager.BuildProvider();
            var publicshService = provider.GetService<PublicshEventService>();
            var rpcServer = new RpcService(port);
            var server = new HostService();
            rpcServer.RegisterRpcServer(HostServer.ServiceKey.Key, new IHostService.Processor(server));
            rpcServer.RegisterRpcServer(EventServer.ServiceKey.Key, new IPublishEventService.Processor(publicshService));
            //  rpcServer.Start();
        }
        [Fact]
        public void TestClient()
        {
            Config.HostServerAddress = "localhost";
            Config.HostServerPort = 8888;
            var client = new RpcClient<IHostService.Client>(Config.HostServerAddress, Config.HostServerPort, HostServer.ServiceKey.Key).Client;
            var eventManager = new EventManager("aaa");
            try
            {
                var a = client.GetAddress(HostServer.ServiceKey.Key);
                eventManager.RegistEvent("aa", HostServer.ServiceKey.Key, o => { });
                eventManager.RemoveEvent("bbb", HostServer.ServiceKey.Key);
            }
            catch (System.Exception e)
            {
            }

        }

        [Fact]
        public void TestEventManager()
        {
            serverManager.AddSingleton<IEventManager>(new EventManager("aaa"));
            serverManager.AddSingleton<PublicshEventService>();
            var provider = serverManager.BuildProvider();
            var publicshService = provider.GetService<PublicshEventService>();
            Assert.NotNull(publicshService);
        }

    }
}
