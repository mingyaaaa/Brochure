using Brochure.Core.Server;
using EventServer.Server;
using HostServer.Server;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Brochure.Core.Test.HostServerTest
{
    public class HostServerTest
    {
        private IServerManager serverManager;
        private CancellationTokenSource s = new CancellationTokenSource();

        public HostServerTest()
        {
            serverManager = new ServerManager();
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
            serverManager.AddSingleton<IEventManager>(new EventManager("aaa"));
            serverManager.AddSingleton<PublicshEventService>();
            var provider = serverManager.BuildProvider();
            var publicshService = provider.GetService<PublicshEventService>();
            var rpcServer = new RpcService(port);
            var server = new HostService();
            rpcServer.RegisterRpcServerAsync(HostServer.ServiceKey.Key, new IHostService.AsyncProcessor(server));
            rpcServer.RegisterRpcServerAsync(EventServer.ServiceKey.Key, new IPublishEventService.AsyncProcessor(publicshService));
            await rpcServer.StartAsync();
        }

        [Fact]
        public async Task TestClient()
        {
            Config.HostServerAddress = "127.0.0.1";
            Config.HostServerPort = 8000;
            var client = new RpcClient<IHostService.Client>(Config.HostServerAddress, Config.HostServerPort, HostServer.ServiceKey.Key).Client;
            var eventManager = new EventManager("aaa");
            try
            {
                var a = await client.GetAddressAsync(HostServer.ServiceKey.Key, s.Token);
                await eventManager.RegistEventAsync("aa", HostServer.ServiceKey.Key, o =>
                 {
                     int aa = 1;
                 });
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
