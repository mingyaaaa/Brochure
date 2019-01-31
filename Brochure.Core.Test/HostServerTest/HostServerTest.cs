using Brochure.Core.Models;
using Brochure.Core.Server;
using Brochure.DI.AspectCore;
using Brochure.Interface;
using HostServer;
using HostServer.Server;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Brochure.Core.Test.HostServerTest
{
    public class HostServerTest
    {
        private IDIServiceManager serverManager;

        public HostServerTest()
        {
            serverManager = new AspectCoreDI(new ServiceCollection());
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
            var rpcClient = new RpcClient<IHostService.Client>(Config.HostServerAddress, Config.HostServerPort, HostServer.ServiceKey.HostServiceKey);
            var eventManager = new SubscribeEventManager();
            var t = 0;
            try
            {
                while (t < 10000000)
                {
                    var a = await rpcClient.Client.GetAddressAsync(HostApp.AppKey, CancelTokenSource.Default.Token);
                    await rpcClient.Client.RegistAddressAsync(Config.AppKey, Config.HostServerAddress, -1, Config.HostServerPort, CancelTokenSource.Default.Token);
                    await rpcClient.Client.HealthCheckAsync(Config.AppKey, Config.HostServerAddress, -1, Config.HostServerPort, CancelTokenSource.Default.Token);
                    await Task.Delay(1000);
                    t++;
                }
            }
            catch (System.Exception e)
            {
            }
        }

        [Fact]
        public async void TestEventManager()
        {
            serverManager.AddTransient<IHostManager, HostManager>();
            serverManager.AddTransient<HostManagerProvider>();
            DI.ServerManager = serverManager;

            DI.ServiceProvider = serverManager.BuildServiceProvider();
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
    public class S
    {
        public S()
        {
            B = ReflectorUtil.CreateInstance<B.BB>();
        }
        public string a = "1";
        public B.BB B;
        ~S()
        {
            Console.Write("释放S");
        }
    }
    public class B
    {
        public class BB : IDisposable
        {
            public void Dispose()
            {
            }

            public async Task WriteS()
            {
                await Task.Run(() =>
                {
                    Console.Write("S");
                });

            }
        }

    }
}
