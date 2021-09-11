using ServerLib.Interface;

namespace ServerLib.Impl
{
    public class AdvancedAsyncSocketServer : DefaultAsyncSocketServer
    {
        public byte[] _endPacket { get; set; }

        public AdvancedAsyncSocketServer(int port, byte[] endPacket)
            : base(port)
        {
            _endPacket = endPacket;
        }

        public override IAsyncSocketClient NewAsyncSocketClient(System.Net.Sockets.Socket client)
        {
            return new AdvancedAsyncSocketClient(client, _endPacket);
        }

    }
}
