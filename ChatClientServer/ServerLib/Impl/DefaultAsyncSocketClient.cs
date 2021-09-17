using System;
using System.Net;
using System.Net.Sockets;
using ServerLib.Interface;
using System.Collections.Generic;

namespace ServerLib.Impl
{
    public class DefaultAsyncSocketClient : IAsyncSocketClient
    {
        public Socket _clientSocket { get; set; }
        public string _clientID { get; set; }
        public string _clientName { get; set; }
        public string _clientIP { get; set; }
        public string _clientPort { get; set; }
        public bool _isConnected { get { return (_clientSocket == null) ? false :_clientSocket.Connected; } }
        public List<string> _messageList = new List<string>();

        public event ServerLib.Event.AsyncSocketConnectEventHandler _onConnectEvent;
        public event ServerLib.Event.AsyncSocketReceiveEventHandler _onReceiveEvent;
        public event ServerLib.Event.AsyncSocketSendEventHandler _onSendEvent;
        public event ServerLib.Event.AsyncSocketCloseEventHandler _onCloseEvent;
        public event ServerLib.Event.AsyncSocketErrorEventHandler _onErrorEvent;

        public DefaultAsyncSocketClient()
        {
        }

        public DefaultAsyncSocketClient(Socket socket)
        {
            _clientSocket = socket;

            string ipPortString = socket.RemoteEndPoint.ToString();
            string[] temp = ipPortString.Split(':');
            _clientIP = temp[0];
            _clientPort = temp[1];

            _clientID = "-";
            _clientName = "-";

            Receive();
        }

        public void Connect(string hostIPAddress, int port)
        {
            IPAddress[] ips = Dns.GetHostAddresses(hostIPAddress); /// IP반환
            IPEndPoint remoteEP = new IPEndPoint(ips[0], port);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client.BeginConnect(remoteEP, new AsyncCallback(OnConnectCallback), client);
            }
            catch (SocketException e)
            {
                ErrorOcured(e);
                return;
            }
        }

        public virtual void OnConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                client.EndConnect(ar);
            }
            catch (SocketException e)
            {
                ErrorOcured(e);
                return;
            }

            _clientSocket = client;

            Connected(this);
            Receive();
        }

        public virtual void Receive()
        {
            Receive(new DefaultAsyncStateObject(this));
        }

        public virtual void Receive(IAsyncStateObject aso)
        {
            try
            {
                _clientSocket.BeginReceive(aso._receiveBuffer, 0, aso._bufferSize, SocketFlags.None, new AsyncCallback(OnReceiveCallBack), aso);
            }
            catch (SocketException e)
            {
                ErrorOcured(e);
                return;
            }
        }

        public virtual void OnReceiveCallBack(IAsyncResult ar)
        {
            if (!_clientSocket.Connected)
                return;

            IAsyncStateObject aso = (IAsyncStateObject)ar.AsyncState;
            Socket clientSocket = aso._client._clientSocket;

            try
            {
                aso._receivedSize = clientSocket.EndReceive(ar);
            }
            catch (SocketException e)
            {
                ErrorOcured(e);
                return;
            }

            if (aso._receivedSize <= 0)
            {
                ErrorOcured(new SocketException(10064));
                return;
            }

            //DataReceived(aso._receivedSize, aso._receiveMessageList);
            Receive();
        }

        public virtual void Send(byte[] sendByte)
        {
            if (!_clientSocket.Connected)
                return;

            try
            {
                _clientSocket.BeginSend(sendByte, 0, sendByte.Length, SocketFlags.None, new AsyncCallback(OnSendCallBack), _clientSocket);
            }
            catch (SocketException e)
            {
                ErrorOcured(e);
                return;
            }
        }

        public virtual void OnSendCallBack(IAsyncResult ar)
        {
            if (!_clientSocket.Connected)
                return;

            Socket clientSocket = (Socket)ar.AsyncState;
            int sendByte = -1;

            try
            {
                sendByte = clientSocket.EndSend(ar);
            }
            catch (SocketException e)
            {
                ErrorOcured(e);
                return;
            }

            Sent(sendByte);
        }

        public void Close()
        {
            if (_clientSocket == null)// || !_clientSocket.Connected)
                return;

            _clientSocket.Shutdown(SocketShutdown.Both);
            _clientSocket.Disconnect(false);

            Closed();
        }


        public virtual void Connected(IAsyncSocketClient client)
        {
            if (_onConnectEvent != null)
                _onConnectEvent(this, new ServerLib.Event.AsyncSocketConnectionEventArgs(client));
        }

        public virtual void ErrorOcured(Exception e)
        {
            if (_onErrorEvent != null)
                _onErrorEvent(this, new ServerLib.Event.AsyncSocketErrorEventArgs(e));

            if (e is SocketException)
                Close();
        }

        public virtual void DataReceived(int receiveByteSize, List<string> receiveMessageList)
        {
            if (_onReceiveEvent != null)
                _onReceiveEvent(this, new ServerLib.Event.AsyncSocketReceiveEventArgs(receiveByteSize, receiveMessageList));
        }

        public virtual void Sent(int sendByteSize)
        {
            if (_onSendEvent != null)
                _onSendEvent(this, new ServerLib.Event.AsyncSocketSendEventArgs(sendByteSize));
        }

        public virtual void Closed()
        {
            if (_onCloseEvent != null)
                _onCloseEvent(this, new ServerLib.Event.AsyncSocketConnectionEventArgs(this));
        }
    }
}
