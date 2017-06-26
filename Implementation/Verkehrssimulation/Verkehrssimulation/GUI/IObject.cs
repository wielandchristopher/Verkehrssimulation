using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Verkehrssimulation.GUI
{
    
    interface IObject
    {
        /* 
        * Fügt ein Auto der GUI hinzu
        */
        bool addCarObject(int x, int y, int id);


        /* 
         * Fügt ein LKW der GUI hinzu
         */
        bool addLKWObject(int x, int y, int dir, int id);

        /* 
         * Aktualisiert die Position eines Autos in der GUI anhand seiner ID
        */
        bool updateCarWithID(int x, int y, int id, int typ, int dir);

    }
}
