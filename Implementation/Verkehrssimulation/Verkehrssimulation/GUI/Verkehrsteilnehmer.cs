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
    class Verkehrsteilnehmer
    {
        Shape shp;
        int posx, posy;
        int id;
        int direction;


        public Verkehrsteilnehmer(double x,double y, int newid) 
        {
            shp = new Rectangle(); // shape kann alles sein Rect,Ellipse,Polygon,...

            /* EVENTS */
            shp.MouseEnter += enter;
            shp.MouseLeave += leave;
            shp.MouseDown += consoleMsg;


            shp.Width = 5;
            shp.Height = 5;
            shp.Fill = new SolidColorBrush(Colors.DarkBlue);
            
            id = newid;
            Canvas.SetTop(shp, x);
            Canvas.SetLeft(shp, y);
        }

        public Verkehrsteilnehmer(double x, double y, int direction, int newid)
        {
            shp = new Rectangle(); // shape kann alles sein Rect,Ellipse,Polygon,...

            /* EVENTS */
            shp.MouseEnter += enter;
            shp.MouseLeave += leave;
            shp.MouseDown += consoleMsg;


            shp.Width = 5;
            shp.Height = 10;
            shp.Fill = new SolidColorBrush(Colors.Purple);

            id = newid;
            Canvas.SetTop(shp, x);
            Canvas.SetLeft(shp, y);
        }

        public void update(int x, int y)
        {
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
        
        public int getID()
        {
            return id;
        }

        public void enter(object sender, EventArgs e)
        {
            ((Shape)sender).ToolTip = id;
            ((Shape)sender).Width = 25;
            ((Shape)sender).Height = 25;

        }

        public void leave(object sender, EventArgs e)
        {
            ((Shape)sender).Width = 5;
            ((Shape)sender).Height = 5;

        }

        public void consoleMsg(object sender, EventArgs e)
        {
            Console.WriteLine("top - " + Canvas.GetTop(shp));
            Console.WriteLine("left - " + Canvas.GetLeft(shp));
            Console.WriteLine("Das Auto mit der ID " + this.id );

        }
        
    }
}
