﻿using Ampelsteuerung;
using CallbackCli;
using System;
using System.Diagnostics;
using System.ServiceModel;

namespace Verkehrssimulation.AmpelHandler
{
    public class AmpelHandler
    {
        IAmpelService trafficlight;
        CallbackClient _callback;
        AmpelHandler client;
        AmpelHandler nobodyneed;

        //Diese Funktion muss gestartet werden, damit eine Verbindung zum Server aufgebaut werden kann. 
        public void StartAmpelsteuerung()
        {
            try
            {              
                _callback = new CallbackClient(nobodyneed);
                DuplexChannelFactory<IAmpelService> factory = new DuplexChannelFactory<IAmpelService>(_callback, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/Ampelsteuerung"));
                trafficlight = factory.CreateChannel();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public AmpelHandler()
        {
            client = new AmpelHandler();
            client.StartAmpelsteuerung();
        }

        public void functionx()
        {
            try
            {
                client.trafficlight.setAmpelAnzahl(5);
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



        //[STAThread]
        //static void Main()
        //{
        //    TestClient test = new TestClient();
        //    test.StartAmpelsteuerung();

        //    //IN IAmepService sind alle Funktionen beschrieben!
        //    try
        //    {
        //        //Setzt die Anzahl der Ampeln. Sind keine Ampeln gesetzt, kann man logischerweise auch keinen Status abfragen. 

        //        //Diese Funktion muss immer zuerst ausgeführt werden. 
        //        /*test.trafficlight.setAmpelAnzahl(5);*/

        //        //Gibt man der Funktion getAmpelStatus den integer 0 mit, so erhält man den Status aller Ampeln. Ansonsten erhält man den Status der Spezifischen ID 
        //        /*test.trafficlight.getAmpelStatus (0);*/

        //        //Hier noch einige Beispiele von Funktionen: 

        //        //Dieser Befehl schaltet die Ampel mit der ID 1 aus. Gib 0 mit, und alle AMpeln werden ausgeschaltet. 
        //        /*test.trafficlight.setAmpelAusfall(1);*/

        //        //Diese Funktion zB setzt die Rotphase der Ampel mit der ID 1 auf 15 Sekunden. 
        //        /*test.trafficlight.setRotPhase(1,15);*/

        //        //Hier erhältst du in Sekunden die Rotphase einer Ampel zurück. (Was genau zurückgegeben wird steht alles in der IAmpelService)
        //        /*test.trafficlight.getRotPhase(1);*/

        //        //Hier würdest du die AMpel mit der ID 1 wieder einschalten. 
        //        /*test.trafficlight.setAmpelOn(1);*/

        //        //Testet alle Ampeln durch und prüft, welöche Ampel ausgefallen ist, und gibt diese Zurück. 0 Fragt alle Ampeln ab. Spezifische ID die spezifische Ampel. 
        //        /*test.trafficlight.getAmpelAusfall(0);*/

        //    }
        //    catch (NullReferenceException nre)
        //    {
        //        Console.WriteLine("Der Server ist nicht gestartet!");
        //        nre.ToString();
        //    }
        //    catch (EndpointNotFoundException enfe)
        //    {
        //        Console.WriteLine("Der Server ist nicht gestartet!");
        //        enfe.ToString();
        //    }
        //}
    }
}
