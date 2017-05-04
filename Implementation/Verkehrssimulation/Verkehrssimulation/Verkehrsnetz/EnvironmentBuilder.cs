using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Verkehrssimulation.Verkehrsnetz
{
    class EnvironmentBuilder 
    {
        JObject obj; // für json -> Projekt-> nu-getpakete verwalten -> json linq irgendwas
        private Canvas canvas;
        List<EnvElement> elem;
        public EnvironmentBuilder(Canvas mycanvas)
        {            
            canvas = mycanvas;
            Console.WriteLine("Buidler loaded");
            elem = new List<EnvElement>();
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
            JArray ungeregelte_kreuzungen = (JArray)obj.GetValue("ungeregelte_kreuzungen");
            int xpos, ypos = 0;

            foreach (JObject obj in geregelte_kreuzungen)
            {
                xpos = obj.GetValue("xpos").Value<int>();
                ypos = obj.GetValue("ypos").Value<int>();
                addObject(xpos, ypos);

                addAmpel(xpos + 60, ypos + 60, 1);
                addAmpel(xpos + 30, ypos + 60, 2);
                addAmpel(xpos + 7, ypos + 30, 3);
                addAmpel(xpos + 60, ypos + 7, 4);
            }

            foreach (JObject obj in ungeregelte_kreuzungen)
            {
                addObject(obj.GetValue("xpos").Value<int>(), obj.GetValue("ypos").Value<int>());
            }

            /*for (int i = 0; i<700; i+=100)
            {
                for(int y = 0; y<700; y+=100)
                {
                    addObject(i, y);
                }
            }*/


        }

        public void addObject(int x, int y)
        {

            //StreetType { Street = 1, ThreeKreuzung = 2, FourKreuzung = 3, Grass = 4 };
            EnvElement e = new Streetelem(x, y, 1, 3);
            elem.Add(e);

            canvas.Children.Add(e.getImage());
            Canvas.SetTop(e.getImage(), x);
            Canvas.SetLeft(e.getImage(), y);
        }

        public void addAmpel(int x, int y, int dir)
        {
            GUI.Ampel a = new GUI.Ampel(x, y, dir, 1);
            canvas.Children.Add(a.getShape());
            canvas.Children.Add(a.getCircleGreen());
            canvas.Children.Add(a.getCircleYellow());
            canvas.Children.Add(a.getCircleRed());
        }
    }
}
