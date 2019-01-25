using Brochure.Core.Models;
using Brochure.Core.Server;
using HostServer;
using HostServer.Server;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Brochure.Core.Test.HostServerTest
{
    public class HostServerTest
    {
        private IServerManager serverManager;

        public HostServerTest()
        {
            serverManager = new ServerManager();
            Config.HostServerAddress = "127.0.0.1";
            Config.HostServerPort = 8000;
            Config.AppKey = "aaaa";
        }

        private string host = "127.0.0.1";
        private int port = 8000;

        public class HostService : IHostService.IAsync
        {
            public async Task<HostConfig> GetAddressAsync(string servicekey, CancellationToken cancellationToken)
            {
                await Task.Delay(1);
                return new HostConfig("aa");
            }

            public async Task HealthCheckAsync(string servicekey, string host, int restPort, int rpcPort, CancellationToken cancellationToken)
            {
                await Task.Delay(1);
            }

            public async Task RegistAddressAsync(string servicekey, string host, int restPort, int rpcPort, CancellationToken cancellationToken)
            {
                await Task.Delay(1);
            }

            public async Task<bool> RegistEventTypeAsync(string eventName, string eventSourceKey, CancellationToken cancellationToken)
            {
                await Task.Delay(1);
                return true;
            }

            public async Task<bool> RemoveEventTypeAsync(string eventName, string eventSourceKey, CancellationToken cancellationToken)
            {
                await Task.Delay(1);
                return true;
            }
        }

        [Fact]
        public async void TestServer()
        {
            var subManager = new SubscribeEventManager();
            var rpcServer = new RpcService(port);
            var server = new HostService();
            rpcServer.RegiestPublisheEventService(subManager);
            await rpcServer.StartAsync();
            while (true)
                await Task.Delay(5000);
        }

        [Fact]
        public async Task TestClient()
        {
            Config.HostServerAddress = "127.0.0.1";
            Config.HostServerPort = 8000;
            var client = new RpcClient<IHostService.Client>(Config.HostServerAddress, Config.HostServerPort, HostServer.ServiceKey.HostServiceKey).Client;
            var eventManager = new SubscribeEventManager();
            try
            {
                var a = await client.GetAddressAsync(HostServer.ServiceKey.HostServiceKey, CancelTokenSource.Default.Token);
            }
            catch (System.Exception e)
            {
            }
        }

        [Fact]
        public async void TestEventManager()
        {
            var server = new RpcService(8002);
            var hostClient = new RpcClient<IHostService.Client>(HostApp.AppKey, ServiceKey.HostServiceKey);
            await hostClient.Client.RegistAddressAsync(Config.AppKey, Config.HostServerAddress, -1, 8002, CancelTokenSource.Default.Token);
            var subscribeManager = new SubscribeEventManager();
            server.RegiestPublisheEventService(subscribeManager);
            await subscribeManager.RegistEventAsync(EventType.EventName1, HostApp.AppKey, (a) =>
          {
              var b = 1;
          });

            await server.StartAsync();
            while (true)
                await Task.Delay(5000);
            //注册事件
        }
    }
}