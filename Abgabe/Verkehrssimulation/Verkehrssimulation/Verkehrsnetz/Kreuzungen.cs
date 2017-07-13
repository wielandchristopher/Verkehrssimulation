using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsnetz
{
    public interface IKreuzung
    {
        void Update();
        void InitAmpeln();
    }

    public abstract class Kreuzung : IKreuzung
    {
        protected int id { get; set; }
        public int n_status { get; set; }
        public int s_status { get; set; }
        public int w_status { get; set; }
        public int e_status { get; set; }

        protected int idn, ids, idw, ide;

        public int getID()
        {
            return this.id;
        }


        public abstract void Update();

        internal void setOffline()
        {
            MainWindow.trafficlight.setAmpelAusfall(idn);
            MainWindow.trafficlight.setAmpelAusfall(ids);
            MainWindow.trafficlight.setAmpelAusfall(idw);
            MainWindow.trafficlight.setAmpelAusfall(ide);

        }

        internal void setOnline()
        {
            MainWindow.trafficlight.setAmpelOn(idn);
            MainWindow.trafficlight.setAmpelOn(ids);
            MainWindow.trafficlight.setAmpelOn(idw);
            MainWindow.trafficlight.setAmpelOn(ide);
        }

        public abstract void InitAmpeln();

        internal void printStatus()
        {
            if (n_status > -2 && s_status > -2 && w_status > -2 && e_status > -2)
                Console.WriteLine("\nID: " + this.id + "\n" + "/n " + this.n_status + "/e " + this.e_status + "/s " + this.s_status + "/w " + this.w_status);

        }

        internal bool writeStatus(int ampelid, int mycase)
        {


            if (this.idn == ampelid)
            {
                this.n_status = mycase;
                return true;
            }
            else if (ids == ampelid)
            {
                this.s_status = mycase;

                return true;
            }
            else if (idw == ampelid)
            {
                this.w_status = mycase;
                return true;
            }
            else if (this.ide == ampelid)
            {
                this.e_status = mycase;

                return true;
            }
            return false;
        }
    }

    public class TKreuzung : Kreuzung, IKreuzung
    {


        public TKreuzung(int id, int idn, int ide, int ids, int idw)
        {

            // 0 = Rot, 1 = Gelb, 2 = Grün, 3 = Ausfall
            this.id = id;
            this.idn = idn;
            this.ids = ids;
            this.idw = idw;
            this.ide = ide;

            this.n_status = -1;
            this.w_status = -1;
            this.e_status = -1;
            this.s_status = -1;

            InitAmpeln();

        }

        public override void InitAmpeln()
        {
            if (idn < 0)
            {
                MainWindow.trafficlight.setAmpelStatus(ids, 0);
                MainWindow.trafficlight.setAmpelStatus(ide, 2);
                MainWindow.trafficlight.setAmpelStatus(idw, 2);

            }
            else if (ids < 0)
            {
                MainWindow.trafficlight.setAmpelStatus(idn, 0);
                MainWindow.trafficlight.setAmpelStatus(ide, 2);
                MainWindow.trafficlight.setAmpelStatus(idw, 2);

            }
            else if (idw < 0)
            {
                MainWindow.trafficlight.setAmpelStatus(ids, 2);
                MainWindow.trafficlight.setAmpelStatus(ide, 0);
                MainWindow.trafficlight.setAmpelStatus(idn, 2);

            }
            else if (ide < 0)
            {
                MainWindow.trafficlight.setAmpelStatus(ids, 2);
                MainWindow.trafficlight.setAmpelStatus(idn, 2);
                MainWindow.trafficlight.setAmpelStatus(idw, 0);
            }
        }

        public override void Update()
        {
            if (idn > -1)
            {
                n_status = MainWindow.trafficlight.getAmpelStatus(idn);
            }
            if (ids > -1)
            {
                s_status = MainWindow.trafficlight.getAmpelStatus(ids);
            }
            if (idw > -1)
            {
                w_status = MainWindow.trafficlight.getAmpelStatus(idw);
            }
            if (ide > -1)
            {
                e_status = MainWindow.trafficlight.getAmpelStatus(ide);
            }

        }
    }

    public class FKreuzung : Kreuzung, IKreuzung
    {


        public FKreuzung(int id, int ids, int idw, int idn, int ide)
        {

            // 0 = Rot, 1 = Gelb, 2 = Grün, 3 = Ausfall
            this.id = id;
            this.idn = idn;
            this.ids = ids;
            this.idw = idw;
            this.ide = ide;

            this.n_status = -1;
            this.w_status = -1;
            this.e_status = -1;
            this.s_status = -1;



            //Console.WriteLine("North: " + idn +" South: "+ ids + " West: " + idw + " East: " +  ide);

            InitAmpeln();


        }

        public override void InitAmpeln()
        {
            MainWindow.trafficlight.setAmpelStatus(this.idn, 2);
            MainWindow.trafficlight.setAmpelStatus(this.ids, 2);
            MainWindow.trafficlight.setAmpelStatus(this.idw, 0);
            MainWindow.trafficlight.setAmpelStatus(this.ide, 0);
        }

        public override void Update()
        {
            n_status = MainWindow.trafficlight.getAmpelStatus(idn);
            s_status = MainWindow.trafficlight.getAmpelStatus(ids);
            w_status = MainWindow.trafficlight.getAmpelStatus(idw);
            e_status = MainWindow.trafficlight.getAmpelStatus(ide);
        }


    }

    public class Env_Ampelhandler
    {

        private List<Kreuzung> kreuzungen;
        private int cnt = 0;
        private JObject configobj;

        public Env_Ampelhandler(int ampelcnt, JObject obj)
        {
            configobj = obj;
            kreuzungen = new List<Kreuzung>();
            cnt = ampelcnt;

            try
            {
                MainWindow.trafficlight.setAmpelAnzahl(cnt);
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


            int newID = 0;
            int x = 0;
            JArray geregelte_kreuzungen = (JArray)obj.GetValue("geregelte_kreuzungen");

            foreach (JObject k in geregelte_kreuzungen)
            {

                if (k.GetValue("type").Value<int>() == 4)
                {
                    kreuzungen.Add(new FKreuzung(newID, x, x + 1, x + 2, x + 3));
                    x = x + 4;
                    newID++;
                }
                else if (k.GetValue("type").Value<int>() == 3)
                {

                    switch (k.GetValue("nopath").Value<int>())
                    {
                        case 1: // north closed
                            kreuzungen.Add(new TKreuzung(newID, -1, x + 1, x, x + 2));
                            break;
                        case 2: // east closed
                            kreuzungen.Add(new TKreuzung(newID, x + 1, -1, x, x + 2));

                            break;
                        case 3: // south closed
                            kreuzungen.Add(new TKreuzung(newID, x + 1, x + 2, -1, x));
                            break;
                        case 4: // west closed
                            kreuzungen.Add(new TKreuzung(newID, x + 2, x + 1, x, -1));
                            break;
                        default:
                            break;
                    }
                    x = x + 3;
                    newID++;
                }
            }




            //while (x < cnt)
            //{
            //    kreuzungen.Add(new FKreuzung(newID, x, x + 1, x + 2, x + 3));
            //    x = x + 4;
            //    newID++;
            //}
        }

        public Kreuzung getKreuzung(int id)
        {
            foreach (Kreuzung k in kreuzungen)
            {
                if (k.getID() == id)
                {
                    return k; // wenn gefunden
                }
            }
            return null; // wenn nicht vorhanden
        }

        internal void setOffline(bool val)
        {
            foreach (Kreuzung k in kreuzungen)
            {
                if (val)
                {
                    k.setOnline();
                    k.InitAmpeln();
                }
                else
                {
                    k.setOffline();

                }
            }
        }

        internal void updateKreuzungen()
        {
            foreach (Kreuzung k in kreuzungen)
            {
                k.Update();
            }
        }

        internal void printStatus()
        {
            foreach (Kreuzung k in kreuzungen)
            {
                k.printStatus();
            }
        }

        internal void updateAmpel(int ampelid, int mycase)
        {
            foreach (Kreuzung k in kreuzungen)
            {
                if (k.writeStatus(ampelid, mycase))
                {
                    return;
                }
            }
        }
    }

}
