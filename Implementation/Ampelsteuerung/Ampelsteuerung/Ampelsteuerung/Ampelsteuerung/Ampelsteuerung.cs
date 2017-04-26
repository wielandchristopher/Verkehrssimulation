using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ampelsteuerung
{
    class Ampeln
    {
        public int getStatus()
        {
            return Status;
        }

        public int setStatus(int value)
        {
            return Status = value;
        }

        int Status;
        int ID;
        bool defect;

    }

    class Ampelsteuerung
    {

        public List<Ampeln> factory(int anzahl)
        {
            List<Ampeln> Trafficlights = new List<Ampeln>();


            for(int i = 0; i<anzahl; i++)
            {
                Ampeln Ampel = new Ampeln();
                Ampel.setStatus(2);
                Trafficlights.Add(Ampel);
            }


            while (true)
            {
                
                Thread.Sleep(10000); //10 seconds
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
