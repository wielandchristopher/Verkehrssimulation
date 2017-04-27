using SimpleServer;
using System;


namespace Client
{
    class CallbackClient: IChatCallback
    {
        TestClient _client;

        public CallbackClient(TestClient client)
        {
            _client = client;
        }

        public void OnNewMessage(string msg)
        {
            Console.WriteLine(msg + "OnNewMessage");
        }
    }
}
