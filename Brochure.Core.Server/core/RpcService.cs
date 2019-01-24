using Brochure.Core.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Thrift;
using Thrift.Protocols;
using Thrift.Server;
using Thrift.Transports;
using Thrift.Transports.Server;

namespace Brochure.Core.Server
{
    public class RpcService
    {
        private int _port;
        private TServerTransport transport;
        private TMultiplexedProcessor multiProcessor;
        private TBaseServer server;
        private ServiceStatus status = ServiceStatus.Stop;
        private ITProtocolFactory jsonFactory;

        public RpcService(int port)
        {
            _port = port;
            multiProcessor = new TMultiplexedProcessor();
            transport = new TServerSocketTransport(_port);
            jsonFactory = new TJsonProtocol.Factory();
            var log = new LoggerFactory();
            server = new AsyncBaseServer(multiProcessor, transport, jsonFactory, jsonFactory, log);
        }

        public void RegisterRpcServer(string serverName, ITAsyncProcessor processor)
        {
            //默认注册一个
            multiProcessor.RegisterProcessor(serverName, processor);
        }

        public async Task StartAsync()
        {
            if (status == ServiceStatus.Start)
                return;
            status = ServiceStatus.Start;
            await server.ServeAsync(CancelTokenSource.Default.Token);
        }

        public void Stop()
        {
            if (status == ServiceStatus.Stop)
                return;
            server?.Stop();
            transport.Close();
            status = ServiceStatus.Stop;
        }

        private enum ServiceStatus
        {
            Start,
            Stop,
        }
    }
}