namespace Brochure.Core.Server.Extends
{
    public static class IServerManagerExtend
    {
        public static IServerManager ConfigHostAddress(this IServerManager manager, string address, int restPort, int rpcPort)
        {
            Config.HostServerAddress = address;
            Config.HostServerPort = rpcPort;
            return manager;
        }
    }
}
