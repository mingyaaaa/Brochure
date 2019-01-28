using Brochure.Interface;
using System;
using System.Net;
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
        private IHostManager _hostManager { get; }

        public RpcClient(string host, int port, string serverName)
        {
            _host = host;
            _port = port;
            transport = new TSocketClientTransport(IPAddress.Parse(host), port);
            TProtocol protocol = new TJsonProtocol(transport);
            var protocolUserService = new TMultiplexedProtocol(protocol, serverName);
            Client = ReflectorUtil.CreateInstance<T>(protocolUserService);
            OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public RpcClient(string serviceAppKey, string serviceName)
        {
            if (string.IsNullOrWhiteSpace(Config.HostServerAddress))
                throw new Exception("服务Config服务配置错误,请配置服务注册地址");
            if (Config.HostServerPort == 0)
                throw new Exception("服务Config服务配置错误,请配置服务注册地址");
            var hostConfig = _hostManager.GetHostAsync(serviceAppKey).ConfigureAwait(false).GetAwaiter().GetResult();
            _host = hostConfig.HostAddress;
            _port = hostConfig.RpcPort;
            transport = new TSocketClientTransport(IPAddress.Parse(_host), _port);
            TProtocol protocol = new TJsonProtocol(transport);
            var protocolUserService = new TMultiplexedProtocol(protocol, serviceName);
            Client = ReflectorUtil.CreateInstance<T>(protocolUserService);
            OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public RpcClient(string serviceAppKey) : this(serviceAppKey, serviceAppKey)
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

        public T Client { get; }
    }
}
