using Ampelsteuerung;
using CallbackCli;
using System;
using System.ServiceModel;

namespace Client
{
    public partial class TestClient
    {
        IAmpelService _chatSrv;
        CallbackClient _callback;
        TestClient _client;

        public void StartVerkehrssimulation()
        {
            try
            {
                _callback = new CallbackClient(_client);
                DuplexChannelFactory<IAmpelService> factory = new DuplexChannelFactory<IAmpelService>(_callback, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/Ampelsteuerung"));
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
            //getAmpelInformation(ID der Ampel (in int), Ausfall der Ampel (in String))
            try
            {
                test._chatSrv.getAmpelInformation(1, "check");

            }
            catch(NullReferenceException nre)
            {
                Console.WriteLine("Der Server ist nicht gestartet!");
                nre.ToString();
            }

        }
    }
}
