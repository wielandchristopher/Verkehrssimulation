// (C) Copyright 2013 by Autodesk, Inc. 
//
// Written by Philippe Leefsma, December 2013.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            logger.Text +=
                System.Environment.NewLine +
                DateTime.Now.ToLocalTime().ToString() + ": " +
                msg;
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

                    _host = new ServiceHost(
                       svc,
                       new Uri[] { new Uri("net.pipe://localhost") });

                    _host.AddServiceEndpoint(
                        typeof(IChatService),
                        new NetNamedPipeBinding(), "ADNChatSrv");

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
