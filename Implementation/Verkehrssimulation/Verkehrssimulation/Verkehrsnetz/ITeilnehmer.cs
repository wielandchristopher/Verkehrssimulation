using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsnetz
{
    interface ITeilnehmer
    {
        int getNeededEnvironmentRules(int x, int y);
        StreetInfo getNeededStreetRules(int x, int y);
        bool isObstacleInMyWay(int x, int y);
        bool isOutside(int x, int y);
        List<Obstacle> getObstacles();
        List<EntryPoint> getEnvironmentEntries();
    }
}
