using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Verkehrssimulation.GUI
{
    class ObjectHandler:IObject
    {
        List<Verkehrsteilnehmer> objlist;
        Canvas canvas;

        public ObjectHandler(Canvas mycanvas)
        {
            objlist = new List<Verkehrsteilnehmer>();
            canvas = mycanvas;
            canvas.MouseLeftButtonDown += addobstacle;

        }

        public void addobstacle(object sender, EventArgs e)
        {
            Console.WriteLine("add obstacle");
            

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

        public bool updateCarWithID(int x, int y, int id, int typ)
        {

            foreach(Verkehrsteilnehmer obj in this.objlist)
            {
                if (obj.getID() == id)
                {
                    obj.update(x, y);
                }
            }
            return true;
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
