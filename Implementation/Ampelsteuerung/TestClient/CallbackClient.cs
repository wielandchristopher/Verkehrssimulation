using Ampelsteuerung;
using System;
using Client;

namespace CallbackCli
{
    class CallbackClient: IAmpelCallback
    {
        CallbackClient _client;
        private TestClient _client1;

        public CallbackClient(TestClient _client1)
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
