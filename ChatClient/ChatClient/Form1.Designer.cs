namespace ChatClient
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_send = new System.Windows.Forms.Button();
            this.chatbox_input = new System.Windows.Forms.RichTextBox();
            this.chatbox_users = new System.Windows.Forms.ListView();
            this.column_user = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chatbox_main = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_send);
            this.panel1.Controls.Add(this.chatbox_input);
            this.panel1.Controls.Add(this.chatbox_users);
            this.panel1.Controls.Add(this.chatbox_main);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(693, 363);
            this.panel1.TabIndex = 0;
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(544, 301);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(121, 37);
            this.btn_send.TabIndex = 3;
            this.btn_send.Text = "Send Message";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // chatbox_input
            // 
            this.chatbox_input.Location = new System.Drawing.Point(3, 264);
            this.chatbox_input.Name = "chatbox_input";
            this.chatbox_input.Size = new System.Drawing.Size(535, 74);
            this.chatbox_input.TabIndex = 2;
            this.chatbox_input.Text = "";
            this.chatbox_input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chatbox_input_KeyDown);
            // 
            // chatbox_users
            // 
            this.chatbox_users.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_user});
            this.chatbox_users.HideSelection = false;
            this.chatbox_users.Location = new System.Drawing.Point(544, 3);
            this.chatbox_users.Name = "chatbox_users";
            this.chatbox_users.Size = new System.Drawing.Size(121, 255);
            this.chatbox_users.TabIndex = 1;
            this.chatbox_users.UseCompatibleStateImageBehavior = false;
            this.chatbox_users.View = System.Windows.Forms.View.Details;
            // 
            // column_user
            // 
            this.column_user.Text = "Users";
            // 
            // chatbox_main
            // 
            this.chatbox_main.Location = new System.Drawing.Point(0, 0);
            this.chatbox_main.Name = "chatbox_main";
            this.chatbox_main.ReadOnly = true;
            this.chatbox_main.Size = new System.Drawing.Size(538, 258);
            this.chatbox_main.TabIndex = 0;
            this.chatbox_main.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 363);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "AMO Chatroom Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox chatbox_input;
        private System.Windows.Forms.ListView chatbox_users;
        private System.Windows.Forms.RichTextBox chatbox_main;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.ColumnHeader column_user;
    }
}

