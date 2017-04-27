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
            //LoadJson();
            LoadEnvironment();
        }

        public void LoadJson()
        {
            
            using (StreamReader r = new StreamReader("env_config.json"))
            {                
                string json = r.ReadToEnd();
                obj = JObject.Parse(json);
            }
        }

        public void LoadEnvironment()
        {
            //JArray arr = (JArray)obj.GetValue("geregelte_kreuzungen");
            //foreach (JObject obj in arr)
            //{
            //    addObject(obj.GetValue("xpos").Value<double>(), obj.GetValue("ypos").Value<double>());
            //}

            for (int i = 0; i<700; i+=100)
            {
                for(int y = 0; y<700; y+=100)
                {
                    addObject(i, y);
                }
            }


        }

        public void addObject(int x, int y)
        {

            //StreetType { Street = 1, ThreeKreuzung = 2, FourKreuzung = 3, Grass = 4 };
            EnvElement e = new Streetelem(x,y,1,3);
            elem.Add(e);

            canvas.Children.Add(e.getImage());
            Canvas.SetTop(e.getImage(), x);
            Canvas.SetLeft(e.getImage(), y);        
        }
    }
}
