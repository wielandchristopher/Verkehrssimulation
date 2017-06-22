using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsnetz
{
    interface IGUI
    {
        void addObstacle(int startx, int starty, int endx, int endy);
    }
}
