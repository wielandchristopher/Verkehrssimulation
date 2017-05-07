using System;
using System.Windows;
using System.Windows.Threading;
using Verkehrssimulation.GUI;
using Verkehrssimulation.Verkehrsnetz;
using Verkehrssimulation.Verkehrsteilnehmer;
using Ampelsteuerung;
using CallbackCli;
using System.ServiceModel;

namespace Verkehrssimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dispatchTimer, dpTimer2;
        private ObjectHandler oh;
        private GUI.AmpelHandler ap;
        AmpelHandler.AmpelHandler extAH = new AmpelHandler.AmpelHandler();
        IAmpelService trafficlight;
        CallbackClient _callback;
        MainWindow client;

        //Diese Funktion muss gestartet werden, damit eine Verbindung zum Server aufgebaut werden kann. 
        public void StartAmpelsteuerung()
        {
            try
            {
                _callback = new CallbackClient(client);
                DuplexChannelFactory<IAmpelService> factory = new DuplexChannelFactory<IAmpelService>(_callback, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/Ampelsteuerung"));
                trafficlight = factory.CreateChannel();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Tick += dispatchTimer_Tick;
            dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            // Test mit blinkender Ampel
            dpTimer2 = new DispatcherTimer();
            dpTimer2.Tick += dpTimer2_Tick;
            dpTimer2.Interval = new TimeSpan(0, 0, 0, 0, 1000);

            oh = new ObjectHandler(myCanvas);
            ap = new GUI.AmpelHandler(myCanvas);

            EnvironmentHandler envhandler = new EnvironmentHandler();
            TrafficHandler traffichandler = new TrafficHandler(ref envhandler, ref oh);

            StartAmpelsteuerung();
            try
            {
                trafficlight.setAmpelAnzahl(5);                  
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

            EnvironmentBuilder builder = new EnvironmentBuilder(myCanvas, ref ap, ref extAH);

            dispatchTimer.Start();
            dpTimer2.Start();

        }

        private void dispatchTimer_Tick(object sender, EventArgs e)
        {
            //oh.UpdateAll(); //update aller elemente (um 5 verschieben je nach direction)
        }

        private void dpTimer2_Tick(object sender, EventArgs e)
        {
            //ap.blinky();
        } 
    }
}
