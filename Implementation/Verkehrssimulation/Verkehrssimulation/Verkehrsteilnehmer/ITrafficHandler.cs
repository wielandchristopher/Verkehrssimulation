using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verkehrssimulation.Verkehrsnetz;

namespace Verkehrssimulation.Verkehrsteilnehmer
{
    interface ITrafficHandler
    {
        void updateAll();
        void createNewVerkehrsteilnehmer(int x, int y, int speed, int typ, int direction, int nextDirection);
        void addCarToEntryPoint(EntryPoint entrypoint, int typ, int speed = 5, int moveForward = 0);
        void removeVerkehrsteilnehmer(int id);
    }
}
