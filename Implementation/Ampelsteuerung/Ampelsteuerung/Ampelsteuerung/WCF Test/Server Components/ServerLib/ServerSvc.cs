using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using Interfaces;

namespace ServerLib
{
    [ServiceBehavior(Name = "ServerSvc", InstanceContextMode = InstanceContextMode.Single)] 
    class ServerSvc: IChatService
    {
        Dictionary<string, IChatCallback> _subscribers = null;
        ServerControl _serverCtrl = null;

        public ServerSvc(ServerControl serverCtrl){
            _serverCtrl = serverCtrl;
            _subscribers = new Dictionary<string, IChatCallback>();
        }
        
        public void SendMessage(string msg){

            OperationContext ctx = OperationContext.Current;

            foreach (var subscriber in _subscribers){
                try{
                    if (ctx.SessionId == subscriber.Key)
                        continue;

                    if (null != subscriber.Value){
                        Thread thread = new Thread(delegate(){subscriber.Value.OnNewMessage(msg);});
                        thread.Start();
                    }             
                }
                catch (Exception ex){
                    continue;
                }
            }
            _serverCtrl.LogMessage("Message: " + msg);
        }

        public bool Subscribe(){
            try{
                OperationContext ctx = OperationContext.Current;

                IChatCallback callback = ctx.GetCallbackChannel<IChatCallback>();

                if (!_subscribers.ContainsKey(ctx.SessionId)){
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

        public bool Unsubscribe(){
            try{
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
