using System;
using System.Text;
using System.Linq;
using System.Net.Sockets;
using System.Collections.Generic;

using ServerLib.Interface;

namespace ServerLib.Impl
{
    public class AdvancedAsyncSocketClient : DefaultAsyncSocketClient
    {
        private byte[] _endPacket;

        public AdvancedAsyncSocketClient(byte[] endPacket)
            : base()
        {
            _endPacket = endPacket;
        }

        public AdvancedAsyncSocketClient(Socket socket, byte[] endPacket)
        {
            _clientSocket = socket;
            _endPacket = endPacket;

            string ipPortString = socket.RemoteEndPoint.ToString();
            string[] temp = ipPortString.Split(':');
            _clientIP = temp[0];
            _clientPort = temp[1];

            Receive();
        }

        public override void Send(byte[] sendByte)
        {
            List<byte> list = sendByte.ToList<byte>();
            list.AddRange(_endPacket);

            base.Send(list.ToArray());
        }

        public override void Receive()
        {
            Receive(new AdvancedAsyncStateObject(_endPacket, this));
        }

        public override void Receive(IAsyncStateObject aso)
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

        public override void OnReceiveCallBack(IAsyncResult ar)
        {
            if (!_clientSocket.Connected)
                return;

            IAdvancedAsyncStateObject aso = (IAdvancedAsyncStateObject)ar.AsyncState;
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

            if (aso._haveEndPacket)
            {
                _messageList.Clear();
                List<byte[]> messageList = aso._messageList;

                for (int i = 0; i < messageList.Count; i++)
                {
                    _messageList.Add(Encoding.UTF8.GetString(messageList[i], 0, messageList[i].Length));
                }

                DataReceived(_messageList.Count, _messageList);
                Receive();
            }
            else
            {
                aso.ClearReceiveByte();
                Receive(aso);
            }
        }
    }
}
