﻿using Ampelsteuerung;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using Verkehrssimulation.Verkehrsteilnehmer;
using Verkehrssimulation;

namespace Verkehrssimulation.Verkehrsnetz
{
    class EnvironmentBuilder : ITeilnehmer
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
        List<Obstacle> obstacles;

        public EnvironmentBuilder(Canvas mycanvas, ref GUI.AmpelHandler _ah, ref IAmpelService _trafficlight)
        {

            ah = _ah;
            trafficlight = _trafficlight;

            canvas = mycanvas;
            Console.WriteLine("Buidler loaded");

            entrypoints = new List<EntryPoint>();
            elem = new List<Streetelem>();
            LoadJson();
            LoadEnvironment();

            obstacles = new List<Obstacle>();
            obstacles.Add(new Obstacle(250, 250, 260, 260));
            obstacles.Add(new Obstacle(350, 350, 360, 360));
        }


        public void LoadJson()
        {

            using (StreamReader r = new StreamReader("../../Verkehrsnetz/env_config.json"))
            {
                string json = r.ReadToEnd();
                obj = JObject.Parse(json);
            }
        }

        public void LoadEnvironment()
        {
            JArray geregelte_kreuzungen = (JArray)obj.GetValue("geregelte_kreuzungen");
            int xpos, ypos = 0;

            Console.WriteLine("geregelte_kreuzungen.Count: " + geregelte_kreuzungen.Count);
            env_ah = new Env_Ampelhandler(geregelte_kreuzungen.Count*4);
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
                xpos = obj.GetValue("xpos").Value<int>();
                ypos = obj.GetValue("ypos").Value<int>();

                elems[xpos / 100, ypos / 100] = addObject(xpos, ypos, 3);

                ah.addTrafficLight(xpos + 60, ypos + 60, 1, ampelcnt++);
                ah.addTrafficLight(xpos + 30, ypos + 60, 2, ampelcnt++);
                ah.addTrafficLight(xpos + 7, ypos + 30, 3, ampelcnt++);
                ah.addTrafficLight(xpos + 60, ypos + 7, 4, ampelcnt++);

                addSolution(xpos, ypos);
            }
            fillWithGrass();
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

        int alternate = 0;
        private int ampelidconnector = 0;

        public void alternateLight()
        {
            int x = 0;
            while (x < ampelcnt)
            {
                ah.yellowBlinky(x);

                /*ah.setNext(x);

                if (alternate % 2 == 0)
                {
                    if (x % 2 == 0)
                    {
                        ah.setNext(x);
                    }
                    else
                    {
                        ah.setNext(x);
                    }
                }
                else
                {
                    if (x % 2 == 0)
                    {
                        
                        ah.setNext(x);
                    }
                    else
                    {
                        ah.setNext(x);
                    }
                }*/

                x++;
            }

            alternate++;
            Console.WriteLine(alternate);
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


        public void UpdateGUIAmpeln()
        {
            // ampelthread abfragen und an gui leiten

            for (int tmp = 1; tmp <= trafficlight.getAmpelAnzahl(); tmp++)
            {

                switch (trafficlight.getAmpelStatus(tmp))
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
                        Console.WriteLine("ausfall oder ausgeschaltet");
                        break;
                    default:
                        Console.WriteLine("nicht gehandelter status");
                        break;
                }
                //Console.WriteLine("trafficlight.getAmpelStatus(" + tmp + "): " + trafficlight.getAmpelStatus(tmp));
            }
        }


        public EnvElement.StreetType getStreetType(int x, int y) //geht
        {
            //holt den straßentyp und die ausrichtung vom aktuellen feld wo das auto fährt
            //Console.WriteLine(this.elems[x / 100, y / 100].getStreetType().ToString());

            if (x > 600) { x = 600; }
            else if (x < 0) { x = 0; }

            if (y > 600) { y = 600; }
            else if (y < 0) { y = 0; }
            return this.elems[x / 100, y / 100].getStreetType();
        }

        public int getAmpelID(int x, int y) //geht
        {
            //holt den straßentyp und die ausrichtung vom aktuellen feld wo das auto fährt
            //Console.WriteLine(this.elems[x / 100, y / 100].getStreetType().ToString());

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

        public List<Obstacle> getObstacles()
        {
            return this.obstacles;
        }

        public void getRules()
        {
            // holt die regeln
        }



        public int getNeededEnvironmentRules(int x, int y)
        {
            return (int)getStreetType(x, y);
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
            info.layout = -1; // NI
            info.steigungHorizontal = -1; // NI
            info.steigungVertical = -1; // NI
            
            FKreuzung kreuzung = null;
            if (this.getAmpelID(x, y) > -1)
            {
                kreuzung = this.env_ah.getKreuzung(this.getAmpelID(x, y));
                info.ampelstatusDown = kreuzung.s_status;
                info.ampelstatusLeft = kreuzung.w_status;
                info.ampelstatusRight = kreuzung.e_status;
                info.ampelstatusUp = kreuzung.n_status;
            }

            return info;
        }
    }

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

    public class Obstacle
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }

        public Obstacle(int startx, int starty, int endx, int endy)
        {
            this.StartX = startx;
            this.StartY = starty;
            this.EndX = endx;
            this.EndY = endy;
        }
    }

    public class StreetInfo
    {
        public int type { get; set; }
        public int layout { get; set; } //only 3 Kreuzung, set null else
        public int ampelstatusUp { get; set; } // 1 -> rot, 2-> gelb, 3 -> rot //TODO please make an enum null -> no ampel,
        public int ampelstatusDown { get; set; }
        public int ampelstatusLeft { get; set; }
        public int ampelstatusRight { get; set; }
        public double steigungHorizontal { get; set; } //----->
        public double steigungVertical { get; set; } // ^

        //TODO getter und setter
    }

    public class FKreuzung
    {
        private int id { get; set; }
        public int n_status { get; set; }
        public int s_status { get; set; }
        public int w_status { get; set; }
        public int e_status { get; set; }

        private int idn, ids, idw, ide;

        public int getID()
        {
            return this.id;
        }

        public FKreuzung(int id , int idn, int ids, int idw, int ide)
        {

            // 0 = Rot, 1 = Gelb, 2 = Grün, 3 = Ausfall
            this.id = id;
            this.idn = idn;
            this.ids = ids;
            this.idw = idw;
            this.ide = ide;


        }

        public void Update()
        {
            n_status = MainWindow.trafficlight.getAmpelStatus(idn);
            s_status = MainWindow.trafficlight.getAmpelStatus(ids);
            w_status = MainWindow.trafficlight.getAmpelStatus(idw);
            e_status = MainWindow.trafficlight.getAmpelStatus(ide);
        }


    }

    public class Env_Ampelhandler
    {

        private List<FKreuzung> kreuzungen;
        private int cnt = 0;

        public Env_Ampelhandler(int ampelcnt)
        {
            
            
            kreuzungen = new List<FKreuzung>();
            cnt = ampelcnt;

            MainWindow.trafficlight.setAmpelAnzahl(cnt);
            

            int x = 1;
            int newID = 0;
            while (x < cnt+1)
            {
                kreuzungen.Add(new FKreuzung(newID, x, x + 1, x + 2, x + 3));
                x = x + 4;
                newID++;
            }
        }

        public FKreuzung getKreuzung(int id)
        {
            foreach(FKreuzung k in kreuzungen)
            {
                if (k.getID() == id)
                {
                    k.Update();
                    
                    return k; // wenn gefunden
                }
            }
            return null; // wenn nicht vorhanden
        }
    }

    public class ObstacleHandler
    {
        private List<Obstacle> obstacles;

        public ObstacleHandler()
        {
            obstacles = new List<Obstacle>();
        }

        public void addObstacle(Obstacle obs)
        {
            obstacles.Add(obs);
        }

        public bool inArea(int x, int y)
        {
            return false;
        }

        public bool checkObstacles(int x, int y)
        {
            foreach(Obstacle obs in obstacles)
            {
                if(inArea(x, y))
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
