using Ampelsteuerung;
using System;
using Verkehrssimulation.AmpelHandler;
using Verkehrssimulation;

namespace CallbackCli
{
    class CallbackClient: IAmpelCallback
    {

        CallbackClient _client;
        private AmpelHandler _client1;
        private MainWindow client;

        public CallbackClient(AmpelHandler _client1)
        {
            this._client1 = _client1;
        }

        public CallbackClient(CallbackClient client)
        {
            _client = client;
        }

        public CallbackClient()
        {
        }

        public CallbackClient(MainWindow client)
        {
            this.client = client;
        }

        public void OnNewMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
