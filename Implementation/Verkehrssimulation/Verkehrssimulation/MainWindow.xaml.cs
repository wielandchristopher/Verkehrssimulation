using System;
using System.Windows;
using System.Windows.Threading;
using Verkehrssimulation.GUI;
using Verkehrssimulation.Verkehrsnetz;
using Verkehrssimulation.Verkehrsteilnehmer;
using Ampelsteuerung;
using CallbackCli;
using System.ServiceModel;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace Verkehrssimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dispatchTimer;
        private ObjectHandler oh;
        private AmpelHandler ap;
        private TrafficHandler th;
        private EnvironmentBuilder builder;
        public static IAmpelService trafficlight;
        private RabbitMQ.RabbitMQHandler mqhandler;

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

            mqhandler = new RabbitMQ.RabbitMQHandler();


            dispatchTimer           = new DispatcherTimer();
            dispatchTimer.Tick      += dispatchTimer_Tick;
            dispatchTimer.Interval  = new TimeSpan(0, 0, 0, 0, 100);
           
            ap      = new GUI.AmpelHandler(myCanvas);
            builder = new EnvironmentBuilder(myCanvas, ref ap, ref trafficlight);
            oh      = new ObjectHandler(myCanvas, ref builder);
            th      = new TrafficHandler(ref builder, ref oh);

            th.createNewVerkehrsteilnehmer(210, 155, 4, (int)TrafficObject.Fahrzeugtyp.Car, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
            th.createNewVerkehrsteilnehmer(35, 155, 4, (int)TrafficObject.Fahrzeugtyp.Car, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
            th.createNewVerkehrsteilnehmer(10, 155, 4, (int)TrafficObject.Fahrzeugtyp.Car, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
            th.createNewVerkehrsteilnehmer(280, 145, 4, (int)TrafficObject.Fahrzeugtyp.Car, (int)TrafficObject.Dir.Left, (int)TrafficObject.Dir.Down);
            th.createNewVerkehrsteilnehmer(255, 285, 4, (int)TrafficObject.Fahrzeugtyp.Car, (int)TrafficObject.Dir.Up, (int)TrafficObject.Dir.Up);
            th.createNewVerkehrsteilnehmer(320, 355, 4, (int)TrafficObject.Fahrzeugtyp.Car, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
            th.createNewVerkehrsteilnehmer(255, 655, 4, (int)TrafficObject.Fahrzeugtyp.Car, (int)TrafficObject.Dir.Up, (int)TrafficObject.Dir.Up);
            th.createNewVerkehrsteilnehmer(255, 645, 4, (int)TrafficObject.Fahrzeugtyp.Car, (int)TrafficObject.Dir.Up, (int)TrafficObject.Dir.Up);


            dispatchTimer.Start();
        }
        
        private void dispatchTimer_Tick(object sender, EventArgs e)
        {
            builder.UpdateGUIAmpeln();
            th.updateAll();
        }

        private void SliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int speed = (int)SliderSpeed.Value;
        }

        private void SliderLKW_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int lkw = (int)SliderLKW.Value;
            IGUI myInterface = th;
            myInterface.updateTruckRatio(lkw);
            Console.WriteLine("Slider LKWs: " + lkw);
        }

        private void SliderNum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int num = (int)SliderNum.Value;
            IGUI myInterface = th;
            myInterface.updateCarAmount(num);
            Console.WriteLine("Slider Anzahl: " + num);
        }
    }
}
