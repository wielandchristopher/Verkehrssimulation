using Ampelsteuerung;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using Verkehrssimulation.Verkehrsteilnehmer;
using Verkehrssimulation;
using System.ServiceModel;
using System.Windows.Input;

namespace Verkehrssimulation.Verkehrsnetz
{

    class EnvironmentHandler : I_ENV_VKTeilnehmer, I_ENV_GUI
    {
        JObject obj; // für json -> Projekt-> nu-getpakete verwalten -> json linq irgendwas
        private Canvas canvas;
        List<Streetelem> elem;
        GUI.AmpelHandler ah;
        public Env_Ampelhandler env_ah;
        IAmpelService trafficlight;
        Streetelem[,] elems = new Streetelem[7, 7];
        int ampelcnt = 0;
        List<EntryPoint> entrypoints;
        ObstacleHandler oh;
        private int ampelidconnector = 0;

        public EnvironmentHandler(Canvas mycanvas, ref GUI.AmpelHandler _ah, ref IAmpelService _trafficlight)
        {
            oh = new ObstacleHandler();
            ah = _ah;
            trafficlight = _trafficlight;

            canvas = mycanvas;
            //mycanvas.MouseMove += getEnvType;
            //Console.WriteLine("Buidler loaded");

            entrypoints = new List<EntryPoint>();
            elem = new List<Streetelem>();
            LoadJson();
            LoadEnvironment();
            
        }

        private void getEnvType(object sender, MouseEventArgs e)
        {
            Console.WriteLine("X: "+ e.GetPosition(canvas).X + "/ Y: " + e.GetPosition(canvas).Y + "\n" + this.getStreetType((int)e.GetPosition(canvas).Y, (int)e.GetPosition(canvas).X).ToString());
        }

        private void LoadJson()
        {

            using (StreamReader r = new StreamReader("../../Verkehrsnetz/env_config.json"))
            {
                string json = r.ReadToEnd();
                obj = JObject.Parse(json);
            }
        }

        private int calcAmpelCnt()
        {
            int cnt = 0;
            JArray geregelte_kreuzungen = (JArray)obj.GetValue("geregelte_kreuzungen");

            foreach(JObject obj in geregelte_kreuzungen)
            {
                cnt += obj.GetValue("type").Value<int>();
            }
            //Console.WriteLine(cnt); // ist richtig berechnet 
            return cnt;
        }

        private void LoadEnvironment()
        {
            JArray geregelte_kreuzungen = (JArray)obj.GetValue("geregelte_kreuzungen");
            int xpos, ypos = 0;
            
            //Console.WriteLine("geregelte_kreuzungen.Count: " + geregelte_kreuzungen.Count);
            env_ah = new Env_Ampelhandler(calcAmpelCnt(), obj);
            //trafficlight.setAmpelAnzahl(12);

            //Console.WriteLine("trafficlight.getAmpelAnzahl(): " + trafficlight.getAmpelAnzahl());


            //for (int tmp = 1; tmp <= trafficlight.getAmpelAnzahl(); tmp++)
            //{

            //    trafficlight.setGelbPhase(tmp, 1);
            //    trafficlight.setGruenPhase(tmp, 1);
            //    trafficlight.setRotPhase(tmp, 1);
            //    //trafficlight.setAmpelOn(tmp);

            //    Console.WriteLine("trafficlight.getAmpelStatus(" + tmp + "): " + trafficlight.getAmpelStatus(tmp));
            //}


            foreach (JObject obj in geregelte_kreuzungen)
            {

                if (obj.GetValue("type").Value<int>() == 4)
                {
                    xpos = obj.GetValue("xpos").Value<int>();
                    ypos = obj.GetValue("ypos").Value<int>();

                    elems[xpos / 100, ypos / 100] = addObject(xpos, ypos, 3);

                    ah.addTrafficLight(xpos + 60, ypos + 60, 1, ampelcnt++);
                    ah.addTrafficLight(xpos + 30, ypos + 60, 2, ampelcnt++);
                    ah.addTrafficLight(xpos + 7, ypos + 30, 3, ampelcnt++);
                    ah.addTrafficLight(xpos + 60, ypos + 7, 4, ampelcnt++);

                    addSolution(xpos, ypos);
                }
                else if(obj.GetValue("type").Value<int>() == 3)
                {
                    xpos = obj.GetValue("xpos").Value<int>();
                    ypos = obj.GetValue("ypos").Value<int>();
                    int nopath = obj.GetValue("nopath").Value<int>();

                    elems[xpos / 100, ypos / 100] = addObject(xpos, ypos, 2, nopath);

                    //rotateElement(3, xpos, ypos);

                    switch (nopath)
                    {
                        case 1:
                            rotateElement(2, xpos, ypos);
                            break;
                        case 2:
                            rotateElement(3, xpos, ypos);
                            break;
                        case 3: // nicht drehen
                            break;
                        case 4:
                            rotateElement(1, xpos, ypos);
                            break;
                        default:
                            break;
                    }


                    addThreeGuiLights(nopath,xpos,ypos);


                    addSolution(xpos, ypos, nopath);
                }
                else
                {
                    Console.WriteLine("Type not implemented - ignored in the Environment");
                }


            }
            fillWithGrass();
        }

        private void addThreeGuiLights(int nopath,int xpos,int ypos)
        {

            switch (nopath)
            {
                case 1: // kein norden -> keine ampel bei links oben
                    ah.addTrafficLight(xpos + 60, ypos + 60, 1, ampelcnt++); // rechts unten
                    ah.addTrafficLight(xpos + 30, ypos + 60, 2, ampelcnt++); // links unten
                    ah.addTrafficLight(xpos + 60, ypos + 7, 4, ampelcnt++); // rechts oben

                    break;
                case 2: // kein East
                    ah.addTrafficLight(xpos + 60, ypos + 60, 1, ampelcnt++); // rechts unten
                    ah.addTrafficLight(xpos + 7, ypos + 30, 3, ampelcnt++); // links oben
                    ah.addTrafficLight(xpos + 60, ypos + 7, 4, ampelcnt++); // rechts oben
                    break;
                case 3: // kein south
                    ah.addTrafficLight(xpos + 30, ypos + 60, 2, ampelcnt++); // links unten
                    ah.addTrafficLight(xpos + 7, ypos + 30, 3, ampelcnt++); // links oben
                    ah.addTrafficLight(xpos + 60, ypos + 7, 4, ampelcnt++); // rechts oben
                    break;
                case 4: // kein west
                    ah.addTrafficLight(xpos + 60, ypos + 60, 1, ampelcnt++); // rechts unten
                    ah.addTrafficLight(xpos + 30, ypos + 60, 2, ampelcnt++); // links unten
                    ah.addTrafficLight(xpos + 7, ypos + 30, 3, ampelcnt++); // links oben
                    break;
                default:
                    break;
            }
            

        }

        private void rotateElement(int cnt, int xpos, int ypos)
        {
            for(int i = 0; i < cnt; i++)
            {
                ((Streetelem)elems[xpos / 100, ypos / 100]).elemRotate(null, null);
            }
        }

        private void fillWithGrass()
        {
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (elems[x, y] == null)
                    {
                        elems[x, y] = addObject(x * 100, y * 100, 4); // correct 4
                    }

                }
            }
        }

        public void printEntryPoints()
        {
            foreach (EntryPoint e in this.entrypoints)
            {
                Console.WriteLine("Entrypoint:" + e.TileX + "/" + e.TileY);
            }
        }

        private void addEntryPoints(int xpos, int ypos)
        {
            int MAX = 600;
            int MIN = 0;

            this.entrypoints.Add(new EntryPoint(xpos, MAX));
            this.entrypoints.Add(new EntryPoint(xpos, MIN));
            this.entrypoints.Add(new EntryPoint(MAX, ypos));
            this.entrypoints.Add(new EntryPoint(MIN, ypos));
        }

        private void addEntryPoints(int xpos, int ypos, int nopath)
        {
            int MAX = 600;
            int MIN = 0;
            
            // v ausschließen des entrypoints bei dem die 3er kreuzung nicht weitergeführt wird

            if (nopath != 1)
            this.entrypoints.Add(new EntryPoint(xpos, MIN));

            if (nopath != 2)
            this.entrypoints.Add(new EntryPoint(MAX, ypos));

            if (nopath != 3)
            this.entrypoints.Add(new EntryPoint(xpos, MAX));

            if (nopath != 4)
            this.entrypoints.Add(new EntryPoint(MIN, ypos));
        }

        private void addSolution(int xpos, int ypos)
        {

            addEntryPoints(ypos, xpos);

            for (int i = 0; i < 700; i += 100)
            {
                if (i != xpos)
                {


                    if (elems[i / 100, ypos / 100] != null && ((Streetelem)elems[i / 100, ypos / 100]).getStreetType() == EnvElement.StreetType.Street)
                    {
                        ((Streetelem)elems[i / 100, ypos / 100]).updateType(EnvElement.StreetType.FourKreuzung);

                    }
                    else if (elems[i / 100, ypos / 100] == null)
                    {
                        elems[i / 100, ypos / 100] = addObject(i, ypos, 1); // correct 1
                        ((Streetelem)elems[i / 100, ypos / 100]).elemRotate(null, null);
                    }

                }

            }

            for (int i = 0; i < 700; i += 100)
            {
                if (i != ypos)
                {

                    if (elems[xpos / 100, i / 100] != null && ((Streetelem)elems[xpos / 100, i / 100]).getStreetType() == EnvElement.StreetType.Street)
                    {
                        ((Streetelem)elems[xpos / 100, i / 100]).updateType(EnvElement.StreetType.FourKreuzung);

                    }
                    else if (elems[xpos / 100, i / 100] == null)
                    {
                        elems[xpos / 100, i / 100] = addObject(xpos, i, 1); // correct 1
                    }

                }

            }

        }

        private void addSolution(int xpos, int ypos, int nopath)
        {

            addEntryPoints(ypos, xpos, nopath);

            int initxval = 0;
            int inityval = 0;
            int maxxval = 700;
            int maxyval = 700;

            //Console.WriteLine("nopath" + nopath);

            switch (nopath)
            {
                case 1:
                    initxval = xpos;
                    break;
                case 2:
                    maxyval = ypos;
                    break;
                case 3:
                    maxxval = xpos;
                    break;
                case 4:
                    inityval = ypos;
                    break;
                default:
                    break;
            }

            for (int i = initxval; i < maxxval; i += 100)
            {
                if (i != xpos)
                {
                    
                    if (elems[i / 100, ypos / 100] != null && ((Streetelem)elems[i / 100, ypos / 100]).getStreetType() == EnvElement.StreetType.Street)
                    {
                        ((Streetelem)elems[i / 100, ypos / 100]).updateType(EnvElement.StreetType.FourKreuzung);

                    }
                    else if (elems[i / 100, ypos / 100] == null)
                    {
                        elems[i / 100, ypos / 100] = addObject(i, ypos, 1); // correct 1
                        ((Streetelem)elems[i / 100, ypos / 100]).elemRotate(null, null);
                    }

                }

            }

            for (int i = inityval; i < maxyval; i += 100)
            {
                if (i != ypos)
                {

                    if (elems[xpos / 100, i / 100] != null && ((Streetelem)elems[xpos / 100, i / 100]).getStreetType() == EnvElement.StreetType.Street)
                    {
                        ((Streetelem)elems[xpos / 100, i / 100]).updateType(EnvElement.StreetType.FourKreuzung);

                    }
                    else if (elems[xpos / 100, i / 100] == null)
                    {
                        elems[xpos / 100, i / 100] = addObject(xpos, i, 1); // correct 1
                    }

                }

            }

        }

        public void setAmpeln()
        {
            int x = 0;
            while (x < ampelcnt)
            {
                ah.setNext(x);
                x++;
            }

            ah.setNext(7);
        }

        public Streetelem addObject(int x, int y, int type)
        {

            //StreetType { Street = 1, ThreeKreuzung = 2, FourKreuzung = 3, Grass = 4 };
            int ampelid = -1;
            if (type == 3)
            {
                ampelid = ampelidconnector++;
            }
            Streetelem e = new Streetelem(x, y, 1, type,ampelid);
            elem.Add(e);

            canvas.Children.Add(e.getImage());
            Canvas.SetTop(e.getImage(), x);
            Canvas.SetLeft(e.getImage(), y);
            return e;
        }

        public Streetelem addObject(int x, int y, int type,int off)
        {

            //StreetType { Street = 1, ThreeKreuzung = 2, FourKreuzung = 3, Grass = 4 };
            int ampelid = -1;
            if (type == 2)
            {
                ampelid = ampelidconnector++;
            }
            Streetelem e = new Streetelem(x, y, 1, type, ampelid);
            elem.Add(e);

            canvas.Children.Add(e.getImage());
            Canvas.SetTop(e.getImage(), x);
            Canvas.SetLeft(e.getImage(), y);
            return e;
        }

        public void UpdateGUIAmpeln()
        {
            // ampelthread abfragen und an gui leiten



            for (int tmp = 1; tmp <= trafficlight.getAmpelAnzahl(); tmp++)
            {
                int mycase = trafficlight.getAmpelStatus(tmp);
                this.env_ah.updateAmpel(tmp-1,mycase);

                switch (mycase)
                {                  

                    case 0:
                        ah.setRed(tmp - 1);
                        break;
                    case 1:
                        ah.setYellow(tmp - 1);
                        break;
                    case 2:
                        ah.setGreen(tmp - 1);
                        break;
                    case 3:
                        ah.yellowBlinky(tmp - 1);
                        break;
                    default:
                        Console.WriteLine("nicht gehandelter status");
                        break;
                }

                
            }
            //Console.Clear();
            //this.env_ah.printStatus();
            
            //this.env_ah.updateKreuzungen();
        }

        public bool isObstacleInMyWay(int x, int y)
        {
            return this.oh.checkObstacles(x, y);
        }

        public EnvElement.StreetType getStreetType(int x, int y) //geht
        {

            if (x > 600) { x = 600; }
            else if (x < 0) { x = 0; }

            if (y > 600) { y = 600; }
            else if (y < 0) { y = 0; }
            return this.elems[x / 100, y / 100].getStreetType();
        }

        public Streetelem getStreetElement(int x, int y)
        {
            if (x > 600) { x = 600; }
            else if (x < 0) { x = 0; }

            if (y > 600) { y = 600; }
            else if (y < 0) { y = 0; }
            return this.elems[x / 100, y / 100];
        }

        public int getAmpelID(int x, int y) //geht
        {

            if (x > 600) { x = 600; }
            else if (x < 0) { x = 0; }

            if (y > 600) { y = 600; }
            else if (y < 0) { y = 0; }
            return this.elems[x / 100, y / 100].getAmpelID();
        }

        public List<EntryPoint> getEnvironmentEntries()
        {
            return this.entrypoints;
        }

        /// <summary>
        /// Methode für Hannes zum hinzufügen der Obstacles zu meiner liste
        /// </summary>
        /// <param name="startx"></param>
        /// <param name="starty"></param>
        /// <param name="endx"></param>
        /// <param name="endy"></param>
        public void addObstacle(int startx, int starty, int endx, int endy)
        {
            //Console.WriteLine("obstacle hinzugefügt bei: X=" + startx + " / Y=" + starty);
            this.oh.addObstacle(new Obstacle(startx, starty, endx, endy));
        }

        /// <summary>
        /// gibt zurück ob die position x,y ausserhalb des spielfeldes ist ( für das ummelden der Fahrzeuge auf ein neues environment)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool isOutside(int x, int y)
        {
            int MAX = 600;
            int MIN = 0;

            if (x >=MAX || x <= MIN)
            {
                return true;
            }
            else if(y>=MAX || y <= MIN)
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Gibt den Staßentyp zurück
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int getNeededEnvironmentRules(int x, int y)
        {
            return (int)getStreetType(x, y);
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="val"></param>
        public void setAmpelanlage(bool val)
        {
            this.env_ah.setOffline(val);
            this.UpdateGUIAmpeln();
        }

        /// <summary>
        /// Funktion zum ahbolen der Streetinfo für das Modul Verkehrsteilnehmer (Andras)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public StreetInfo getNeededStreetRules(int x, int y) 
        {

            
            StreetInfo info = new StreetInfo();
       
            info.type = (int)getStreetType(x, y);


            if(info.type == 2)
            {

            }
            //Console.WriteLine("abgefragter Streeettype {0}{1} : " + getStreetType(x, y).ToString(), x,y);

            if ((int)getStreetElement(x, y).getStreetType() == 1)
            {
                info.layout = getStreetElement(x, y).getRotation();
            }
            else
            {
                info.layout = -1; // keine straße. kreuzungen werden weiter unten behandelt
            }

            info.steigungHorizontal = -1; // NI
            info.steigungVertical = -1; // NI
            
            Kreuzung kreuzung = null;

            //Console.WriteLine("ampelid:" + this.getAmpelID(x, y));
            if (this.getAmpelID(x, y) > -1)
            {
                kreuzung = this.env_ah.getKreuzung(this.getAmpelID(x, y));


                if (kreuzung.s_status == -1)
                {
                    info.layout = 4;
                }
                if (kreuzung.n_status == -1)
                {
                    info.layout = 2;
                }
                if (kreuzung.w_status == -1)
                {
                    info.layout = 3;
                }
                if (kreuzung.e_status == -1)
                {
                    info.layout = 1;
                }
                
                info.ampelstatusDown = kreuzung.s_status;
                info.ampelstatusLeft = kreuzung.w_status;
                info.ampelstatusRight = kreuzung.e_status;
                info.ampelstatusUp = kreuzung.n_status;

                //if (kreuzung.getID() == 0)
                //{
                //    Console.Clear();
                //    Console.WriteLine("\nn " + kreuzung.n_status + "\ne " + kreuzung.e_status + "\ns " + kreuzung.s_status + "\nw " + kreuzung.w_status);
                //    // 0 = Rot, 1 = Gelb, 2 = Grün, 3 = Ausfall
                //}
            }
            else
            {
                info.ampelstatusDown = 3;
                info.ampelstatusLeft = 3;
                info.ampelstatusRight = 3;
                info.ampelstatusUp = 3;
            }

            //Console.WriteLine("layout" + info.layout);

            return info;
        }

        /// <summary>
        /// Gibt die liste der obstacles zurück
        /// </summary>
        /// <returns></returns>
        public List<Obstacle> getObstacles()
        {
            return oh.getObstacles();
        }
    }

    /// <summary>
    /// Abbildung der Ein/Austrittspunkte
    /// </summary>
    public class EntryPoint
    {
        public int TileX { get; set; }
        public int TileY { get; set; }

        public EntryPoint(int xtile, int ytile)
        {
            this.TileX = xtile;
            this.TileY = ytile;
        }
    }

    /// <summary>
    /// Straßeninfo zum übergeben an die Fahrzeuge
    /// </summary>
    public class StreetInfo
    {
        public int type { get; set; }
        public int layout { get; set; } //only 3 Kreuzung, gerade Straße, set -1 else
        public int ampelstatusUp { get; set; }
        public int ampelstatusDown { get; set; }
        public int ampelstatusLeft { get; set; }
        public int ampelstatusRight { get; set; }
        public double steigungHorizontal { get; set; } //----->
        public double steigungVertical { get; set; } // ^

        //Erklärung Layout: (muss so sein!!)
        //gerade Straße:
        //1: ===========
        //2: ||
        //   ||
        //   ||
        //   ||
        //   ||
        //   ||
        //3er Kreuzung:
        //1:
        //    ||
        // ===
        //    ||
        //2:
        // ===  ===
        //    ||
        //3:
        // ||
        //   ===
        // ||
        //4:
        //    ||
        // ===  ===
    }

    

}
