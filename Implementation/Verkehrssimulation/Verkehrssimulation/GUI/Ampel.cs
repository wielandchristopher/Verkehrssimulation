using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Verkehrssimulation.GUI
{
    class Ampel : AmpelInterface
    {
        int id;
        Shape shp;
        Shape green;
        Shape red;
        int posx, posy;
        int dir; // 1 = vertical, 2 = horizontal, 3 = horizontal swapped, 3 = vertical swapped

        public int objid { get { return id; } set { id = value; } }
        public int xpos { get { return posx; } set { posx = value; } }
        public int ypos { get { return posy; } set { posy = value; } }
        public int objdir { get { return dir; } set { dir = value; } }
        public Shape objshp { get { return shp; } set { shp = value; } }
        public Shape greenLight { get { return green; } set { green = value; } }
        public Shape redLight { get { return red; } set { red = value; } }

        public Ampel(double x, double y, int dir)
        {             
            // grün
            green = new Ellipse();
            //id = newid;
            green.Width = 8;
            green.Height = 8;
            green.Fill = new SolidColorBrush(Colors.Green);

            // rot
            red = new Ellipse();
            red.Width = 8;
            red.Height = 8;
            red.Fill = new SolidColorBrush(Colors.Red);
            //id = newid;

            // shape
            shp = new Rectangle();
            //id = newid;
            shp.Fill = new SolidColorBrush(Colors.Black);
            Canvas.SetTop(shp, x);
            Canvas.SetLeft(shp, y);

            if (dir == 1)
            {
                Canvas.SetTop(green, x + 11);
                Canvas.SetLeft(green, y + 1);

                Canvas.SetTop(red, x + 1);
                Canvas.SetLeft(red, y + 1);

                shp.Width = 10;
                shp.Height = 22;
            }

            if (dir == 2)
            {
                Canvas.SetTop(green, x + 1);
                Canvas.SetLeft(green, y + 11);

                Canvas.SetTop(red, x + 1);
                Canvas.SetLeft(red, y + 1);

                shp.Width = 22;
                shp.Height = 10;
            }


            if (dir == 3)
            {
                Canvas.SetTop(green, x + 1);
                Canvas.SetLeft(green, y + 1);

                Canvas.SetTop(red, x + 11);
                Canvas.SetLeft(red, y + 1);

                shp.Width = 10;
                shp.Height = 22;
            }

            if (dir == 4)
            {
                Canvas.SetTop(green, x + 1);
                Canvas.SetLeft(green, y + 1);

                Canvas.SetTop(red, x + 1);
                Canvas.SetLeft(red, y + 11);

                shp.Width = 22;
                shp.Height = 10;
            }
        }

        public Shape getShape()
        {
            return this.shp;
        }

        public Shape getCircleGreen()
        {
            return this.green;
        }

        public Shape getCircleRed()
        {
            return this.red;
        }
    }
}
