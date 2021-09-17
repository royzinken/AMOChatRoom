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
    public partial class privateChatForm : Form
    {
        public privateChatForm()
        {
            InitializeComponent();
            this.Text = $"Private chat with: {Form1._privateChat}";

            chatbox_main.SelectionColor = Color.DarkBlue;
            chatbox_main.AppendText($"Private chat started with {Form1._privateChat}\n");
        }
    }
}
