using Brochure.Interface;

namespace Brochure.Core.Server.Extends
{
    public static class IServerManagerExtend
    {
        public static IDIServiceManager ConfigHostAddress(this IDIServiceManager manager, string address, int restPort, int rpcPort)
        {
            Config.HostServerAddress = address;
            Config.HostServerPort = rpcPort;
            return manager;
        }
    }
}
