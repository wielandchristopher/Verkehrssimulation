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
    class Testobject : ObjectInterface 
    {
        Shape shp;
        int posx, posy;
        int direction;
        int id;

        public int objid { get { return id; } set { id = value; }} // not needed - tested für zugriff from Obj interface
        public int xpos  { get { return posx; } set { posx = value; }}
        public int ypos  { get { return posy; } set { posy = value; }}
        public Shape objshp { get { return shp; } set { shp = value; }}


        public Testobject(double x,double y, int cardirection, int newid) 
        //cardirection braucht die gui dann nicht! nur für test
        {
            shp = new Rectangle(); // shape kann alles sein Rect,Ellipse,Polygon,...
            shp.Width = 25;
            shp.Height = 25;
            shp.Fill = new SolidColorBrush(Colors.Black);
            direction = cardirection;
            id = newid;
            Canvas.SetTop(shp, x);
            Canvas.SetLeft(shp, y);
        }

        public Shape getShape()
        {
            return this.shp;
        }

        public void setColor(Color c)
        {
            this.shp.Fill = new SolidColorBrush(c);
        }

        public void update(int move)
        {
            if (direction == 1)
            {
                Canvas.SetTop(shp, Canvas.GetTop(shp) + move); // getTop später nicht notwendig da pos genau kommt
            }
            if (direction == 2)
            {
                Canvas.SetTop(shp, Canvas.GetTop(shp) - move);
            }
            if (direction == 3)
            {
                Canvas.SetLeft(shp, Canvas.GetLeft(shp) - move);
            }
            if (direction == 4)
            {
                Canvas.SetLeft(shp, Canvas.GetLeft(shp) + move);

            }
        }

        public int getID()
        {
            return id;
        }

    }
}
