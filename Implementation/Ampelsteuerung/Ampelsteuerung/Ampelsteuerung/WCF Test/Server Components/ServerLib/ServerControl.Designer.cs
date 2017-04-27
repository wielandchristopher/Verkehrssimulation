namespace ServerLib
{
    partial class ServerControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.logger = new System.Windows.Forms.TextBox();
            this.bStartStopServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // logger
            // 
            this.logger.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.logger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logger.Location = new System.Drawing.Point(0, 0);
            this.logger.Multiline = true;
            this.logger.Name = "logger";
            this.logger.ReadOnly = true;
            this.logger.Size = new System.Drawing.Size(150, 127);
            this.logger.TabIndex = 3;
            // 
            // bStartStopServer
            // 
            this.bStartStopServer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bStartStopServer.Location = new System.Drawing.Point(0, 127);
            this.bStartStopServer.Name = "bStartStopServer";
            this.bStartStopServer.Size = new System.Drawing.Size(150, 23);
            this.bStartStopServer.TabIndex = 2;
            this.bStartStopServer.Text = "Start/Stop Server ... ";
            this.bStartStopServer.UseVisualStyleBackColor = true;
            this.bStartStopServer.Click += new System.EventHandler(this.bStartStopServer_Click);
            // 
            // ServerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.logger);
            this.Controls.Add(this.bStartStopServer);
            this.Name = "ServerControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox logger;
        private System.Windows.Forms.Button bStartStopServer;
    }
}
