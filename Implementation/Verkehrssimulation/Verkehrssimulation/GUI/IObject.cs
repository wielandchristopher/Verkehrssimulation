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


        // Configurationseinstellungen der GUI

        /* 
         * Gibt Anteil der LKWs in Prozent zurück (0 - 100)
         */
         
        //int getLKWFraction();

        /* 
         * Gibt Anzahl der Fahrzeuge zurück die "gespawnt" werden sollen (0 - 100)
        */
        //int getVehicleNumber();
    }
}
