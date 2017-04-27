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
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interfaces;

namespace Client
{
    public partial class Client : Form
    {
        IChatService _chatSrv;
        Callback _callback; 

        public Client()
        {
            InitializeComponent();

            try
            {
                _callback = new Callback(this);

                DuplexChannelFactory<IChatService> factory =
                    new DuplexChannelFactory<IChatService>(
                        _callback,
                        new NetNamedPipeBinding(),
                        new EndpointAddress("net.pipe://localhost/ADNChatSrv"));

                _chatSrv = factory.CreateChannel();

                bool res = _chatSrv.Subscribe();
            }
            catch(Exception ex)
            {
                LogErrorMessage(ex.Message);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        { 
            try
            {
                bool res = _chatSrv.Unsubscribe();
            }
            catch
            {

            }
        }

        void LogErrorMessage(string msg)
        {
            LogMessage(
                msg,
                new Font("Microsoft San Serif", 10, FontStyle.Italic), 
                Color.Red);
        }

        void LogUserMessage(string msg)
        {
            LogMessage(
                msg,
                new Font("Microsoft San Serif", 10),
                Color.Black);
        }

        public void LogPeerMessage(string msg)
        {
            LogMessage(
                msg, 
                new Font("Microsoft San Serif", 10, FontStyle.Italic), 
                Color.Blue);
        }

        void LogMessage(string msg, Font font, Color color)
        {
            string newMsg = 
                System.Environment.NewLine +
                DateTime.Now.ToLocalTime().ToString() + " - " +
                msg +
                System.Environment.NewLine;

            logger.AppendText(newMsg, font, color);
        }

        void ClearMessages()
        {
            logger.Text = string.Empty;
        }

        private void tbMessage_TextChanged(object sender, EventArgs e)
        {
            bSendMsg.Enabled = 
                (tbUser.Text.Length != 0) && 
                (tbMessage.Text.Length != 0);
        }

        private void tbUser_TextChanged(object sender, EventArgs e)
        {
            bSendMsg.Enabled =
               (tbUser.Text.Length != 0) &&
               (tbMessage.Text.Length != 0);
        }   

        private void bSendMsg_Click(object sender, EventArgs e)
        {
            LogUserMessage("You say: " + Environment.NewLine + tbMessage.Text);

            try
            {
                string msg = tbMessage.Text;

                tbMessage.Text = string.Empty;

                _chatSrv.SendMessage(tbUser.Text, msg);
            }
            catch (Exception ex)
            {
                LogErrorMessage(ex.Message);
            }
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(
            this RichTextBox box, 
            string text, 
            Font font, 
            Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.SelectionFont = font;

            box.AppendText(text);

            box.SelectionColor = box.ForeColor;
        }
    }
}
