using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Verkehrssimulation.GUI
{
    class AmpelHandler
    {
        List<Ampel> ampellist;
        Canvas canvas;

        public AmpelHandler(Canvas mycanvas)
        {
            ampellist = new List<Ampel>();
            canvas = mycanvas;
        }

        public bool addTrafficLight(int posx, int posy, int dir, int id)
        {
            Ampel ampel = new Ampel(posx, posy, dir, id);
            ampellist.Add(ampel);

            canvas.Children.Add(ampel.getShape());
            canvas.Children.Add(ampel.getCircleGreen());
            canvas.Children.Add(ampel.getCircleYellow());
            canvas.Children.Add(ampel.getCircleRed());
            return true;
        }

        public void setRedLight(int id)
        {
            foreach (Ampel a in ampellist)
            {
                if (a.objid == id)
                {
                    // turn off yellow light
                    Shape yellow = a.getCircleYellow();
                    yellow.Fill = new SolidColorBrush(Colors.White);

                    // turn on red light
                    Shape red = a.getCircleRed();
                    red.Fill = new SolidColorBrush(Colors.Red);

                    a.red = true;
                    a.yellow = false;
                }
                else
                {
                    new Exception("Die Ampel mit der ID:" + id + "wurde nicht gefunden");
                }
            }
        }

        public void setGreenLight(int id)
        {
            foreach (Ampel a in ampellist)
            {
                if (a.objid == id)
                {
                    // turn off yellow light
                    Shape yellow = a.getCircleYellow();
                    yellow.Fill = new SolidColorBrush(Colors.White);

                    // turn on green light
                    Shape green = a.getCircleGreen();
                    green.Fill = new SolidColorBrush(Colors.Green);

                    a.green = true;
                    a.yellow = false;
                }
                else
                {
                    new Exception("Die Ampel mit der ID:" + id + "wurde nicht gefunden");
                }
            }
        }

        public void setYellowLight(int id)
        {
            foreach (Ampel a in ampellist)
            {
                if (a.objid == id)
                {
                    if (a.red == true)
                    {
                        // turn off red light
                        Shape red = a.getCircleRed();
                        red.Fill = new SolidColorBrush(Colors.White);

                        // turn on yellow light
                        Shape yellow = a.getCircleYellow();
                        yellow.Fill = new SolidColorBrush(Colors.Yellow);

                        a.yellow = true;
                        a.red = false;
                    }
                    else if (a.green == true)
                    {
                        // turn off green light
                        Shape green = a.getCircleGreen();
                        green.Fill = new SolidColorBrush(Colors.White);

                        // turn on yellow light
                        Shape yellow = a.getCircleYellow();
                        yellow.Fill = new SolidColorBrush(Colors.Yellow);

                        a.yellow = true;
                        a.green = false;
                    }
                    else
                    {
                        new Exception("No traffic light has been set");
                    }
                }
                else
                {
                    new Exception("Die Ampel mit der ID:" + id + "wurde nicht gefunden");
                }

            }
        }

        //private void initialize()
        //{
        //    int j = 0;
        //    // Ampel Kreuzung unten
        //    for (int i = 60; i < 700; i += 100)
        //    {
        //        for (int y = 60; y < 700; y += 100)
        //        {
        //            addAmpel(new Ampel(i, y, 1, j+1));
        //        }
        //    }

        //    // Ampel Kreuzung rechts
        //    for (int i = 30; i < 700; i += 100)
        //    {
        //        for (int y = 60; y < 700; y += 100)
        //        {
        //            addAmpel(new Ampel(i, y, 2, j+1));
        //        }
        //    }

        //    // Ampel Kreuzung oben
        //    for (int i = 7; i < 700; i += 100)
        //    {
        //        for (int y = 30; y < 700; y += 100)
        //        {
        //            addAmpel(new Ampel(i, y, 3, j+1));
        //        }
        //    }

        //    // Ampel Kreuzung links
        //    for (int i = 60; i < 700; i += 100)
        //    {
        //        for (int y = 7; y < 700; y += 100)
        //        {
        //            Ampel a = new Ampel(i, y, 4, j+1);
        //            addAmpel(a);
        //        }
        //    }
        //}
    }
}
