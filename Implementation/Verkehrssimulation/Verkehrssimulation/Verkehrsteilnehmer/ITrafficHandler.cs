using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsteilnehmer
{
    interface ITrafficHandler
    {
        void updateAll();
        void createNewVerkehrsteilnehmer(int x, int y, int speed, int typ, int direction, int nextDirection);
        void removeVerkehrsteilnehmer(int id);
    }
}
