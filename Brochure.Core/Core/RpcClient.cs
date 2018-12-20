using HostServer.Server;
using System;
using Thrift.Protocol;
using Thrift.Transport;

namespace Brochure.Core
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
        public RpcClient(string serviceKey, string serviceName)
        {
            if (string.IsNullOrWhiteSpace(Config.HostServerAddress))
                throw new Exception("服务Config服务配置错误,请配置服务注册地址");
            if (Config.HostServerPort == 0)
                throw new Exception("服务Config服务配置错误,请配置服务注册地址");
            using (var hostrpc = new RpcClient<IHostService.Client>(Config.HostServerAddress, Config.HostServerPort, HostServer.ServiceKey.Key))
            {
                try
                {
                    var hostconfig = hostrpc.Client.GetAddress(serviceKey);
                    _host = hostconfig.Host;
                    _port = hostconfig.RpcPort;
                    transport = new TSocket(_host, _port);
                    TProtocol protocol = new TBinaryProtocol(transport);
                    var protocolUserService = new TMultiplexedProtocol(protocol, serviceName);
                    Client = ReflectorUtil.CreateInstance<T>(protocolUserService);
                    Open();
                }
                catch (Exception e)
                {
                    throw new Exception("无法连接到服务,请确认服务是否注册");
                }

            }
        }
        public RpcClient(string serviceKey) : this(serviceKey, serviceKey)
        {

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
            //transport.Flush();
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
