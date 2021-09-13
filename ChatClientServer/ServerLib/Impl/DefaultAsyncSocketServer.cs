using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using ServerLib.Interface;

namespace ServerLib.Impl
{
    public class DefaultAsyncSocketServer : IAsyncSocketServer
    {
        /// <summary>
        /// 비동기 연결의 경우 Close시 CallBack이벤트가 발생하는 것을 막기 위해 사용
        /// </summary>
        private bool _isRunning = false;
        public Socket _serverSocket { get; private set; }
        public int _bindingPort { get; private set; }
        public List<IAsyncSocketClient> _clientList { get; private set; }

        public event ServerLib.Event.AsyncSocketAcceptEventHandler _onAcceptEvent;
        public event ServerLib.Event.AsyncSocketErrorEventHandler _onErrorEvent;
        public event ServerLib.Event.AsyncSocketCloseEventHandler _onCloseEvent;

        public event ServerLib.Event.AsyncSocketReceiveEventHandler _onClientReceiveEvent;
        public event ServerLib.Event.AsyncSocketSendEventHandler _onClientSendEvent;
        public event ServerLib.Event.AsyncSocketCloseEventHandler _onClientCloseEvent;
        public event ServerLib.Event.AsyncSocketErrorEventHandler _onClientErrorEvent;

        public DefaultAsyncSocketServer(int port)
        {
            _bindingPort = port;
            _clientList = new List<IAsyncSocketClient>();
        }

        public virtual void BeginAccept()
        {
            try
            {
                _serverSocket.BeginAccept(new AsyncCallback(OnAcceptCallBack), _serverSocket);
            }
            catch (System.Exception ex)
            {
                ErrorOcured(ex);
                return;
            }
        }

        public virtual void OnAcceptCallBack(IAsyncResult ar)
        {
            if (!_isRunning)
                return;

            Socket serverSocket = (Socket)ar.AsyncState;
            Socket client;

            try
            {
                client = serverSocket.EndAccept(ar);
            }
            catch (SocketException e)
            {
                ErrorOcured(e);
                return;
            }

            AcceptClient(client);
            BeginAccept();
        }

        public virtual void AcceptClient(Socket client)
        {
            IAsyncSocketClient asyncClient = NewAsyncSocketClient(client);
            asyncClient._onReceiveEvent += new ServerLib.Event.AsyncSocketReceiveEventHandler(AsyncClientOnReceive);
            asyncClient._onSendEvent += new ServerLib.Event.AsyncSocketSendEventHandler(AsyncClientOnSend);
            asyncClient._onCloseEvent += new ServerLib.Event.AsyncSocketCloseEventHandler(AsyncClientOnClose);
            asyncClient._onErrorEvent += new ServerLib.Event.AsyncSocketErrorEventHandler(AsyncClientOnError);

            _clientList.Add(asyncClient);
            Accepted(asyncClient);
        }

        public virtual IAsyncSocketClient NewAsyncSocketClient(Socket client)
        {
            return new DefaultAsyncSocketClient(client);
        }

        void AsyncClientOnError(object sender, ServerLib.Event.AsyncSocketErrorEventArgs e)
        {
            if (_onClientErrorEvent != null && _isRunning)
                _onClientErrorEvent(sender, e);

            if (e._asyncSocketException is SocketException && _isRunning)
                ((IAsyncSocketClient)sender).Close();
        }

        void AsyncClientOnClose(object sender, ServerLib.Event.AsyncSocketConnectionEventArgs e)
        {
            if (_onClientCloseEvent != null && _isRunning)
            {
                _clientList.Remove(e._clientSocket);
                _onClientCloseEvent(sender, e);
            }
        }

        void AsyncClientOnSend(object sender, ServerLib.Event.AsyncSocketSendEventArgs e)
        {
            if (_onClientSendEvent != null && _isRunning)
                _onClientSendEvent(sender, e);
        }

        void AsyncClientOnReceive(object sender, ServerLib.Event.AsyncSocketReceiveEventArgs e)
        {
            if (_onClientReceiveEvent != null && _isRunning)
                _onClientReceiveEvent(sender, e);
        }


        public IAsyncSocketClient GetClientByID(string id)
        {
            foreach (IAsyncSocketClient client in _clientList)
            {
                if (client._clientID == id)
                    return client;
            }

            return null;
        }

        public void SetClientIDandNameByIP(string ip,string port, string id, string name)
        {
            foreach (IAsyncSocketClient client in _clientList)
            {
                if (client._clientIP == ip && client._clientPort == port)
                {
                    client._clientID = id;
                    client._clientName = name;
                }
            }
        }

        public void Listen()
        {
            if (_isRunning)
                return;

            _isRunning = true;

            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _bindingPort));
            _serverSocket.Listen(100);

            BeginAccept();
        }

        public void Close()
        {
            _isRunning = false;
            _serverSocket.Close();

            foreach (IAsyncSocketClient client in _clientList)
                client.Close();

            _clientList.Clear();
            Closed();
        }

        public void SendToAllClient(byte[] sendByte)
        {
            foreach (IAsyncSocketClient client in _clientList)
            {
                client.Send(sendByte);
            }
        }

        public void SendToSpecificClientByID(byte[] sendByte, string id)
        {
            foreach (IAsyncSocketClient client in _clientList)
            {
                if (client._clientID == id)
                {
                    client.Send(sendByte);
                    break;
                }
            }
        }

        public void SendToSpecificClientByIP(byte[] sendByte, string ip, string port)
        {
            foreach (IAsyncSocketClient client in _clientList)
            {
                if (client._clientIP == ip && client._clientPort == port)
                {
                    client.Send(sendByte);
                    break;
                }
            }
        }

        public void SendToOtherClient(byte[] sendByte, IAsyncSocketClient myClient)
        {
            foreach (IAsyncSocketClient client in _clientList)
            {
                if (myClient != client)
                    client.Send(sendByte);
            }
        }


        public virtual void Accepted(IAsyncSocketClient client)
        {
            if (_onAcceptEvent != null)
                _onAcceptEvent(this, new ServerLib.Event.AsyncSocketAcceptEventArgs(client));
        }

        public virtual void ErrorOcured(Exception e)
        {
            if (_onErrorEvent != null)
                _onErrorEvent(this, new ServerLib.Event.AsyncSocketErrorEventArgs(e));
        }

        public virtual void Closed()
        {
            if (_onCloseEvent != null)
                _onCloseEvent(this, new ServerLib.Event.AsyncSocketConnectionEventArgs(this));
        }
    }
}
