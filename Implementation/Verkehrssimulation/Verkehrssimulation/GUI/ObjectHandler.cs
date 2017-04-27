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
        Random random;

        public ObjectHandler(Canvas mycanvas)
        {
            random = new Random();
            objlist = new List<Auto>();
            canvas = mycanvas;

            //  public Testobject(double x,double y, int cardirection, int newid)  
            initializeRandomCars(23);
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
                obj.update(random);
            }
        }
        private int getlinepos(int line, int dir)
        {
            int pos = line * 100;
            if (dir == 1 || dir == 4)
            {
                pos += +42;
            }
            else if (dir == 2 || dir == 3)
            {
                pos += +53;
            }

            return pos;
        }

        public void initializeRandomCars(int x)
        {
            
            

            for (int i=0;i< x; i++)
            {
                int newdir = random.Next(1, 5);
                int side = random.Next(1, 3);
                int startpos = 0;
                if (side == 1)
                {
                    startpos = 700;
                }
                else
                {
                    startpos = 0;
                }

                if (newdir>=3) // 1 down, 2 up
                {
                    if (startpos == 700)
                    {
                        addObject(new Auto(getlinepos(random.Next(0, 7), newdir), startpos, newdir, i, random.Next(1, 5)));
                    }
                    else
                    {
                        addObject(new Auto(getlinepos(random.Next(0, 7), newdir), startpos, newdir, i, random.Next(1, 5)));
                    }
                    
                }
                else // 3 right, 4 left
                {
                    if (startpos == 0)
                    {
                        addObject(new Auto(startpos, getlinepos(random.Next(0, 7), newdir), newdir, i, random.Next(1, 5)));
                    }
                    else
                    {
                        addObject(new Auto(startpos, getlinepos(random.Next(0, 7), newdir), newdir, i, random.Next(1, 5)));
                    }
                    
                }

                
            }
            
            //addObject(new Auto(700, getlinepos(4, 2), 2, 26, 2));
            //addObject(new Auto(0, getlinepos(5, 1), 1, 27, 1));
            //addObject(new Auto(700, getlinepos(1, 2), 2, 28, 4));

            //addObject(new Auto(getlinepos(6, 1), 0, 3, 29, 3));
            //addObject(new Auto(getlinepos(4, 2), 700, 3, 30, 5));
            //addObject(new Auto(getlinepos(5, 1), 0, 4, 31, 2));
            //addObject(new Auto(getlinepos(1, 2), 700, 4, 32, 1));
        }

        public void UpdateID(int id) 
        {
            foreach (Auto obj in objlist)
            {
                if (obj.getID() == id)
                {
                    obj.update(random);
                }

            }
        }


    }
}
