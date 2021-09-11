using System;
using System.Net.Sockets;

using ServerLib.Interface;

namespace ServerLib.Event
{
    /// <summary>
    /// 클라이언트가 서버에 접속, 종료 시 발생하는 이벤트
    /// </summary>
    public delegate void AsyncSocketConnectEventHandler(object sender, AsyncSocketConnectionEventArgs e);
    public delegate void AsyncSocketCloseEventHandler(object sender, AsyncSocketConnectionEventArgs e);

    public class AsyncSocketConnectionEventArgs : EventArgs
    {
        public IAsyncSocketClient _clientSocket { get; private set; }
        public IAsyncSocketServer _serverSocket { get; private set; }

        public AsyncSocketConnectionEventArgs(IAsyncSocketClient client)
        {
            _clientSocket = client;
        }

        public AsyncSocketConnectionEventArgs(IAsyncSocketServer server)
        {
            _serverSocket = server;
        }
    }
}
