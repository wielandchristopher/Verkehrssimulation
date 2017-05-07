using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;


namespace Verkehrssimulation.Verkehrsnetz
{
    class EnvironmentBuilder 
    {
        JObject obj; // für json -> Projekt-> nu-getpakete verwalten -> json linq irgendwas
        private Canvas canvas;
        List<Streetelem> elem;
        GUI.AmpelHandler ah;
        AmpelHandler.AmpelHandler extah;
        Streetelem[,] elems = new Streetelem[7, 7];
        int ampelcnt = 0;

        public EnvironmentBuilder(Canvas mycanvas, ref GUI.AmpelHandler _ah, ref AmpelHandler.AmpelHandler _extah)
        {
            ah = _ah;
            extah = _extah;
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
                ah.addTrafficLight(xpos + 30, ypos + 60, 2, ampelcnt++);
                ah.addTrafficLight(xpos + 7, ypos + 30, 3, ampelcnt++);
                ah.addTrafficLight(xpos + 60, ypos + 7, 4, ampelcnt++);

                addSolution(xpos,ypos);


            }

            for(int i = 0; i< ampelcnt; i++)
            {
                ah.setNext(i);
                ah.setNext(i);
                ah.setNext(i);
                ah.setNext(i);
                ah.setNext(i);
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
                        elems[x, y] = addObject(x * 100, y * 100, 4);
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
                        elems[i / 100, ypos / 100] = addObject(i, ypos, 1);
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
                        elems[xpos / 100, i / 100] = addObject(xpos, i, 1);
                    }
                    
                }

            }

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

    }
}
