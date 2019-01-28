using Brochure.Interface;
using System.Threading.Tasks;

namespace HostManager.Cunsul
{
    public class ConsulHostManager : IHostManager
    {
        public Task<HostConfiguration> GetHostAsync(string appkey)
        {
            throw new System.NotImplementedException();
        }

        public Task HealthCheckAsync(string servicekey, string host, int restPort, int rpcPort)
        {
            throw new System.NotImplementedException();
        }

        public Task RegistHostAsync(string appKey, string ip, int restport, int rpcport)
        {
            throw new System.NotImplementedException();
        }
    }
}
