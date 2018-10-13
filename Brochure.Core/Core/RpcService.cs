using Thrift.Protocol;
using Thrift.Transport;

namespace Brochure.Core.Core
{
    public class RpcService<T> where T : class
    {
        private string _host;
        private int _port;
        private TTransport transport;
        public RpcService (string host, int port)
        {
            _host = host;
            _port = port;
            transport = new TSocket (host, port);
            TProtocol protocol = new TBinaryProtocol (transport);
            Client = ReflectorUtil.CreateInstance<T> (protocol);
        }

        public void Open ()
        {
            if (!transport.IsOpen)
                transport.Open ();
        }

        public void Close ()
        {
            transport.Flush ();
            if (transport.IsOpen)
                transport.Close ();
        }
        public T Client { get; }
    }
}