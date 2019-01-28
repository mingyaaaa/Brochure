using System.Threading.Tasks;

namespace Brochure.Interface
{
    public interface IHostManager
    {
        Task RegistHostAsync(string appKey, string ip, int restport, int rpcport);

        Task<HostConfiguration> GetHostAsync(string appkey);

        Task HealthCheckAsync(string servicekey, string host, int restPort, int rpcPort);
    }
}
