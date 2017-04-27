using System;
using System.Windows.Forms;
using System.ServiceModel;
using Interfaces;

namespace ServerLib
{
    public partial class ServerControl : UserControl
    {
        ServiceHost _host;
        bool _serverRunning = false;

        public ServerControl()
        {
            InitializeComponent();
        }

        public void LogMessage(string msg)
        {
            logger.Text += System.Environment.NewLine + DateTime.Now.ToLocalTime().ToString() + ": " + msg;
        }

        void ClearMessages()
        {
            logger.Text = string.Empty;
        }

        private void bStartStopServer_Click(object sender, EventArgs e)
        {
            try
            {
                _serverRunning = !_serverRunning;

                if (_serverRunning)
                {
                    ServerSvc svc = new ServerSvc(this);

                    _host = new ServiceHost(svc, new Uri[] { new Uri("net.pipe://localhost") });

                    _host.AddServiceEndpoint(typeof(IChatService), new NetNamedPipeBinding(), "Ampelsteuerung");
                    _host.Open();

                    ClearMessages();
                    LogMessage("Server started ...");
                }
                else
                {
                    _host.Close();
                    _host = null;
                    LogMessage("Server stopped");
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message);
            }
        }
    }
}
