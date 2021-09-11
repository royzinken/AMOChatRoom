using System.Collections.Generic;
using ServerLib.Interface;

namespace ServerLib.Impl
{
    class DefaultAsyncStateObject : IAsyncStateObject
    {
        public int _bufferSize { get; private set; }
        public int _receivedSize { get; set; }
        public byte[] _receiveBuffer { get; set; }
        //public List<string> _receiveMessageList { get; set; }
        public IAsyncSocketClient _client { get; private set; }

        public DefaultAsyncStateObject(IAsyncSocketClient client)
            : this(client, 1024)
        {
        }

        public DefaultAsyncStateObject(IAsyncSocketClient client, int bufferSize)
        {
            _client = client;
            _bufferSize = bufferSize;
            _receiveBuffer = new byte[bufferSize];
        }
    }
}
