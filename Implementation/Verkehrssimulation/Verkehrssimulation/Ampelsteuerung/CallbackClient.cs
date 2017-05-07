using Ampelsteuerung;
using System;
using Verkehrssimulation.AmpelHandler;

namespace CallbackCli
{
    class CallbackClient: IAmpelCallback
    {

        CallbackClient _client;
        private AmpelHandler _client1;

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

        public void OnNewMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
