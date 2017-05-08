using Ampelsteuerung;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;


namespace Verkehrssimulation.Verkehrsnetz
{
    class EnvironmentBuilder : IF_Teilnehmer
    {
        JObject obj; // für json -> Projekt-> nu-getpakete verwalten -> json linq irgendwas
        private Canvas canvas;
        List<Streetelem> elem;
        GUI.AmpelHandler ah;
        IAmpelService trafficlight;
        Streetelem[,] elems = new Streetelem[7, 7];
        int ampelcnt = 0;

        public EnvironmentBuilder(Canvas mycanvas, ref GUI.AmpelHandler _ah, ref IAmpelService _trafficlight)
        {
            ah = _ah;
            trafficlight = _trafficlight;
            
            canvas = mycanvas;
            Console.WriteLine("Buidler loaded");
            
            elem = new List<Streetelem>();
            LoadJson();
            LoadEnvironment();
       
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

            foreach (JObject obj in geregelte_kreuzungen)
            {
                xpos = obj.GetValue("xpos").Value<int>();
                ypos = obj.GetValue("ypos").Value<int>();
                
                elems[xpos/100,ypos/100] = addObject(xpos, ypos, 3);



                ah.addTrafficLight(xpos + 60, ypos + 60, 1,ampelcnt++);
                ah.setNext(ampelcnt - 1);
                ah.addTrafficLight(xpos + 30, ypos + 60, 2, ampelcnt++);
                ah.setNext(ampelcnt - 1);
                ah.setNext(ampelcnt - 1);
                ah.setNext(ampelcnt - 1);
                ah.addTrafficLight(xpos + 7, ypos + 30, 3, ampelcnt++);
                ah.setNext(ampelcnt - 1);

                ah.addTrafficLight(xpos + 60, ypos + 7, 4, ampelcnt++);
                ah.setNext(ampelcnt - 1);
                ah.setNext(ampelcnt - 1);
                ah.setNext(ampelcnt - 1);


                Console.WriteLine(ampelcnt);
                
                //trafficlight.setAmpelAnzahl(12);
                

                addSolution(xpos,ypos);
            }



            fillWithGrass();

            /*for (int i = 0; i<700; i+=100)
            {
                for(int y = 0; y<700; y+=100)
                {
                    addObject(i, y);
                }
            }*/


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

        private void addSolution(int xpos, int ypos)
        {
            for (int i = 0; i < 700; i += 100)
            {
                if (i != xpos) {


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
                if (i != ypos) {                   

                    if (elems[xpos / 100, i / 100] != null && ((Streetelem)elems[xpos / 100, i / 100]).getStreetType() == EnvElement.StreetType.Street)
                    {
                        ((Streetelem)elems[xpos / 100, i / 100]).updateType(EnvElement.StreetType.FourKreuzung);

                    }
                    else if(elems[xpos / 100, i / 100]==null)
                    {
                        elems[xpos / 100, i / 100] = addObject(xpos, i, 1); // correct 1
                    }
                    
                }

            }

        }

        int alternate = 0;
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
            Streetelem e = new Streetelem(x, y, 1, type);
            elem.Add(e);

            canvas.Children.Add(e.getImage());
            Canvas.SetTop(e.getImage(), x);
            Canvas.SetLeft(e.getImage(), y);
            return e;
        }


        public void UpdateGUIAmpeln()
        {
            // ampelthread abfragen und an gui leiten
        }

        public void InitAmpelThread()
        {
            // ampeln bei wieland initieeren
        }

        public void GetAmpelInfo(int id)
        {


            //Console.WriteLine("Anzahl: " + trafficlight.getAmpelAnzahl());

            //for(int x = 1; x < trafficlight.getAmpelAnzahl(); x++)
            //{
            //    trafficlight.setRotPhase(x, 2);

            //    Console.WriteLine("status von Ampel "+x+": " + trafficlight.getAmpelStatus(x));
            //}
            

        }

        public EnvElement.StreetType getStreetType(int x, int y) //geht
        {
            //holt den straßentyp und die ausrichtung vom aktuellen feld wo das auto fährt
            //Console.WriteLine(this.elems[x / 100, y / 100].getStreetType().ToString());

            if (x > 600){x = 600;}
            else if (x < 0){ x = 0;}

            if (y > 600){y = 600;}
            else if (y < 0){y = 0;}
            return this.elems[x / 100, y / 100].getStreetType();
        }

        public void getRules()
        {
            // holt die regeln
        }



        public int getNeededEnvironmentRules(int x, int y)
        {
            return (int)getStreetType(x,y);
        }
    }
}
