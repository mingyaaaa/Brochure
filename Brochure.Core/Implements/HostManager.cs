using System.Threading.Tasks;
using Brochure.Core.Models;
using Brochure.Interface;

namespace Brochure.Core
{
    public class HostManager : IHostManager
    {
        //  public RpcClient<IHostService.Client> client;

        public HostManager ()
        {
            // client = new RpcClient<IHostService.Client>(Config.HostServerAddress, Config.HostServerPort, HostServer.ServiceKey.HostServiceKey);
        }

        public async Task<HostConfiguration> GetHostAsync (string appkey)
        {
            // await client.OpenAsync();
            // var hostconfig = await client.Client.GetAddressAsync(appkey, CancelTokenSource.Default.Token);
            // client.Close();
            return new HostConfiguration
            {
                // HostAddress = hostconfig.Host,
                // RestPort = hostconfig.RestPort,
                // RpcPort = hostconfig.RpcPort,
            };
        }

        public async Task HealthCheckAsync (string servicekey, string host, int restPort, int rpcPort)
        {
            // await client.OpenAsync();
            // await client.Client.HealthCheckAsync(servicekey, host, restPort, rpcPort, CancelTokenSource.Default.Token);
            // client.Close();
        }

        public async Task RegistHostAsync (string appKey, string ip, int restport, int rpcport)
        {
            //await client.Client.RegistAddressAsync(appKey, ip, restport, rpcport, CancelTokenSource.Default.Token);
        }
    }
}