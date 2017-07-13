using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Verkehrssimulation.Verkehrsteilnehmer;

namespace Verkehrssimulation.RabbitMQ
{
    public class RabbitMQHandler
    {

        /**
        * RABBIT MQ
        * 
        * Group - User - PW gehört noch angepasst
        * Methoden/Klassen gehören noch ausgelagert
        * 
        * */

        RemoteTransaction remoteTransaction;
        ITrafficHandler th;

        public RabbitMQHandler()
        {
            th = TrafficHandler.getInstance();
            // RabbitMQ
            remoteTransaction = new RemoteTransaction(10.1, "PKW");
            Send(remoteTransaction, "group3");
            Receive();
        }

        public void Send(RemoteTransaction transaction, String group)
        {
            th = TrafficHandler.getInstance();
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

        public  void Receive()
        {
            th = TrafficHandler.getInstance();
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
                    ITrafficHandler th = TrafficHandler.getInstance();
                    int counter = 0;
                    consumer.Received += (model, ea) =>
                    {
                        var body = GetString(ea.Body);
                        var transaction = JsonConvert.DeserializeObject<RemoteTransaction>(body);

                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false); //Important. When the message get's not acknowledged, it gets sent again

                        Console.WriteLine("[x] Received:");
                        PrintTransaction(transaction);

                        if (th != null)
                        {
                            int speed = (int) transaction.Speed / 20;
                            if (speed > 5)
                                speed = 5;
                            if (transaction.CarType == "PKW")                            
                                th.addCarToEntryPoint(null, (int)TrafficObject.Fahrzeugtyp.Car, speed, counter*5);                            
                            if (transaction.CarType == "LKW")                          
                                th.addCarToEntryPoint(null, (int)TrafficObject.Fahrzeugtyp.Truck, speed, counter*5);
                            counter++;                           
                        }
                    };

                    channel.BasicConsume(queue: "group3",
                                         noAck: false,  //If noAck: false the command channel.BasicAck (see above) has to be implemented. Don't set it true, or the message will not get resubmitted, if the bank was offline
                                         consumer: consumer);

                    // Console.WriteLine(" Press [enter] to exit receive.");
                    // Console.ReadLine();
                    //Thread.Sleep(100);
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

        public  void PrintTransaction(RemoteTransaction transaction)
        {
            Console.WriteLine("CarId    : " + transaction.CarId);
            Console.WriteLine("GroupId  : " + transaction.GroupId);
            Console.WriteLine("CarType : " + transaction.CarType);
            Console.WriteLine("Speed    : " + transaction.Speed);
            Console.WriteLine("Timestamp: " + transaction.Timestamp);
        }

        private  byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private  string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

    }
}
