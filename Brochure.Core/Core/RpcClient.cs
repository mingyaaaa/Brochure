using HostServer.Server;
using System;
using Thrift.Protocol;
using Thrift.Transport;

namespace Brochure.Core.Core
{
    public class RpcClient<T> : IDisposable where T : class
    {
        private string _host;
        private int _port;
        private TTransport transport;
        public RpcClient(string host, int port, string serverName)
        {
            _host = host;
            _port = port;
            transport = new TSocket(host, port);
            TProtocol protocol = new TBinaryProtocol(transport);
            var protocolUserService = new TMultiplexedProtocol(protocol, serverName);
            Client = ReflectorUtil.CreateInstance<T>(protocolUserService);
            Open();
        }
        public RpcClient()
        {
            using (var hostrpc = new RpcClient<IHostService.Client>(Config.HostServerAddress, Config.HostServerPort, HostServer.ServiceKey.Key))
            {
                var hostconfig = hostrpc.Client.GetAddress(HostServer.ServiceKey.Key);
                _host = hostconfig.Host;
                _port = hostconfig.RpcPort;
                transport = new TSocket(_host, _port);
                TProtocol protocol = new TBinaryProtocol(transport);
                var protocolUserService = new TMultiplexedProtocol(protocol, hostconfig.ServerName);
                Client = ReflectorUtil.CreateInstance<T>(protocolUserService);
                Open();
            }
        }

        public void Open()
        {
            if (transport == null)
                return;
            if (!transport.IsOpen)
                transport.Open();
        }

        public void Close()
        {
            if (transport == null)
                return;
            transport.Flush();
            if (transport.IsOpen)
                transport.Close();
        }

        public void Dispose()
        {
            Close();
        }
        ~RpcClient()
        {
            Close();
        }
        public T Client { get; }

    }
}
