namespace SimpleServer
{
    partial class SimpleServerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleServerForm));
            this.serverControl = new ServerLib.ServerControl();
            this.SuspendLayout();
            // 
            // serverControl
            // 
            this.serverControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverControl.Location = new System.Drawing.Point(0, 0);
            this.serverControl.Name = "serverControl";
            this.serverControl.Size = new System.Drawing.Size(484, 412);
            this.serverControl.TabIndex = 0;
            // 
            // SimpleServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 412);
            this.Controls.Add(this.serverControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SimpleServer";
            this.Text = "ADN WCF Server";
            this.ResumeLayout(false);

        }

        #endregion

        private ServerLib.ServerControl serverControl;

    }
}

