﻿using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

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
        public int getSekundenzähler()
        {
            return sekundenzähler;
        }
        public int setSekundenzähler(int value)
        {
            return sekundenzähler = value;
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
       
        int sekundenzähler = 0;
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
        bool run = false;
        public static List<Ampeln> Trafficlights = new List<Ampeln>();
        public static int Anzahl = 0;
        static Timer Ampeltimer;

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
                    Ampelsteuerung Ampel = new Ampelsteuerung();


                    while (getAmpelAnzahl() == 0){}

                    if (getAmpelAnzahl() != 0)
                    {
                        Trafficlights = Ampel.factory(getAmpelAnzahl());
                        run = true;
                        Ampeltimer = new Timer(1000);
                        Ampeltimer.Elapsed += (sender, e) => HandleTimer();
                        Ampeltimer.Enabled = true;
                    }
                    while (run) {
                        
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
        //Tickt jede Sekunde in diese Funktion herein und Prüft bzw. setzt den Ampelstatus
        private Task HandleTimer()
        {
            int i;
            for (i = 0; i < Trafficlights.Count; i++)
            {
                //Setzt den sekundenzähler um 1 hoch
                Trafficlights.ElementAt(i).setSekundenzähler(Trafficlights.ElementAt(i).getSekundenzähler() + 1);

                if (Trafficlights.ElementAt(i).getSekundenzähler() >= 0 && Trafficlights.ElementAt(i).getSekundenzähler() < Trafficlights.ElementAt(i).getRotPhase() && !Trafficlights.ElementAt(i).getDefect())
                {                                     
                    //Ampel Rot
                    Trafficlights.ElementAt(i).setStatus(0);
                }
                else if (Trafficlights.ElementAt(i).getSekundenzähler() >= Trafficlights.ElementAt(i).getRotPhase() && Trafficlights.ElementAt(i).getSekundenzähler() < Trafficlights.ElementAt(i).getGelbPhase() + Trafficlights.ElementAt(i).getRotPhase() && !Trafficlights.ElementAt(i).getDefect())
                {
                    //Ampel Gelb
                    Trafficlights.ElementAt(i).setStatus(1);
                }
                else if (Trafficlights.ElementAt(i).getSekundenzähler() >= 1+Trafficlights.ElementAt(i).getGelbPhase() + Trafficlights.ElementAt(i).getRotPhase() && Trafficlights.ElementAt(i).getSekundenzähler() < Trafficlights.ElementAt(i).getGelbPhase() + Trafficlights.ElementAt(i).getRotPhase() + Trafficlights.ElementAt(i).getGruenPhase() && !Trafficlights.ElementAt(i).getDefect())
                {
                    //Ampel Gruen
                    Trafficlights.ElementAt(i).setStatus(2);
                }
                else if (Trafficlights.ElementAt(i).getSekundenzähler() >= Trafficlights.ElementAt(i).getGelbPhase() + Trafficlights.ElementAt(i).getRotPhase() + Trafficlights.ElementAt(i).getGruenPhase() && Trafficlights.ElementAt(i).getSekundenzähler() < Trafficlights.ElementAt(i).getGelbPhase() + Trafficlights.ElementAt(i).getRotPhase() + Trafficlights.ElementAt(i).getGruenPhase() + Trafficlights.ElementAt(i).getGelbPhase() && !Trafficlights.ElementAt(i).getDefect())
                {
                    //Ampel Gelb
                    Trafficlights.ElementAt(i).setStatus(1);
                }
                else if (Trafficlights.ElementAt(i).getSekundenzähler() >= Trafficlights.ElementAt(i).getGelbPhase() + Trafficlights.ElementAt(i).getRotPhase() + Trafficlights.ElementAt(i).getGruenPhase() + Trafficlights.ElementAt(i).getGelbPhase() && !Trafficlights.ElementAt(i).getDefect())
                {
                    Trafficlights.ElementAt(i).setSekundenzähler(0);
                }
                
                if (Trafficlights.ElementAt(i).getDefect() == true)
                {
                    Trafficlights.ElementAt(i).setStatus(3);
                }               
            }
            return null;
        }
        //Gibt 2 Integer zurück: 
        //Erster integer gibt ampelID zurück
        //Zweiter integer gibt Status der Ampel zurück 
        public int getAmpelStatus(int ampelid)
        {
            int AmpelStatus = 0;

            if (ampelid == 0)
            {
                int i;

                for (i = 0; i < Trafficlights.Count; i++)
                {
                    AmpelStatus = Trafficlights.ElementAt(i).getStatus();

                    //return Trafficlights.ElementAt(i).getID() + " " + AmpelStatus.ToString();

                    return AmpelStatus;

                }
            }
            else if(ampelid != 0)
            {
                AmpelStatus = Trafficlights.ElementAt(ampelid-1).getStatus();
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
            if (ampelid > -1)
            {
                Trafficlights.ElementAt(ampelid).setDefect(true); // deleted ampelid - 1 . needed?
                Trafficlights.ElementAt(ampelid).setStatus(3);
            }

        }
        public void setAmpelOn(int ampelid)
        {
            if (ampelid > -1)
            {
                Trafficlights.ElementAt(ampelid).setDefect(false);
                Trafficlights.ElementAt(ampelid).setStatus(0);
            }

        }
        public void setAmpelStatus(int ampelid, int neuerStatus)
        {
                switch (neuerStatus)
            {
                case 0:
                    Trafficlights.ElementAt(ampelid).setSekundenzähler(0);
                    Trafficlights.ElementAt(ampelid).setStatus(0);
                    break;
                case 1:
                    Trafficlights.ElementAt(ampelid).setSekundenzähler(Trafficlights.ElementAt(ampelid).getRotPhase());
                    Trafficlights.ElementAt(ampelid).setStatus(1);
                    break;
                case 2:
                    Trafficlights.ElementAt(ampelid).setSekundenzähler(Trafficlights.ElementAt(ampelid).getRotPhase() + Trafficlights.ElementAt(ampelid).getGelbPhase());
                    Trafficlights.ElementAt(ampelid).setStatus(2);
                    break;
                case 3:
                    Trafficlights.ElementAt(ampelid).setStatus(3);
                    Trafficlights.ElementAt(ampelid).setDefect(true);
                    break;
                default:
                    Trafficlights.ElementAt(ampelid - 1).setStatus(0);
                    break;
            }
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
        //Generiert die Ampeln 
        public List<Ampeln> factory(int anzahl)
        {
            for (int i = 0; i < anzahl; i++)
            {
                Ampeln Ampel = new Ampeln();
                Ampel.setID(i + 1);

                Ampel.setStatus(2);              

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
