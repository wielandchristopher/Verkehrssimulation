using System;
using System.ServiceModel;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace Ampelsteuerung
{
    class Ampeln
    {
        public int getStatus()
        {
            return Status;
        }
        public int getID()
        {
            return ID;
        }
        public bool setDefect(bool value)
        {
            return defect = value;
        }

        public bool getDefect()
        {
            return defect;
        }
        public int setStatus(int value)
        {
            return Status = value;
        }
        public int setID(int value)
        {
            return ID = value;
        }

        int Status; // 0 = Rot, 1 = Gelb, 2 = Grün, 3 = Ausfall
        int ID;
        bool defect = false;
    }

    [ServiceBehavior(Name = "Ampelsteuerung", InstanceContextMode = InstanceContextMode.Single)]
    class Ampelsteuerung : IAmpelService
    {
        ServiceHost host;
        bool _serverRunning = false;

        public static List<Ampeln> Trafficlights = new List<Ampeln>();

        private void StartServer()
        {
            try
            {
                _serverRunning = !_serverRunning;

                if (_serverRunning)
                {
                    host = new ServiceHost(new Ampelsteuerung(), new Uri[] { new Uri("net.pipe://localhost") });
                    host.AddServiceEndpoint(typeof(IAmpelService), new NetNamedPipeBinding(), "Ampelsteuerung");
                    host.Open();

                    Console.WriteLine("Server ist gestartet!!");

                    Ampelsteuerung Ampel = new Ampelsteuerung();

                    // 5 Ampeln werden angelegt!
                    Trafficlights = Ampel.factory(5);

                    while (true)
                    {
                        int Status = 0;
                        for (int i = 0; i < Trafficlights.Count; i++)
                        {
                            Trafficlights.ElementAt(i).getStatus();
                            if (Trafficlights.ElementAt(i).getStatus() >= 2)
                            {
                                if (Trafficlights.ElementAt(i).getDefect() == false)
                                {
                                    Status = Trafficlights.ElementAt(i).setStatus(0);
                                }
                                else
                                    Status = Trafficlights.ElementAt(i).setStatus(3);
                            }
                            else
                            {
                                Status = Trafficlights.ElementAt(i).setStatus(Trafficlights.ElementAt(i).getStatus() + 1);
                            }
                        }
                        if (Status == 1)
                        {
                            Thread.Sleep(3000); //3 Sekunden bei Gelb
                        }
                        else
                        {
                            Thread.Sleep(10000); //10 Sekunden bei Rot und Grünund Ausfall
                        }
                    }
                }
                else
                {
                    host.Close();
                    host = null;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public void getAmpelInformation(int ampelid, string ausfall)
        {           
            bool Ausgeschalten = false;
            int AmpelStatus;
            OperationContext ctx = OperationContext.Current;
            IAmpelCallback answer = OperationContext.Current.GetCallbackChannel<IAmpelCallback>();

            if (ampelid == 0)
            {
                for (int i = 0; i < Trafficlights.Count; i++)
                {
                    AmpelStatus = Trafficlights.ElementAt(i).getStatus();
                    answer.OnNewMessage("Status der Ampel mit der ID: " + Trafficlights.ElementAt(i).getID() + " hat den Status: " + AmpelStatus.ToString());
                }
            }
            else
            {
                AmpelStatus = Trafficlights.ElementAt(ampelid-1).getStatus();
                answer.OnNewMessage("Status der Ampel mit der ID: " + Trafficlights.ElementAt(ampelid - 1).getID() + " hat den Status: " + AmpelStatus.ToString());
            }
            if (ausfall.Equals("check"))
            {
                if (ampelid == 0)
                {
                    for (int i = 0; i < Trafficlights.Count; i++)
                    {
                        int count = 0;
                        Ausgeschalten = Trafficlights.ElementAt(i).getDefect();
                        if (Ausgeschalten)
                        {
                            count++;
                        }
                        answer.OnNewMessage("Ausgeschalten sind: " + count.ToString() + " Ampeln.");
                    }
                }
                else
                {
                    Ausgeschalten = Trafficlights.ElementAt(ampelid-1).getDefect(); 
                    if (!Ausgeschalten)
                    {
                        answer.OnNewMessage("Diese Ampel funktioniert einwandfrei");
                    }
                    else
                    {
                        answer.OnNewMessage("Diese Ampel ist nicht ausgefallen oder ausgeschaltet");
                    }
                }
            }
        }
        public List<Ampeln> factory(int anzahl)
        {
            for (int i = 0; i < anzahl; i++)
            {
                Ampeln Ampel = new Ampeln();
                Ampel.setID(i + 1);
                Ampel.setStatus(2);
                Trafficlights.Add(Ampel);
            }
            return Trafficlights;
        }
        [STAThread]
        static void Main()
        {
            Ampelsteuerung test = new Ampelsteuerung();
            test.StartServer();
        }
    }
}

