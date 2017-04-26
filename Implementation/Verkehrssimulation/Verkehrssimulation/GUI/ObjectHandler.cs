using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Verkehrssimulation.GUI
{
    class ObjectHandler : ObjHandlerInterface
    {

        
        List<Testobject> objlist;
        Canvas canvas;

        public ObjectHandler(Canvas mycanvas)
        {
            objlist = new List<Testobject>();
            canvas = mycanvas;
        }

        public bool addObject(Testobject obj)
        {
            objlist.Add(obj);
            canvas.Children.Add(obj.getShape());
            return true;
        }

        public void UpdateAll() // just for testing
        {
            foreach( Testobject obj in objlist){
                //obj.setColor(Colors.Green);
                obj.update(5);
            }
        }

        public void UpdateID(int id) 
        {
            foreach (Testobject obj in objlist)
            {
                if (obj.getID() == id)
                {
                    obj.update(5);
                }

            }
        }


    }
}
