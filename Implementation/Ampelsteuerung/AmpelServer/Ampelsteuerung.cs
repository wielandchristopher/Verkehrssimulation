using System;
using System.ServiceModel;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

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
    class Ampelsteuerung : IChatService
    {
        ServiceHost host;
        bool _serverRunning = false;        
        List<Ampeln> Trafficlights = new List<Ampeln>();

        private void StartServer()
        {
            try
            {
                _serverRunning = !_serverRunning;

                if (_serverRunning)
                {
                    host = new ServiceHost(new Ampelsteuerung(), new Uri[] { new Uri("net.pipe://localhost") });
                    host.AddServiceEndpoint(typeof(IChatService), new NetNamedPipeBinding(), "Ampelsteuerung");
                    host.Open();

                    Console.WriteLine("Server ist gestartet!!");

                    Ampelsteuerung Ampel = new Ampelsteuerung();
                    // 5 Ampeln werden angelegt!
                    Ampel.factory(5);

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
        public void getAmpelInformation(string ampelid, string ausfall)
        {
            bool Ausgeschalten;
            int AmpelID;
            //Stelle Verbindung zu Client her

            if (ampelid.Equals("0"))
            {
                for (int i = 0; i < Trafficlights.Count; i++)
                {
                    AmpelID = Trafficlights.ElementAt(i).getID();
                    //und sende ihm appe AmpelIDs hintereinander
                }
            }
            else
            {
                AmpelID = Trafficlights.ElementAt(Int32.Parse(ampelid)).getID();
                //und sende ihm  den Status der spezifisch übergebenen ID
            }
            if (ausfall.Equals("check"))
            {
                if (ampelid.Equals("0"))
                {
                    for (int i = 0; i < Trafficlights.Count; i++)
                    {
                        Ausgeschalten = Trafficlights.ElementAt(i).getDefect();
                        //und sende ihm appe AmpelStatus hintereinander
                    }
                }
                else
                {
                    Ausgeschalten = Trafficlights.ElementAt(Int32.Parse(ampelid)).getDefect(); //true oder false
                                                                                               //und sende ihm  den Status der spezifisch übergebenen ID
                }
                //sende ihm den Status des Ausfalls der Ampel mit 
            }

            Console.WriteLine(ampelid, ausfall);
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

            while (true)
            {
                int Status = 0;
                for (int i = 0; i < anzahl; i++)
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
                    Console.WriteLine("AmpelID: " + Trafficlights.ElementAt(i).getID());
                    Console.WriteLine("AmpelStatus: " + Trafficlights.ElementAt(i).getStatus() + "\n");

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

        [STAThread]
        static void Main()
        {
            Ampelsteuerung test = new Ampelsteuerung();
            test.StartServer();
        }
    }
}

