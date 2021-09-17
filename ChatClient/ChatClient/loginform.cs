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
using System.Windows.Forms;

namespace ChatClient
{
    public partial class loginform : Form
    {
        CHATROOMSTATE _chatroomstate;

        private string _sendBuffer;
        private string _endPacket = "\r\n";
        IAsyncSocketClient _servercon = chatserver.ChatServerConn();

        public loginform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_username.Text.Contains(" "))
                return;

            _servercon._clientID = txt_username.Text;
            _chatroomstate = CHATROOMSTATE.CHATROOM_LOGIN;

            _sendBuffer = Convert.ToString((int)_chatroomstate + _endPacket.ToString() + txt_username.Text + _endPacket.ToString() + txt_password.Text + _endPacket.ToString());
            _servercon.Send(Encoding.UTF8.GetBytes(_sendBuffer));
        }

        private void loginform_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void txt_username_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void txt_password_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }
    }
}
