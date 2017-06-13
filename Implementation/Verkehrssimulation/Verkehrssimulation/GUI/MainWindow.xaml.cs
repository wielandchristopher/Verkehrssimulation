﻿using System;
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

        /**
         * RABBIT MQ
         * 
         * Group - User - PW gehört noch angepasst
         * Methoden/Klassen gehören noch ausgelagert
         * 
         * */
        static void Send(RemoteTransaction transaction, String group)
        {
            var factory = new ConnectionFactory();
            factory.Uri = "amqp://user3:bQx3TzjSUFxLakHr@rabbit.binna.eu/";  //Insert your own user and password

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var transactionString = JsonConvert.SerializeObject(transaction);   //Awesome function

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "car.production",  //Choose between bank.development and bank.production depending of the queue (e.g. 70 is production, 71 is development)
                                    routingKey: group,   //This relates to the queue name of the receiver bank
                                    basicProperties: properties,    //Set the properties to persistent, otherwise the messages will get lost if the server restarts
                                    body: GetBytes(transactionString));

            }

        }

        static void Receive()
        {
            var factory = new ConnectionFactory();
            factory.Uri = "amqp://user3:bQx3TzjSUFxLakHr@rabbit.binna.eu/";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "group3",
                                 durable: true, //Always use durable: true, because the queue on the server is configured this way. Otherwise you'll not be able to connect
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = GetString(ea.Body);
                        var transaction = JsonConvert.DeserializeObject<RemoteTransaction>(body);

                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false); //Important. When the message get's not acknowledged, it gets sent again

                        Console.WriteLine("[x] Received:");
                        PrintTransaction(transaction);
                    };

                    channel.BasicConsume(queue: "group3",
                                         noAck: false,  //If noAck: false the command channel.BasicAck (see above) has to be implemented. Don't set it true, or the message will not get resubmitted, if the bank was offline
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit receive.");
                    Console.ReadLine();
                }
            }
        }

        public class RemoteTransaction
        {
            public Guid CarId { get; set; }
            public Guid GroupId { get; set; }
            public String CarType { get; set; }
            public double Speed { get; set; }
            public long Timestamp { get; set; }        // Unix format timestamp in milliseconds
            public int Errorcode { get; set; }         // 0 = no error

            private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            public static long GetCurrentUnixTimestampMillis()
            {
                return (long)(DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
            }

            public static DateTime DateTimeFromUnixTimestampMillis(long millis)
            {
                return UnixEpoch.AddMilliseconds(millis);
            }

            public RemoteTransaction(double speed, string cartype, int errorcode = 0)
            {
                this.CarId = Guid.NewGuid();
                this.GroupId = Guid.NewGuid();
                this.CarType = cartype;
                this.Speed = speed;
                this.Timestamp = GetCurrentUnixTimestampMillis();
                this.Errorcode = errorcode;
            }

            public string GetStatus()
            {
                switch (this.Errorcode)
                {
                    case 0:
                        return "Success";
                    case -1:
                        return "Json-Format invalid / Data could not be converted";
                    case -2:
                        return "Transaction rejected";
                    default:
                        return "Error not specified";
                }
            }
        }

        static void PrintTransaction(RemoteTransaction transaction)
        {
            Console.WriteLine("CarId    : " + transaction.CarId);
            Console.WriteLine("GroupId  : " + transaction.GroupId);
            Console.WriteLine("CarType : " + transaction.CarType);
            Console.WriteLine("Speed    : " + transaction.Speed);
            Console.WriteLine("Timestamp: " + transaction.Timestamp);
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public MainWindow()
        {
            InitializeComponent();
            StartAmpelsteuerung();


            // RabbitMQ
            var remoteTransaction = new RemoteTransaction(10.1, "PKW");
            Send(remoteTransaction, "group3");
            Receive();
            

            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Tick += dispatchTimer_Tick;
            dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            // Test mit blinkender Ampel
            dpTimer2 = new DispatcherTimer();
            dpTimer2.Tick += dpTimer2_Tick;
            dpTimer2.Interval = new TimeSpan(0, 0, 0, 10, 500);

            oh = new ObjectHandler(myCanvas);
            ap = new GUI.AmpelHandler(myCanvas);

            builder = new EnvironmentBuilder(myCanvas, ref ap, ref trafficlight);

   
            th = new TrafficHandler(ref builder, ref oh);

            MainAmpelsteuerung(this);

            

            th.createNewVerkehrsteilnehmer(210, 155, 5, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
            th.createNewVerkehrsteilnehmer(35, 155, 5, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
            th.createNewVerkehrsteilnehmer(10, 155, 5, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
            th.createNewVerkehrsteilnehmer(280, 145, 5, (int)TrafficObject.Dir.Left, (int)TrafficObject.Dir.Down);
            th.createNewVerkehrsteilnehmer(255, 655, 5, (int)TrafficObject.Dir.Up, (int)TrafficObject.Dir.Up);
            th.createNewVerkehrsteilnehmer(255, 645, 5, (int)TrafficObject.Dir.Up, (int)TrafficObject.Dir.Up);


            dispatchTimer.Start();
            dpTimer2.Start();
            builder.printEntryPoints();
        }
        [STAThread]
        private void MainAmpelsteuerung(MainWindow mainWindow)
        {
            mainWindow.StartAmpelsteuerung();
            try
            {
                //mainWindow.trafficlight.setAmpelAnzahl(5);
                //string j = mainWindow.trafficlight.getAmpelStatus(2);
                //int i = mainWindow.trafficlight.getAmpelAnzahl();
               // Console.WriteLine(j);
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
            //a++;

            //if(a%5 == 0)
            //{
            //    this.builder.alternateLight();
            //}

            //Console.WriteLine("trafficlight.getAmpelAnzahl(): " + trafficlight.getAmpelAnzahl());


            //for (int tmp = 0; tmp < trafficlight.getAmpelAnzahl(); tmp++)
            //{
            //    Console.WriteLine("trafficlight.getAmpelStatus(" + tmp + "): " + trafficlight.getAmpelStatus(tmp));
            //}
            //builder.UpdateGUIAmpeln();
            //builder.printEntryPoints();
            //Console.WriteLine(a);
            //this.builder.GetAmpelInfo(4);

            //builder.getStreetType(400, 600);
            //builder.getStreetType(100, 100);
            //builder.getStreetType(200, 100);
            //builder.getStreetType(100, 200);
            //ap.blinky();
        } 
    }
}