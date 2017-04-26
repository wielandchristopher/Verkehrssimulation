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

        
        List<Auto> objlist;
        Canvas canvas;

        public ObjectHandler(Canvas mycanvas)
        {
            objlist = new List<Auto>();
            canvas = mycanvas;
        }

        public bool addObject(Auto obj)
        {
            objlist.Add(obj);
            canvas.Children.Add(obj.getShape());
            return true;
        }

        public bool addStrasse(Strasse obj)
        {
            canvas.Children.Add(obj.getShape());
            return true;
        }

        public void UpdateAll() // just for testing
        {
            foreach( Auto obj in objlist){
                //obj.setColor(Colors.Green);
                obj.update(5);
            }
        }

        public void UpdateID(int id) 
        {
            foreach (Auto obj in objlist)
            {
                if (obj.getID() == id)
                {
                    obj.update(5);
                }

            }
        }


    }
}
