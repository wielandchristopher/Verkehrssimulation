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
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Interfaces;

namespace ServerLib
{
    [ServiceBehavior(
        Name = "ServerSvc",
        InstanceContextMode = InstanceContextMode.Single)] 
    class ServerSvc: IChatService
    {
        Dictionary<string, IChatCallback> _subscribers = null;

        ServerControl _serverCtrl = null;

        public ServerSvc(ServerControl serverCtrl)
        {
            _serverCtrl = serverCtrl;

            _subscribers = 
                new Dictionary<string, IChatCallback>();
        }

        public void SendMessage(string user, string msg)
        {
            OperationContext ctx = OperationContext.Current;

            foreach (var subscriber in _subscribers)
            {
                try
                {
                    if (ctx.SessionId == subscriber.Key)
                        continue;

                    if (null != subscriber.Value)
                    {
                        Thread thread = new Thread(delegate()
                        {
                            subscriber.Value.OnNewMessage(user, msg);
                        });

                        thread.Start();
                    }             
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            _serverCtrl.LogMessage("Message from " + user + ": " + msg);
        }

        public bool Subscribe()
        {
            try
            {
                OperationContext ctx = OperationContext.Current;

                IChatCallback callback =
                    ctx.GetCallbackChannel<IChatCallback>();

                if (!_subscribers.ContainsKey(ctx.SessionId))
                {
                    _subscribers.Add(ctx.SessionId, callback);
                    _serverCtrl.LogMessage("New user connected: " + ctx.SessionId);
                }

                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Unsubscribe()
        {
            try
            {
                OperationContext ctx = OperationContext.Current;

                if (!_subscribers.ContainsKey(ctx.SessionId))
                    return false;

                _subscribers.Remove(ctx.SessionId);

                _serverCtrl.LogMessage("User disconnected. Id:" + ctx.SessionId);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
