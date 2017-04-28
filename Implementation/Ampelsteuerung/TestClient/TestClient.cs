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

            try
            {
                test._chatSrv.getAmpelStatus (0);
                //test._chatSrv.setAmpelAusfall(2);
                //test._chatSrv.setAmpelAusfall(1);
                test._chatSrv.setRotPhase(1,10);
                test._chatSrv.getRotPhase(1);
                //test._chatSrv.setAmpelOn(1);
                //test._chatSrv.getAmpelAusfall(0);
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine("Der Server ist nicht gestartet!");
                nre.ToString();
            }
            catch (EndpointNotFoundException enfe)
            {
                Console.WriteLine("Der Server ist nicht gestartet!");
                enfe.ToString();
            }

        }
    }
}
