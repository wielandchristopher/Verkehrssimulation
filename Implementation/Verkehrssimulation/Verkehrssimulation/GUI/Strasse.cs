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
    class Strasse : ObjectInterface
    {
        int id;
        Shape shp;
        //Shape vertical;
        //Shape horizontal;
        int posx, posy;

        public int objid { get { return id; } set { id = value; } }
        public int xpos { get { return posx; } set { posx = value; } }
        public int ypos { get { return posy; } set { posy = value; } }
        //public Shape objvertical { get { return vertical; } set { vertical = value; } }
        //public Shape objhorizontal { get { return horizontal; } set { horizontal = value; } }
        public Shape objshp { get { return shp; } set { shp = value; } }

        public Strasse(double x, double y, int newid)
        {
            shp = new Rectangle();
            shp.Width = 250;
            shp.Height = 50;
            shp.Fill = new SolidColorBrush(Colors.Gray);
            id = newid;
            Canvas.SetTop(shp, x);
            Canvas.SetLeft(shp, y);
            /*
            // Horizontale Straße
            horizontal = new Rectangle();
            horizontal.Width = 250;
            horizontal.Height = 50;
            horizontal.Fill = new SolidColorBrush(Colors.Gray);
            id = newid;
            Canvas.SetTop(horizontal, x);
            Canvas.SetLeft(horizontal, y);
            
            // Vertikale Straße
            vertical = new Rectangle();
            vertical.Width = 50;
            vertical.Height = 250;
            vertical.Fill = new SolidColorBrush(Colors.Gray);
            id = newid;
            Canvas.SetTop(vertical, x);
            Canvas.SetLeft(vertical, y);*/
        }

        public Shape getShape()
        {
            return this.shp;
        }
    }
}
