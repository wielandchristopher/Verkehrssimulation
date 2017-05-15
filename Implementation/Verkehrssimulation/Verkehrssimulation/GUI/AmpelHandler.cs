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

        public void setRed(int id)
        {
            foreach (Ampel a in ampellist)
            {
                if (a.objid == id)
                {

                    Shape yellow = a.getCircleYellow();
                    yellow.Fill = new SolidColorBrush(Colors.White);

                    Shape green = a.getCircleGreen();
                    green.Fill = new SolidColorBrush(Colors.White);

                    Shape red = a.getCircleRed();
                    red.Fill = new SolidColorBrush(Colors.Red);

                    a.green = false;
                    a.yellow = false;
                    a.red = true;

                }
            }
        }
        public void setGreen(int id)
        {
            foreach (Ampel a in ampellist)
            {
                if (a.objid == id)
                {

                    Shape yellow = a.getCircleYellow();
                    yellow.Fill = new SolidColorBrush(Colors.White);

                    Shape green = a.getCircleGreen();
                    green.Fill = new SolidColorBrush(Colors.Green);

                    Shape red = a.getCircleRed();
                    red.Fill = new SolidColorBrush(Colors.White);

                    a.green = true;
                    a.yellow = false;
                    a.red = false;

                }
            }
        }
        public void setYellow(int id)
        {
            foreach (Ampel a in ampellist)
            {
                if (a.objid == id)
                {

                    Shape yellow = a.getCircleYellow();
                    yellow.Fill = new SolidColorBrush(Colors.Yellow);

                    Shape green = a.getCircleGreen();
                    green.Fill = new SolidColorBrush(Colors.White);

                    Shape red = a.getCircleRed();
                    red.Fill = new SolidColorBrush(Colors.White);

                    a.green = false;
                    a.yellow = true;
                    a.red = false;

                }
            }
        }


        public void setNext(int id)
        {
            foreach (Ampel a in ampellist)
            {
                if (a.objid == id)
                {
                    if (a.yellow == true && a.red == true)
                    {
                        // turn off yellow light
                        Shape yellow = a.getCircleYellow();
                        yellow.Fill = new SolidColorBrush(Colors.White);

                        // turn on green light
                        Shape green = a.getCircleGreen();
                        green.Fill = new SolidColorBrush(Colors.Green);

                        a.green = true;
                        a.yellow = false;
                        a.red = false;
                    }
                    else if (a.yellow == true && a.green == true)
                    {
                        // turn off yellow light
                        Shape yellow = a.getCircleYellow();
                        yellow.Fill = new SolidColorBrush(Colors.White);

                        // turn on red light
                        Shape red = a.getCircleRed();
                        red.Fill = new SolidColorBrush(Colors.Red);

                        a.red = true;
                        a.yellow = false;
                        a.green = false;
                    }
                    else if (a.red == true)
                    {
                        // turn off red light
                        Shape red = a.getCircleRed();
                        red.Fill = new SolidColorBrush(Colors.White);

                        // turn on yellow light
                        Shape yellow = a.getCircleYellow();
                        yellow.Fill = new SolidColorBrush(Colors.Yellow);

                        a.yellow = true;
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

        public void yellowBlinky(int id)
        {
            foreach (Ampel a in ampellist)
            {

                if (a.objid == id)
                {
                    Shape yellow = a.getCircleYellow();
                    Shape red = a.getCircleRed();
                    red.Fill = new SolidColorBrush(Colors.White);

                    Shape green = a.getCircleGreen();
                    green.Fill = new SolidColorBrush(Colors.White);

                    a.green = false;
                    a.red = false;

                    if (a.yellow == true)
                    {
                        yellow.Fill = new SolidColorBrush(Colors.White);
                        a.yellow = false;
                    }
                    else if(a.yellow == false)
                    {
                        yellow.Fill = new SolidColorBrush(Colors.Yellow);
                        a.yellow = true;

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
