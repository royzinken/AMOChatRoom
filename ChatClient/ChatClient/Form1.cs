using ServerLib.Impl;
using ServerLib.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        CHATROOM_COMMAND,
        CHATROOM_KICK_USER,
    }
    public partial class Form1 : Form
    {
        private IAsyncSocketClient _client;
        private bool _isLogin;
        private string _sendBuffer;
        private string _endPacket = "\r\n";
        CHATROOMSTATE _chatroomstate;

        loginform mainform = new loginform();
        IAsyncSocketClient _servercon;


        public static string _privateChat;

        public Form1()
        {
            InitializeComponent();

            _servercon = chatserver.ChatServerConn();

            if (!_servercon._isConnected)
            {
                MessageBox.Show("Chatserver not online!");
                System.Environment.Exit(1);
            }

            HeartBeat.RunWorkerAsync();
            CreateClientSocket();
        }
        public void CreateClientSocket()
        {
            if (_servercon._isConnected)
            {
                _servercon._onConnectEvent += new ServerLib.Event.AsyncSocketConnectEventHandler(Client_OnConnect);
                _servercon._onSendEvent += new ServerLib.Event.AsyncSocketSendEventHandler(Client_OnSend);
                _servercon._onReceiveEvent += new ServerLib.Event.AsyncSocketReceiveEventHandler(Client_OnReceive);
                _servercon._onErrorEvent += new ServerLib.Event.AsyncSocketErrorEventHandler(Client_OnError);
                _servercon._onCloseEvent += new ServerLib.Event.AsyncSocketCloseEventHandler(Client_OnClose);
            }
        }
        public void ServerPacketAnalysis(ServerLib.Event.AsyncSocketReceiveEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ServerLib.Event.AsyncSocketReceiveEventArgs>(ServerPacketAnalysis), e);

            }
            else
            {
                _chatroomstate = (CHATROOMSTATE)Int32.Parse(e._receiveMessageList[0]);

                switch (_chatroomstate)
                {
                    case CHATROOMSTATE.LOGIN_SUCCEED:
                        {
                            string[] users = e._receiveMessageList[1].Split(',');

                            foreach(var user in users)
                            {
                                var listViewItem = new ListViewItem(user);

                                if (user == _servercon._clientID)
                                {
                                    //listViewItem.Font = new Font(listViewItem.Font, listViewItem.Font.Style | FontStyle.Bold);
                                    listViewItem.ForeColor = Color.Lime;

                                    chatbox_users.Items.Insert(0,listViewItem);
                                }
                                else
                                {
                                    chatbox_users.Items.Add(listViewItem);
                                }
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
                            chatbox_main.SelectionColor = Color.Green;
                            chatbox_main.AppendText($"[{DateTime.Now.ToString("dddd HH:mm")}] ");
                            chatbox_main.SelectionColor = Color.Blue;
                            chatbox_main.AppendText($"{e._receiveMessageList[1]} \n");
                        }
                        break;
                    case CHATROOMSTATE.CHATROOM_USER_ADD:
                        {
                            chatbox_main.SelectionColor = Color.DarkBlue;
                            chatbox_main.AppendText($"{e._receiveMessageList[1]} Joined the chat!\n");

                            var listViewItem = new ListViewItem(e._receiveMessageList[1]);

                            chatbox_users.Items.Add(listViewItem);
                        }
                        break;
                    case CHATROOMSTATE.CHATROOM_USER_REMOVE:
                        {
                            for (int i = chatbox_users.Items.Count - 1; i >= 0; i--)
                            {
                                if (chatbox_users.Items[i].Text == e._receiveMessageList[1])
                                {
                                    chatbox_main.SelectionColor = Color.Orange;
                                    chatbox_main.AppendText($"{e._receiveMessageList[1]} Left the chat!\n");
                                    chatbox_users.Items[i].Remove();
                                }
                            }
                        }
                        break;
                    case CHATROOMSTATE.CHATROOM_KICK_USER:
                        {
                            chatserver.ChatServerConn().Close();
                            MessageBox.Show("You have been kicked!");
                            System.Environment.Exit(1);
                        }
                        break;
                }
            }
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
            CHATROOMSTATE MESSAGE_TYPE = CHATROOMSTATE.CHATROOM_SEND_MESSAGE;

            if (String.IsNullOrWhiteSpace(chatbox_input.Text))
            {
                chatbox_input.Clear();
                return;
            }


            if (chatbox_input.Text.StartsWith("/"))
                MESSAGE_TYPE = CHATROOMSTATE.CHATROOM_COMMAND;


            _sendBuffer = Convert.ToString((int)MESSAGE_TYPE + _endPacket.ToString() + chatbox_input.Text + _endPacket.ToString());
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
            _servercon.Close();
        }

        private void HeartBeat_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                if (_servercon._isConnected == false)
                {
                    MessageBox.Show("Server disconnected!");
                    System.Environment.Exit(1);
                }
                Thread.Sleep(1000);
            }
        }

        private void chatbox_main_TextChanged(object sender, EventArgs e)
        {
            chatbox_main.SelectionStart = chatbox_main.Text.Length;
            chatbox_main.ScrollToCaret();
        }

        private void chatbox_users_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var senderList = (ListView)sender;

            _privateChat = senderList.HitTest(e.Location).Item.Text;

            privateChatForm frm = new privateChatForm();
            frm.Show();
        }
    }
}
