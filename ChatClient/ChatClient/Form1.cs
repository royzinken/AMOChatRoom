using ServerLib.Impl;
using ServerLib.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
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
    public partial class Form1 : Form
    {
        private IAsyncSocketClient _client;
        private bool _isLogin;
        private string _sendBuffer;
        private string _endPacket = "\r\n";
        CHATROOMSTATE _chatroomstate;

        loginform mainform = new loginform();

        public Form1()
        {
            InitializeComponent();
            CreateClientSocket();
        }
        public void CreateClientSocket()
        {
            chatserver.ChatServerConn()._onConnectEvent += new ServerLib.Event.AsyncSocketConnectEventHandler(Client_OnConnect);
            chatserver.ChatServerConn()._onSendEvent += new ServerLib.Event.AsyncSocketSendEventHandler(Client_OnSend);
            chatserver.ChatServerConn()._onReceiveEvent += new ServerLib.Event.AsyncSocketReceiveEventHandler(Client_OnReceive);
            chatserver.ChatServerConn()._onErrorEvent += new ServerLib.Event.AsyncSocketErrorEventHandler(Client_OnError);
            chatserver.ChatServerConn()._onCloseEvent += new ServerLib.Event.AsyncSocketCloseEventHandler(Client_OnClose);
        }
        public void ServerPacketAnalysis(ServerLib.Event.AsyncSocketReceiveEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ServerLib.Event.AsyncSocketReceiveEventArgs>(ServerPacketAnalysis), e);

            }
            else
            {
                CHATROOMSTATE type = (CHATROOMSTATE)Int32.Parse(e._receiveMessageList[0]);

                switch (type)
                {
                    case CHATROOMSTATE.LOGIN_SUCCEED:
                        {
                            string[] users = e._receiveMessageList[1].Split(',');

                            foreach(var user in users)
                            {
                                var listViewItem = new ListViewItem(user);

                                chatbox_users.Items.Add(listViewItem);
                            }
                            mainform.Hide();
                            this.Show();
                        }
                        break;
                    case CHATROOMSTATE.LOGIN_FAILED:
                        {
                            MessageBox.Show("Login failed!!");
                        }
                        break;
                    case CHATROOMSTATE.CHATROOM_SEND_MESSAGE:
                        {
                            chatbox_main.AppendText(e._receiveMessageList[1] + "\n");
                        }
                        break;
                    case CHATROOMSTATE.CHATROOM_USER_ADD:
                        {
                            var listViewItem = new ListViewItem(e._receiveMessageList[1]);

                            chatbox_users.Items.Add(listViewItem);
                        }
                        break;
                    case CHATROOMSTATE.CHATROOM_USER_REMOVE:
                        {
                            for (int i = chatbox_users.Items.Count - 1; i >= 0; i--)
                            {
                                if (chatbox_users.Items[i].Text == e._receiveMessageList[1])
                                    chatbox_users.Items[i].Remove();

                            }
                        }
                        break;
                }
            }

           
            //string serverMessage = Convert.ToString(e._receiveMessageList[0]);


            //addMessage(serverMessage);
            //this.Dispatcher.Invoke(() =>
            //{
            //    chatbox_main.AppendText(Environment.NewLine + serverMessage);
            //});



            //_errorCode = (ERRORCODES)Int32.Parse(serverMessage);

            //switch (_errorCode)
            //{
            //    case ERRORCODES.LOGIN_SUCCEED:
            //        break;
            //}

        }
        void Client_OnReceive(object sender, ServerLib.Event.AsyncSocketReceiveEventArgs e)
        {
            ServerPacketAnalysis(e);
        }

        void Client_OnClose(object sender, ServerLib.Event.AsyncSocketConnectionEventArgs e)
        {

        }

        void Client_OnError(object sender, ServerLib.Event.AsyncSocketErrorEventArgs e)
        {

        }

        void Client_OnConnect(object sender, ServerLib.Event.AsyncSocketConnectionEventArgs e)
        {

        }

        void Client_OnSend(object sender, ServerLib.Event.AsyncSocketSendEventArgs e)
        {

        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            sendMessage();
        }
        public void sendMessage()
        {
            _sendBuffer = Convert.ToString((int)CHATROOMSTATE.CHATROOM_SEND_MESSAGE + _endPacket.ToString() + chatbox_input.Text + _endPacket.ToString());
            chatserver.ChatServerConn().Send(Encoding.UTF8.GetBytes(_sendBuffer));
            chatbox_input.Clear();
        }
        private void chatbox_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.SuppressKeyPress = (e.KeyData == Keys.Enter);
                sendMessage();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!_isLogin)
            {
                mainform.Show();
                this.Hide();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            chatserver.ChatServerConn().Close();
        }
    }
}
