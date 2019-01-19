using HostServer.Server;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocols;
using Thrift.Transports;
using Thrift.Transports.Client;

namespace Brochure.Core
{
    public class RpcClient<T> : IDisposable where T : TBaseClient
    {
        private string _host;
        private int _port;
        private TClientTransport transport;
        private CancellationTokenSource s = new CancellationTokenSource();

        public RpcClient(string host, int port, string serverName)
        {
            _host = host;
            _port = port;
            transport = new TSocketClientTransport(IPAddress.Parse(host), port);
            TProtocol protocol = new TJsonProtocol(transport);
            var protocolUserService = new TMultiplexedProtocol(protocol, serverName);
            Client = ReflectorUtil.CreateInstance<T>(protocolUserService);
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
                    var hostconfig = hostrpc.Client.GetAddressAsync(serviceKey, s.Token).ConfigureAwait(false).GetAwaiter().GetResult();
                    _host = hostconfig.Host;
                    _port = hostconfig.RpcPort;
                    transport = new TSocketClientTransport(IPAddress.Parse(_host), _port);
                    TProtocol protocol = new TBinaryProtocol(transport);
                    var protocolUserService = new TMultiplexedProtocol(protocol, serviceName);
                    Client = ReflectorUtil.CreateInstance<T>(protocolUserService);
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

        public async Task OpenAsync()
        {
            try
            {
                if (transport == null)
                    return;
                if (!transport.IsOpen)
                    await transport.OpenAsync();
            }
            catch (Exception)
            {
                transport.Close();
            }
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

        private T Client { get; }
    }
}
