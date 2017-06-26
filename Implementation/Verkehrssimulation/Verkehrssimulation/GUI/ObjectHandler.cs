using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Verkehrssimulation.Verkehrsnetz;

namespace Verkehrssimulation.GUI
{
    class ObjectHandler:IObject
    {
        List<Verkehrsteilnehmer> objlist;
        Canvas canvas;
        private EnvironmentBuilder builder;

        public ObjectHandler(Canvas mycanvas, ref EnvironmentBuilder builder)
        {
            objlist = new List<Verkehrsteilnehmer>();
            this.builder = builder;
            canvas = mycanvas;
            canvas.MouseLeftButtonDown += addObstacle;

        }

        public bool addCarObject(int x, int y, int id)
        {
            Verkehrsteilnehmer obj = new Verkehrsteilnehmer(x, y, id);
            objlist.Add(obj);
            canvas.Children.Add(obj.getShape());
            return true;
        }
        public bool addLKWObject(int x, int y, int dir, int id)
        {
            Verkehrsteilnehmer obj = new Verkehrsteilnehmer(x, y, dir, id);
            objlist.Add(obj);
            canvas.Children.Add(obj.getShape());
            return true;
        }

        public bool updateCarWithID(int x, int y, int id, int type, int dir)
        {

            foreach(Verkehrsteilnehmer obj in this.objlist)
            {
                if (obj.getID() == id)
                {
                    obj.update(x, y, type, dir);
                }
            }
            return true;
        }

        public void addObstacle(object sender, EventArgs e)
        {
            Point p = Mouse.GetPosition(canvas);
            Obstacle obs = new Obstacle((int)p.X, (int)p.Y, 1);
            canvas.Children.Add(obs.getShape());
            builder.addObstacle((int)p.X, (int)p.Y, (int)p.X + 1, (int)p.Y + 1);


        }

        //private int getlinepos(int line, int dir)
        //{
        //    int pos = line * 100;
        //    if (dir == 1 || dir == 4)
        //    {
        //        pos += +42;
        //    }
        //    else if (dir == 2 || dir == 3)
        //    {
        //        pos += +53;
        //    }

        //    return pos;
        //}
    }
}
