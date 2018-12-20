using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace Brochure.Core.Server
{
    public class RpcService
    {
        private int _port;
        private TServerTransport transport;
        private TMultiplexedProcessor multiProcessor;
        private TServer server;
        private ServiceStatus status = ServiceStatus.Stop;
        public RpcService(int port)
        {
            _port = port;
            multiProcessor = new TMultiplexedProcessor();
            transport = new TServerSocket(_port);
            TServer server = new TThreadPoolServer(multiProcessor, transport);
        }
        public void RegisterRpcServer(string serverName, TProcessor processor)
        {
            //默认注册一个
            multiProcessor.RegisterProcessor(serverName, processor);
        }
        public void Start()
        {
            if (status == ServiceStatus.Start)
                return;
            server = server ?? new TThreadPoolServer(multiProcessor, transport);
            server.Serve();
            status = ServiceStatus.Start;
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
