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
        private TrafficHandler th;
        EnvironmentBuilder builder;
        IAmpelService trafficlight;

        //Diese Funktion muss gestartet werden, damit eine Verbindung zum Server aufgebaut werden kann. 
        public void StartAmpelsteuerung()
        {
            try
            {
                DuplexChannelFactory<IAmpelService> factory = new DuplexChannelFactory<IAmpelService>(new CallbackClient(this), new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/Ampelsteuerung"));
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
            StartAmpelsteuerung();

            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Tick += dispatchTimer_Tick;
            dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            // Test mit blinkender Ampel
            dpTimer2 = new DispatcherTimer();
            dpTimer2.Tick += dpTimer2_Tick;
            dpTimer2.Interval = new TimeSpan(0, 0, 0, 0, 300);

            oh = new ObjectHandler(myCanvas);
            ap = new GUI.AmpelHandler(myCanvas);

            builder = new EnvironmentBuilder(myCanvas, ref ap, ref trafficlight);

   
            th = new TrafficHandler(ref builder, ref oh);

            MainAmpelsteuerung(this);

            

            th.createNewVerkehrsteilnehmer(155, 155, 4, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
            th.createNewVerkehrsteilnehmer(35, 155, 4, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
            th.createNewVerkehrsteilnehmer(10, 155, 4, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);

            dispatchTimer.Start();
            dpTimer2.Start();
        }
        [STAThread]
        private void MainAmpelsteuerung(MainWindow mainWindow)
        {
            mainWindow.StartAmpelsteuerung();
            try
            {
                mainWindow.trafficlight.setAmpelAnzahl(5);
                string j = mainWindow.trafficlight.getAmpelStatus(2);
                //int i = mainWindow.trafficlight.getAmpelAnzahl();
                Console.WriteLine(j);
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

        private void dispatchTimer_Tick(object sender, EventArgs e)
        {
            //oh.UpdateAll(); //update aller elemente (um 5 verschieben je nach direction)
            th.updateAll();
        }

        int a = 0;

        private void dpTimer2_Tick(object sender, EventArgs e)
        {
            a++;

            if(a%5 == 0)
            {
                this.builder.alternateLight();
            }

            Console.WriteLine(a);
            this.builder.GetAmpelInfo(4);

            //builder.getStreetType(400, 600);
            //builder.getStreetType(100, 100);
            //builder.getStreetType(200, 100);
            //builder.getStreetType(100, 200);
            //ap.blinky();
        } 
    }
}
