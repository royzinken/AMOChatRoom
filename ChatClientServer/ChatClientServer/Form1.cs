using ServerLib.Impl;
using ServerLib.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChatClientServer
{
    enum CHATROOMSTATE
    {
        CHATROOM_LOGIN,
        LOGIN_SUCCEED,
        LOGIN_FAILED,
        CHATROOM_LOGOUT,
        CHATROOM_SEND_MESSAGE,
        CHATROOM_USER_ADD,
        CHATROOM_USER_REMOVE,
    }
    enum LOGTYPES
    {
        INFO,
        WARNING,
        ERROR
    }
    delegate void SetLogCallback(LOGTYPES type, string _ip, string _user, string _messag);
    public partial class Form1 : Form
    {
        CHATROOMSTATE _ChatRoomState;

        private bool _isRunning = false;
        private string _endPacket = "\r\n";
        private IAsyncSocketServer _ChatClientServer;
        private List<string> _SendToClient = new List<string>();
        private string _sendBuffer;
        private List<string> Userlist = new List<string>();
        public Form1()
        {
            InitializeComponent();
            if (!_isRunning)
            {
                _isRunning = true;

                _ChatClientServer = new AdvancedAsyncSocketServer(5010, Encoding.UTF8.GetBytes("\r\n"));

                _ChatClientServer._onAcceptEvent += new ServerLib.Event.AsyncSocketAcceptEventHandler(Server_OnAccept);
                _ChatClientServer._onErrorEvent += new ServerLib.Event.AsyncSocketErrorEventHandler(Server_OnError);
                _ChatClientServer._onCloseEvent += new ServerLib.Event.AsyncSocketCloseEventHandler(Server_OnClose);

                _ChatClientServer._onClientReceiveEvent += new ServerLib.Event.AsyncSocketReceiveEventHandler(Server_OnClientReceive);
                _ChatClientServer._onClientSendEvent += new ServerLib.Event.AsyncSocketSendEventHandler(Server_OnClientSend);
                _ChatClientServer._onClientCloseEvent += new ServerLib.Event.AsyncSocketCloseEventHandler(Server_OnClientClose);
                _ChatClientServer._onClientErrorEvent += new ServerLib.Event.AsyncSocketErrorEventHandler(Server_OnClientError);

                _ChatClientServer.Listen();
                writeLog(LOGTYPES.INFO, "Local", "Server", "Server Started!");
            }

        }
        private void writeLog(LOGTYPES type, string _ip,string _user,string _message)
        {
            ListViewItem listViewItem = new ListViewItem(DateTime.Now.ToString("hh:mm:ss"));

            switch (type)
            {
                case LOGTYPES.INFO:
                    listViewItem.ForeColor = Color.Cyan;
                    break;
                case LOGTYPES.WARNING:
                    listViewItem.ForeColor = Color.Orange;
                    break;
                case LOGTYPES.ERROR:
                    listViewItem.ForeColor = Color.Red;
                    break;
            }
            
           
            listViewItem.SubItems.Add(_ip);
            listViewItem.SubItems.Add(_user);
            listViewItem.SubItems.Add(_message);

            if (listView1.InvokeRequired)
            {
                // Call this same method but append THREAD2 to the text
                Action safeWrite = delegate { writeLog(type, _ip, _user, _message); };
                listView1.Invoke(safeWrite);
            }
            else
                listView1.Items.Add(listViewItem);

        }
        public void ClientPacketAnalysis(object sender, List<string> messageList)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, List<string>>(ClientPacketAnalysis), sender, messageList);
            }
            else
            {
                string clientIP = ((IAsyncSocketClient)sender)._clientIP;
                string clientID = ((IAsyncSocketClient)sender)._clientID;
                string clientName = ((IAsyncSocketClient)sender)._clientName;

                _ChatRoomState = (CHATROOMSTATE)Int32.Parse(messageList[0]);

                switch (_ChatRoomState)
                {
                    case CHATROOMSTATE.CHATROOM_LOGIN:
                        {
                            int _return = LoginCheck(sender, messageList);
                            
                            string user_list = string.Join(",", Userlist.ToArray());

                            _sendBuffer = Convert.ToString((int)_return + _endPacket.ToString() + user_list + _endPacket.ToString());
                            _ChatClientServer.SendToSpecificClientByIP(Encoding.UTF8.GetBytes(_sendBuffer), ((IAsyncSocketClient)sender)._clientIP);
                        }
                        break;
                    case CHATROOMSTATE.CHATROOM_LOGOUT:
                        writeLog(LOGTYPES.INFO, clientIP, clientID, "logged out!");
                        break;
                    case CHATROOMSTATE.CHATROOM_SEND_MESSAGE:
                        {
                            _sendBuffer = Convert.ToString((int)CHATROOMSTATE.CHATROOM_SEND_MESSAGE + _endPacket.ToString() + $"{clientID}: {messageList[1]}");
                            _ChatClientServer.SendToAllClient(Encoding.UTF8.GetBytes(_sendBuffer));
                            writeLog(LOGTYPES.INFO, clientIP, clientID, messageList[1]);
                        }
                        break;
                }
            }
        }
        public int LoginCheck(object sender, List<string> messageList)
        {
            string clientIP = ((IAsyncSocketClient)sender)._clientIP;
            string username = messageList[1];
            string password = messageList[2];

            //if (username == "test" && password == "test2")
            //{
                if (!Userlist.Any(_user => _user == username))
                    Userlist.Add(username);

                _ChatClientServer.SetClientIDandNameByIP(clientIP, username, username);

            _sendBuffer = Convert.ToString((int)CHATROOMSTATE.CHATROOM_USER_ADD + _endPacket.ToString() + username + _endPacket.ToString());
            _ChatClientServer.SendToOtherClient(Encoding.UTF8.GetBytes(_sendBuffer), ((IAsyncSocketClient)sender));
            writeLog(LOGTYPES.INFO, clientIP, username, "logged in!");

            return (int)CHATROOMSTATE.LOGIN_SUCCEED;
            //}
            return (int)CHATROOMSTATE.LOGIN_FAILED;
        }
        void Server_OnAccept(object sender, ServerLib.Event.AsyncSocketAcceptEventArgs e)
        {
            writeLog(LOGTYPES.INFO,e._clientSocket._clientIP, "Client", "Connected");
        }
        void Server_OnError(object sender, ServerLib.Event.AsyncSocketErrorEventArgs e)
        {
            writeLog(LOGTYPES.ERROR, "", "Client Error", e._asyncSocketException.Message);
        }
        void Server_OnClose(object sender, ServerLib.Event.AsyncSocketConnectionEventArgs e)
        {
            writeLog(LOGTYPES.INFO,e._clientSocket._clientIP, "Server ", "Disconnected");
        }
        void Server_OnClientReceive(object sender, ServerLib.Event.AsyncSocketReceiveEventArgs e)
        {
            ClientPacketAnalysis(sender, e._receiveMessageList);
        }
        static void Server_OnClientSend(object sender, ServerLib.Event.AsyncSocketSendEventArgs e)
        {

        }
        void Server_OnClientClose(object sender, ServerLib.Event.AsyncSocketConnectionEventArgs e)
        {
            Userlist.Remove(e._clientSocket._clientID);

            _sendBuffer = Convert.ToString((int)CHATROOMSTATE.CHATROOM_USER_REMOVE + _endPacket.ToString() + $"{e._clientSocket._clientID}");
            _ChatClientServer.SendToAllClient(Encoding.UTF8.GetBytes(_sendBuffer));

            writeLog(LOGTYPES.INFO,((IAsyncSocketClient)sender)._clientIP, e._clientSocket._clientID, "Disconnected");
        }
        void Server_OnClientError(object sender, ServerLib.Event.AsyncSocketErrorEventArgs e)
        {
            writeLog(LOGTYPES.ERROR,"", "Client Error", e._asyncSocketException.Message);
        }
    }
}
