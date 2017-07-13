using Ampelsteuerung;
using System;
using Verkehrssimulation;

namespace CallbackCli
{
    class CallbackClient: IAmpelCallback
    {
        private MainWindow client;

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
