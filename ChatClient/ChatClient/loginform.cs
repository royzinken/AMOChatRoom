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

        public loginform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            _chatroomstate = CHATROOMSTATE.CHATROOM_LOGIN;

            _sendBuffer = Convert.ToString((int)_chatroomstate + _endPacket.ToString() + txt_username.Text + _endPacket.ToString() + txt_password.Text + _endPacket.ToString());
            chatserver.ChatServerConn().Send(Encoding.UTF8.GetBytes(_sendBuffer));
        }

        private void loginform_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
