﻿using System;
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
        public int getRotPhase()
        {
            return rotphase;
        }
        public int getGelbPhase()
        {
            return gelbphase;
        }
        public int getGruenPhase()
        {
            return gruenphase;
        }
        public bool getDefect()
        {
            return defect;
        }
        public int getID()
        {
            return ID;
        }
        public int setRotPhase(int value)
        {
            return rotphase = value;
        }
        public int setGelbPhase(int value)
        {
            return gelbphase = value;
        }
        public int setGruenPhase(int value)
        {
            return gruenphase = value;
        }
        public bool setDefect(bool value)
        {
            return defect = value;
        }
        public int setStatus(int value)
        {
            return Status = value;
        }
        public int setID(int value)
        {
            return ID = value;
        }
        public void startTimer(int status)
        {
            System.Windows.Forms.Timer AmpelTimer = new System.Windows.Forms.Timer();
            int time = 0;

            while (status == 0)
            {
                AmpelTimer.Interval = time = rotphase;
                AmpelTimer.Start();

                while (time > 0)
                {
                    Thread.Sleep(1000);
                    time--;
                }
                status = setStatus(1);
            }
            while (status == 1)
            {
                AmpelTimer.Interval = time = gelbphase;
                AmpelTimer.Start();

                while (time > 0)
                {
                    Thread.Sleep(1000);
                    time--;
                }
                status = setStatus(2);
            }
            while (status == 2)
            {
                AmpelTimer.Interval = time = gruenphase;
                AmpelTimer.Start();

                while (time > 0)
                {
                    Thread.Sleep(1000);
                    time--; 
                }
                status = setStatus(0);
            }            
        }

        int Status; // 0 = Rot, 1 = Gelb, 2 = Grün, 3 = Ausfall
        int ID;
        bool defect = false;
        int rotphase = 3; //in sekunden
        int gelbphase = 1; //in sekunden
        int gruenphase = 3; //in sekunden
    }

    [ServiceBehavior(Name = "Ampelsteuerung", InstanceContextMode = InstanceContextMode.Single)]
    class Ampelsteuerung : IAmpelService
    {
        ServiceHost host;
        bool _serverRunning = false;
        public static List<Ampeln> Trafficlights = new List<Ampeln>();
        public static int Anzahl = 0;
        public static int Flag = 1;

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

                                       
                    while (true)
                    {
                        if (Anzahl != 0 && Flag == 1)
                        {
                            Trafficlights = Ampel.factory(getAmpelAnzahl());
                            for (int i = 0; i < Trafficlights.Count; i++)
                            {
                                Trafficlights.ElementAt(i).startTimer(Trafficlights.ElementAt(i).getStatus());
                            }
                            Flag = 0;
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
        //Gibt 2 Integer zurück: 
        //Erster integer gibt ampelID zurück
        //Zweiter integer gibt Status der Ampel zurück 
        public int getAmpelStatus(int ampelid)
        {
            int AmpelStatus = 0;

            if (ampelid == 0)
            {
                for (int i = 0; i < Trafficlights.Count; i++)
                {
                    AmpelStatus = Trafficlights.ElementAt(i).getStatus();
                    return AmpelStatus;
                }
            }
            else
            {
                AmpelStatus = Trafficlights.ElementAt(ampelid - 1).getStatus();
                return  AmpelStatus; //Trafficlights.ElementAt(ampelid - 1).getID() + " " +
            }
            return -1;
        }
        //Gibt integer und boolean zurück: 
        //integer ist die AmpelID
        //true - Ampel Funktioniert, false - Ampel ausgeschaltet
        public string getAmpelAusfall(int ampelid)
        {
            bool Ausgeschalten = false;
            if (ampelid == 0)
            {
                for (int i = 0; i < Trafficlights.Count; i++)
                {
                    Ausgeschalten = Trafficlights.ElementAt(i).getDefect();
                    if (Ausgeschalten)
                    {
                        return Trafficlights.ElementAt(i).getID() + " false";
                    }
                    else
                    {
                        return Trafficlights.ElementAt(i).getID() + " true";
                    }
                }
            }
            else
            {
                Ausgeschalten = Trafficlights.ElementAt(ampelid - 1).getDefect();
                if (Ausgeschalten)
                {
                    int ausgabe = ampelid - 1;
                    return ausgabe + " false";
                }
                else
                {
                    int ausgabe = ampelid - 1;
                    return ausgabe + " true";
                }
            }
            return "";
        }
        public void setAmpelAusfall(int ampelid)
        {
            Trafficlights.ElementAt(ampelid - 1).setDefect(true);
            Trafficlights.ElementAt(ampelid - 1).setStatus(3);
        }
        public void setAmpelOn(int ampelid)
        {
            Trafficlights.ElementAt(ampelid - 1).setDefect(false);
            Trafficlights.ElementAt(ampelid - 1).setStatus(0);
        }
        public void setAmpelStatus(int ampelid, int neuerStatus)
        {
            Trafficlights.ElementAt(ampelid).setStatus(neuerStatus);
        }
        //Gibt 2 Integer zurück: 
        //Erster integer gibt ampelID zurück
        //Zweiter integer gibt die Sekunden der Rotphase zurück
        public string getRotPhase(int ampelid)
        {
            return ampelid + " " + Trafficlights.ElementAt(ampelid - 1).getRotPhase();
        }
        //Gibt 2 Integer zurück: 
        //Erster integer gibt ampelID zurück
        //Zweiter integer gibt die Sekunden der Gelbphase zurück
        public string getGelbPhase(int ampelid)
        {
            return ampelid + " " + Trafficlights.ElementAt(ampelid - 1).getGelbPhase();
        }
        //Gibt 2 Integer zurück: 
        //Erster integer gibt ampelID zurück
        //Zweiter integer gibt die Sekunden der Grünphase zurück
        public string getGruenPhase(int ampelid)
        {
            return ampelid + " " + Trafficlights.ElementAt(ampelid - 1).getGruenPhase();
        }
        public void setGruenPhase(int ampelid, int zeit)
        {
            Trafficlights.ElementAt(ampelid - 1).setGruenPhase(zeit);
        }
        public void setGelbPhase(int ampelid, int zeit)
        {
            Trafficlights.ElementAt(ampelid - 1).setGelbPhase(zeit);
        }
        public void setRotPhase(int ampelid, int zeit)
        {
            Trafficlights.ElementAt(ampelid - 1).setRotPhase(zeit);
        }
        public void setAmpelAnzahl(int anzahl)
        {
            Anzahl = anzahl;
        }
        //Gibt die Anzahl der erstellten Ampeln zurück
        public int getAmpelAnzahl()
        {
            return Anzahl;
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

