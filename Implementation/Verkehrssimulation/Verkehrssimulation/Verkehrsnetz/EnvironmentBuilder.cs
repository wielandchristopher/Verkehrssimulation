using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Verkehrssimulation.Verkehrsnetz
{
    class EnvironmentBuilder
    {
        JObject obj; // für json -> Projekt-> nu-getpakete verwalten -> json linq irgendwas
        private Canvas canvas;
        List<Image> imgs;
        public EnvironmentBuilder(Canvas mycanvas)
        {
            canvas = mycanvas;
            Console.WriteLine("Buidler loaded");
             imgs = new List<Image>();
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
            //foreach(JObject obj in arr)
            //{
            //    addObject(obj.GetValue("xpos").Value<double>(), obj.GetValue("ypos").Value<double>());
            //}

            for(int i = 0; i<700; i+=100)
            {
                for(int y = 0; y<700; y+=100)
                {
                    addObject(i, y);
                }
            }
 

            //Console.WriteLine("Geregelte Kreuzungen:");
            //Console.WriteLine(((JObject)(obj.GetValue("geregelte_kreuzungen")[0])).GetValue("xpos"));

            //Console.WriteLine("Ungeregelte Kreuzungen:");
            //Console.WriteLine(obj.GetValue("ungeregelte_kreuzungen"));

            //Console.WriteLine("Schilder Kreuzungen:");
            //Console.WriteLine(obj.GetValue("schilder"));

        }

        public void addObject(double x, double y)
        {
            // Create the image element.
            Image simpleImage = new Image();
            simpleImage.Width = 100;
            simpleImage.Height = 100;
            simpleImage.MouseEnter += enter;
            simpleImage.MouseLeave += leave;

            // Create source.
            BitmapImage bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri(@"/Verkehrsnetz/4kreuzung.bmp", UriKind.RelativeOrAbsolute);
            bi.EndInit();

            imgs.Add(simpleImage);

            canvas.Children.Add(simpleImage);
            Console.WriteLine(x);
            Canvas.SetTop(simpleImage, x);
            Canvas.SetLeft(simpleImage, y);
            // Set the image source.
            simpleImage.Source = bi;
        }

        public void enter(object sender, EventArgs e)
        {
            ((Image)sender).Opacity = 0.9;
        }

        public void leave(object sender, EventArgs e)
        {
            ((Image)sender).Opacity = 1;
        }



    }
}
