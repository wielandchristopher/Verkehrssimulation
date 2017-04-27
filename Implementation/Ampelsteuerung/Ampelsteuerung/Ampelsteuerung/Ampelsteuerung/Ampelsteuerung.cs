using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;

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

    class Ampelsteuerung
    {
        public List<Ampeln> factory(int anzahl)
        {
            List<Ampeln> Trafficlights = new List<Ampeln>();

            for(int i = 0; i<anzahl; i++)
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
                        if(Trafficlights.ElementAt(i).getDefect() == false)
                        {
                            Status= Trafficlights.ElementAt(i).setStatus(0);
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
                if (Status == 1) {
                    Thread.Sleep(3000); //3 Sekunden bei Gelb
                }
                else {
                    Thread.Sleep(10000); //10 Sekunden bei Rot und Grünund Ausfall
                }
            }          
        }

        static void Main(string[] args)
        {
            Ampelsteuerung src = new Ampelsteuerung();
            src.factory(5);
            Console.ReadLine();

        }
    }

}
