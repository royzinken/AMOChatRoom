using System;
using System.Net.Sockets;

using ServerLib.Interface;

namespace ServerLib.Event
{
    public delegate void AsyncSocketAcceptEventHandler(object sender, AsyncSocketAcceptEventArgs e);
    
    public class AsyncSocketAcceptEventArgs : EventArgs
    {
        public IAsyncSocketClient _clientSocket { get; set; }

        public AsyncSocketAcceptEventArgs(IAsyncSocketClient client)
        {
            _clientSocket = client;
        }
    }
}
