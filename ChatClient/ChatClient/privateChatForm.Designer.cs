namespace ChatClient
{
    partial class privateChatForm
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
            this.btn_send = new System.Windows.Forms.Button();
            this.chatbox_input = new System.Windows.Forms.RichTextBox();
            this.chatbox_main = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(553, 332);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(121, 37);
            this.btn_send.TabIndex = 6;
            this.btn_send.Text = "Send Message";
            this.btn_send.UseVisualStyleBackColor = true;
            // 
            // chatbox_input
            // 
            this.chatbox_input.Location = new System.Drawing.Point(12, 295);
            this.chatbox_input.Name = "chatbox_input";
            this.chatbox_input.Size = new System.Drawing.Size(535, 74);
            this.chatbox_input.TabIndex = 5;
            this.chatbox_input.Text = "";
            // 
            // chatbox_main
            // 
            this.chatbox_main.Location = new System.Drawing.Point(12, 12);
            this.chatbox_main.Name = "chatbox_main";
            this.chatbox_main.ReadOnly = true;
            this.chatbox_main.Size = new System.Drawing.Size(675, 258);
            this.chatbox_main.TabIndex = 4;
            this.chatbox_main.Text = "";
            // 
            // privateChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 376);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.chatbox_input);
            this.Controls.Add(this.chatbox_main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "privateChatForm";
            this.Text = "Chat with:";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.RichTextBox chatbox_input;
        private System.Windows.Forms.RichTextBox chatbox_main;
    }
}