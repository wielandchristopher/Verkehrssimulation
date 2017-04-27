namespace Client
{
    partial class Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelDown = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.bSendMsg = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.logger = new System.Windows.Forms.RichTextBox();
            this.panelMain.SuspendLayout();
            this.panelDown.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panelDown);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(334, 412);
            this.panelMain.TabIndex = 0;
            // 
            // panelDown
            // 
            this.panelDown.Controls.Add(this.tbMessage);
            this.panelDown.Controls.Add(this.tbUser);
            this.panelDown.Controls.Add(this.label3);
            this.panelDown.Controls.Add(this.label2);
            this.panelDown.Controls.Add(this.label1);
            this.panelDown.Controls.Add(this.bSendMsg);
            this.panelDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDown.Location = new System.Drawing.Point(0, 293);
            this.panelDown.Name = "panelDown";
            this.panelDown.Size = new System.Drawing.Size(334, 119);
            this.panelDown.TabIndex = 0;
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.logger);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(0, 0);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(334, 293);
            this.panelCenter.TabIndex = 1;
            // 
            // bSendMsg
            // 
            this.bSendMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bSendMsg.Enabled = false;
            this.bSendMsg.Location = new System.Drawing.Point(0, 96);
            this.bSendMsg.Name = "bSendMsg";
            this.bSendMsg.Size = new System.Drawing.Size(334, 23);
            this.bSendMsg.TabIndex = 0;
            this.bSendMsg.Text = "Send ...";
            this.bSendMsg.UseVisualStyleBackColor = true;
            this.bSendMsg.Click += new System.EventHandler(this.bSendMsg_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Message:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "User:";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(58, 3);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(286, 20);
            this.tbUser.TabIndex = 4;
            this.tbUser.TextChanged += new System.EventHandler(this.tbUser_TextChanged);
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(58, 26);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(285, 64);
            this.tbMessage.TabIndex = 5;
            this.tbMessage.TextChanged += new System.EventHandler(this.tbMessage_TextChanged);
            // 
            // logger
            // 
            this.logger.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.logger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logger.Location = new System.Drawing.Point(0, 0);
            this.logger.Name = "logger";
            this.logger.ReadOnly = true;
            this.logger.Size = new System.Drawing.Size(334, 293);
            this.logger.TabIndex = 0;
            this.logger.Text = "";
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 412);
            this.Controls.Add(this.panelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Client";
            this.Text = "ADN WCF Client";
            this.panelMain.ResumeLayout(false);
            this.panelDown.ResumeLayout(false);
            this.panelDown.PerformLayout();
            this.panelCenter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panelDown;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bSendMsg;
        private System.Windows.Forms.RichTextBox logger;
    }
}

