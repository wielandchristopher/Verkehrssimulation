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
    class Auto : ObjectInterface 
    {
        Shape shp;
        int posx, posy;
        int direction;
        int id;
        int move;

        public int objid { get { return id; } set { id = value; }} // not needed - tested für zugriff from Obj interface
        public int xpos  { get { return posx; } set { posx = value; }}
        public int ypos  { get { return posy; } set { posy = value; }}
        public Shape objshp { get { return shp; } set { shp = value; }}


        public Auto(double x,double y, int cardirection, int newid, int speed) 
        //cardirection braucht die gui dann nicht! nur für test
        {
            shp = new Rectangle(); // shape kann alles sein Rect,Ellipse,Polygon,...

            /* EVENTS */
            shp.MouseEnter += enter;
            shp.MouseLeave += leave;
            shp.MouseDown += consoleMsg;


            shp.Width = 5;
            shp.Height = 5;
            shp.Fill = new SolidColorBrush(Colors.Black);
            direction = cardirection;
            move = speed;
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

        public void update(Random random)
        {
            int predirection = direction;
            int mv = (((int)Canvas.GetTop(shp)) + move);
            int mv2 = (((int)Canvas.GetLeft(shp)) + move);


            //if (mv % 100 == 53 || mv2 % 100 == 53)
            //{
            //    direction = random.Next(1, 5);
            //    if (direction == 2 && predirection == 1)
            //    {
            //        Canvas.SetLeft(shp, Canvas.GetLeft(shp) + 11);
            //    }
            //    else if (direction == 1 && predirection == 2)
            //    {
            //        Canvas.SetLeft(shp, Canvas.GetLeft(shp) - 11);
            //    }
            //    else if (direction == 3 && predirection == 4)
            //    {
            //        Canvas.SetTop(shp, Canvas.GetTop(shp) - 11);
            //    }
            //    else if (direction == 4 && predirection == 3)
            //    {
            //        Canvas.SetTop(shp, Canvas.GetTop(shp) + 11);
            //    }
            //}



            if (direction == 1)
            {
                if((Canvas.GetTop(shp) + move) > 700)
                {
                    Canvas.SetTop(shp, 0);
                }
                else
                {
                    Canvas.SetTop(shp, Canvas.GetTop(shp) + move); // getTop später nicht notwendig da pos genau kommt
                }
     
            }
            if (direction == 2)
            {
                if ((Canvas.GetTop(shp) + move) < 0 )
                {
                    Canvas.SetTop(shp, 700);
                }
                else
                {
                    Canvas.SetTop(shp, Canvas.GetTop(shp) - move); // getTop später nicht notwendig da pos genau kommt
                }
                
            }
            if (direction == 3)
            {
                if ((Canvas.GetLeft(shp) + move) > 700 )
                {
                    Canvas.SetLeft(shp, 0);
                }
                else
                {
                    Canvas.SetLeft(shp, Canvas.GetLeft(shp) + move); // getTop später nicht notwendig da pos genau kommt
                }
                
            }
            if (direction == 4)
            {
                if ((Canvas.GetLeft(shp) + move) < 0)
                {
                    Canvas.SetLeft(shp, 700);
                }
                else
                {
                    Canvas.SetLeft(shp, Canvas.GetLeft(shp) - move); // getTop später nicht notwendig da pos genau kommt
                }
            }
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
            Console.WriteLine("Das Auto mit der ID " + this.id + " fährt mit der Geschwindigkeit " + this.move);

        }
        
    }
}
