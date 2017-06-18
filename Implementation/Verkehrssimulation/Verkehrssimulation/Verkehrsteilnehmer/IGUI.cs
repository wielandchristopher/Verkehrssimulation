using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsteilnehmer
{
    interface IGUI
    {
        void updateCarAmount(int cars);
        void updateTruckRatio(int percent);
    }
}
