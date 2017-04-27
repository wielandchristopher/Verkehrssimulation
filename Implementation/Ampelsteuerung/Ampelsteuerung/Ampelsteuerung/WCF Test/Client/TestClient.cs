using SimpleServer;
using System;
using System.ServiceModel;

namespace Client
{
    public partial class TestClient
    {
        IChatService _chatSrv;
        CallbackClient _callback;
        TestClient _client;

        public void StartVerkehrssimulation()
        {
            try
            {
                _callback = new CallbackClient(_client);
                DuplexChannelFactory<IChatService> factory = new DuplexChannelFactory<IChatService>(_callback, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/Ampelsteuerung"));
                _chatSrv = factory.CreateChannel();               
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        [STAThread]
        static void Main()
        {

            TestClient test = new TestClient();
            test.StartVerkehrssimulation();
            //Sende in dieser Reihenfolge für informationen:
            //ID Der Ampel: 0 steht für alle ampeln, 1 - n für eine Spezifische. 
            //getAmpelInformation(ID der Ampel (in String), Ausfall der Ampel (in String))
            test._chatSrv.getAmpelInformation("0", "false");
            //Noch funktion zum ersten erstellen der Ampeln
        }
    }
}
