using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace Brochure.Core.Core
{
    public class RpcService
    {
        private int _port;
        private TServerTransport transport;
        private TMultiplexedProcessor multiProcessor;
        private TServer server;
        public RpcService(int port)
        {
            _port = port;
            multiProcessor = new TMultiplexedProcessor();
            transport = new TServerSocket(_port);
            TServer server = new TThreadPoolServer(multiProcessor, transport);
        }
        public void RegisterPrcServer(string serverName, TProcessor processor)
        {
            multiProcessor.RegisterProcessor(serverName, processor);
        }
        public void Start()
        {
            server = server ?? new TThreadPoolServer(multiProcessor, transport);
            server.Serve();
        }

        public void Stop()
        {
            server?.Stop();
        }
    }
}
